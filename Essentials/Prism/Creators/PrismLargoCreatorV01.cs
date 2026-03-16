using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppMonomiPark.SlimeRancher.Damage;
using Il2CppMonomiPark.SlimeRancher.Slime;
using Starlight.Prism.Data;
using Starlight.Prism.Lib;
using Starlight.Prism.Wrappers;
using UnityEngine.Localization;


namespace Starlight.Prism.Creators;

public class PrismLargoCreatorV01
{
    private PrismLargo _createdLargo;

    public PrismBaseSlime FirstSlime;
    public PrismBaseSlime SecondSlime;
    
    public PrismLargoMergeSettings LargoMergeSettings = new ();
    private string name => FirstSlime.SlimeDefinition.Name + SecondSlime.SlimeDefinition.Name;
    private string referenceID => "SlimeDefinition.Modded" + name;


    public LocalizedString CustomLocalized;
    public GameObject CustomBasePrefab = null;


    public PrismLargoCreatorV01(PrismNativeBaseSlime firstSlime, PrismNativeBaseSlime secondSlime)
    {
        this.FirstSlime = firstSlime;
        this.SecondSlime = secondSlime;
    }
    public PrismLargoCreatorV01(PrismNativeBaseSlime firstSlime, PrismBaseSlime secondSlime)
    {
        this.FirstSlime = firstSlime;
        this.SecondSlime = secondSlime;
    }
    public PrismLargoCreatorV01(PrismBaseSlime firstSlime, PrismBaseSlime secondSlime)
    {
        this.FirstSlime = firstSlime;
        this.SecondSlime = secondSlime;
    }
    public PrismLargoCreatorV01(PrismBaseSlime firstSlime, PrismNativeBaseSlime secondSlime)
    {
        this.FirstSlime = firstSlime;
        this.SecondSlime = secondSlime;
    }
    
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        foreach (var t in name)
            if (!((t >= 'A' && t <= 'Z') || (t >= 'a' && t <= 'z')))
                return false;

        if (FirstSlime == null | SecondSlime == null) return false;
        if (CustomBasePrefab == null) return true;
        if (!CustomBasePrefab.HasComponent<SlimeAppearanceApplicator>()) return false;
        if (!CustomBasePrefab.HasComponent<IdentifiableActor>()) return false;
        return true;
    }
    
    
    
    
    
    public PrismLargo CreateLargo()
    {
        if (!IsValid()) return null;
        if (_createdLargo != null) return _createdLargo;


        if (FirstSlime.DoesLargoComboExist(SecondSlime)) return null;

        if (LargoMergeSettings == null) LargoMergeSettings = new PrismLargoMergeSettings();
        var firstSlimeDef = FirstSlime.GetSlimeDefinition();
        var secondSlimeDef = SecondSlime.GetSlimeDefinition();

        if (firstSlimeDef.IsLargo || secondSlimeDef.IsLargo)
            return null;

        firstSlimeDef.CanLargofy = true;
        secondSlimeDef.CanLargofy = true;

        SlimeDefinition baseLargo;
        if(firstSlimeDef.ReferenceId=="SlimeDefinition.Boom"||secondSlimeDef.referenceId=="SlimeDefinition.Boom") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkBoom");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Phosphor"||secondSlimeDef.referenceId=="SlimeDefinition.Phosphor") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkPhosphor");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Hyper"||secondSlimeDef.referenceId=="SlimeDefinition.Hyper") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.HyperPink");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Sloomber"||secondSlimeDef.referenceId=="SlimeDefinition.Sloomber") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkSloomber");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Tabby"||secondSlimeDef.referenceId=="SlimeDefinition.Tabby") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkTabby");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Crystal"||secondSlimeDef.referenceId=="SlimeDefinition.Crystal") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkCrystal");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Saber"||secondSlimeDef.referenceId=="SlimeDefinition.Saber") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.SaberPink");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Honey"||secondSlimeDef.referenceId=="SlimeDefinition.Honey") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkHoney");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Dervish"||secondSlimeDef.referenceId=="SlimeDefinition.Dervish") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkDervish");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Hunter"||secondSlimeDef.referenceId=="SlimeDefinition.Hunter") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkHunter");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Angler"||secondSlimeDef.referenceId=="SlimeDefinition.Angler") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.AnglerPink");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Dervish"||secondSlimeDef.referenceId=="SlimeDefinition.Dervish") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkDervish");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Cotton"||secondSlimeDef.referenceId=="SlimeDefinition.Cotton") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.CottonPink");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Batty"||secondSlimeDef.referenceId=="SlimeDefinition.Batty") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.BattyPink");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Flutter"||secondSlimeDef.referenceId=="SlimeDefinition.Flutter") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.FlutterPink");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Twin"||secondSlimeDef.referenceId=="SlimeDefinition.Twin") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkTwin");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Tangle"||secondSlimeDef.referenceId=="SlimeDefinition.Tangle") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkTangle");
        
        else if(firstSlimeDef.ReferenceId=="SlimeDefinition.Ringtail"||secondSlimeDef.referenceId=="SlimeDefinition.Ringtail") 
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.RingtailPink");
        
        else
            baseLargo=LookupEUtil.largoTypes.GetEntryByRefID("SlimeDefinition.PinkRock");
        
        var largoDef = Object.Instantiate(baseLargo);
        largoDef.BaseSlimes = new[]
        {
            firstSlimeDef, secondSlimeDef
        };
        largoDef.SlimeModules = new[]
        {
            firstSlimeDef.SlimeModules[0],secondSlimeDef.SlimeModules[0]
        };

        largoDef._pediaPersistenceSuffix = "modded"+firstSlimeDef.name.ToLower() + "_" + secondSlimeDef.name.ToLower() + "_largo";

        largoDef.referenceId = referenceID;

        if (CustomLocalized != null)
            largoDef.localizedName = CustomLocalized;
        else
            largoDef.localizedName = AddTranslation(firstSlimeDef.name + " " + secondSlimeDef.name + " Largo",
                "l." + largoDef._pediaPersistenceSuffix);


        largoDef.FavoriteToyIdents = new Il2CppReferenceArray<ToyDefinition>(PrismLibMerging.MergeFavoriteToys(FirstSlime, SecondSlime));

        largoDef.hideFlags = HideFlags.DontUnloadUnusedAsset;
        largoDef.Name = name;
        largoDef.name = name;
        largoDef.prefab = Object.Instantiate(baseLargo.prefab, PrefabHolder.transform);
        largoDef.prefab.name = $"slime{name}";
        largoDef.prefab.GetComponent<Identifiable>().identType = largoDef;
        largoDef.prefab.GetComponent<SlimeEat>().SlimeDefinition = largoDef;
        largoDef.prefab.GetComponent<SlimeAppearanceApplicator>().SlimeDefinition = largoDef;
        largoDef.prefab.GetComponent<PlayWithToys>().SlimeDefinition = largoDef;
        largoDef.prefab.GetComponent<ReactToToyNearby>().SlimeDefinition = largoDef;
        if(!firstSlimeDef.prefab.HasComponent<RockSlimeRoll>()&&!secondSlimeDef.prefab.HasComponent<RockSlimeRoll>())
            largoDef.prefab.RemoveComponent<RockSlimeRoll>();
        if(!firstSlimeDef.prefab.HasComponent<DamagePlayerOnTouch>()&&!secondSlimeDef.prefab.HasComponent<DamagePlayerOnTouch>())
            largoDef.prefab.RemoveComponent<DamagePlayerOnTouch>();

        SlimeAppearance appearance = Object.Instantiate(baseLargo.AppearancesDefault[0]);
        largoDef.AppearancesDefault[0] = appearance;
        appearance.hideFlags = HideFlags.DontUnloadUnusedAsset;
        appearance.name = firstSlimeDef.AppearancesDefault[0].name + secondSlimeDef.AppearancesDefault[0].name;

        appearance._dependentAppearances = new[]
        {
            firstSlimeDef.AppearancesDefault[0], secondSlimeDef.AppearancesDefault[0]
        };

        bool firstFace = PrismLibMerging.ShouldUseFirstStructure(LargoMergeSettings.Face, !PrismLibMerging.GetLargoHasDefaultFace(FirstSlime.GetSlimeAppearance()));
        if (firstFace)
            appearance._face = Object.Instantiate(FirstSlime.GetSlimeAppearance()._face);
        else appearance._face = Object.Instantiate(SecondSlime.GetSlimeAppearance()._face);
        var optimalPriortization = PrismLibMerging.GetOptimalV01(firstSlimeDef,secondSlimeDef);
        if (LargoMergeSettings.BaseColors==PrismColorMergeStrategy.Merge||
            (LargoMergeSettings.BaseColors == PrismColorMergeStrategy.Optimal && optimalPriortization==PrismThreeMergeStrategy.Merge))
        {
            var firstPalette = FirstSlime.GetSlimeAppearance()._colorPalette;
            var secondPalette = SecondSlime.GetSlimeAppearance()._colorPalette;
            appearance._splatColor = Color.Lerp(FirstSlime.GetSlimeAppearance()._splatColor, SecondSlime.GetSlimeAppearance()._splatColor, 0.5f);
            largoDef.color = Color.Lerp(firstSlimeDef.color, secondSlimeDef.color, 0.5f);
            appearance._colorPalette = new SlimeAppearance.Palette()
            {
                Ammo = Color.Lerp(firstPalette.Ammo, secondPalette.Ammo, 0.5f),
                Bottom = Color.Lerp(firstPalette.Bottom, secondPalette.Bottom, 0.5f),
                Middle = Color.Lerp(firstPalette.Middle, secondPalette.Middle, 0.5f),
                Top = Color.Lerp(firstPalette.Top, secondPalette.Top, 0.5f),
            };
        }
        else
        {
            SlimeAppearance prioritizedAppearance = null;
            switch (optimalPriortization)
            {
                case PrismThreeMergeStrategy.PrioritizeSecond :
                    prioritizedAppearance = SecondSlime.GetSlimeAppearance();
                    largoDef.color = secondSlimeDef.color;
                    break;
                default:
                    prioritizedAppearance = FirstSlime.GetSlimeAppearance();
                    largoDef.color = firstSlimeDef.color;
                    break;
            }
            appearance._splatColor = prioritizedAppearance._splatColor;
            appearance._colorPalette = new SlimeAppearance.Palette()
            {
                Ammo = prioritizedAppearance._colorPalette.Ammo,
                Bottom = prioritizedAppearance._colorPalette.Bottom, 
                Middle = prioritizedAppearance._colorPalette.Middle,
                Top = prioritizedAppearance._colorPalette.Top,
            };
        }
        appearance._structures = PrismLibMerging.MergeStructuresV01(appearance._dependentAppearances[0],
            appearance._dependentAppearances[1], LargoMergeSettings,optimalPriortization);

        try
        {
            largoDef.Diet = PrismLibMerging.MergeDiet(firstSlimeDef.Diet, secondSlimeDef.Diet);
        }
        catch
        {
            switch (LargoMergeSettings.Body)
            {
                case PrismBfMergeStrategy.KeepSecond:
                    largoDef.Diet = secondSlimeDef.Diet;
                    LogBigError("Largo Error",
                        "Failed to merge diet, and largo settings are incorrectly set! Defaulting to slime 2's diet.");
                    break;
                default:
                    largoDef.Diet = firstSlimeDef.Diet;
                    LogBigError("Largo Error",
                        "Failed to merge diet, and largo settings are incorrectly set! Defaulting to slime 1's diet.");
                    break;
            }
        }

        PrismShortcuts.mainAppearanceDirector.RegisterDependentAppearances(largoDef, largoDef.AppearancesDefault[0]);
        PrismShortcuts.mainAppearanceDirector.UpdateChosenSlimeAppearance(largoDef, largoDef.AppearancesDefault[0]);

        
        PrismLibSaving.SetupForSaving(largoDef,largoDef.referenceId);
        
        //gameContext.SlimeDefinitions.RefreshIndexes();
        gameContext.SlimeDefinitions.RefreshDefinitions();
        
        IdentifiableType firstPlort = null;
        IdentifiableType secondPlort = null; 
        foreach (var pair in gameContext.SlimeDefinitions._largoDefinitionByBasePlorts)
        {
            if(pair.value!=largoDef) continue;
            firstPlort = pair.Key.Plort1;
            secondPlort = pair.Key.Plort2;
            break;
        }
        if(firstPlort!=null&&secondPlort!=null)
        {
            FirstSlime.AddEatmapToSlime(PrismLibDiet.CreateEatmapEntry(SlimeEmotions.Emotion.AGITATION, 0.5f, null, secondPlort, largoDef), true);
            SecondSlime.AddEatmapToSlime(PrismLibDiet.CreateEatmapEntry(SlimeEmotions.Emotion.AGITATION, 0.5f, null, firstPlort, largoDef), true);
            FirstSlime.RefreshEatMap();
            SecondSlime.RefreshEatMap();
        }



        if(LargoMergeSettings.MergeComponents)
            PrismLibMerging.MergeComponentsV01(largoDef.prefab, firstSlimeDef.prefab, secondSlimeDef.prefab);

        
        if (FirstSlime.GetIsNative())
            largoDef.Prism_AddToGroup(firstSlimeDef.Name+"LargoGroup");
        else
        {
            if (LookupEUtil.allIdentifiableTypeGroups.ContainsKey(firstSlimeDef.Name + "ModdedLargoGroup"))
                largoDef.Prism_AddToGroup(firstSlimeDef.Name + "ModdedLargoGroup");
            else
            {
                var creator = new PrismIdentifiableTypeGroupCreatorV01(firstSlimeDef.Name + "ModdedLargoGroup", PrismShortcuts.EmptyTranslation);
                creator.MemberTypes = new List<IdentifiableType>() { largoDef };
                var group = creator.CreateIdentifiableTypeGroup();
                group.AddToGroup("EdibleSlimeGroup");
                group.AddToGroup("LargoGroup");
                if(FirstSlime.IsInImmediateGroup("SlimesSinkInShallowWaterGroup")&&SecondSlime.IsInImmediateGroup("SlimesSinkInShallowWaterGroup"))
                    group.AddToGroup("SlimesSinkInShallowWaterGroup");
            }
        }
        
        if (SecondSlime.GetIsNative())
            largoDef.Prism_AddToGroup(secondSlimeDef.Name+"LargoGroup");
        else
        {
            if (LookupEUtil.allIdentifiableTypeGroups.ContainsKey(secondSlimeDef.Name + "ModdedLargoGroup"))
                largoDef.Prism_AddToGroup(secondSlimeDef.Name + "ModdedLargoGroup");
            else
            {
                var creator = new PrismIdentifiableTypeGroupCreatorV01(secondSlimeDef.Name + "ModdedLargoGroup", PrismShortcuts.EmptyTranslation);
                creator.MemberTypes = new List<IdentifiableType>() { largoDef };
                var group = creator.CreateIdentifiableTypeGroup();
                group.AddToGroup("EdibleSlimeGroup");
                group.AddToGroup("LargoGroup");
                if(FirstSlime.IsInImmediateGroup("SlimesSinkInShallowWaterGroup")&&SecondSlime.IsInImmediateGroup("SlimesSinkInShallowWaterGroup"))
                    group.AddToGroup("SlimesSinkInShallowWaterGroup");
            }
        }
        
        
        var prismLargo = new PrismLargo(largoDef, false);
        prismLargo.RefreshEatMap();
        
        //if(plort!=null)
          //  PrismLibDiet.AddEatProduction(prismLargo, plort);
        
        _createdLargo = prismLargo;
        PrismShortcuts.PrismLargoBases.Add(_createdLargo,(FirstSlime,SecondSlime));
        PrismShortcuts.PrismLargos.Add(largoDef.ReferenceId,_createdLargo);
        return _createdLargo;
    }   
}