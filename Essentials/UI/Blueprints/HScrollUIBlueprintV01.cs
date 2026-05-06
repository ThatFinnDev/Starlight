using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class HScrollUIBlueprintV01 : UIBlueprint
{
    public Sprite backgroundSprite;
    public Sprite handleSprite;
    public UIColor color = UIColor.Accent;
    public bool invertDirection = false;

    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        //image.sprite = backgroundSprite;
        image.color = theme.GetColor(UIColor.Transparent);
        var scrollbar = obj.AddComponent<Scrollbar>();
        scrollbar.direction = invertDirection?Scrollbar.Direction.RightToLeft:Scrollbar.Direction.LeftToRight;
        var backgroundArea = new GameObject("Background");
        var backgroundAreaRect = backgroundArea.AddComponent<RectTransform>();
        backgroundArea.transform.SetParent(obj);
        backgroundAreaRect.sizeDelta = new Vector2(0, Size.y*ScaleFactor/5);
        backgroundAreaRect.anchoredPosition = Vector2.zero;
        backgroundAreaRect.anchorMin = new Vector2(0,0.5f);
        backgroundAreaRect.anchorMax = new Vector2(1,0.5f);
        var backgroundImage = backgroundArea.AddComponent<Image>();
        backgroundImage.sprite = backgroundSprite;
        backgroundImage.color = theme.GetColor(color);
        
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
        handle.AddComponent<Image>().sprite = handleSprite ?? EmbeddedResourceEUtil.LoadSprite("Assets.scrollV.png");
        
        scrollbar.handleRect = handleRect;
    }
}