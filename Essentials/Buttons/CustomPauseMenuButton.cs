using Starlight.Patches.InGame;
using UnityEngine.Localization;

namespace Starlight.Buttons;

public class CustomPauseMenuButton
{
    public LocalizedString label;
    public int insertIndex;
    internal CustomPauseItemModel _model;
    public SystemAction action;
    public bool enabled = true;
    public CustomPauseMenuButton(LocalizedString label, int insertIndex, SystemAction action)
    {
        this.label = label; ;
        this.insertIndex = insertIndex;
        this.action = action;

        foreach (CustomPauseMenuButton entry in SR2PauseMenuButtonPatch.buttons)
            if (entry.label == this.label) { MelonLogger.Error($"There is already a button with the name {this.label}"); return; }

        SR2PauseMenuButtonPatch.buttons.Add(this);
    }

    public void Remove()
    {
        enabled = false;
    }
    public void AddAgain()
    {
        enabled = true;
    }
}
