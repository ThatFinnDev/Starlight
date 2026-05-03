using Starlight.Managers;

namespace Starlight.Commands;

internal class LandPlotRemoveCommand : StarlightCommand
{
    public override string ID => "landplotremove";
    public override string Usage => "landplotremove <name>";
    public override CommandType type => CommandType.LandPlot | CommandType.Cheat;

    public override List<string> GetAutoComplete(int argIndex, string[] args)
    {
        if (argIndex == 0)
            try { if (inGame) return StarlightSaveManager.inGameData.CustomPlots.Keys.ToNetList(); } catch  { }

        return null;
    }

    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(0,1)) return SendUsage();
        if (!inGame) return SendLoadASaveFirst();

        if (args==null)
        {
            Camera cam = MiscEUtil.GetActiveCamera(); if (!cam) return SendNoCamera(); 
            if (Physics.Raycast(new Ray(cam.transform.position, cam.transform.forward), out var hit,Mathf.Infinity,MiscEUtil.defaultMask))
            {
                var obj = hit.transform.gameObject;
                LandPlotLocation loc = null;
                while (true)
                {
                    loc = obj.GetComponent<LandPlotLocation>();
                    if (loc != null) break;
                    if (obj.transform.parent == null) break;
                    obj = obj.transform.parent.gameObject;
                }

                if (loc == null) return SendNotLookingAtValidObject();
                var _id = loc._id.Substring(loc.IdPrefix().Length);
                SendMessage(_id);
                if (!StarlightSaveManager.inGameData.CustomPlots.ContainsKey(_id)) return SendErrorTr("cmd.dellandplot.native");
                StarlightSaveManager.inGameData.CustomPlots.Remove(_id);
                SpawnEUtil.RemoveCustomLandPlot(_id);
                SendMessageTr("cmd.dellandplot.success");
                return true;
            }
            return SendNotLookingAtAnything();
        }

        var id = "starlightown." + args[0];
        if (!StarlightSaveManager.inGameData.CustomPlots.ContainsKey(id)) return SendErrorTr("cmd.dellandplot.no", args[0]); 
        StarlightSaveManager.inGameData.CustomPlots.Remove(id);
        if(!SpawnEUtil.HasCustomLandPlot(id)) return SendErrorTr("cmd.dellandplot.no",args[0]);
        SpawnEUtil.RemoveCustomLandPlot(id);
        SendMessageTr("cmd.dellandplot.success");
        return true;
    }
}