using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;

namespace Starlight.Patches.Saving.Fixer;

[HarmonyPriority(-99999999)]
[HarmonyPatch(typeof(GameModelPushHelpers), nameof(GameModelPushHelpers.PushGame))]
internal static class SaveFixerPushGame
{
    private static bool NeedsRemoving(int integer,ILoadReferenceTranslation r)
    {
        try { if (r.GetIdentifiableType(integer) == null) return true; }
        catch (Exception e) { return true; }
        return false;
    }
    internal static void Prefix(ActorIdProvider actorIdProvider, ISaveReferenceTranslation saveReferenceTranslation, GameV09 gameState, GameModel gameModel)
    {
        if (!StarlightEntryPoint.disableFixSaves)
            try {
                var loadTranslation = saveReferenceTranslation.ToNonIVariant().CreateLoadReferenceTranslation(gameState);
                foreach (var id in gameState.Drone.Cloud.IDs.ToArray())
                {
                    try {
                        if (NeedsRemoving(id,loadTranslation))
                            gameState.Drone.Cloud.IDs.Remove(id);
                    }
                    catch (Exception e) { LogError(e); }
                }
            }
            catch (Exception e) { LogError(e); }
    }

}