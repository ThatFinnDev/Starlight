using Il2CppMonomiPark.SlimeRancher.UI.MainMenu;

namespace Starlight.Patches.MainMenu;

[HarmonyPatch(typeof(NewGameOptionsUIRoot), nameof(NewGameOptionsUIRoot.Awake))]
internal static class NewGameRootUIPatch
{
    internal static void Prefix(NewGameOptionsUIRoot __instance)
    {
        StarlightEntryPoint.BaseUIAddSliders.Add(__instance);
    }
}