using System;
using Il2CppMonomiPark.SlimeRancher.UI.ButtonBehavior;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.Options;
using Il2CppMonomiPark.SlimeRancher.Platform;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu.Definition;
using Il2CppSystem.Linq;
using Il2CppTMPro;
using Starlight.Buttons;
using Starlight.Enums;
using Starlight.Managers;
using Starlight.Storage;

namespace Starlight.Patches.MainMenu;


[HarmonyPatch(typeof(MainMenuLandingRootUI), nameof(MainMenuLandingRootUI.Init))]
internal static class MainMenuLandingRootUIInitPatch
{
    private static CustomMainMenuContainerButton _rootStub = null;
    internal static CustomMainMenuContainerButton rootStub
    {
        get
        {
            if (_rootStub != null) return _rootStub;
            _rootStub = new CustomMainMenuContainerButton(-1);
            return _rootStub;
        }
    }
    
    internal static Dictionary<CustomMainMenuButton,HashSet<CustomMainMenuContainerButton>> buttons = new ();
    
    internal static bool safeLock;
    internal static bool postSafeLock;
    internal static void Prefix(MainMenuLandingRootUI __instance)
    {
        if (InjectOptionsButtons.HasFlag()) try { StarlightOptionsButtonManager.GenerateMissingButtons(); }catch (Exception e) { MelonLogger.Error(e); }
        if (!InjectMainMenuButtons.HasFlag()) return;
        foreach (var pair in buttons)
        {
            if (!pair.Value.Contains(rootStub)) continue;
            var button = pair.Key;
            if (button is CustomMainMenuContainerButton containerButton)
            {
                try
                {
                    if (containerButton._definition2 == null) continue;
                    
                    var list = new List<ButtonBehaviorDefinition>();
                    foreach (var pair2 in buttons)
                    {
                        if(pair2.Value.Contains(containerButton))
                        {
                            if (pair2.Key._definition != null) list.Add(pair2.Key._definition);
                            else if (pair2.Key._definition2!=null) list.Add(pair2.Key._definition2);
                        }
                    }
                    button._definition2._subMenuItems = ScriptableObject.CreateInstance<ButtonBehaviorConfiguration>();
                    button._definition2._subMenuItems.items = list.ToIl2CppList();
                    if (__instance._mainMenuConfig.items.Contains(containerButton._definition2)) continue;
                    int _offset = 0;
                    foreach (var item in __instance._mainMenuConfig.items) 
                        if(item is LoadGameItemDefinition) if(!(item is CustomMainMenuItemDefinition)) _offset=1;
                    __instance._mainMenuConfig.items.Insert(Math.Clamp(containerButton.insertIndex+_offset,0,__instance._mainMenuConfig.items.Count), containerButton._definition2);
                }
                catch (Exception e) { MelonLogger.Error(e); }
            }
            else
            {
                try
                {
                    if (button._definition != null)
                    {
                        if (__instance._mainMenuConfig.items.Contains(button._definition))
                            continue;
                        int _offset = 0;
                        foreach (var item in __instance._mainMenuConfig.items) 
                            if(item is LoadGameItemDefinition) if(!(item is CustomMainMenuItemDefinition)) _offset=1;
                        __instance._mainMenuConfig.items.Insert(Math.Clamp(button.insertIndex+_offset,0,__instance._mainMenuConfig.items.Count), button._definition);
                    }
                }
                catch (Exception e) { MelonLogger.Error(e); }
            }

        }
    }

    static void ChangeVersionLabel()
    {
        if (EnableLocalizedVersionPatch.HasFlag()) 
            try
            {
                var versionLabel = Get<LocalizedVersionText>("Version Label").GetComponent<TextMeshProUGUI>();
                if(!versionLabel.text.Contains("Mel"))
                {
                    if (StarlightUpdateManager.newVersion != null)
                        if (StarlightUpdateManager.newVersion != BuildInfo.DisplayVersion)
                        {
                            if (StarlightUpdateManager.updatedStarlight) versionLabel.text = translation("patches.localizedversionpatch.downloadedversion", StarlightUpdateManager.newVersion, versionLabel.text);
                            else versionLabel.text = translation("patches.localizedversionpatch.newversion", StarlightUpdateManager.newVersion, versionLabel.text);
                        }
                    versionLabel.text = translation("patches.localizedversionpatch.default", mlVersion, versionLabel.text);
                }
            }
            catch {  }
    }
    private static void Postfix()
    {
        ChangeVersionLabel();
        ExecuteInTicks((() => { ChangeVersionLabel();}), 1);
        ExecuteInTicks((() => { ChangeVersionLabel();}), 3);
        ExecuteInTicks((() => { ChangeVersionLabel();}), 10);            
    }

    public static bool alreadyLoadedOptions = false;
    
}
