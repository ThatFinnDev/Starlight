using MelonLoader;
using Starlight.Expansion;
using Starlight.Patches.Context;

namespace Starlight.Managers;

public static class StarlightCounterGateManager
{
    internal static List<object> useOcclusionCullingList = new List<object>();
    internal static List<object> disableCheatsList = new List<object>();
    public static bool playerCameraUseOcclusionCulling => useOcclusionCullingList.Count == 0;
    public static bool disableCheats => disableCheatsList.Count != 0;

    internal static void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        RefreshOcclusionCulling();
    }
    static void RefreshDisableCheats()
    {
        try
        {
            if(GameContextPatch.cheatMenuButton!=null)
                if(disableCheats) 
                    GameContextPatch.cheatMenuButton.Remove();
                else
                    GameContextPatch.cheatMenuButton.AddAgain();
        }
        catch 
        { }
    }
    static void RefreshOcclusionCulling()
    {
        try
        {
            foreach (var cam in Camera.allCameras)
                if(cam.name.Contains("Player")||cam.name.Contains("SRLECamera"))
                    cam.useOcclusionCulling = playerCameraUseOcclusionCulling;
        }
        catch 
        { }
    }
    public static void RegisterFor_PlayerCameraDisableUseOcclusionCulling(this StarlightExpansionVXX expansion)
    {
        if (!useOcclusionCullingList.Contains(expansion)) useOcclusionCullingList.Add(expansion);
        RefreshOcclusionCulling();
    }
    public static void DeregisterFor_PlayerCameraDisableUseOcclusionCulling(this StarlightExpansionVXX expansion)
    {
        if (!useOcclusionCullingList.Contains(expansion)) useOcclusionCullingList.Remove(expansion);
        RefreshOcclusionCulling();
    }
    public static void RegisterFor_PlayerCameraDisableUseOcclusionCulling(this MelonBase melon)
    {
        if (!useOcclusionCullingList.Contains(melon)) useOcclusionCullingList.Add(melon);
        RefreshOcclusionCulling();
    }
    public static void DeregisterFor_PlayerCameraDisableUseOcclusionCulling(this MelonBase melon)
    {
        if (!useOcclusionCullingList.Contains(melon)) useOcclusionCullingList.Remove(melon);
        RefreshOcclusionCulling();
    }
    
    
    
    
    public static void RegisterFor_DisableCheats(this StarlightExpansionVXX expansion)
    {
        if (!disableCheatsList.Contains(expansion)) disableCheatsList.Add(expansion);
        RefreshDisableCheats();
    }
    public static void DeregisterFor_DisableCheats(this StarlightExpansionVXX expansion)
    {
        if (!disableCheatsList.Contains(expansion)) disableCheatsList.Remove(expansion);
        RefreshDisableCheats();
    }
    public static void RegisterFor_DisableCheats(this MelonBase melon)
    {
        if (!disableCheatsList.Contains(melon)) disableCheatsList.Add(melon);
        RefreshDisableCheats();
    }
    public static void DeregisterFor_DisableCheats(this MelonBase melon)
    {
        if (!disableCheatsList.Contains(melon)) disableCheatsList.Remove(melon);
        RefreshDisableCheats();
    }
}