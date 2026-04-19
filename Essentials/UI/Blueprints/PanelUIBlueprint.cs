using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class PanelUIBlueprint : UIBlueprint
{
    public UIColor color = UIColor.Primary;
    public PanelUIBlueprint()
    {
    }
    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        image.color = theme.GetColor(color);
    }
}
