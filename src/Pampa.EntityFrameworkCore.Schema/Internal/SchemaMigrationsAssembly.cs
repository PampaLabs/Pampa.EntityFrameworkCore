using System.Reflection;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "Multi-tenancy support")]
internal class SchemaMigrationsAssembly : MigrationsAssembly
{
    private readonly ISchemaContext _schemaContext;

    private readonly Type _contextType;

    private ModelSnapshot? _modelSnapshot;

    public SchemaMigrationsAssembly(
        ICurrentDbContext currentContext,
        IDbContextOptions options,
        IMigrationsIdGenerator idGenerator,
        IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
        : base(currentContext, options, idGenerator, logger)
    {
        _contextType = currentContext.Context.GetType();
        _schemaContext = currentContext.Context.GetService<ISchemaContext>();
    }

    public override ModelSnapshot? ModelSnapshot
        => _modelSnapshot
                ??= (from t in Assembly.GetConstructibleTypes()
                     where t.IsSubclassOf(typeof(ModelSnapshot))
                         && t.GetCustomAttribute<DbContextAttribute>()?.ContextType == _contextType
                     select (ModelSnapshot)Activator.CreateInstance(t.AsType(), _schemaContext)!)
                .FirstOrDefault();

    public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
    {
        ArgumentNullException.ThrowIfNull(activeProvider, nameof(activeProvider));

        Migration migration = (Migration)Activator.CreateInstance(migrationClass.AsType(), _schemaContext)!;
        migration.ActiveProvider = activeProvider;

        return migration;
    }
}