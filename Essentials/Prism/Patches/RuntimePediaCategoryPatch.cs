using Il2CppMonomiPark.SlimeRancher.Pedia;
using Starlight.Prism.Data;
using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight.Prism.Patches;

[PrismPatch()]
[HarmonyPatch(typeof(PediaCategory), nameof(PediaCategory.GetRuntimeCategory))]
internal class RuntimePediaCategoryPatch
{
    private static readonly Dictionary<string, PrismPediaCategoryType> Categories = new()
    {
        {"Slimes", PrismPediaCategoryType.Slimes},
        {"Resources", PrismPediaCategoryType.Resources},
        {"Blueprints", PrismPediaCategoryType.Blueprints},
        {"World", PrismPediaCategoryType.World},
        {"Weather", PrismPediaCategoryType.Weather},
        {"Toys", PrismPediaCategoryType.Toys},
        {"Ranch", PrismPediaCategoryType.Ranch},
        {"Science", PrismPediaCategoryType.Science},
        {"Tutorials", PrismPediaCategoryType.Tutorial},
    };
    public static void Postfix(PediaCategory __instance, ref PediaRuntimeCategory __result)
    {
        if (Categories.TryGetValue(__instance.name, out PrismPediaCategoryType category))
        {
            PrismLibPedia.PediaCategories.Remove(category);
            PrismLibPedia.PediaCategories.Add(category,__instance);
            foreach (var pedia in PrismLibPedia.PediaEntryLookup[category])
            {
                if (!__result._items.Contains(pedia))
                     __result._items.Add(pedia);
            }
        }
    }
}