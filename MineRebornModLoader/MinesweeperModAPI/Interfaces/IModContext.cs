using MinesweeperCore;

namespace MinesweeperModAPI.Interfaces;

public interface IModContext
{
    Game Game { get; }

    IModManager ModManager { get; }

    void Draw();

    void HandleGameOver();

    void LoadGame(string path);

    IDisposable AddMenuItem(string menuId, string title, Action onClick);
}