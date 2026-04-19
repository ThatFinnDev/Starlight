using Starlight.Components.AssetBundle;
using UnityEngine.Rendering;

namespace Starlight.UI.Blueprints;

public abstract class UIBlueprint
{
    public static float ScaleFactor => Screen.height / 1080f;
    public string Name = "<MissingName>"; 
    public Vector2 Size = new(100,100);
    public Vector2 Position = Vector2.zero;
    public Vector2 Rotation = new ();
    public Vector4 Anchors = new (0.5f, 0.5f, 0.5f, 0.5f);
    public UIBlueprint[] Children;
    public int CornerRadius = 10;
    protected RectTransform CustomChildHolder;
    public RectTransform Render(UITheme theme, Transform parent)
    {
        var obj = new GameObject(Name);
        obj.transform.localRotation=Quaternion.Euler(Rotation.x,0,Rotation.y);
        obj.transform.SetParent(parent);
        var rectT = obj.AddComponent<RectTransform>();
        rectT.sizeDelta = Size*ScaleFactor;
        rectT.anchoredPosition = Position*ScaleFactor;
        rectT.anchorMin = new Vector2(Anchors.x, Anchors.y);
        rectT.anchorMax = new Vector2(Anchors.z, Anchors.w);
        try { OnRender(theme, rectT); } catch (Exception e) { LogError(e); }

        var sortGroup = obj.AddComponent<SortingGroup>();
        sortGroup.enabled = false;
        sortGroup.sortingOrder = Mathf.FloorToInt(CornerRadius*ScaleFactor);
        obj.AddComponent<RoundedUIImage>().CornerRadius = CornerRadius*ScaleFactor;
        
        if (Children != null)
            foreach (var child in Children)
                child.Render(theme, CustomChildHolder ?? rectT);
        return rectT;
    }
    
    protected abstract void OnRender(UITheme theme, RectTransform obj);
}
