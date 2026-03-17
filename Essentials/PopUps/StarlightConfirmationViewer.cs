using System;
using Il2CppTMPro;
using Starlight.Enums;
using Starlight.Storage;
using UnityEngine.UI;

namespace Starlight.Popups;


[InjectIntoIL]
public class StarlightConfirmationViewer : StarlightPopUp
{
    private string _text;
    private int _variant;
    private readonly Action _okAction = null;
    private readonly Action _yesAction = null;
    private readonly Action _noAction = null;
    private readonly Action _escapeAction = null;
    public new static void PreAwake(GameObject obj, List<object> objects)
    {
        var comp = obj.AddComponent<StarlightConfirmationViewer>();
        comp._text = objects[0].ToString();
        comp._variant= int.Parse(objects[1].ToString() ?? string.Empty);
        
        comp.ReloadFont();
        
    }
    protected override void OnOpen()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        var okButton = gameObject.GetObjectRecursively<Button>("OKButtonRec");
        var yesButton = gameObject.GetObjectRecursively<Button>("YesButtonRec");
        var noButton = gameObject.GetObjectRecursively<Button>("NoButtonRec");
        if (_variant == 0)
        {
            okButton.gameObject.SetActive(true);
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            okButton.onClick.AddListener((Action)(() =>
            {
                if(_okAction!=null) _okAction.Invoke();
                Close();
            }));
        }
        else if (_variant == 1)
        {
            okButton.gameObject.SetActive(false);
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
            yesButton.onClick.AddListener((Action)(() =>
            {
                if(_yesAction!=null) _yesAction.Invoke();
                Close();
            }));
            noButton.onClick.AddListener((Action)(() =>
            {
                if(_noAction!=null) _noAction.Invoke();
                Close();
            }));
        }
        var textMesh = gameObject.GetObjectRecursively<TextMeshProUGUI>("TextViewerText");
        textMesh.SetText(_text);
    }
    
    
    
    public static void Open(string text, Action yesAction, Action noAction, Action escapeAction)
    {
        if (!MenuEUtil.isAnyMenuOpen)
        {
            _Open("ConfirmationViewer",typeof(StarlightConfirmationViewer),StarlightMenuTheme.Default,new List<object>(){text,1,yesAction,noAction,escapeAction});
            return;
        }
        _Open("ConfirmationViewer",typeof(StarlightConfirmationViewer),MenuEUtil.GetOpenMenu().GetTheme(),new List<object>(){text,1,yesAction,noAction,escapeAction});
    }
    public static void Open(string text, Action yesAction, Action noAction, Action escapeAction, StarlightMenuTheme theme)
    {
        _Open("ConfirmationViewer",typeof(StarlightConfirmationViewer),theme,new List<object>(){text,1,yesAction,noAction,escapeAction});
    }
    
    
    public static void Open(string text, Action okAction, Action escapeAction)
    {
        if (!MenuEUtil.isAnyMenuOpen)
        {
            _Open("ConfirmationViewer",typeof(StarlightConfirmationViewer),StarlightMenuTheme.Default,new List<object>(){text,0,okAction,escapeAction});
            return;
        }
        _Open("ConfirmationViewer",typeof(StarlightConfirmationViewer),MenuEUtil.GetOpenMenu().GetTheme(),new List<object>(){text,0,okAction,escapeAction});
    }
    public static void Open(string text, Action okAction, Action escapeAction, StarlightMenuTheme theme)
    {
        _Open("ConfirmationViewer",typeof(StarlightConfirmationViewer),theme,new List<object>(){text,0,okAction,escapeAction});
    }
    
    
    
    protected override void OnUpdate()
    {
        if (LKey.Escape.OnKeyDown())
        {
            if(_escapeAction!=null) _escapeAction.Invoke();
            Close();
        }
    }
}