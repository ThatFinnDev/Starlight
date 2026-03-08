using Il2CppTMPro;
using Starlight.Enums;
using Starlight.Storage;
using UnityEngine.InputSystem;

namespace Starlight.Popups;

[InjectClass]
public class StarlightTextViewer : StarlightPopUp
{
    private string _text;
    public new static void PreAwake(GameObject obj, List<object> objects)
    {
        var comp = obj.AddComponent<StarlightTextViewer>();
        comp._text = objects[0].ToString();
        comp.ReloadFont();
        
    }
    protected override void OnOpen()
    {
        var textMesh = gameObject.GetObjectRecursively<TextMeshProUGUI>("TextViewerText");
        textMesh.SetText(_text);
    }
    
    public static void Open(string text)
    {
        if (!MenuEUtil.isAnyMenuOpen)
        {
            _Open("TextViewer",typeof(StarlightTextViewer),StarlightMenuTheme.Default,new List<object>(){text});
            return;
        }
        _Open("TextViewer",typeof(StarlightTextViewer),MenuEUtil.GetOpenMenu().GetTheme(),new List<object>(){text});
    }
    public static void Open(string text, StarlightMenuTheme theme)
    {
        _Open("TextViewer",typeof(StarlightTextViewer),theme,new List<object>(){text});
    }
    protected override void OnUpdate()
    {
        if (LKey.Escape.OnKeyDown())
            Close();
    }
}