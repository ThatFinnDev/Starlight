using Il2CppMonomiPark.SlimeRancher.Persist;
using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight.Prism.Patches;

[PrismPatch()]
[HarmonyPatch(typeof(GameV09), nameof(GameV09.LoadSummaryData))]
internal static class GameLoadSummaryPatch
{
    static void Prefix()
    {
        try
        {
            foreach (var actor in PrismLibSaving.savedIdents)
            {
                PrismLibSaving.RefreshIfNotFound(autoSaveDirector._saveReferenceTranslation,actor.Value);
            }
        }
        catch {}
    }
}