namespace MinesweeperModAPI.Models;

public sealed class ModInfo
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Version { get; init; }
    public required string Author { get; init; }

    public string Description { get; init; } = string.Empty;
}