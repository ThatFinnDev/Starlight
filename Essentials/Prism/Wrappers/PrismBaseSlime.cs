using Starlight.Prism.Data;
// ReSharper disable NotAccessedField.Global

namespace Starlight.Prism.Wrappers;

public class PrismBaseSlime : PrismSlime
{
    internal readonly bool allowLargos;
    internal bool disableAutoModdedLargos;
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
                    {
                        return structure.DefaultMaterials[0];
                    }
                }
                catch
                {
                    // ignored
                }
        }
        catch
        {
            // ignored
        }

        return null;
    }
    internal PrismBaseSlime(SlimeDefinition slimeDefinition, bool isNative): base(slimeDefinition, isNative)
    {
        this.slimeDefinition = slimeDefinition;
        this.isNative = isNative;
        if (base.slimeDefinition.CanLargofy)
            allowLargos = true;
        if (!isNative)
            disableAutoModdedLargos = true;
    }
    internal PrismBaseSlime(SlimeDefinition slimeDefinition, bool isNative, bool allowLargos, bool disableAutoModdedLargos): base(slimeDefinition, isNative)
    {
        this.slimeDefinition = slimeDefinition;
        this.isNative = isNative;
        this.allowLargos = allowLargos;
        this.disableAutoModdedLargos = disableAutoModdedLargos;
    }
}