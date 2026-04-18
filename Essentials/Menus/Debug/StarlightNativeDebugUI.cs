using System;
using Il2CppInterop.Runtime.Attributes;
using Il2CppTMPro;
using Starlight.Components;
using Starlight.Components.Debug;
using Starlight.Enums;
using Starlight.Enums.Features;
using Starlight.Managers;
using Starlight.Storage;
using UnityEngine.UI;

namespace Starlight.Menus.Debug;

internal class StarlightNativeDebugUI : StarlightMenu
{
    // TODO
    // DebugUI contains like nothing :/
    // It gets activated by instantiating
    // There are 2 variants, one for keyboard, one for gamepad
    // It has a prefab and some input actions
    // Maybe some helper like:
    // GameDebugDirectorHelper
    // SceneDebugDirectorHelper
    // Also what are all of those DebugUIHandler <Things>
    internal DebugDirectorFixer ddf => DebugDirectorFixer.Instance;
    public new static MenuIdentifier GetMenuIdentifier() => new ("nativedebugui",StarlightMenuFont.SR2,StarlightMenuTheme.Default,"NativeDebugUI");
    protected override bool createCommands => false;
    protected override bool inGameOnly => true;
    protected override void OnAwake()
    {
        requiredFeatures = new List<FeatureFlag>() { RestoreDebugDebugUI }.ToArray();
        openActions = new List<MenuActions> { MenuActions.PauseGameFalse, }.ToArray();
        closeActions = new List<MenuActions> { MenuActions.UnPauseGameFalse, MenuActions.EnableInput }.ToArray();
    }

    private GameObject debugUIPrefab => ddf.director._uiDefaultPrefab;
    private List<DebugUI> _debugUIs = new ();
    private DebugUI _rootDebugUI;

    private readonly DebugUIEntry[] _rootEntries =
    [
        new DebugUIEntry() { text = "TestButton" },
        new DebugUIEntry() { text = "Toggle Noclip", action = () => StarlightCommandManager.ExecuteByString("noclip")},
        new DebugUIEntry() { text = "SubMenu", closesMenu = false,action = () => MenuEUtil.GetMenu<StarlightNativeDebugUI>().OpenEntries(
            new []
            {
                new DebugUIEntry() { text = "SubButton" },
                new DebugUIEntry() { text = "SubMenu", closesMenu = false, action = () => MenuEUtil.GetMenu<StarlightNativeDebugUI>().OpenEntries(new []
                {
                    new DebugUIEntry() { text = "Subsubbutton" },
                }) },
            }) }
    ];
    
    // In reality it's tab
    internal static readonly LKey OpenKey = LKey.F10;
    public new static GameObject GetMenuRootObject()
    {
        var obj = new GameObject("NativeDebugUI");
        var rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero; 
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.localPosition = Vector2.zero;
        rect.sizeDelta = rect.GetParentSize();
        return obj;
    }
    protected override void OnOpen()
    {
        GetComponent<RectTransform>().sizeDelta=GetComponent<RectTransform>().GetParentSize();
        foreach (var debugUI in _debugUIs) Destroy(debugUI.gameObject);
        _debugUIs = new();

        _rootDebugUI = OpenEntries(_rootEntries);
    }

    protected override void OnClose()
    {
        foreach (var debugUI in _debugUIs) Destroy(debugUI.gameObject);
        _debugUIs = new();
    }
    public void GoBack()
    {
        if (_debugUIs.Count >= 1) CloseEntries(_debugUIs[_debugUIs.Count - 1]);
        else Close();
    }
    
    
    public void CloseEntries(DebugUI toClose)
    {
        foreach (var ui in _debugUIs) ui.gameObject.SetActive(false);
        _debugUIs.Remove(toClose);
        Destroy(toClose.gameObject);
        if (_debugUIs.Count >= 1) _debugUIs[_debugUIs.Count-1].gameObject.SetActive(true);
        else Close();
    }
    [HideFromIl2Cpp] public DebugUI OpenEntries(params DebugUIEntry[] buttons)
    {
        foreach (var ui in _debugUIs) ui.gameObject.SetActive(false);
        var instance = Instantiate(debugUIPrefab, null as Transform);
        instance.transform.parentInternal = transform;
        var debugUI = instance.GetComponent<DebugUI>();
        _debugUIs.Add(debugUI);

        foreach (var b in buttons) if(b!=null) AddButton(debugUI,b);
        
        return debugUI;
    }
    [HideFromIl2Cpp] public void AddButton(DebugUI debugUI,DebugUIEntry entry)
    {
        if (entry == null) return;
        var instance = Instantiate(debugUI.buttonPrefab, debugUI.grid.transform);
        instance.GetObjectRecursively<TextMeshProUGUI>("Name").text = entry.text;
        
        if (entry.icon == null) instance.GetObjectRecursively<GameObject>("Icon").SetActive(false);
        else instance.GetObjectRecursively<Image>("Icon").sprite = entry.icon;

        var b = instance.GetObjectRecursively<Button>("Content");
        if(entry.action!=null) b.onClick.AddListener(entry.action);
        if(entry.closesMenu) b.onClick.AddListener((Action)(() => Close()));
    }
    
    public override void OnCloseUIPressed()
    {
        if (MenuEUtil.isAnyPopUpOpen) return;
        if(_debugUIs.Count>=1) GoBack();
        else Close();
    }
}