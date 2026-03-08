using Il2CppMonomiPark.SlimeRancher.UI.ButtonBehavior;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu.Definition;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu.Definition.ButtonBehavior;
using UnityEngine.Localization;
using Starlight.Patches.MainMenu;
namespace Starlight.Buttons;

public class CustomMainMenuButton
{
    public LocalizedString label;
    public Sprite icon;
    public int insertIndex;
    internal CustomMainMenuItemDefinition _definition;
    internal CustomMainMenuSubItemDefinition _definition2;
    public SystemAction action;

    internal CustomMainMenuButton(int THIS_IS_A_STUB)
    {
        
    }
    public CustomMainMenuButton(LocalizedString label, Sprite icon, int insertIndex, SystemAction action)
    {
        this.label = label;
        this.icon = icon;
        this.insertIndex = insertIndex;
        this.action = action;

        MainMenuLandingRootUIInitPatch.buttons.Add(this,new HashSet<CustomMainMenuContainerButton>(){MainMenuLandingRootUIInitPatch.rootStub});

        if (this is CustomMainMenuContainerButton)
        {
            _definition = null;
            _definition2 = ScriptableObject.CreateInstance<CustomMainMenuSubItemDefinition>();
            _definition2._label = label;
            _definition2.name = label.TableEntryReference.Key;
            _definition2._icon = icon;
            _definition2.hideFlags |= HideFlags.HideAndDontSave;
            _definition2.customAction = action;
        }
        else
        {
            _definition = ScriptableObject.CreateInstance<CustomMainMenuItemDefinition>();
            _definition._label = label;
            _definition.name = label.TableEntryReference.Key;
            _definition._icon = icon;
            _definition.hideFlags |= HideFlags.HideAndDontSave;
            _definition.customAction = action;
        }
        if (StarlightEntryPoint.MainMenuLoaded)
        {
            MainMenuLandingRootUI mainMenu = Object.FindObjectOfType<MainMenuLandingRootUI>();
            if (mainMenu != null)
            {
                mainMenu.gameObject.SetActive(false);
                mainMenu.gameObject.SetActive(true);
            }
        }
    }
}
