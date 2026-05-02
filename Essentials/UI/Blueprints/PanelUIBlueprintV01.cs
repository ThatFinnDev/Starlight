using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class PanelUIBlueprintV01 : UIBlueprint
{
    public Sprite sprite;
    public UIColor color = UIColor.Secondary;
    public Color? customColor = null;
    public PanelUIBlueprintV01()
    {
    }
    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        image.color = customColor ?? theme.GetColor(color);
        image.sprite = sprite;
    }
}
