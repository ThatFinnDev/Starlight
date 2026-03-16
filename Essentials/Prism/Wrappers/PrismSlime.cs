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
        this.SlimeDefinition = slimeDefinition;
        this.IsNative = isNative;
    }
    internal SlimeDefinition SlimeDefinition;
    protected bool IsNative;
    
    public SlimeDefinition GetSlimeDefinition() => SlimeDefinition;
    public string GetReferenceID() => SlimeDefinition.ReferenceId;
    public string GetName() => SlimeDefinition.name;
    public Sprite GetIcon() => SlimeDefinition.icon;
    public LocalizedString GetLocalized() => SlimeDefinition.LocalizedName;
    public Color32 GetVacColor() => SlimeDefinition.color;
    public GameObject GetPrefab() => SlimeDefinition.prefab;
    public SlimeAppearance GetSlimeAppearance() => SlimeDefinition.AppearancesDefault[0];
    public SlimeDiet GetSlimeDiet() => SlimeDefinition.Diet;
    public bool GetIsNative() => IsNative;
    
    public void SetIcon(Sprite newIcon)
    {
        SlimeDefinition.icon = newIcon;
        foreach (var appearance in SlimeDefinition.Appearances.ToList())
            appearance._icon=newIcon;
    }
    public void SetVacColor(Color32 newColor)
    {
        SlimeDefinition.color = newColor;
        foreach (var appearance in SlimeDefinition.Appearances.ToList())
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