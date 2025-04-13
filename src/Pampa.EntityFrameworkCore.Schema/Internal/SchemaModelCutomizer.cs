namespace Microsoft.EntityFrameworkCore.Infrastructure;

internal class SchemaModelCustomizer : RelationalModelCustomizer
{
    public SchemaModelCustomizer(ModelCustomizerDependencies dependencies) : base(dependencies)
    {
    }

    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);

        var schemaContext = context.GetService<ISchemaContext>();
        modelBuilder.Model.SetDefaultSchema(schemaContext.Schema);
    }
}
