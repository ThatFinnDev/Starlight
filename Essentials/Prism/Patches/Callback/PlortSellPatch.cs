using Il2CppMonomiPark.SlimeRancher.Economy;
using Starlight.Storage;

namespace Starlight.Prism.Patches.Callback;

[PrismPatch()]
[HarmonyPatch(typeof(PlortEconomyDirector), nameof(PlortEconomyDirector.RegisterSold))]
static class PlortSellPatch
{
    // ReSharper disable once InconsistentNaming
    public static void Postfix(PlortEconomyDirector __instance, IdentifiableType id, int count)
    {
        Callbacks.Invoke_onPlortSold(count, id);
    }
}