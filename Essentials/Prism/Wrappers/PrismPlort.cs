using Starlight.Prism.Data;
using UnityEngine.Localization;
// ReSharper disable MemberCanBePrivate.Global

namespace Starlight.Prism.Wrappers;

public class PrismPlort
{
    public static implicit operator IdentifiableType(PrismPlort prismPlort)
    {
        return prismPlort.GetIdentifiableType();
    }
    
    public static implicit operator PrismPlort(PrismNativePlort nativePlort)
    {
        return nativePlort.GetPrismPlort();
    }

    internal readonly IdentifiableType identifiableType;
    private readonly bool _isNative;
    public IdentifiableType GetIdentifiableType() => identifiableType;
    public string GetReferenceID() => identifiableType.ReferenceId;
    public string GetName() => identifiableType.name;
    public Sprite GetIcon() => identifiableType.icon;
    public LocalizedString GetLocalized() => identifiableType.LocalizedName;
    public Color32 GetVacColor() => identifiableType.color;
    public GameObject GetPrefab() => identifiableType.prefab;
    public bool GetIsNative() => _isNative;
    
    
    public void SetIcon(Sprite newIcon)
    {
        identifiableType.icon = newIcon;
    }
    public void SetVacColor(Color32 newColor)
    {
        identifiableType.color = newColor;
    }
    
    internal PrismPlort(IdentifiableType identifiableType, bool isNative)
    {
        this.identifiableType = identifiableType;
        this._isNative = isNative;
    }
}