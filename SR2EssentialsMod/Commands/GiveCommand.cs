using Il2CppMonomiPark.SlimeRancher.Player;
using Unity.Mathematics;

namespace SR2E.Commands;

internal class GiveCommand : SR2ECommand
{
    public override string ID => "give";
    public override string Usage => "give <item> [amount] [allowOverflow(true/false)";
    public override CommandType type => CommandType.Cheat;

    public override List<string> GetAutoComplete(int argIndex, string[] args)
    {
        if (argIndex == 0)
            return LookupEUtil.GetVaccableStringListByPartialName(args == null ? null : args[0], true,MAX_AUTOCOMPLETE.Get());
        if (argIndex == 1)
            return new List<string> { "1", "5", "10", "20", "30", "50" };

        return null;
    }

    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(1,3)) return SendUsage();
        if (!inGame) return SendLoadASaveFirst();

        string identifierTypeName = args[0];
        var type = LookupEUtil.GetIdentifiableTypeByName(identifierTypeName);
        if (type == null) return SendNotValidIdentType(identifierTypeName);
        string itemName = type.GetName();
        if (type.isGadget()) return SendIsGadgetNotItem(itemName);
        
        bool allowOverflow = false;
        int amount = 1;
        if (args.Length >= 2) if(!TryParseInt(args[1], out amount,1, true)) return false;
        if (args.Length >= 3) if (!TryParseBool(args[2], out allowOverflow)) return false;


        var slotID = -1;
        var i = -1;
        foreach (var slot in sceneContext.PlayerState.Ammo.Slots)
        {
            i++;
            if (!slot.IsUnlocked) continue;
            if (slot.Id!=null&&slot.Id.ReferenceId != type.ReferenceId) continue;
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
                if (slot.Count==0) continue;
                slotID = i;
                break;
            }
        }
        if (slotID == -1)
            return SendError(translation("cmd.give.nospace"));

        if (type is SlimeDefinition)
        {
            var data = new AmmoSlot.AmmoMetadata();
            data.Id = type; 
            data.Emotions = new float4();
            var success = sceneContext.PlayerState.Ammo.MaybeAddToSpecificSlot(data, slotID, amount, allowOverflow);
            if (!success && !allowOverflow)
            {
                var invSlot = sceneContext.PlayerState.Ammo.Slots[slotID];
                invSlot.Clear();
                sceneContext.PlayerState.Ammo.MaybeAddToSpecificSlot(data, slotID, invSlot.MaxCount, allowOverflow);
            }
        }
        else
        {
            var success = sceneContext.PlayerState.Ammo.MaybeAddResource(type, slotID, amount, allowOverflow);
            if (!success && !allowOverflow)
            {
                var invSlot = sceneContext.PlayerState.Ammo.Slots[slotID];
                invSlot.Count = invSlot.MaxCount;
            }
        }
       

        SendMessage(translation("cmd.give.success",amount,itemName));
        return true;
    }
}
