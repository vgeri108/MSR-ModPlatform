using MinesweeperModAPI.Models;

namespace MinesweeperModAPI.Interfaces;

public interface IModManager
{
    IReadOnlyList<ModEntry> GetMods();

    void EnableMod(string modId);
    void DisableMod(string modId);
    void UnloadMod(string modId);
}
