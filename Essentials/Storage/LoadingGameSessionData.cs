using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;

namespace Starlight.Storage;

public class LoadingGameSessionData
{
    public readonly ActorIdProvider actorIdProvider;
    public readonly ISaveReferenceTranslation iSaveReferenceTranslation;
    public readonly SaveReferenceTranslation saveReferenceTranslation;
    public readonly GameV10 gameV10;
    public readonly GameModel gameModel;

    internal LoadingGameSessionData(ActorIdProvider actorIdProvider, ISaveReferenceTranslation iSaveReferenceTranslation,
        SaveReferenceTranslation saveReferenceTranslation, GameV10 gameV10, GameModel gameModel)
    {
        this.actorIdProvider = actorIdProvider;
        this.iSaveReferenceTranslation = iSaveReferenceTranslation;
        this.saveReferenceTranslation = saveReferenceTranslation;
        this.gameV10 = gameV10;
        this.gameModel = gameModel;
    }
}