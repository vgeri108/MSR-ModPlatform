using MinesweeperCore;

namespace MinesweeperModAPI.Interfaces;

public interface IModContext
{
    Game Game { get; }

    void Draw();

    void HandleGameOver();

    void LoadGame(string path);
}