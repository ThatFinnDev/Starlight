using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using Starlight.Prism.Lib;

namespace Starlight.Prism.Patches;


//[PrismPatch()]
//[HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.Awake))]
// Starlight's patch loads it
internal static class SaveDirectorPatch
{
    internal static void Postfix(AutoSaveDirector __instance)
    {
        PrismShortcuts.EmptyTranslation = AddTranslation("");
        PrismShortcuts.UnavailableIcon = Get<Sprite>("unavailableIcon");
        PrismLibPedia.PediaDetailTypesInitialize();
        
        foreach (var category in GetAll<PediaCategory>())
            try { category.GetRuntimeCategory(); 
            } catch { }


        foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
            try { expansion.OnPrismCreateAdditions(); }
            catch (Exception e) { LogError(e); }
        
        
        foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
            try { expansion.AfterPrismCreateAdditions(); }
            catch (Exception e) { LogError(e); }
        
        
        // Doing this so it executes after all expansions have made their slimes.
        foreach (var largoAction in PrismShortcuts.CreateLargoActions)
            try { largoAction.Invoke(); }
            catch (Exception e) { LogError(e); }
        
        
        foreach (var expansion in StarlightEntryPoint.ExpansionV01S)
            try { expansion.AfterPrismLargosCreated(); }
            catch (Exception e) { LogError(e); }
        
        
    }
}
