namespace Microsoft.EntityFrameworkCore.Infrastructure.Internal;

internal class SchemaModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
    {
        var schemaContext = context.GetService<ISchemaContext>();

        return new
        {
            Type = context.GetType(),
            Schema = schemaContext.Schema,
            DesignTime = designTime
        };
    }

    public object Create(DbContext context)
        => Create(context, false);
}