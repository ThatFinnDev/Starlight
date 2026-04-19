/*namespace SR2E.Commands;

public class AppearanceCommand: SR2ECommand
{
    public override string ID => "appearance";
    public override string Usage => "appearance";
    public override CommandType type => CommandType.Miscellaneous | CommandType.Cheat;

    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(0,0)) return SendNoArguments();
        if (!inGame) return SendLoadASaveFirst();

        Camera cam = MiscEUtil.GetActiveCamera(); if (!cam) return SendNoCamera();
            

        if (Physics.Raycast(new Ray(cam.transform.position, cam.transform.forward), out var hit,Mathf.Infinity,MiscEUtil.defaultMask))
        {
            var applicator = hit.transform.GetComponent<SlimeAppearanceApplicator>();
            var actor = hit.transform.GetComponent<IdentifiableActor>();
            if (applicator&&actor&&actor.identType.TryCast<SlimeDefinition>())
            {
                var currentIndex = 0;
                var def = actor.identType.Cast<SlimeDefinition>();
                for (int i = 0; i < def.AppearancesDefault.Count; i++)
                    if (def.AppearancesDefault[i] == applicator.Appearance)
                    {
                        currentIndex = i;
                        break;
                    }

                currentIndex++;
                if (currentIndex >= def.AppearancesDefault.Count) currentIndex = 0;
                applicator.Appearance = def.AppearancesDefault[currentIndex];
                applicator.ApplyAppearance();
                SendMessage(translation("cmd.appearance.success"));
                return true;

            }
            return SendNotLookingAtValidObject();
        }
        return SendNotLookingAtAnything();
    }
}*/