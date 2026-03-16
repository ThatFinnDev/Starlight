using Il2CppMonomiPark.SlimeRancher.UI;
using SR2E.Storage;

namespace SR2E.Prism.Patches;

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

        MelonLogger.Msg("test");
        if (eatStrings == null) return;
        MelonLogger.Msg("test 2");
        if (eatStrings._foodGroupStringMap == null) return;
        MelonLogger.Msg("test 3");
        
        foreach (var group in LookupEUtil._identifiableTypeGroupList.items)
            if (group._localizedName != null && group._isFood)
                eatStrings._foodGroupStringMap.TryAdd(group, group._localizedName);
    }
}