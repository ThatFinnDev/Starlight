using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using Starlight.Prism.Data;
using Starlight.Prism.Lib;
using UnityEngine.Localization;
// ReSharper disable MemberCanBePrivate.Global

namespace Starlight.Prism.Wrappers;

public class PrismPediaEntry
{
    public static implicit operator PediaEntry(PrismPediaEntry prismPediaEntry)
    {
        return prismPediaEntry.GetPediaEntry();
    }
    
    internal PrismPediaEntry(PediaEntry pediaEntry, bool isNative)
    {
        this.pediaEntry = pediaEntry;
        this.isNative = isNative;
    }
    internal PediaEntry pediaEntry;
    protected bool isNative;
    
    public PediaEntry GetPediaEntry() => pediaEntry;
    public string GetPersistenceID() => pediaEntry.PersistenceId;
    public bool GetIsUnlockedInitially() => pediaEntry._isUnlockedInitially;
    public string GetName() => pediaEntry.name;
    public Sprite GetIcon() => pediaEntry.Icon;
    public LocalizedString GetTitle() => pediaEntry.Title;
    public LocalizedString GetDescription() => pediaEntry.Description;
    public int GetDetailCount() => pediaEntry._details.Count;
    public bool GetIsNative() => isNative;

    public void AddDetail(PrismPediaDetail? detail)
    {
        if(detail!=null)
            pediaEntry._details = pediaEntry._details.AddToNew(detail.ConvertToNativeType());
    }
    public void RemoveAtDetail(int index)
    {
        if (index < 0) return;
        if (index >= pediaEntry._details.Count) return;
        pediaEntry._details = pediaEntry._details.RemoveAtToNew(index);
    }    
    public void ReplaceDetail(int index,PrismPediaDetail? detail)
    {
        if (index < 0) return;
        if (index >= pediaEntry._details.Count) return;
        if (detail == null) return;
        pediaEntry._details = pediaEntry._details.ReplaceToNew(detail.ConvertToNativeType(),index);
    }    
    public void InsertDetail(int index,PrismPediaDetail? detail)
    {
        if (index < 0) return;
        if (index >= pediaEntry._details.Count) return;
        if (detail == null) return;
        pediaEntry._details = pediaEntry._details.InsertToNew(detail.ConvertToNativeType(),index);
    }

    public void ClearDetails()
    {
        pediaEntry._details = new Il2CppReferenceArray<PediaEntryDetail>(0);
    }
    
    public void AddAdditionalFact(PrismPediaAdditionalFact? fact)
    {
        if (fact == null) return;
        if (!PrismLibPedia._additionalFactsMap.ContainsKey(pediaEntry))
            PrismLibPedia._additionalFactsMap[pediaEntry] = new List<PrismPediaAdditionalFact>();
        PrismLibPedia._additionalFactsMap[pediaEntry].Add(fact.Value);
    }
    public void RemoveAdditionalFact(PrismPediaAdditionalFact? fact)
    {
        if (fact == null) return;
        if (!PrismLibPedia._additionalFactsMap.ContainsKey(pediaEntry)) return;
        if (PrismLibPedia._additionalFactsMap[pediaEntry].Contains(fact.Value))
            PrismLibPedia._additionalFactsMap[pediaEntry].Remove(fact.Value);
    }
    public void RemoveAtAdditionalFact(int index)
    {
        if (index < 0) return;
        if (!PrismLibPedia._additionalFactsMap.ContainsKey(pediaEntry)) return;
        if (PrismLibPedia._additionalFactsMap[pediaEntry].Count>index)
            PrismLibPedia._additionalFactsMap[pediaEntry].RemoveAt(index);
    }

    public void ClearAdditionalFacts()
    {
        if (PrismLibPedia._additionalFactsMap.ContainsKey(pediaEntry)) return;
            PrismLibPedia._additionalFactsMap.Remove(pediaEntry);
    }
}