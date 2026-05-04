namespace Starlight.Patches.InGame;

[HarmonyPatch(typeof(GadgetDirector), nameof(GadgetDirector.GetItemCount))]
internal static class GadgetDirectorGetItemCountPatch
{
    internal static bool Prefix(IdentifiableType id, ref int __result)
    {
        if (!UIDisplayInteractableOnInteractPatch.takeOverNextUI) return true;
        if (sceneContext.PlortEconomyDirector.TryGetMarketValues(id, out var value, out _))
            __result = value;
        else __result = -1;
        return false;
    }
    [HarmonyFinalizer]
    static Exception Finalizer(Exception __exception)
    {
        if (!UIDisplayInteractableOnInteractPatch.takeOverNextUI) return __exception;
        return null;
    }
}