using MinesweeperCore;
using MinesweeperModAPI.Interfaces;

namespace MinesweeperModLoader.Runtime;

internal sealed class ModContext : IModContext
{
    private Game game;
    private readonly Action draw;
    private readonly Action handleGameOver;
    private readonly Action<string> loadGame;

    public Game Game => game;

    public ModContext(
        Game game,
        Action draw,
        Action handleGameOver,
        Action<string> loadGame)
    {
        this.game = game;
        this.draw = draw;
        this.handleGameOver = handleGameOver;
        this.loadGame = loadGame;
    }

    public void Draw() => draw();

    public void HandleGameOver() => handleGameOver();

    public void UpdateGame(Game game)
    {
        this.game = game;
    }
    public void LoadGame(string path) => loadGame(path);
}