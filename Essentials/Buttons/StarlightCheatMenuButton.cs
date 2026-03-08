using Il2CppTMPro;
using Starlight.Menus;
using UnityEngine.UI;

namespace Starlight.Buttons;

public class StarlightCheatMenuButton
{
    public string label;
    public SystemAction action;
    public Button buttonInstance;
    public TextMeshProUGUI textInstance;
    public StarlightCheatMenuButton(string label, SystemAction action)
    {
        this.label = label;
        this.action = action;

        foreach (StarlightCheatMenuButton entry in StarlightCheatMenu.cheatButtons)
            if (entry.label == this.label) { MelonLogger.Error($"There is already a button with the name {this.label}"); return; }

        StarlightCheatMenu.cheatButtons.Add(this);
    }
}