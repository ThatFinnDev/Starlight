using UnityEngine.Localization;

namespace Starlight.Prism.Wrappers;

public class PrismLiquid
{
    public static implicit operator LiquidDefinition(PrismLiquid prismLiquid)
    {
        return prismLiquid.GetLiquidDefinition();
    }
    
    internal PrismLiquid(LiquidDefinition liquidDef, bool isNative)
    {
        this.LiquidDef = liquidDef;
        this.IsNative = isNative;
    }
    internal LiquidDefinition LiquidDef;
    protected bool IsNative;
    
    public LiquidDefinition GetLiquidDefinition() => LiquidDef;
    public string GetReferenceID() => LiquidDef.ReferenceId;
    public string GetName() => LiquidDef.name;
    public LocalizedString GetLocalized() => LiquidDef.LocalizedName;
    public Color32 GetColor() => LiquidDef.color;
    public GameObject GetPrefab() => LiquidDef.prefab;
    public bool GetIsNative() => IsNative;
    public bool GetIsWater() => LiquidDef.IsWater;
}