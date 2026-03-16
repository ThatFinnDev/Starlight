namespace Starlight.Prism.Wrappers;

public class PrismLargo : PrismSlime
{

    internal PrismLargo(SlimeDefinition slimeDefinition, bool isNative): base(slimeDefinition, isNative)
    {
        this.SlimeDefinition = slimeDefinition;
        this.IsNative = isNative;
    }
}