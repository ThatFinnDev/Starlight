using Il2CppMonomiPark.SlimeRancher.World;
using Starlight.Storage;

namespace Starlight.Prism.Patches.Callback;

[PrismPatch()]
[HarmonyPatch(typeof(PlayerZoneTracker), nameof(PlayerZoneTracker.OnExited))]
static class ZoneExitPatch
{
    public static void Postfix(ZoneDefinition zone)
    {
        Callbacks.Invoke_onZoneExit(zone);
    }
}