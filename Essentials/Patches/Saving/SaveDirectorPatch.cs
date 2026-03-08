using Il2CppMonomiPark.SlimeRancher;
using Il2CppSystem.Reflection;
using Starlight.Enums;
using Starlight.Managers;

namespace Starlight.Patches.Saving;

[HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.Awake))]
internal static class SaveDirectorPatch
{
    internal static void Prefix(AutoSaveDirector __instance)
    {
        foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
            try { expansion.BeforeSaveDirector(__instance); }
            catch (Exception e) { MelonLogger.Error(e); }
        StarlightCallEventManager.ExecuteWithArgs(CallEvent.BeforeSaveDirectorLoad,("saveDirector",__instance));
        
    }
    internal static void Postfix(AutoSaveDirector __instance)
    {
        LookupEUtil.IdentifiableTypeGroupList = Get<IdentifiableTypeGroupList>("All Type Groups List");
        if(StarlightEntryPoint.isPrismInUse)
            try
            {
                Prism.Patches.SaveDirectorPatch.Postfix(__instance);
            }
            catch (Exception e) { MelonLogger.Error(e); }
        
        
        foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
            try { expansion.AfterSaveDirector(__instance);
            } catch (Exception e) { MelonLogger.Error(e); }
        StarlightCallEventManager.ExecuteWithArgs(CallEvent.AfterSaveDirectorLoad,("saveDirector",__instance));
        
    }
}
