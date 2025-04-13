using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides extension methods for configuring <see cref="DbContextOptionsBuilder"/>.
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Enables schema isolation support in the database context configuration.
    /// </summary>
    /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> instance.</param>
    /// <param name="schema">The database schema.</param>
    /// <returns>The modified <see cref="DbContextOptionsBuilder"/> instance.</returns>
    public static DbContextOptionsBuilder UseSchemaIsolation(this DbContextOptionsBuilder optionsBuilder, string? schema)
    {
        ConfigureServices(optionsBuilder);

        schema ??= MigrationHelper.SchemaToken;

        var extension = new SchemaOptionsExtension(new SchemaContext(schema));
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }

    private static void ConfigureServices(this DbContextOptionsBuilder builder)
    {
        builder
            .ReplaceService<IMigrationsAssembly, SchemaMigrationsAssembly>()
            .ReplaceService<IModelCacheKeyFactory, SchemaModelCacheKeyFactory>()
            .ReplaceService<IModelCustomizer, SchemaModelCustomizer>();
    }
}
