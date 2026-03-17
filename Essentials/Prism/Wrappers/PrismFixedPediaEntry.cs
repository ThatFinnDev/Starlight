using Il2CppMonomiPark.SlimeRancher.Pedia;

namespace Starlight.Prism.Wrappers;

public class PrismFixedPediaEntry : PrismPediaEntry
{
    public static implicit operator FixedPediaEntry(PrismFixedPediaEntry fixedPediaEntry)
    {
        return fixedPediaEntry.GetFixedPediaEntry();
    }
    public static implicit operator PrismFixedPediaEntry(FixedPediaEntry fixedPediaEntry)
    {
        return fixedPediaEntry.GetPrismFixedPediaEntry();
    }
    public FixedPediaEntry GetFixedPediaEntry() => PediaEntry.TryCast<FixedPediaEntry>();
    
    internal PrismFixedPediaEntry(PediaEntry pediaEntry, bool isNative): base(pediaEntry, isNative)
    {
        this.PediaEntry = pediaEntry;
        this.IsNative = isNative;
    }
    
    public void SetIcon(Sprite newIcon)
    {
        GetFixedPediaEntry()._icon = newIcon;
    }
}