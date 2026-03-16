using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight.Prism.Patches;

[PrismPatch()]
[HarmonyPatch(typeof(DirectedActorSpawner))]
internal class SpawnerPatch
{
    [HarmonyPatch(nameof(DirectedActorSpawner.Awake))]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    static void PostAwake(DirectedActorSpawner __instance)
    {
        foreach (var action in PrismLibSpawning.ExecuteOnSpawnerAwake)
            try
            {
                action.Invoke(__instance);
            }
            catch (Exception e) { LogError(e); }
    }
    
}