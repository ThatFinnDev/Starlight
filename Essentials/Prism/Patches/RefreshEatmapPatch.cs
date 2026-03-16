using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight.Prism.Patches;

[PrismPatch()]
[HarmonyPatch(typeof(SlimeDiet), nameof(SlimeDiet.RefreshEatMap))]
internal class RefreshEatmapPatch
{
    // ReSharper disable once InconsistentNaming
    public static void Postfix(SlimeDiet __instance, SlimeDefinition definition)
    {
        if (PrismLibDiet.CustomEatmaps.TryGetValue(definition, out var eatMap))
        {
            foreach (var eat in eatMap)
            {
                if(eat.Value)
                    if(!PrismLibDiet._CarefulCheck(__instance.EatMap,eat.Key)) continue;
                __instance.EatMap.Add(eat.Key);
            }
        }
    }
}