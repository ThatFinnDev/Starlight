using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight.Prism.Patches.Landplot;

[PrismPatch()]
[HarmonyPatch(typeof(LandPlot),nameof(LandPlot.OnDestroy))]
internal static class LandPlotOnDestroyPatch
{
    [HarmonyFinalizer]
    static Exception Finalizer(LandPlot __instance,Exception __exception)
    {
        if (__exception == null) return null;
        try { if (PrismLibLandPlots.LandPlotLocations.Contains(__instance.transform.parent.GetComponent<LandPlotLocation>())) return null; } catch { }
        return __exception;
    }
}