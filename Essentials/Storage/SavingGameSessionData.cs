using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;

namespace Starlight.Storage;

public class SavingGameSessionData
{
    public readonly ISaveReferenceTranslation iSaveReferenceTranslation;
    public readonly SaveReferenceTranslation saveReferenceTranslation;
    public readonly GameV10 gameV10;
    public readonly GameModel gameModel;
    public readonly GameMetadata gameMetadata;
    public readonly SavedGameInfoProvider savedGameInfoProvider;

    internal SavingGameSessionData(ISaveReferenceTranslation iSaveReferenceTranslation, SaveReferenceTranslation saveReferenceTranslation, 
        GameV10 gameV10, GameModel gameModel, GameMetadata gameMetadata, SavedGameInfoProvider savedGameInfoProvider)
    {
        this.gameMetadata = gameMetadata;
        this.iSaveReferenceTranslation = iSaveReferenceTranslation;
        this.saveReferenceTranslation = saveReferenceTranslation;
        this.gameV10 = gameV10;
        this.gameModel = gameModel;
        this.savedGameInfoProvider = savedGameInfoProvider;
    }
}
