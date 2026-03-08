using UnityEngine.Localization;
// ReSharper disable MemberCanBePrivate.Global

namespace Starlight.Prism.Wrappers;

public class PrismIdentifiableTypeGroup
{
    public static implicit operator IdentifiableTypeGroup(PrismIdentifiableTypeGroup group)
    {
        return group.GetIdentifiableTypeGroup();
    }
    
    public static implicit operator PrismIdentifiableTypeGroup(IdentifiableTypeGroup group)
    {
        return group.GetPrismIdentifiableGroup();
    }

    internal PrismIdentifiableTypeGroup(IdentifiableTypeGroup group, bool isNative)
    {
        this.group = group;
        this._isNative = isNative;
    }

    internal readonly IdentifiableTypeGroup group;
    private readonly bool _isNative;
    
    public IdentifiableTypeGroup GetIdentifiableTypeGroup() => group;
    public string GetReferenceID() => group.ReferenceId;
    public string GetName() => group.name;
    public bool GetIsFood() => group._isFood;
    public LocalizedString GetLocalized() => group.LocalizedName;
    public bool GetIsNative() => _isNative;
    
    public void SetIsFood(bool isFood)
    {
        group._isFood=isFood;
    }

}