using Il2CppMonomiPark.SlimeRancher;

namespace Starlight.Prism.Lib;
/// <summary>
/// A library of helper functions for dealing with saving and loading
/// </summary>
public class PrismLibSaving
{
    internal static readonly Dictionary<string, IdentifiableType> SavedIdents = new ();
    
    
    /// <summary>
    /// Setups an identifiable type for saving
    /// </summary>
    /// <param name="ident">The identifiable type to setup</param>
    /// <param name="refID">The reference ID to use for saving</param>
    public static void SetupForSaving(IdentifiableType ident, string refID = null)
    {
        if (ident == null) return;
        if (string.IsNullOrWhiteSpace(refID)) refID = ident.ReferenceId;
        SavedIdents.TryAdd(refID, ident);

        if(!autoSaveDirector._configuration._identifiableTypes.IsMember(ident))
            gameContext.LookupDirector.AddIdentifiableTypeToGroup(ident, autoSaveDirector._configuration._identifiableTypes);
        gameContext.LookupDirector._identifiableTypeByRefId.TryAdd(refID, ident);
        SetupSaveForIdent(refID, ident);
    }

    private static void SetupSaveForIdent(string refID, IdentifiableType ident)
    {
        var t = autoSaveDirector._saveReferenceTranslation;
        t._identifiableTypeLookup.TryAdd(refID, ident);

        if (t._identifiableTypeToPersistenceId._primaryIndex.Count > 0)
            if (!t._identifiableTypeToPersistenceId._primaryIndex.Contains(refID))
                t._identifiableTypeToPersistenceId._primaryIndex =
                    t._identifiableTypeToPersistenceId._primaryIndex.AddToNew(refID);

        t._identifiableTypeToPersistenceId._reverseIndex.TryAdd(refID,
            t._identifiableTypeToPersistenceId._reverseIndex.Count);

        if (ident.TryCast<SlimeDefinition>())
        {
            gameContext.SlimeDefinitions._slimeDefinitionsByIdentifiable.TryAdd(ident, ident.Cast<SlimeDefinition>());
            if (!gameContext.SlimeDefinitions.Slimes.Contains(ident.Cast<SlimeDefinition>()))
                gameContext.SlimeDefinitions.Slimes = gameContext.SlimeDefinitions.Slimes.AddToNew(ident.Cast<SlimeDefinition>());
        }

        ident.referenceId = refID;
    }



    internal static void RefreshIfNotFound(SaveReferenceTranslation table, IdentifiableType ident)
    {
        try
        {
            table.GetPersistenceId(ident);
        }
        catch
        {
            foreach (var refresh in SavedIdents)
                SetupSaveForIdent(refresh.Key, refresh.Value);
        }
    }
}