using Il2CppTMPro;
using Starlight.Menus;
using UnityEngine.UI;

namespace Starlight.Buttons;

public class StarlightCheatMenuButton
{
    public string Label;
    public SystemAction Action;
    public Button ButtonInstance;
    public TextMeshProUGUI TextInstance;
    public StarlightCheatMenuButton(string label, SystemAction action)
    {
        this.Label = label;
        this.Action = action;

        foreach (StarlightCheatMenuButton entry in StarlightCheatMenu.cheatButtons)
            if (entry.Label == this.Label) { LogError($"There is already a button with the name {this.Label}"); return; }

        StarlightCheatMenu.cheatButtons.Add(this);
    }
}