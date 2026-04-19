using Il2CppMonomiPark.SlimeRancher.Slime;
using Il2CppMonomiPark.SlimeRancher.UI;

namespace Starlight.Commands.Dev;

public class AppearanceCommand: StarlightCommand
{
    public override string ID => "appearance";
    public override string Usage => "appearance";
    public override CommandType type => CommandType.Miscellaneous | CommandType.Cheat;

    public override bool Execute(string[] args)
    {
        if (!args.IsBetween(0,0)) return SendNoArguments();
        if (!inGame) return SendLoadASaveFirst();

        var cam = MiscEUtil.GetActiveCamera(); if (!cam) return SendNoCamera();


        if (Physics.Raycast(new Ray(cam.transform.position, cam.transform.forward), out var hit,Mathf.Infinity,MiscEUtil.defaultMask))
        {
            var applicator = hit.transform.GetComponent<SlimeAppearanceApplicator>();
            var actor = hit.transform.GetComponent<IdentifiableActor>();
            if (applicator&&actor&&actor.identType.TryCast<SlimeDefinition>())
            {
                var currentIndex = 0;
                var slimeRadiant = hit.transform.GetComponent<SlimeRadiant>();
                var def = actor.identType.Cast<SlimeDefinition>();
                for (int i = 0; i < def.AppearancesDefault.Count; i++)
                    if (def.AppearancesDefault[i] == applicator.Appearance)
                    {
                        currentIndex = i;
                        break;
                    }

                currentIndex++;
                if (currentIndex >= def.AppearancesDefault.Count) currentIndex = 0;
                var newAppearance = def.AppearancesDefault[currentIndex];
                if(slimeRadiant)
                {
                    if (newAppearance.name.Contains("Radiant"))
                    {
                        slimeRadiant.SetRadiant();
                        slimeRadiant.SetRadiantAppearance();
                    }
                    else
                    {
                        slimeRadiant._slimeModel.RadiantBaseType = null;
                    }
                }
                applicator.Appearance = newAppearance;
                applicator.ApplyAppearance();
                applicator.HandleChosenAppearanceChanged(def,newAppearance);
                var animator = hit.transform.GetObjectRecursively<Animator>("Appearance");
                if (animator)
                {
                    animator.enabled = false;
                    animator.enabled = true;
                }

                foreach (var ui in GetAllInScene<TargetingUI>())
                {
                    try
                    {
                        ui._currentTarget = null;
                        ui.Update();
                    } catch {}
                }
                SendMessage(translation("cmd.appearance.success"));
                return true;

            }
            return SendNotLookingAtValidObject();
        }
        return SendNotLookingAtAnything();
    }
}