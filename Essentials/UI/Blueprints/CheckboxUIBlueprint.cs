using UnityEngine;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class CheckboxUIBlueprint : UIBlueprint
{
    public bool IsOn;

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var backgroundImage = obj.AddComponent<Image>();
        backgroundImage.color = theme.SecondaryColor;

        var checkmarkObj = new GameObject("Checkmark");
        checkmarkObj.transform.SetParent(obj);
        var checkmarkImage = checkmarkObj.AddComponent<Image>();
        checkmarkImage.color = theme.AccentColor;
        checkmarkObj.SetActive(IsOn);

        var toggle = obj.AddComponent<Toggle>();
        toggle.targetGraphic = backgroundImage;
        toggle.graphic = checkmarkImage;
        toggle.isOn = IsOn;
        toggle.onValueChanged.AddListener((System.Action<bool>)(value => { checkmarkObj.SetActive(value); }));
    }
}
