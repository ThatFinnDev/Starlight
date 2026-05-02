using Starlight.Prism.Data;
using Starlight.Prism.Lib;
using Starlight.Prism.Wrappers;
using UnityEngine.Localization;

namespace Starlight.Prism.Creators;

public class PrismPlortCreatorV01
{
    private PrismPlort _createdPlort;

    public string Name;
    public Sprite Icon;
    public LocalizedString Localized;
    private string referenceID => "IdentifiableType.Modded" + Name + "Plort";

    public Color32 VacColor = new Color32(0,0,0,255);
    public GameObject CustomBasePrefab = null;


    public PrismMarketData? ModdedMarketData = null;
    
    
    public PrismPlortCreatorV01(string name, Sprite icon, LocalizedString localized)
    {
        this.Name = name;
        this.Icon = icon;
        this.Localized = localized;
    }
    
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Name)) return false;
        for (int i = 0; i < Name.Length; i++)
            if (!((Name[i] >= 'A' && Name[i] <= 'Z') || (Name[i] >= 'a' && Name[i] <= 'z')))
                return false;
        if (Localized==null) return false;
        if (CustomBasePrefab != null)
        {
            if (!CustomBasePrefab.HasComponent<IdentifiableActor>()) return false;
        }
        return true;
    }

    public PrismPlort CreatePlort()
    {
        if (!IsValid()) return null;
        if (_createdPlort != null) return _createdPlort;
        var plort = ScriptableObject.CreateInstance<IdentifiableType>();
        plort.hideFlags = HideFlags.DontUnloadUnusedAsset;
        plort.name = Name + "Plort";
        plort.color = VacColor;
        plort.icon = Icon;
        plort.icon ??= PrismShortcuts.UnavailableIcon;
        plort.IsPlort = true;
        
        
        plort.localizedName = Localized;
        plort._pediaPersistenceSuffix = "modded"+Name.ToLower()+"_plort";
        
        if(ModdedMarketData.HasValue)
            PrismLibMarket.MakeSellable(plort, ModdedMarketData.Value);
        plort.Prism_AddToGroup("PlortGroup");
        plort.Prism_AddToGroup("EdiblePlortFoodGroup");
        plort.Prism_AddToGroup("PlortGroupDroneExplorer");
        plort.Prism_AddToGroup("IdentifiableTypesGroup");
        
        var basePrefab = CustomBasePrefab;
        if (basePrefab == null) basePrefab = PrismNativePlort.Pink.GetPrismPlort().GetPrefab();
        plort.prefab = CreatePrefab("plort"+Name, basePrefab);
        plort.prefab.GetComponent<IdentifiableActor>().identType = plort;
        
        PrismLibSaving.SetupForSaving(plort,referenceID);
        
        var prismPlort = new PrismPlort(plort, false);
        
        
        _createdPlort = prismPlort;
        PrismShortcuts.PrismPlorts.Add(plort.ReferenceId,prismPlort);
        return _createdPlort;
    }
}


