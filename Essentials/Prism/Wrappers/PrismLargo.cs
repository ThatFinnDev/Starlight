namespace Starlight.Prism.Wrappers;

public class PrismLargo : PrismSlime
{
    public SlimeAppearance GetSlimeAppearanceFirstRadiant()
    {
        foreach (var appearance in SlimeDefinition.AppearancesDefault)
            if (appearance.name.Contains("Radiant"))
                return appearance;
        return null;
    }
    public SlimeAppearance GetSlimeAppearanceSecondRadiant()
    {
        var waited = false;
        foreach (var appearance in SlimeDefinition.AppearancesDefault)
            if (appearance.name.Contains("Radiant"))
                if(!waited)
                    waited = true;
                else return appearance;
        return null;
    }
    internal PrismLargo(SlimeDefinition slimeDefinition, bool isNative): base(slimeDefinition, isNative)
    {
        this.SlimeDefinition = slimeDefinition;
        this.IsNative = isNative;
    }
}