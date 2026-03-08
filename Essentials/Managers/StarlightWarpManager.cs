using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppMonomiPark.SlimeRancher.Regions;
using Il2CppMonomiPark.SlimeRancher.SceneManagement;
using Il2CppMonomiPark.SlimeRancher.World.Teleportation;
using Starlight.Enums;
using Starlight.Storage;

namespace Starlight.Managers;

public static class StarlightWarpManager
{
    internal static Dictionary<string, StaticTeleporterNode> teleporters = new Dictionary<string, StaticTeleporterNode>();
    internal static Warp warpTo = null;

    /// <summary>
    /// Saves warp to be used in the warp command into a name
    /// </summary>
    /// <param name="warpName">The name for the warp</param>
    /// <param name="warp">The Warp</param>
    /// <returns>StarlightError: NoError or AlreadyExists</returns>
    public static StarlightError AddWarp(string warpName, Warp warp)
    {
        if (StarlightSaveManager.data.warps.ContainsKey(warpName)) return StarlightError.AlreadyExists;
        StarlightSaveManager.data.warps.Add(warpName, warp);
        StarlightSaveManager.Save();
        return StarlightError.NoError;
    }

    /// <summary>
    /// Gets a saved warp from a name
    /// </summary>
    /// <param name="warpName"></param>
    /// <returns>The saved warp</returns>
    public static Warp GetWarp(string warpName)
    {
        if (!StarlightSaveManager.data.warps.ContainsKey(warpName)) return null;
        return StarlightSaveManager.data.warps[warpName];
    }

    /// <summary>
    /// Removes a saved warp by its name
    /// </summary>
    /// <param name="warpName">The warp to be removed</param>
    /// <returns>StarlightError: NoError, DoesntExist</returns>
    public static StarlightError RemoveWarp(string warpName)
    {
        if (!StarlightSaveManager.data.warps.ContainsKey(warpName)) return StarlightError.DoesntExist;
        StarlightSaveManager.data.warps.Remove(warpName);
        StarlightSaveManager.Save();
        return StarlightError.NoError;
    }
    
    internal static void OnSceneLoaded()
    {
        if(warpTo==null) return;
        if (sceneContext == null) { warpTo = null; return; }
        if (sceneContext.PlayerState == null) { warpTo = null; return; }
        
        foreach (SceneGroup group in systemContext.SceneLoader.SceneGroupList.items)
            if (group.IsGameplay) if (group.ReferenceId == warpTo.sceneGroup)
                if (warpTo.sceneGroup == sceneContext.RegionRegistry.CurrentSceneGroup.ReferenceId)
                {
                    SRCharacterController cc = sceneContext.Player.GetComponent<SRCharacterController>();
                    cc.Position = warpTo.position;
                    cc.Rotation = warpTo.rotation;
                    cc.BaseVelocity = Vector3.zero;
                    warpTo = null;
                    break;
                }
    }
}