using Il2CppMonomiPark.SlimeRancher.Player;
using Unity.Mathematics;

namespace Starlight.Commands;

internal class GiveCommand : StarlightCommand
{
    public override string ID => "give";
    public override string Usage => "give <item> [amount] [radiant(true/false)] [allowOverflow(true/false)]";
    public override CommandType type => CommandType.Cheat;

    public override List<string> GetAutoComplete(int argIndex, string[] args)
    {
        if (argIndex == 0)
            return LookupEUtil.GetVaccableStringListByPartialName(args == null ? null : args[0], true,MAX_AUTOCOMPLETE.Get());
        if (argIndex == 1)
            return new List<string> { "1", "5", "10", "20", "30", "50" };
        if (argIndex == 2)
            return new List<string> { "true", "false" };
        if (argIndex == 3)
            return new List<string> { "true", "false" };

        return null;
    }

    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(1,4)) return SendUsage();
        if (!inGame) return SendLoadASaveFirst();

        string identifierTypeName = args[0];
        var ident = LookupEUtil.GetIdentifiableTypeByName(identifierTypeName);
        if (!ident) return SendNotValidIdentType(identifierTypeName);
        string itemName = ident.GetName();
        if (ident.IsGadget()) return SendIsGadgetNotItem(itemName);
        
        bool allowOverflow = false;
        bool makeRadiant = false;
        int amount = 1;
        if (args.Length >= 2) if(!TryParseInt(args[1], out amount,1, true)) return false;
        if (args.Length >= 3) if (!TryParseBool(args[2], out makeRadiant)) return false;
        if (args.Length >= 4) if (!TryParseBool(args[3], out allowOverflow)) return false;

        bool isSlime = ident.TryCast<SlimeDefinition>();

        var slotID = -1;
        var i = -1;
        foreach (var slot in sceneContext.PlayerState.Ammo.Slots)
        {
            i++;
            if (!slot.IsUnlocked) continue;
            if (!slot.Id) continue;
            if (slot.Id.ReferenceId != ident.ReferenceId) continue;
            if (isSlime)
                if (slot.Radiant != makeRadiant) continue;
            slotID = i;
            break;
        }
        if (slotID == -1)
        {
            i = -1;
            foreach (var slot in sceneContext.PlayerState.Ammo.Slots)
            {
                i++;
                if (!slot.IsUnlocked) continue;
                if (slot.Id) continue;
                if (!slot.Definition.IsAllowed(ident)) continue;
                slotID = i;
                break;
            }
        }
        if (slotID == -1)
            return SendError(translation("cmd.give.nospace"));
        
        var success = sceneContext.PlayerState.Ammo.MaybeAddResource(ident, slotID, amount, allowOverflow);
        var invSlot = sceneContext.PlayerState.Ammo.Slots[slotID];
        if (!success && !allowOverflow)
        {
            invSlot.Count = invSlot.MaxCount;
        }
        invSlot.Radiant = makeRadiant;
        if(makeRadiant && isSlime)
        {
            //Refresh the appearance in the slot
            var count = invSlot.Count;
            invSlot._count++;
            invSlot.Count = count;
            ExecuteInTicks(() =>
            {
                var execGetter = invSlot.Count;
            },1);
        }
        invSlot.Metadata.Radiant = makeRadiant;

        SendMessage(translation("cmd.give.success",amount,itemName));
        return true;
    }
}
