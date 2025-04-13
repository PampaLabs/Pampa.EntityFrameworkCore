# EntityFrameworkCore Relational Schema

This package enables database schema isolation in applications using `Entity Framework Core`.

## Installation

To use this extension with `EntityFrameworkCore`, you will first need to install the package.

```
dotnet add package Pampa.EntityFrameworkCore.Schema
```

## Usage

When configuring the `DbContext`, set up the connection string, schema, and migrations.

```csharp
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    var schema = "Customer";

    options.UseSqlServer(connectionString, builder =>
    {
        builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);

        builder.MigrationsHistoryTable(
            tableName: HistoryRepository.DefaultTableName,
            schema: schema
        );
    })
    .UseSchemaIsolation(schema);
});
```

## Migrations

When using schema isolation, migrations need a small manual change after being scaffolded to allow schema substitution at runtime.

### Why

EF Core hardcodes schema names like `::SCHEMA::` into generated files. To support dynamic schemas, you must inject it schema value.

**This enables the migrations system to correctly resolve the schema at runtime.**

### What to do

After running:

```bash
dotnet ef migrations add InitialCreate
```

EF Core will generate files like:
- `Migrations/InitialCreate.cs`
- `Migrations/InitialCreate.Designer.cs`
- `Migrations/ApplicationDbContextModelSnapshot.cs`

You must apply changes to all three.

### Migration (InitialCreate.cs)

You need to define a public constructor to receive the schema. Then replace the `"::SCHEMA::"` token with the schema value.

**Before**

```csharp
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "::SCHEMA::");

        // ...
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // ...
    }
}
```

**After**

```csharp
public partial class InitialCreate(ISchemaContext schemaContext) : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: schemaContext.Schema);

        // ...
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // ...
    }
}
```

### Migration designer (InitialCreate.Designer.cs)

Replace the `"::SCHEMA::"` token with the schema value. The migration is a partial class, so the constructor should already be modified in this step.

**Before**

```csharp
[DbContext(typeof(ApplicationDbContext))]
[Migration("00000000000000_InitialCreate")]
partial class InitialCreate
{
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("::SCHEMA::")
            .HasAnnotation("ProductVersion", "8.0.14")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        // ...
    }
}
```

**After**

```csharp
[DbContext(typeof(ApplicationDbContext))]
[Migration("00000000000000_InitialCreate")]
partial class InitialCreate
{
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema(schemaContext.Schema)
            .HasAnnotation("ProductVersion", "8.0.14")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        // ...
    }
}
```

### Snapshot

You need to define a public constructor to receive the schema. Then replace the `"::SCHEMA::"` token with the schema value.

**Before**

```csharp
[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("::SCHEMA::")
            .HasAnnotation("ProductVersion", "8.0.14");

        // ...
    }
}
```

**After**

```csharp
[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot(ISchemaContext schemaContext) : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema(schemaContext.Schema)
            .HasAnnotation("ProductVersion", "8.0.14");

        // ...
    }
}
```



## Contributing

Contributions are welcome! Please open an issue or submit a pull request on GitHub.
