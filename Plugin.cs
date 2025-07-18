using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MorningAutosave;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} patches done: {harmony.GetPatchedMethods().ToList().Count} methods");
    }

    public static void Log(string message)
    {
        Logger.LogInfo(message);
    }
}

[HarmonyPatch]
public class PluginPatches
{
    [HarmonyPatch(typeof(Bed), "GoToNextDay")]
    [HarmonyPostfix]
    private static void Bed_GoToNextDayPatch()
    {
        Save.AutoSaveGame();
    }
}
