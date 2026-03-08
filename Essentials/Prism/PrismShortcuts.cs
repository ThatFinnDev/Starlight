using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Il2CppMonomiPark.SlimeRancher.Pedia;
using Il2CppMonomiPark.SlimeRancher.UI;
using Starlight.Prism.Data;
using Starlight.Prism.Lib;
using Starlight.Prism.Wrappers;
using UnityEngine.Localization;

namespace Starlight.Prism;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
public static class PrismShortcuts
{
    internal static readonly Dictionary<IdentifiableType, PrismMarketData> MarketData = new (0);
    internal static readonly Dictionary<PlortEntry, bool> MarketPlortEntries = new ();
    internal static readonly List<IdentifiableType> RemoveMarketPlortEntries = new ();
    internal static readonly List<SystemAction> CreateLargoActions = new ();
    
    
    internal static readonly Dictionary<IdentifiableTypeGroup, PrismIdentifiableTypeGroup> PrismIdentifiableTypeGroups = new Dictionary<IdentifiableTypeGroup, PrismIdentifiableTypeGroup>();
    internal static readonly Dictionary<string, PrismBaseSlime> PrismBaseSlimes = new Dictionary<string, PrismBaseSlime>();
    internal static readonly Dictionary<string, PrismGordo> PrismGordos = new Dictionary<string, PrismGordo>();
    internal static readonly Dictionary<string, PrismLargo> PrismLargos = new Dictionary<string, PrismLargo>();

    internal static readonly Dictionary<PrismLargo, (PrismBaseSlime, PrismBaseSlime)> PrismLargoBases = new ();
    internal static readonly Dictionary<string, PrismPlort> PrismPlorts = new ();
    internal static readonly Dictionary<FixedPediaEntry, PrismFixedPediaEntry> PrismFixedPediaEntries = new();
    internal static readonly Dictionary<IdentifiablePediaEntry, PrismIdentifiablePediaEntry> PrismIdentifiablePediaEntries = new ();
    internal static readonly Dictionary<PediaEntry, PrismPediaEntry> PrismUnknownPediaEntries = new ();
    internal static LocalizedString EmptyTranslation;
    internal static Sprite UnavailableIcon;
    
    
    private static SlimeAppearanceDirector _mainAppearanceDirector;

    public static SlimeAppearanceDirector mainAppearanceDirector
    {
        get
        {
            if (_mainAppearanceDirector == null)
                
                _mainAppearanceDirector = Get<SlimeAppearanceDirector>("MainSlimeAppearanceDirector");
            return _mainAppearanceDirector;
        }
        set => _mainAppearanceDirector = value;
    }
    internal static readonly Dictionary<string, List<Action>> OnSceneLoaded = new ();
    internal static void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        var pair = OnSceneLoaded.FirstOrDefault(x => sceneName.Contains(x.Key));

        if (pair.Value != null)
            foreach (var action in pair.Value)
                action();
    }
    internal static void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
    }

    
    
    public static PrismBaseSlime GetPrismBaseSlime(this PrismNativeBaseSlime nativeBaseSlime)
    {
        try { return LookupEUtil.baseSlimeTypes.GetEntryByRefID(nativeBaseSlime.GetReferenceID()).GetPrismBaseSlime(); }
        catch
        {
            // ignored
        }

        return null;
    }
    public static PrismPlort GetPrismPlort(this PrismNativePlort nativePlort)
    {
        try { return LookupEUtil.plortTypes.GetEntryByRefID(nativePlort.GetReferenceID()).GetPrismPlort(); }
        catch
        {
            // ignored
        }

        return null;
    }
    public static PrismBaseSlime GetPrismBaseSlime(this SlimeDefinition customOrNativeSlime)
    {
        if (customOrNativeSlime == null) return null;
        if (customOrNativeSlime.IsLargo) return null;
        if (PrismBaseSlimes.TryGetValue(customOrNativeSlime.ReferenceId, out var slime)) return slime;
        var newSlime = new PrismBaseSlime(customOrNativeSlime, true);
        PrismBaseSlimes.Add(customOrNativeSlime.ReferenceId, newSlime);
        return newSlime;
    }
    public static PrismGordo GetPrismGordo(this IdentifiableType customOrNativeGordo)
    {
        if (customOrNativeGordo == null) return null;
        if (customOrNativeGordo.prefab==null) return null;
        if (!customOrNativeGordo.prefab.HasComponent<GordoIdentifiable>()) return null;
        if (PrismGordos.TryGetValue(customOrNativeGordo.ReferenceId, out var gordo)) return gordo;
        var newGordo = new PrismGordo(customOrNativeGordo, true);
        PrismGordos.Add(customOrNativeGordo.ReferenceId, newGordo);
        return newGordo;
    }
    public static PrismLargo GetPrismLargo(this SlimeDefinition customOrNativeSlime)
    {
        if (customOrNativeSlime == null) return null;
        if (!customOrNativeSlime.IsLargo) return null;
        if (PrismLargos.TryGetValue(customOrNativeSlime.ReferenceId, out var largo)) return largo;
        var newSlime = new PrismLargo(customOrNativeSlime, true);
        PrismLargos.Add(customOrNativeSlime.ReferenceId, newSlime);
        return newSlime;
    }
    
    public static PrismSlime GetPrismSlime(this SlimeDefinition customOrNativeSlime)
    {
        if (customOrNativeSlime == null) return null;
        if (customOrNativeSlime.IsLargo)
        {
            if (PrismLargos.TryGetValue(customOrNativeSlime.ReferenceId, out var slime)) return slime;
            var newLargo = new PrismLargo(customOrNativeSlime, true);
            PrismLargos.Add(customOrNativeSlime.ReferenceId, newLargo);
            return newLargo;
        }
        if (PrismBaseSlimes.TryGetValue(customOrNativeSlime.ReferenceId, out var prismSlime)) return prismSlime;
        var newBaseSlime = new PrismBaseSlime(customOrNativeSlime, true);
        PrismBaseSlimes.Add(customOrNativeSlime.ReferenceId, newBaseSlime);
        return newBaseSlime;
        
    }

    public static PrismPlort GetPrismPlort(this IdentifiableType customOrNativePlort)
    {
        if (customOrNativePlort == null) return null;
        if (!customOrNativePlort.IsPlort) return null;
        if (PrismPlorts.TryGetValue(customOrNativePlort.ReferenceId, out var plort)) return plort;
        var newPlort = new PrismPlort(customOrNativePlort, true);
        PrismPlorts.Add(customOrNativePlort.ReferenceId, newPlort);
        return newPlort;
    }

    public static PrismIdentifiableTypeGroup GetPrismIdentifiableGroup(this IdentifiableTypeGroup group)
    {
        if (group == null) return null;
        if (PrismIdentifiableTypeGroups.TryGetValue(group, out var identifiableGroup)) return identifiableGroup;
        var newPlort = new PrismIdentifiableTypeGroup(group, true);
        PrismIdentifiableTypeGroups.Add(group, newPlort);
        return newPlort;
    }


    
    
    public static PrismPediaEntry GetPrismPediaEntry(this PediaEntry customOrNativePedia)
    {
        if (customOrNativePedia == null) return null;
        if (customOrNativePedia.TryCast<FixedPediaEntry>()!=null)
        {
            var pedia = customOrNativePedia.Cast<FixedPediaEntry>();
            if (PrismFixedPediaEntries.TryGetValue(pedia, out var entry)) return entry;
            var newEntry = new PrismFixedPediaEntry(pedia, true);
            PrismFixedPediaEntries.Add(pedia, newEntry);
            return newEntry;
        }
        else if (customOrNativePedia.TryCast<IdentifiablePediaEntry>()!=null)
        {
            var pedia = customOrNativePedia.Cast<IdentifiablePediaEntry>();
            if (PrismIdentifiablePediaEntries.TryGetValue(pedia, out var entry)) return entry;
            var newEntry = new PrismIdentifiablePediaEntry(pedia, true);
            PrismIdentifiablePediaEntries.Add(pedia, newEntry);
            return newEntry;
        }
        else
        {
            if (PrismUnknownPediaEntries.TryGetValue(customOrNativePedia, out var entry)) return entry;
            var newEntry = new PrismPediaEntry(customOrNativePedia, true);
            PrismUnknownPediaEntries.Add(customOrNativePedia, newEntry);
            return newEntry;
        }
    }
    public static PrismFixedPediaEntry GetPrismFixedPediaEntry(this FixedPediaEntry customOrNativePedia)
    {
        if (customOrNativePedia == null) return null;
        if (PrismFixedPediaEntries.TryGetValue(customOrNativePedia, out var entry)) return entry;
        var newPedia = new PrismFixedPediaEntry(customOrNativePedia, true);
        PrismFixedPediaEntries.Add(customOrNativePedia, newPedia);
        return newPedia;
    }
    public static PrismIdentifiablePediaEntry GetPrismIdentifiablePediaEntry(this IdentifiablePediaEntry customOrNativePedia)
    {
        if (customOrNativePedia == null) return null;
        if (PrismIdentifiablePediaEntries.TryGetValue(customOrNativePedia, out var entry)) return entry;
        var newPedia = new PrismIdentifiablePediaEntry(customOrNativePedia, true);
        PrismIdentifiablePediaEntries.Add(customOrNativePedia, newPedia);
        return newPedia;
    }
}