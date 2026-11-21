namespace MinesweeperModAPI.Models;

public sealed class ModEntry
{
    public ModInfo Info { get; }
    public bool IsEnabled { get; }

    public ModEntry(ModInfo info, bool isEnabled)
    {
        Info = info;
        IsEnabled = isEnabled;
    }
}
