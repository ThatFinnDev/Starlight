using Starlight.Components.AssetBundle;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

internal class VScrollUIBlueprintV01 : UIBlueprint
{
    public float spacing = 5f;
    public bool childControlWidth = true;
    public bool childControlHeight = false;
    public bool childExpandWidth = true;
    public bool childExpandHeight = true;
    public bool useContentSizeFitter = true;
    public TextAnchor childAlignment = TextAnchor.UpperLeft;
    public ContentSizeFitter.FitMode verticalFit = ContentSizeFitter.FitMode.MinSize;
    public ContentSizeFitter.FitMode horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
    public bool useScrollBar = true;
    public ScrollRect.MovementType movementType = ScrollRect.MovementType.Clamped;
    public bool scrollByMenuKeys = true;
    
    
    public UIColor backgroundColor = UIColor.Transparent;
    public Color? customBackgroundColor = null;
    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        ignoreCorners = true;
        var image = obj.AddComponent<Image>();
        image.color = customBackgroundColor ?? theme.GetColor(backgroundColor);
        var scrollRect = obj.AddComponent<ScrollRect>();
        
        var viewPortObj = new GameObject("Viewport");
        var viewPortRect = viewPortObj.AddComponent<RectTransform>();
        viewPortObj.transform.SetParent(obj, false);
        viewPortRect.anchorMin = new Vector2(0, 0);
        viewPortRect.anchorMax = new Vector2(1, 1);
        viewPortRect.sizeDelta = Vector2.zero;
        viewPortRect.anchoredPosition = Vector2.zero;
        viewPortRect.offsetMin = new Vector2(0, 0);
        viewPortRect.offsetMax = new Vector2(useScrollBar?-25*ScaleFactor:0, 0);
        var viewPortImage = viewPortObj.AddComponent<Image>();
        var whiteTexture = new Texture2D(1, 1);
        whiteTexture.SetPixel(0, 0, new Color(0,0,0,1));
        whiteTexture.Apply();
        viewPortImage.sprite = whiteTexture.Texture2DToSprite();
        var viewPortMask = viewPortObj.AddComponent<Mask>();
        viewPortMask.showMaskGraphic = false;
        
    
    
        var contentObj = new GameObject("Content");
        var contentRect = contentObj.AddComponent<RectTransform>();
        contentRect.pivot = new Vector2(0.5f, 1f);
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.anchoredPosition = Vector2.zero;
        customChildHolder = contentRect;
        contentRect.SetParent(viewPortObj.transform, false);
        scrollRect.content = contentRect;

        scrollRect.vertical = true;
        scrollRect.horizontal = false;

        scrollRect.movementType = movementType;
        if (useScrollBar)
        {
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            var scrollBar = new VScrollbarUIBlueprintV01()
            {
                name="Scrollbar Vertical",
                size = new (25, size.y),
                anchors = new Vector4(1,0,1,1),
            }.Render(theme,fontTheme,obj);
            scrollBar.offsetMin = new Vector2(-25*ScaleFactor, 0);
            scrollBar.offsetMax = new Vector2(0, 0);
            scrollRect.verticalScrollbar = scrollBar.GetComponent<Scrollbar>();
        }
        var vlg = contentObj.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(5, 5, 5, 5);
        vlg.spacing = spacing;
        vlg.childControlHeight = childControlHeight;
        vlg.childControlWidth = childControlWidth;
        vlg.childForceExpandHeight = childExpandHeight;
        vlg.childForceExpandWidth = childExpandWidth;
        vlg.childAlignment = childAlignment;
        if (useContentSizeFitter)
        {
            var csf = contentObj.AddComponent<ContentSizeFitter>();
            csf.verticalFit = verticalFit;
            csf.horizontalFit = horizontalFit;
        }

        viewPortRect.sizeDelta /= 1.95f;
        contentRect.sizeDelta = Vector2.zero;
        Canvas.ForceUpdateCanvases();
        if (scrollByMenuKeys)
        {
            //var comp = obj.AddComponent<ScrollByMenuKeys>();
            //comp._scrollDownInput = _inputDown;
            //comp._scrollUpInput = _inputUp;
            //comp._scrollPerFrame = 9f;
        }
    }

    protected override void AfterRenderChildren(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        obj.GetComponent<ScrollRect>().verticalNormalizedPosition=1f;
        if(cornerRadius>0)
        {
            var viewport = obj.transform.GetChild(0).gameObject;
            var sortGroup = viewport.AddComponent<SortingGroup>();
            sortGroup.enabled = false;
            sortGroup.sortingOrder = Mathf.FloorToInt(cornerRadius * ScaleFactor);
            viewport.AddComponent<RoundedUIImage>().CornerRadius = cornerRadius * ScaleFactor;
        }
    }
    
}