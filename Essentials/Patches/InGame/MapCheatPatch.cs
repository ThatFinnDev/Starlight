using Il2CppMonomiPark.SlimeRancher.UI.Map;
using Starlight.Menus;

namespace Starlight.Patches.InGame;

[HarmonyPatch(typeof(MapUI), nameof(MapUI.Start))]
internal static class MapCheatPatch
{
    internal static void Postfix(MapUI __instance)
    {
        if (StarlightCheatMenu.removeFog)
        {
            __instance.gameObject.GetObjectRecursively<GameObject>("fog_static").SetActive(false);
            __instance.gameObject.GetObjectRecursively<GameObject>("zone_fog_areas").SetActive(false);
        }
    }
}