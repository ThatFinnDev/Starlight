using UnityEngine.Localization;

namespace Starlight.Prism.Wrappers;

public class PrismToy
{
    public static implicit operator ToyDefinition(PrismToy prismToy)
    {
        return prismToy.GetToyDefinition();
    }
    
    internal PrismToy(ToyDefinition toyDef, bool isNative)
    {
        this.ToyDef = toyDef;
        this.IsNative = isNative;
    }
    internal ToyDefinition ToyDef;
    protected bool IsNative;
    
    public ToyDefinition GetToyDefinition() => ToyDef;
    public string GetReferenceID() => ToyDef.ReferenceId;
    public string GetName() => ToyDef.name;
    public LocalizedString GetLocalized() => ToyDef.LocalizedName;
    public Color32 GetColor() => ToyDef.color;
    public GameObject GetPrefab() => ToyDef.prefab;
    public bool GetIsNative() => IsNative;
}