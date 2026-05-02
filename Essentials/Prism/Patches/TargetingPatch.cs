using Il2CppMonomiPark.SlimeRancher.UI;
using Starlight.Storage;

namespace Starlight.Prism.Patches;

[PrismPatch()]
[HarmonyPatch(typeof(TargetingUI))]
internal class TargetingPatch
{

    [HarmonyPostfix, HarmonyPatch(typeof(TargetingUI), nameof(TargetingUI.ClearAllDisplayInfo))]
    static void ClearAllDisplayInfo(TargetingUI __instance) => Start(__instance);
    [HarmonyPostfix, HarmonyPatch(typeof(TargetingUI), nameof(TargetingUI.Start))]
    static void Start(TargetingUI __instance)
    {
        var eatStrings = __instance.SlimeEatStrings;

        if (eatStrings == null) return;
        if (eatStrings._foodGroupStringMap == null) return;
        
        foreach (var group in LookupEUtil.IdentifiableTypeGroupList.items)
            if (group._localizedName != null && group._isFood)
                eatStrings._foodGroupStringMap.TryAdd(group, group._localizedName);
    }
}