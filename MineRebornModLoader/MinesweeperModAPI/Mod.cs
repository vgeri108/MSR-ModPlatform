using MinesweeperModAPI.Interfaces;
using MinesweeperModAPI.Models;

namespace MinesweeperModAPI;

public abstract class Mod : IMod
{
    public abstract ModInfo Info { get; }

    protected IModContext Context { get; private set; } = null!;

    public virtual void OnLoad(IModContext context)
    {
        Context = context;
    }

    public virtual void OnEnable() { }

    public virtual void OnDisable() { }

    public virtual void OnUnload() { }
}