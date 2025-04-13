using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

internal static class AssemblyExtensions
{
    public static IEnumerable<TypeInfo> GetConstructibleTypes(this Assembly assembly)
        => assembly.GetLoadableDefinedTypes().Where(
            t => !t.IsAbstract
                && !t.IsGenericTypeDefinition);

    public static IEnumerable<TypeInfo> GetLoadableDefinedTypes(this Assembly assembly)
    {
        try
        {
            return assembly.DefinedTypes;
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null).Select(IntrospectionExtensions.GetTypeInfo!);
        }
    }
}