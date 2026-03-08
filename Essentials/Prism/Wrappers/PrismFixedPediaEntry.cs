using Il2CppMonomiPark.SlimeRancher.Pedia;
// ReSharper disable MemberCanBePrivate.Global

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
    public FixedPediaEntry GetFixedPediaEntry() => pediaEntry.TryCast<FixedPediaEntry>();
    
    internal PrismFixedPediaEntry(PediaEntry pediaEntry, bool isNative): base(pediaEntry, isNative)
    {
        this.pediaEntry = pediaEntry;
        this.isNative = isNative;
    }
    
    public void SetIcon(Sprite newIcon)
    {
        GetFixedPediaEntry()._icon = newIcon;
    }
}