using System.Runtime.Loader;
using MinesweeperModAPI.Interfaces;
using MinesweeperModAPI.Models;
using MinesweeperModLoader.Runtime;

namespace MinesweeperModLoader.Models;

public sealed class LoadedMod
{
    public ModInfo Info { get; }
    public bool IsEnabled { get; internal set; }

    internal IMod Instance { get; }
    internal ModContext Context { get; }
    internal AssemblyLoadContext LoadContext { get; }

    internal LoadedMod(
        IMod instance,
        ModContext context,
        AssemblyLoadContext loadContext)
    {
        Instance = instance;
        Context = context;
        LoadContext = loadContext;
        Info = instance.Info;
    }
}