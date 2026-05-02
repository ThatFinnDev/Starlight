using Il2CppMonomiPark.SlimeRancher.Options;
using Starlight.Enums;
using Starlight.Patches.MainMenu;
using Starlight.Storage;
using UnityEngine.Localization;

namespace Starlight.Buttons;
// Make it public on release
internal class CustomOptionsUICategory
{
    private HashSet<CustomOptionsButton> buttons = new ();
    public LocalizedString label;
    public int insertIndex;
    internal OptionsItemCategory _category;
    public Sprite icon;
    public OptionsCategoryVisibleState visibleState;
    public bool enabled = true;

    public CustomOptionsUICategory(LocalizedString label, int insertIndex, Sprite icon, OptionsCategoryVisibleState visibleState)
    {
        this.label = label; 
        this.insertIndex = insertIndex;
        this.icon = icon;
        this.visibleState = visibleState;

        StarlightOptionsButtonManager.customOptionsUICategories.Add(this,new HashSet<CustomOptionsButton>());
    }
    

    public void AddButton(CustomOptionsButton button)
    { 
        if(!StarlightOptionsButtonManager.customOptionsUICategories[this].Contains(button))
            StarlightOptionsButtonManager.customOptionsUICategories[this].Add(button);
    }
    public void RemoveButton(CustomOptionsButton button)
    { 
        if(StarlightOptionsButtonManager.customOptionsUICategories[this].Contains(button))
            StarlightOptionsButtonManager.customOptionsUICategories[this].Remove(button);
    }
    public void Remove()
    {
        enabled = false;
    }
    public void AddAgain()
    {
        enabled = true;
    }
}