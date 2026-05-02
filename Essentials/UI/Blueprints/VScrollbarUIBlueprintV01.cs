using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class VScrollbarUIBlueprintV01 : UIBlueprint
{
    public Sprite backgroundSprite;
    public Sprite handleSprite;
    public UIColor color = UIColor.Accent;
    public Color? customColor = null;
    public bool invertDirection = false;

    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        //image.sprite = backgroundSprite;
        image.color = theme.GetColor(UIColor.Transparent);
        var scrollbar = obj.AddComponent<Scrollbar>();
        scrollbar.direction = invertDirection?Scrollbar.Direction.TopToBottom:Scrollbar.Direction.BottomToTop;
        var backgroundArea = new GameObject("Background");
        var backgroundAreaRect = backgroundArea.AddComponent<RectTransform>();
        backgroundArea.transform.SetParent(obj);
        backgroundAreaRect.sizeDelta = new Vector2(size.x*ScaleFactor / 5, 0);
        backgroundAreaRect.anchoredPosition = Vector2.zero;
        backgroundAreaRect.anchorMin = new Vector2(0.5f, 0);
        backgroundAreaRect.anchorMax = new Vector2(0.5f, 1);
        var backgroundImage = backgroundArea.AddComponent<Image>();
        backgroundImage.sprite = backgroundSprite;
        backgroundImage.color = customColor ?? theme.GetColor(color);
        
        var slidingArea = new GameObject("Sliding Area");
        var slidingAreaRect = slidingArea.AddComponent<RectTransform>();
        slidingArea.transform.SetParent(obj);
        slidingAreaRect.anchorMin = new Vector2(0, 0);
        slidingAreaRect.anchorMax = new Vector2(1, 1);
        slidingAreaRect.offsetMin = new Vector2(0, 0);
        slidingAreaRect.offsetMax = new Vector2(0, 0);
        
        var handle = new GameObject("Handle");
        var handleRect = handle.AddComponent<RectTransform>();
        handle.transform.SetParent(slidingArea.transform);
        handleRect.anchorMin = new Vector2(0, 0);
        handleRect.anchorMax = new Vector2(1, 1);
        handleRect.offsetMin = new Vector2(0, 0);
        handleRect.offsetMax = new Vector2(0, 0);
        handleRect.sizeDelta = new Vector2(-4*ScaleFactor, 0);
        handle.AddComponent<Image>().sprite = handleSprite ?? EmbeddedResourceEUtil.LoadSprite("Assets.scrollV.png");
        
        scrollbar.handleRect = handleRect;
    }
}