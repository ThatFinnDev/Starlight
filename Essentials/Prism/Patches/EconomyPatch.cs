using Il2CppMonomiPark.SlimeRancher.Economy;
using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight.Prism.Patches;

[PrismPatch()]
[HarmonyPatch(typeof(PlortEconomyDirector),nameof(PlortEconomyDirector.InitModel))]
internal static class EconomyPatch
{
    public static void Prefix(PlortEconomyDirector __instance)
    {
        PrismLibMarket.TryRefreshMarketData(__instance._settings);
    }
}