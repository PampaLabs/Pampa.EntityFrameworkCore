namespace Microsoft.EntityFrameworkCore.Migrations;

/// <summary>
/// Provides helper methods and constants for managing database migrations in a multi-tenant environment.
/// </summary>
internal static class MigrationHelper
{
    /// <summary>
    /// The token used to represent the schema placeholder in migration-related strings.
    /// </summary>
    public const string SchemaToken = "::SCHEMA::";
}
