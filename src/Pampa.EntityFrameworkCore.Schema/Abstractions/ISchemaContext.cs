namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a context for a database schema.
/// </summary>
public interface ISchemaContext
{
    /// <summary>
    /// Gets the database schema of the context.
    /// </summary>
    string Schema { get; }
}
