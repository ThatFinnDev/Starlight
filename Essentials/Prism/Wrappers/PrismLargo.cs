namespace Starlight.Prism.Wrappers;

public class PrismLargo : PrismSlime
{
    public SlimeAppearance GetSlimeAppearanceFirstRadiant() => SlimeDefinition.RadiantLargo0;
    public SlimeAppearance GetSlimeAppearanceSecondRadiant() => SlimeDefinition.RadiantLargo1;
    internal PrismLargo(SlimeDefinition slimeDefinition, bool isNative): base(slimeDefinition, isNative)
    {
        this.SlimeDefinition = slimeDefinition;
        this.IsNative = isNative;
    }
}