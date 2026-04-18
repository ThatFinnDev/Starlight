using UnityEngine;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class SeparatorUIBlueprint : UIBlueprint
{
    public bool IsVertical;

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        image.color = theme.SecondaryColor;

        if (IsVertical)
            obj.sizeDelta = new Vector2(Size.x, obj.rect.height);
        else
            obj.sizeDelta = new Vector2(obj.rect.width, Size.y);
        obj.anchoredPosition = Position;

    }
}
