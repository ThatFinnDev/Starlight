using System;

namespace Starlight.Commands;

internal class KillAllCommand : StarlightCommand
{
    public override string ID => "killall";
    public override string Usage => "killall [id]";
    public override CommandType type => CommandType.Cheat;

    public override List<string> GetAutoComplete(int argIndex, string[] args)
    {
        if (argIndex == 0)
            return LookupEUtil.GetStrongFilteredIdentifiableTypeStringListByPartialName(args == null ? null : args[0], true, MAX_AUTOCOMPLETE.Get(),true);
        return null;
    }
    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(1,1)) return SendUsage();
        if (!inGame) return SendLoadASaveFirst();

        bool killall = false;
        if(args[0]=="*") killall = true;
        if (killall)
        {
            foreach (var ident in Resources.FindObjectsOfTypeAll<IdentifiableActor>())
                if (ident.hasStarted)
                {
                    var id = ident._model.actorId;
                    if (ident.identType.name != "Player")
                    {
                        Object.Destroy(ident.gameObject);
                        sceneContext.GameModel.identifiables.Remove(id);
                    }
                }
            SendMessage(Tr("cmd.killall.success"));
            return true;
        }
        string identifierTypeName = args[0];
        var type = LookupEUtil.GetIdentifiableTypeByName(identifierTypeName);
        if (!type) return SendNotValidIdentType(identifierTypeName);
        if (type.IsGadget()) return SendIsGadgetNotItem(type.GetName());
                
        foreach (var ident in Resources.FindObjectsOfTypeAll<IdentifiableActor>())
            if (ident.hasStarted)
                if (ident.identType == type)
                {
                    var id = ident._model.actorId;
                    Object.Destroy(ident.gameObject);
                    sceneContext.GameModel.identifiables.Remove(id);
                }
        SendMessage(Tr("cmd.killall.successspecific",type.GetName()));
        return true;
    }
}
