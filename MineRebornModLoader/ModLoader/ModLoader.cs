using System.Reflection;
using MinesweeperCore;
using MinesweeperModAPI.Interfaces;
using MinesweeperModLoader.Loading;
using MinesweeperModLoader.Models;
using MinesweeperModLoader.Runtime;

namespace MinesweeperModLoader;

public sealed class ModLoader
{
    private readonly List<LoadedMod> mods = [];

    public IReadOnlyList<LoadedMod> Mods => mods.AsReadOnly();

    public void LoadMods(
        string modsDirectory,
        Game game,
        Action draw,
        Action handleGameOver,
        Action<string> loadGame)
    {
        Directory.CreateDirectory(modsDirectory);

        foreach (string dllPath in Directory.GetFiles(modsDirectory, "*.dll", SearchOption.AllDirectories))
        {
            try
            {
                LoadMod(dllPath, game, draw, handleGameOver, loadGame);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[ModLoader] Failed to load {dllPath}: {ex}");
            }
        }

        foreach (LoadedMod mod in mods.ToArray())
        {
            try
            {
                EnableMod(mod.Info.Id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"[ModLoader] Failed to enable {mod.Info.Id}: {ex}");
            }
        }
    }

    private void LoadMod(
        string dllPath,
        Game game,
        Action draw,
        Action handleGameOver,
        Action<string> loadGame)
    {
        ModLoadContext loadContext = new(dllPath);

        try
        {
            Assembly assembly = loadContext.LoadFromAssemblyPath(
                Path.GetFullPath(dllPath));

            Type[] modTypes = assembly.GetTypes()
                .Where(type =>
                    typeof(IMod).IsAssignableFrom(type) &&
                    type is { IsAbstract: false, IsInterface: false })
                .ToArray();

            if (modTypes.Length == 0)
                throw new InvalidOperationException(
                    "No valid mod implementation found.");

            foreach (Type modType in modTypes)
            {
                if (Activator.CreateInstance(modType) is not IMod instance)
                    continue;

                ModInfoValidation(instance.Info);

                if (mods.Any(mod =>
                    mod.Info.Id.Equals(
                        instance.Info.Id,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException(
                        $"Duplicate mod ID: {instance.Info.Id}");
                }

                ModContext context = new(game, draw, handleGameOver, loadGame);

                instance.OnLoad(context);

                mods.Add(new LoadedMod(instance, context, loadContext));
            }
        }
        catch
        {
            loadContext.Unload();
            throw;
        }
    }

    public void UpdateGame(Game game)
    {
        foreach (LoadedMod mod in mods)
            mod.Context.UpdateGame(game);
    }

    public void EnableMod(string modId)
    {
        LoadedMod? mod = FindMod(modId);

        if (mod is null || mod.IsEnabled)
            return;

        mod.Instance.OnEnable();
        mod.IsEnabled = true;
    }

    public void DisableMod(string modId)
    {
        LoadedMod? mod = FindMod(modId);

        if (mod is null || !mod.IsEnabled)
            return;

        mod.Instance.OnDisable();
        mod.IsEnabled = false;
    }

    public void UnloadMod(string modId)
    {
        LoadedMod? mod = FindMod(modId);

        if (mod is null)
            return;

        if (mod.IsEnabled)
            DisableMod(modId);

        mod.Instance.OnUnload();
        mods.Remove(mod);

        mod.LoadContext.Unload();
    }

    public void UnloadAll()
    {
        foreach (LoadedMod mod in mods.ToArray())
        {
            try
            {
                UnloadMod(mod.Info.Id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                    $"[ModLoader] Failed to unload {mod.Info.Id}: {ex}");
            }
        }
    }

    private LoadedMod? FindMod(string modId)
    {
        return mods.FirstOrDefault(mod =>
            mod.Info.Id.Equals(
                modId, StringComparison.OrdinalIgnoreCase));
    }

    private static void ModInfoValidation(
        MinesweeperModAPI.Models.ModInfo info)
    {
        if (string.IsNullOrWhiteSpace(info.Id) ||
            string.IsNullOrWhiteSpace(info.Name) ||
            string.IsNullOrWhiteSpace(info.Version) ||
            string.IsNullOrWhiteSpace(info.Author))
        {
            throw new InvalidOperationException(
                "Mod information contains missing required fields.");
        }
    }
}