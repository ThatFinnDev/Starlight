using Il2CppMonomiPark.SlimeRancher.Pedia;

namespace Starlight.Prism.Wrappers;

public class PrismRadiantSlimePediaEntry : PrismPediaEntry
{
    public static implicit operator RadiantSlimePediaEntry(PrismRadiantSlimePediaEntry radiantSlimePediaEntry)
    {
        return radiantSlimePediaEntry.GetRadiantSlimePediaEntry();
    }
    public static implicit operator PrismRadiantSlimePediaEntry(RadiantSlimePediaEntry radiantSlimePediaEntry)
    {
        return radiantSlimePediaEntry.GetPrismRadiantSlimePediaEntry();
    }
    public RadiantSlimePediaEntry GetRadiantSlimePediaEntry() => PediaEntry.TryCast<RadiantSlimePediaEntry>();
    public IdentifiableType GetIdentifiableType() => GetRadiantSlimePediaEntry().SlimeDefinition;
    internal PrismRadiantSlimePediaEntry(PediaEntry pediaEntry, bool isNative): base(pediaEntry, isNative)
    {
        this.PediaEntry = pediaEntry;
        this.IsNative = isNative;
    }
}