using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore;

internal class SchemaOptionsExtensionInfo : DbContextOptionsExtensionInfo
{
    public override bool IsDatabaseProvider => false;

    public override string LogFragment => nameof(SchemaOptionsExtension);

    public SchemaOptionsExtensionInfo(SchemaOptionsExtension extension) : base(extension)
    {
    }

    public override int GetServiceProviderHashCode() => 0;

    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
    {
    }

    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => false;
}
