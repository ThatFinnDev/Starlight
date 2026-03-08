using Starlight.Enums;
using Starlight.Managers;
using Starlight.Storage;

namespace Starlight.Commands;

internal class DeleteWarpCommand : StarlightCommand
{
    public override string ID => "delwarp";
    public override string Usage => "delwarp <name>";
    public override CommandType type => CommandType.Warp;

    public override List<string> GetAutoComplete(int argIndex, string[] args)
    {
        if (argIndex == 0)
        {
            List<string> warps = new List<string>();
            foreach (KeyValuePair<string, Warp> pair in StarlightSaveManager.data.warps) warps.Add(pair.Key);
            return warps;
        }

        return null;
    }

    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(1,1)) return SendUsage();

        switch (StarlightWarpManager.RemoveWarp(args[0]))
        {
            case StarlightError.DoesntExist: return SendError(translation("cmd.warpstuff.nowarpwithname",args[0]));
            default: SendMessage(translation("cmd.delwarp.success",args[0])); return true;
        }
    }
}
