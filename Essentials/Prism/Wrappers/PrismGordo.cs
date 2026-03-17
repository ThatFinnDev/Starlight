using UnityEngine.Localization;

namespace Starlight.Prism.Wrappers;

public class PrismGordo
{
    public static implicit operator IdentifiableType(PrismGordo prismGordo)
    {
        return prismGordo.GetIdentifiableType();
    }
    
    internal PrismGordo(IdentifiableType identifiableType, bool isNative)
    {
        this._identifiableType = identifiableType;
        this._isNative = isNative;
    }

    private readonly IdentifiableType _identifiableType;
    private readonly bool _isNative;
    
    public IdentifiableType GetIdentifiableType() => _identifiableType;
    public string GetReferenceID() => _identifiableType.ReferenceId;
    public string GetName() => _identifiableType.name;
    public Sprite GetIcon() => _identifiableType.icon;
    public LocalizedString GetLocalized() => _identifiableType.LocalizedName;
    public GameObject GetPrefab() => _identifiableType.prefab;
    public bool GetIsNative() => _isNative;
    
    public void SetIcon(Sprite newIcon)
    {
        _identifiableType.icon = newIcon;
    }
}