using Starlight.Components.AssetBundle;
using UnityEngine.Rendering;

namespace Starlight.UI.Blueprints;

public abstract class UIBlueprint
{
    public static float ScaleFactor => Screen.height / 1080f;
    public string name = "<MissingName>"; 
    public Vector2 size = new(100,100);
    public Vector2 position = Vector2.zero;
    public Vector2 rotation = new ();
    public Vector4 anchors = new (0.5f, 0.5f, 0.5f, 0.5f);
    public Vector2 pivot = new Vector2(0.5f, 0.5f);
    public List<UIBlueprint> children;
    public int cornerRadius = 0;
    protected RectTransform customChildHolder;
    protected bool ignoreCorners = false;
    public List<Il2CppSystem.Type> components;
    public RectTransform Render(UITheme theme, FontTheme fontTheme, Transform parent)
    {
        var obj = new GameObject(name);
        obj.transform.localRotation=Quaternion.Euler(rotation.x,0,rotation.y);
        obj.transform.SetParent(parent);
        var rectT = obj.AddComponent<RectTransform>();
        rectT.pivot = pivot;
        rectT.sizeDelta = size*ScaleFactor;
        rectT.anchoredPosition = position*ScaleFactor;
        rectT.anchorMin = new Vector2(anchors.x, anchors.y);
        rectT.anchorMax = new Vector2(anchors.z, anchors.w);
        try { OnRender(theme, fontTheme, rectT); } catch (Exception e) { LogError(e); }

        if(cornerRadius>0&&!ignoreCorners)
        {
            var sortGroup = obj.AddComponent<SortingGroup>();
            sortGroup.enabled = false;
            sortGroup.sortingOrder = Mathf.FloorToInt(cornerRadius * ScaleFactor);
            obj.AddComponent<RoundedUIImage>().CornerRadius = cornerRadius * ScaleFactor;
        }
        
        if(components != null)
            foreach (var comp in components)
                try { obj.AddComponent(comp); } catch (Exception e) { LogError(e); } 
        
        if (children != null)
            foreach (var child in children)
                try {  child.Render(theme, fontTheme, customChildHolder ?? rectT); } catch (Exception e) { LogError(e); } 
        
        
        try { AfterRenderChildren(theme, fontTheme, rectT); } catch (Exception e) { LogError(e); }
        return rectT;
    }
    
    protected abstract void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj);
    protected virtual void AfterRenderChildren(UITheme theme, FontTheme fontTheme, RectTransform obj) { }
}
