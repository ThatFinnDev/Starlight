using System.Collections.Generic;
using Il2CppTMPro;
using UnityEngine;

namespace Starlight.UI.Blueprints;

public class DropdownUIBlueprint : UIBlueprint
{
    public List<string> Options;

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var dropdown = obj.AddComponent<TMP_Dropdown>();

        dropdown.AddOptions(Options.ToIl2CppList());

    }
}
