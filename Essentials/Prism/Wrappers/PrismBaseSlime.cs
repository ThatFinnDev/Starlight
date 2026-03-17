using Starlight.Prism.Data;

namespace Starlight.Prism.Wrappers;

public class PrismBaseSlime : PrismSlime
{
    internal readonly bool AllowLargos;
    internal bool DisableAutoModdedLargos;
    public static implicit operator PrismBaseSlime(PrismNativeBaseSlime nativeBaseSlime)
    {
        return nativeBaseSlime.GetPrismBaseSlime();
    }

    public Material GetBaseMaterial()
    {
        try
        {
            foreach (var structure in GetSlimeAppearance()._structures)
                try
                {
                    if (structure.Element.Type == SlimeAppearanceElement.ElementType.BODY)
                        return structure.DefaultMaterials[0];
                } catch { }
        } catch { }

        return null;
    }
    internal PrismBaseSlime(SlimeDefinition slimeDefinition, bool isNative): base(slimeDefinition, isNative)
    {
        this.SlimeDefinition = slimeDefinition;
        this.IsNative = isNative;
        if (base.SlimeDefinition.CanLargofy)
            AllowLargos = true;
        if (!isNative)
            DisableAutoModdedLargos = true;
    }
    internal PrismBaseSlime(SlimeDefinition slimeDefinition, bool isNative, bool allowLargos, bool disableAutoModdedLargos): base(slimeDefinition, isNative)
    {
        this.SlimeDefinition = slimeDefinition;
        this.IsNative = isNative;
        this.AllowLargos = allowLargos;
        this.DisableAutoModdedLargos = disableAutoModdedLargos;
    }
}