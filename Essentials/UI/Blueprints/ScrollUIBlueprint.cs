using UnityEngine;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class ScrollUIBlueprint : UIBlueprint
{
    public bool Vertical = true;
    public bool Horizontal = false;

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        image.color = new Color(theme.PrimaryColor.r, theme.PrimaryColor.g, theme.PrimaryColor.b, 0.5f);
        var mask = obj.AddComponent<RectMask2D>();
        var scrollRect = obj.AddComponent<ScrollRect>();

        var contentGo = new GameObject("Content");
        var contentRect = contentGo.AddComponent<RectTransform>();
        CustomChildHolder = contentRect;
        contentRect.SetParent(obj, false);
        scrollRect.content = contentRect;

        scrollRect.vertical = Vertical;
        scrollRect.horizontal = Horizontal;

        if (Vertical)
        {
            var vlg = contentGo.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(5, 5, 5, 5);
            vlg.spacing = 5f;
            vlg.childControlHeight = false;
            vlg.childControlWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childForceExpandWidth = true;
            var csf = contentGo.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        else if (Horizontal)
        {
            var hlg = contentGo.AddComponent<HorizontalLayoutGroup>();
            hlg.padding = new RectOffset(5, 5, 5, 5);
            hlg.spacing = 5f;
            var csf = contentGo.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
    }
}