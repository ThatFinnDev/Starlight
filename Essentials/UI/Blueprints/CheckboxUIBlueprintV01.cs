using Starlight.Components.AssetBundle;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class CheckboxUIBlueprintV01 : UIBlueprint
{
    public Sprite customCheckSprite;
    public UIColor backgroundColor = UIColor.Primary;
    public Color? customBackgroundColor = null;
    public UIColor checkColor = UIColor.Accent;
    public Color? customCheckColor = null;
    public bool defaultValue;
    public System.Action<bool> onValueChanged = null;

    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        IgnoreCorners = true;
        var toggle = obj.AddComponent<Toggle>();
        toggle.isOn = defaultValue;

        var backgroundObj = new GameObject("Background");
        var backgroundRect = backgroundObj.AddComponent<RectTransform>();
        backgroundObj.transform.SetParent(obj);
        backgroundRect.anchorMin = Vector2.zero;
        backgroundRect.anchorMax = new Vector2(1,1);
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;
        var backgroundImage = backgroundObj.AddComponent<Image>();
        backgroundImage.color = customBackgroundColor ?? theme.GetColor(backgroundColor);
        if(CornerRadius>0)
        {
            var sortGroup = backgroundObj.AddComponent<SortingGroup>();
            sortGroup.enabled = false;
            sortGroup.sortingOrder = Mathf.FloorToInt(CornerRadius * ScaleFactor);
            backgroundObj.AddComponent<RoundedUIImage>().CornerRadius = CornerRadius * ScaleFactor;
        }
        
        var checkmarkObj = new GameObject("Checkmark");
        var checkmarkRect = checkmarkObj.AddComponent<RectTransform>();
        checkmarkObj.transform.SetParent(backgroundObj.transform);
        checkmarkRect.anchorMin = Vector2.zero;
        checkmarkRect.anchorMax = new Vector2(1,1);
        checkmarkRect.offsetMin = Vector2.zero;
        checkmarkRect.offsetMax = Vector2.zero;
        checkmarkRect.anchoredPosition = Vector2.zero;
        var checkmarkImage = checkmarkObj.AddComponent<Image>();
        checkmarkImage.color = customCheckColor ?? theme.GetColor(checkColor);
        checkmarkImage.sprite = customCheckSprite ?? EmbeddedResourceEUtil.LoadSprite("Assets.check.png");
        if(CornerRadius>0)
        {
            var sortGroup = checkmarkObj.AddComponent<SortingGroup>();
            sortGroup.enabled = false;
            sortGroup.sortingOrder = Mathf.FloorToInt(CornerRadius * ScaleFactor);
            checkmarkObj.AddComponent<RoundedUIImage>().CornerRadius = CornerRadius * ScaleFactor;
        }
        toggle.graphic = checkmarkImage;
        toggle.targetGraphic = backgroundImage;
        if(onValueChanged!=null)
            toggle.onValueChanged.AddListener(onValueChanged);
    }
}
