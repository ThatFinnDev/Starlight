using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class BackgroundPanelUIBlueprint : UIBlueprint
{
    public BackgroundPanelUIBlueprint()
    {
        Size = new(1330, 580);
    }
    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        image.color = theme.PrimaryColor;
        image.sprite = theme.BackgroundPanelSprite;
    }
}
