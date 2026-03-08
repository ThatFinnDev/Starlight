using Starlight.Managers;
using Starlight.Storage;

namespace Starlight.Commands;

internal class WarpListCommand : StarlightCommand
{
    public override string ID => "warplist";
    public override string Usage => "warplist";
    public override CommandType type => CommandType.Warp;

    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(0,0)) return SendNoArguments();

        if (StarlightSaveManager.data.warps.Count == 0) return SendError(translation("cmd.warplist.error"));

        SendMessage(translation("cmd.warplist.success"));
        foreach (KeyValuePair<string, Warp> pair in StarlightSaveManager.data.warps)
            SendMessage(translation("cmd.warplist.successdesc",pair.Key,pair.Value.sceneGroup,pair.Value.x,pair.Value.y,pair.Value.z));
        return true;
    }
}
