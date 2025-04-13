using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore;

internal class SchemaOptionsExtension : IDbContextOptionsExtension
{
    private readonly ISchemaContext _schemaContext;

    public DbContextOptionsExtensionInfo Info { get; }

    public SchemaOptionsExtension(ISchemaContext schemaContext)
    {
        _schemaContext = schemaContext;
        Info = new SchemaOptionsExtensionInfo(this);
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton(_schemaContext);
    }

    public void Validate(IDbContextOptions options)
    {
    }
}
