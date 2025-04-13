namespace Microsoft.EntityFrameworkCore;

internal class SchemaContext(string schema) : ISchemaContext
{
    public string Schema { get; } = schema;
}
