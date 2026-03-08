namespace Starlight.Prism.Wrappers;

public class PrismLargo : PrismSlime
{

    internal PrismLargo(SlimeDefinition slimeDefinition, bool isNative): base(slimeDefinition, isNative)
    {
        this.slimeDefinition = slimeDefinition;
        this.isNative = isNative;
    }
}