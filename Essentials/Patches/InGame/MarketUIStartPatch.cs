using System.Linq;
using Il2CppMonomiPark.SlimeRancher.UI;

namespace Starlight.Patches.InGame;

[HarmonyPriority(-99999999),HarmonyPatch(typeof(MarketUI), nameof(MarketUI.Start))]
internal static class MarketUIStartPatch
{
    public static MarketUI instance;
    public static void Prefix(MarketUI __instance)
    {
        instance = __instance;
        try
        {
            __instance._config._plorts = __instance._config._plorts.Take(34).ToArray();
        } catch { }
    }
    internal static void Postfix(MarketUI __instance)
    {
        try
        {
            __instance._config._plorts = __instance._config._plorts.Take(34).ToArray();
        } catch { }
        if(StarlightEntryPoint.enableMarketViewer)
            ExecuteInTicks(() =>
            {
                var parent = __instance.transform.parent;
                var decor = parent.GetObjectRecursively<Transform>("decor");
                var activatorPrefab = parent.parent.parent.Find("cellLabCave/Sector/Main Nav/tech/techRefinery (5)/objTerminal01/techActivator");
                var instance = Object.Instantiate(activatorPrefab, decor);
                instance.localPosition = new Vector3(-1.711f, 0.0495f, 0.4242f);
                instance.localRotation = Quaternion.identity;
                instance.localScale = new Vector3(1,1,1);
                instance.gameObject.name = "MarketViewer";
                var trigger = instance.GetObjectRecursively<ScienceUIDisplayActivator>("triggerActivate");
                trigger.name = "StarlightMarketViewerOverride";
            },1);
    }
}