using Il2CppMonomiPark.SlimeRancher.Pedia;

namespace Starlight.Prism.Wrappers;

public class PrismIdentifiablePediaEntry : PrismPediaEntry
{
    public static implicit operator IdentifiablePediaEntry(PrismIdentifiablePediaEntry identifiablePediaEntry)
    {
        return identifiablePediaEntry.GetIdentifiablePediaEntry();
    }
    public static implicit operator PrismIdentifiablePediaEntry(IdentifiablePediaEntry identifiablePediaEntry)
    {
        return identifiablePediaEntry.GetPrismIdentifiablePediaEntry();
    }
    public IdentifiablePediaEntry GetIdentifiablePediaEntry() => PediaEntry.TryCast<IdentifiablePediaEntry>();
    public IdentifiableType GetIdentifiableType() => GetIdentifiablePediaEntry().IdentifiableType;
    internal PrismIdentifiablePediaEntry(PediaEntry pediaEntry, bool isNative): base(pediaEntry, isNative)
    {
        this.PediaEntry = pediaEntry;
        this.IsNative = isNative;
    }
}