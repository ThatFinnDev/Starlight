using Il2CppMonomiPark.SlimeRancher.UI;

namespace Starlight.Patches.InGame;

[HarmonyPatch(typeof(BaseUI), nameof(BaseUI.Close), [])]
internal static class BaseUIClosePatch
{
    internal static void Prefix()
    {
        UIDisplayInteractableOnInteractPatch.takeOverNextUI = false;
    }
}