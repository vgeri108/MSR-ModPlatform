using MinesweeperCore;
using MinesweeperModAPI.Interfaces;

namespace MinesweeperModLoader.Runtime;

internal sealed class ModContext : IModContext
{
    private Game game;
    private readonly Action draw;
    private readonly Action handleGameOver;
    private readonly Action<string> loadGame;
    private readonly Func<string, string, Action, IDisposable> addMenuItem;

    public Game Game => game;
    public IModManager ModManager { get; }

    public ModContext(
        Game game,
        Action draw,
        Action handleGameOver,
        Action<string> loadGame,
        IModManager modManager,
        Func<string, string, Action, IDisposable> addMenuItem)
    {
        this.game = game;
        this.draw = draw;
        this.handleGameOver = handleGameOver;
        this.loadGame = loadGame;
        ModManager = modManager;
        this.addMenuItem = addMenuItem;
    }

    public void Draw() => draw();

    public void HandleGameOver() => handleGameOver();

    public IDisposable AddMenuItem(string menuId, string title, Action onClick) =>
        addMenuItem(menuId, title, onClick);

    public void UpdateGame(Game game)
    {
        this.game = game;
    }

    public void LoadGame(string path) => loadGame(path);
}