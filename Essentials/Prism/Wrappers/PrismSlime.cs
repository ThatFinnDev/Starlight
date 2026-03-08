using Il2CppSystem.Linq;
using UnityEngine.Localization;

namespace Starlight.Prism.Wrappers;

public class PrismSlime
{
    public static implicit operator SlimeDefinition(PrismSlime prismSlime)
    {
        return prismSlime.GetSlimeDefinition();
    }
    
    internal PrismSlime(SlimeDefinition slimeDefinition, bool isNative)
    {
        this.slimeDefinition = slimeDefinition;
        this.isNative = isNative;
    }
    internal SlimeDefinition slimeDefinition;
    protected bool isNative;
    
    public SlimeDefinition GetSlimeDefinition() => slimeDefinition;
    public string GetReferenceID() => slimeDefinition.ReferenceId;
    public string GetName() => slimeDefinition.name;
    public Sprite GetIcon() => slimeDefinition.icon;
    public LocalizedString GetLocalized() => slimeDefinition.LocalizedName;
    public Color32 GetVacColor() => slimeDefinition.color;
    public GameObject GetPrefab() => slimeDefinition.prefab;
    public SlimeAppearance GetSlimeAppearance() => slimeDefinition.AppearancesDefault[0];
    public SlimeDiet GetSlimeDiet() => slimeDefinition.Diet;
    public bool GetIsNative() => isNative;
    
    public void SetIcon(Sprite newIcon)
    {
        slimeDefinition.icon = newIcon;
        foreach (var appearance in slimeDefinition.Appearances.ToList())
            appearance._icon=newIcon;
    }
    public void SetVacColor(Color32 newColor)
    {
        slimeDefinition.color = newColor;
        foreach (var appearance in slimeDefinition.Appearances.ToList())
        {
            appearance._splatColor=newColor;
            appearance._colorPalette = new SlimeAppearance.Palette
            {
                Ammo = newColor, Bottom = appearance._colorPalette.Bottom, Middle = appearance._colorPalette.Middle,
                Top = appearance._colorPalette.Top
            };
            
        }
    }
    
    
    
    
}