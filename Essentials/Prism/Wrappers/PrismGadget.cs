using UnityEngine.Localization;

namespace Starlight.Prism.Wrappers;

public class PrismGadget
{
    public static implicit operator GadgetDefinition(PrismGadget prismGadget)
    {
        return prismGadget.GetGadgetDefinition();
    }
    
    internal PrismGadget(GadgetDefinition gadgetDef, bool isNative)
    {
        this.GadgetDef = gadgetDef;
        this.IsNative = isNative;
    }
    internal GadgetDefinition GadgetDef;
    protected bool IsNative;
    
    public GadgetDefinition GetGadgetDefinition() => GadgetDef;
    public string GetReferenceID() => GadgetDef.ReferenceId;
    public string GetName() => GadgetDef.name;
    public LocalizedString GetLocalized() => GadgetDef.LocalizedName;
    public Color32 GetColor() => GadgetDef.color;
    public GameObject GetPrefab() => GadgetDef.prefab;
    public bool GetIsNative() => IsNative;
}