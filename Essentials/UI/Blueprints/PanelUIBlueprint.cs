using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class PanelUIBlueprint : UIBlueprint
{
    public PanelUIBlueprint()
    {
        Size = new(1330, 540);
    }
    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        image.color = theme.PrimaryColor;
    }
}
