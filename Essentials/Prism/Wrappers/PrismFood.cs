using UnityEngine.Localization;

namespace Starlight.Prism.Wrappers;

public class PrismFood
{
    public static implicit operator IdentifiableType(PrismFood prismFood)
    {
        return prismFood.GetIdentifiableType();
    }
    
    internal PrismFood(IdentifiableType idType, bool isNative)
    {
        this.IDType = idType;
        this.IsNative = isNative;
    }
    internal IdentifiableType IDType;
    protected bool IsNative;
    
    public IdentifiableType GetIdentifiableType() => IDType;
    public string GetReferenceID() => IDType.ReferenceId;
    public string GetName() => IDType.name;
    public LocalizedString GetLocalized() => IDType.LocalizedName;
    public Color32 GetColor() => IDType.color;
    public GameObject GetPrefab() => IDType.prefab;
    public GameObject GetBaitPrefab() => IDType.gordoSnareBaitPrefab;
    public GameObject SetBaitPrefab(GameObject prefab) => IDType.gordoSnareBaitPrefab = prefab;
    public bool GetIsNative() => IsNative;
}