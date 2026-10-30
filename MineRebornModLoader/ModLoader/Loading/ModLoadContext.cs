using System.Reflection;
using System.Runtime.Loader;

namespace MinesweeperModLoader.Loading;

internal sealed class ModLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver resolver;
    private readonly string modDirectory;

    public ModLoadContext(string assemblyPath)
        : base(Path.GetFileNameWithoutExtension(assemblyPath), true)
    {
        resolver = new AssemblyDependencyResolver(assemblyPath);
        modDirectory = Path.GetDirectoryName(assemblyPath)!;
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        Assembly? sharedAssembly = AssemblyLoadContext.Default.Assemblies
            .FirstOrDefault(assembly =>
                AssemblyName.ReferenceMatchesDefinition(
                    assembly.GetName(),
                    assemblyName));

        if (sharedAssembly is not null)
            return sharedAssembly;

        string? assemblyPath =
            resolver.ResolveAssemblyToPath(assemblyName);

        assemblyPath ??= Path.Combine(
            modDirectory,
            $"{assemblyName.Name}.dll");

        return File.Exists(assemblyPath)
            ? LoadFromAssemblyPath(assemblyPath)
            : null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string? libraryPath =
            resolver.ResolveUnmanagedDllToPath(unmanagedDllName);

        return libraryPath is not null
            ? LoadUnmanagedDllFromPath(libraryPath)
            : IntPtr.Zero;
    }
}