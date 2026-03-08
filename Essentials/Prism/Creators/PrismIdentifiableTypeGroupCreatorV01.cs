using Starlight.Prism.Data;
using Starlight.Prism.Wrappers;
using UnityEngine.Localization;

namespace Starlight.Prism.Creators;

public class PrismIdentifiableTypeGroupCreatorV01
{
    
    PrismIdentifiableTypeGroup _createdGroup;
    public string name;
    public LocalizedString localized;
    public List<IdentifiableType> memberTypes;
    public List<IdentifiableTypeGroup> memberGroupes;
    public bool isFood = false;
    public PrismIdentifiableTypeGroupCreatorV01(string name, LocalizedString localized)
    {
        this.name = name;
        this.localized = localized;
    }
    
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        for (int i = 0; i < name.Length; i++)
            if (!((name[i] >= 'A' && name[i] <= 'Z') || (name[i] >= 'a' && name[i] <= 'z')))
                return false;
        return true;
    }

    
    public PrismIdentifiableTypeGroup CreateIdentifiableTypeGroup()
    {
        if (!IsValid()) return null;
        if (_createdGroup != null) return _createdGroup;

        var group = ScriptableObject.CreateInstance<IdentifiableTypeGroup>();
        group.hideFlags = HideFlags.DontUnloadUnusedAsset;

        group._memberTypes = new Il2CppSystem.Collections.Generic.List<IdentifiableType>();
        if(memberTypes!=null)
            foreach (var type in memberTypes)
                group._memberTypes.Add(type);

        group._memberGroups = new Il2CppSystem.Collections.Generic.List<IdentifiableTypeGroup>();
        if(memberGroupes!=null)
            foreach (var subGroup in memberGroupes)
                group._memberGroups.Add(subGroup);

        group._isFood = isFood;
        
        if (localized != null) group._localizedName = localized;
        else group._localizedName = PrismShortcuts.EmptyTranslation;
        group.name = name;

        group.AllowedCategories = new Il2CppSystem.Collections.Generic.List<IdentifiableCategory>();

        group._runtimeObject = new IdentifiableTypeGroupRuntimeObject(group);

            
        if(!LookupEUtil.IdentifiableTypeGroupList.items.Contains(group))
            LookupEUtil.IdentifiableTypeGroupList.items.Add(group);
        gameContext.LookupDirector.RegisterIdentifiableTypeGroup(group);
        if(!gameContext.LookupDirector._allIdentifiableTypeGroups.items.Contains(group))
            gameContext.LookupDirector._allIdentifiableTypeGroups.items.Add(group);

        var prismGroup = new PrismIdentifiableTypeGroup(group, false);
            
        PrismShortcuts.PrismIdentifiableTypeGroups.Add(group,prismGroup);

        _createdGroup = prismGroup;
        return _createdGroup;
    }
}