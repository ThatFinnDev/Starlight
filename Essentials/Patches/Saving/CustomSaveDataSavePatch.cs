using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using Starlight.Managers;
using Starlight.Saving;
using Starlight.Storage;

namespace Starlight.Patches.Saving;

[HarmonyPriority(99999999)]
[HarmonyPatch(typeof(GameModelPullHelpers), nameof(GameModelPullHelpers.PullGame))]
internal static class CustomSaveDataSavePatch
{
    internal static string prefix = "StarlightDataV01";
    internal static string prefixown = "StarlightOwnDataV01";
    internal static void Postfix(GameModel gameModel,SavedGameInfoProvider savedGameInfoProvider, ISaveReferenceTranslation saveReferenceTranslation, GameMetadata metadata, ref GameV09 __result )
    {
        try
        {
            var rootSave = StarlightOptionsButtonManager.OnInGameSave(new SavingGameSessionData(saveReferenceTranslation,
                saveReferenceTranslation.ToNonIVariant(), __result, gameModel,metadata,savedGameInfoProvider));
            if (rootSave != null)
            {
                var base128 = rootSave.ToBytes().EncodeToBase128();
                var finalEntry = $"{prefixown}{base128}";
                __result.ZoneIndex.IndexTable = __result.ZoneIndex.IndexTable.AddToNew(finalEntry);
            }
        }
        catch (Exception e) { LogError(e); }
        
        
        foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
        {
            try
            {
                var rootSave = expansion.OnSaveCustomSaveData(new SavingGameSessionData(saveReferenceTranslation,
                    saveReferenceTranslation.ToNonIVariant(), __result, gameModel,metadata,savedGameInfoProvider)); 
                if (rootSave == null) continue;
                var base128 = rootSave.ToBytes().EncodeToBase128();
                var md5Hash = expansion.GetPackageInfoFromExpansion().Value.ID.CreateMD5();
                var finalEntry = $"{prefix}{md5Hash}{base128}";
                __result.ZoneIndex.IndexTable = __result.ZoneIndex.IndexTable.AddToNew(finalEntry);
            }
            catch (Exception e)
            {
                LogError($"Failed to save custom save data for expansion {expansion.GetPackageInfoFromExpansion().Value.Name}: {e}");
            }
        }
    }
}