using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using Starlight.Expansion;
using Starlight.Saving;
using Starlight.Storage;

namespace Starlight.Patches.Saving;
   
[HarmonyPriority(99999999)]
[HarmonyPatch(typeof(GameModelPushHelpers), nameof(GameModelPushHelpers.PushGame))]
internal static class CustomSaveDataLoadPatch
{
    static Dictionary<StarlightExpansionV01, (RootSave, LoadingGameSessionData)> rootSaves = new();
    static Dictionary<StarlightExpansionV01, LoadingGameSessionData> noRootSaves = new();
    internal static string prefix = "StarlightDataV01";
    internal static string prefixown = "StarlightOwnDataV01";
    internal static void ExecSaveDataReceived()
    {
        foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
            if (rootSaves.ContainsKey(expansion))
                try { expansion.OnCustomSaveDataReceived(rootSaves[expansion].Item1, rootSaves[expansion].Item2); } 
                catch (Exception e) { LogError(e); }
            else if(noRootSaves.ContainsKey(expansion))
                try { expansion.OnNoCustomSaveDataReceived(noRootSaves[expansion]); }
                catch (Exception e) { LogError(e); }
        rootSaves = new Dictionary<StarlightExpansionV01, (RootSave, LoadingGameSessionData)>();
        noRootSaves = new Dictionary<StarlightExpansionV01, LoadingGameSessionData>();
    }
    internal static void Prefix(ActorIdProvider actorIdProvider, ISaveReferenceTranslation saveReferenceTranslation, GameV09 gameState, GameModel gameModel)
    {
        bool hasExecutedOwn = false;
        foreach (var entry in gameState.ZoneIndex.IndexTable)
            if (entry.StartsWith(prefixown))
            {
                try
                {
                    string remaining = entry.Substring(prefixown.Length);
                    var rawBytes = remaining.DecodeFromBase128();
                    var rootSave = RootSave.FromBytes<StarlightOptionsButtonManager.CustomOptionsInGameSave>(rawBytes);
                    var sessionData = new LoadingGameSessionData(actorIdProvider, saveReferenceTranslation, saveReferenceTranslation.ToNonIVariant(), gameState, gameModel);
                    hasExecutedOwn = true;
                    StarlightOptionsButtonManager.OnInGameLoad(rootSave,sessionData);
                }catch (Exception e) { LogError(e); }
            }
        if(!hasExecutedOwn)
            try
            {
                var sessionData = new LoadingGameSessionData(actorIdProvider, saveReferenceTranslation, saveReferenceTranslation.ToNonIVariant(), gameState, gameModel);
                StarlightOptionsButtonManager.OnInGameLoad(null,sessionData);
            }catch (Exception e) { LogError(e); }
        
        
        rootSaves = new Dictionary<StarlightExpansionV01, (RootSave, LoadingGameSessionData)>();
        noRootSaves = new Dictionary<StarlightExpansionV01, LoadingGameSessionData>();
        var executedExpansions = new List<StarlightExpansionV01>();
        foreach (var entry in gameState.ZoneIndex.IndexTable)
            if (entry.StartsWith(prefix))
            {
                string remaining = entry.Substring(prefix.Length);
                
                if (remaining.Length >= 32)
                {
                    string md5Hash = remaining.Substring(0, 32);

                    foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
                        try
                        {
                            if(expansion.info.ID.CreateMD5() == md5Hash)
                            {
                                var sessionData = new LoadingGameSessionData(actorIdProvider, saveReferenceTranslation, saveReferenceTranslation.ToNonIVariant(), gameState, gameModel);
                                RootSave rootSave = null; 
                                try
                                {
                                    var rawBytes = remaining.Substring(32).DecodeFromBase128();
                                    rootSave = RootSave.FromBytes(rawBytes);
                                    if (rootSave == null) throw new Exception("Save Data is null!");;
                                    rootSaves.Add(expansion, (rootSave, sessionData));
                                    executedExpansions.Add(expansion);
                                }
                                catch (Exception e)
                                {
                                    LogError(
                                        $"Failed to save custom save data for expansion {expansion.info.name}: {e}");
                                }
                                if(rootSave!=null)
                                    try
                                    {
                                        expansion.OnEarlyCustomSaveDataReceived(rootSave, sessionData);
                                    }
                                    catch (Exception e) { LogError(e); }
                            }
                        } catch { }
                }
                else LogError("An error occured while loading some custom save data!");
            }
        foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
            if(!executedExpansions.Contains(expansion))
                try
                {
                    var sessionData = new LoadingGameSessionData(actorIdProvider, saveReferenceTranslation, saveReferenceTranslation.ToNonIVariant(), gameState, gameModel);
                    noRootSaves.Add(expansion, sessionData);

                    expansion.OnEarlyNoCustomSaveDataReceived(sessionData);
                }
                catch (Exception e) { LogError(e); }
    }
}