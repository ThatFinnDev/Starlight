using Il2CppMonomiPark.SlimeRancher.Event;

namespace Starlight.Patches.InGame;

[HarmonyPatch(typeof(IdentifiableTypeGroupEventProducer), nameof(IdentifiableTypeGroupEventProducer.RaiseEventForGroupsContainingType))]
internal static class IdentifiableTypeGroupEventProducerPatch
{
    [HarmonyFinalizer]
    static Exception Finalizer(Exception __exception)
    {
        return null;
    }
}