using Il2CppMonomiPark.SlimeRancher.Persist;
using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight.Prism.Patches;

[PrismPatch()]
[HarmonyPatch(typeof(GameV10), nameof(GameV10.LoadSummaryData))]
internal static class GameLoadSummaryPatch
{
    private static void Prefix()
    {
        try
        {
            foreach (var actor in PrismLibSaving.SavedIdents)
                PrismLibSaving.RefreshIfNotFound(autoSaveDirector._saveReferenceTranslation,actor.Value);
        } catch { }
    }
}