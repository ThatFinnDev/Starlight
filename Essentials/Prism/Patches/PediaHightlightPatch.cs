using System.Linq;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight.Prism.Patches;

[PrismPatch()]
[HarmonyPatch(typeof(PediaEntry),nameof(PediaEntry.Highlights))]
internal class PediaHightlightPatch
{
    public static void Postfix(PediaEntry __instance, ref Il2CppReferenceArray<PediaEntryHighlight> __result)
    {
        
        if (__instance == null) return;
        if (!PrismLibPedia.AdditionalFactsMap.ContainsKey(__instance)) return;
        var modifiedResult = __result.ToList();
        foreach (var additionalFact in PrismLibPedia.AdditionalFactsMap[__instance])
        {
            var native = additionalFact.ConvertToNativeType();
            native.Label ??= PrismShortcuts.EmptyTranslation;
            native.Description ??= PrismShortcuts.EmptyTranslation;
            native.Icon ??= PrismShortcuts.UnavailableIcon;
            modifiedResult.Add(native);
        }
        __result=modifiedResult.ToArray();
    }
}