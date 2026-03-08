using System.Linq;
using Il2CppAssets.Script.Util.Extensions;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using Il2CppMonomiPark.SlimeRancher.UI.Fabricator;

namespace Starlight.Patches.Saving.Fixer;


[HarmonyPriority(-99999999)]
[HarmonyPatch(typeof(GameModelPushHelpers), nameof(GameModelPushHelpers.PushPlayer))]
internal static class SaveFixerPushPlayer
{
    private static bool NeedsRemoving(int integer,ILoadReferenceTranslation r)
    {
        try { if (r.GetIdentifiableType(integer) == null) return true; }
        catch (Exception e) { return true; }
        return false;
    }
    internal static void Prefix(GameModel gameModel, PlayerV09 player, ILoadReferenceTranslation loadReferenceTranslation)
    {
        try
        {
            if (!StarlightEntryPoint.disableFixSaves)
            {
                Dictionary<int, int> copyOfItemCounts = new Dictionary<int, int>();
                var enumerator = player.ItemCounts.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var kvp = enumerator._current;
                    copyOfItemCounts.Add(kvp.Key, kvp.Value);
                }
                foreach(var itemCountPair in copyOfItemCounts)
                    if (NeedsRemoving(itemCountPair.Key,loadReferenceTranslation))
                        player.ItemCounts.Remove(itemCountPair.Key);
                    
                foreach(var blueprintID in player.Blueprints._items.ToList())
                    if (NeedsRemoving(blueprintID,loadReferenceTranslation))
                        player.Blueprints.Remove(blueprintID);
                    
                foreach(var availBlueprintID in player.AvailBlueprints._items.ToList())
                    if (NeedsRemoving(availBlueprintID,loadReferenceTranslation))
                        player.AvailBlueprints.Remove(availBlueprintID);
                    
                foreach(var favouriteGadgetID in player.FavoriteGadgets._items.ToList())
                    if (NeedsRemoving(favouriteGadgetID,loadReferenceTranslation))
                        player.FavoriteGadgets.Remove(favouriteGadgetID);

                try
                {
                    foreach(var viewedBluePrintID in player.ViewedItems.ViewedBlueprints.ToNetList())
                        if (NeedsRemoving(viewedBluePrintID,loadReferenceTranslation))
                            player.ViewedItems.ViewedBlueprints.Remove(viewedBluePrintID);
                }
                catch { }
                
            }
        }
        catch (Exception e) { LogError(e); }
        
    }

}