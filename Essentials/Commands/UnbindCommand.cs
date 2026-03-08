using Starlight.Enums;
using Starlight.Managers;
using UnityEngine.InputSystem;

namespace Starlight.Commands;
internal class UnbindCommand : StarlightCommand
{
    public override string ID => "unbind";
    public override string Usage => "unbind <key>";
    public override CommandType type => CommandType.Binding;
    
    public override List<string> GetAutoComplete(int argIndex, string[] args)
    {
        if (argIndex == 0) return LookupEUtil.GetLKeyStringListByPartialName(args[0],true,MAX_AUTOCOMPLETE.Get());
        return null;
    }

    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(1,1)) return SendUsage();

        LKey key;
        if (!TryParseLKey(args[0], out key)) return false;
        
        if (!StarlightBindingManger.isKeyBound(key)) return SendError(translation("cmd.unbind.notbound", args[0]));
        StarlightBindingManger.UnbindKey(key);
        SendMessage(translation("cmd.unbind.success", key));
        return true;
    }
}