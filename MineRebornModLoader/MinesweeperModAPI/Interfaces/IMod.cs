using MinesweeperModAPI.Models;

namespace MinesweeperModAPI.Interfaces;

public interface IMod
{
    ModInfo Info { get; }

    void OnLoad(IModContext context);
    void OnEnable();
    void OnDisable();
    void OnUnload();
}