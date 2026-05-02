using System.Collections.Generic;
using Il2CppTMPro;
using UnityEngine;

namespace Starlight.UI.Blueprints;

internal class DropdownUIBlueprintV01 : UIBlueprint
{
    public List<string> Options;

    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        var dropdown = obj.AddComponent<TMP_Dropdown>();

        dropdown.AddOptions(Options.ToIl2CppList());

    }
}
