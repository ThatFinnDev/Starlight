using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using Starlight.Storage;

namespace Starlight.Patches.Dev;


[DevPatch()]
[HarmonyPatch(typeof(GameModelPushHelpers), nameof(GameModelPushHelpers.PushGame))]
internal static class PushGamePatch
{
    public static ActorIdProvider tmpActorIdProvider;
    public static GameV10 tmpGameState;
    public static ISaveReferenceTranslation tmpSaveReferenceTranslation;
    public static GameModel tmpGameModel;
    public static void Postfix(ActorIdProvider actorIdProvider, ISaveReferenceTranslation saveReferenceTranslation, GameV10 gameState, GameModel gameModel)
    {
        tmpActorIdProvider=actorIdProvider;
        tmpSaveReferenceTranslation=saveReferenceTranslation;
        tmpGameState=gameState;
        tmpGameModel=gameModel;
    }

}