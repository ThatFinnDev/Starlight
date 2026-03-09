using System;
using System.Linq;
using Il2CppInterop.Runtime.Attributes;
using Il2CppMonomiPark.SlimeRancher.Input;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppTMPro;
using MelonLoader;
using Starlight.Components;
using Starlight.Enums;
using Starlight.Enums.Features;
using Starlight.Enums.Sounds;
using Starlight.Managers;
using Starlight.Popups;
using Starlight.Storage;
using UnityEngine.InputSystem;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;
// ReSharper disable EmptyGeneralCatchClause

namespace Starlight.Menus;

public class StarlightModMenu : StarlightMenu
{
    public new static MenuIdentifier GetMenuIdentifier() => new ("modmenu",StarlightMenuFont.SR2,StarlightMenuTheme.Default,"ModMenu");
    protected override bool createCommands => true;
    protected override bool inGameOnly => false;
    
    internal static readonly Dictionary<MelonPreferences_Entry, SystemAction> EntriesWithActions = new ();
    private TextMeshProUGUI _modInfoText;
    private GameObject _entryTemplate;
    private GameObject _headerTemplate;
    private GameObject _warningText;
    private Texture2D _modMenuTabImage;
    private readonly List<Key> _allPossibleUnityKeys = new ();
    private readonly List<KeyCode> _allPossibleUnityKeyCodes = new ();
    private readonly List<LKey> _allPossibleLKey = new ();
    private TextMeshProUGUI _themeMenuText;
    private Button _themeButton;
    private Transform _modContent;
    private Transform _modConfigContent;
    private InputEvent _inputDown;
    private InputEvent _inputUp;
    private Dictionary<GameObject, string> _modButtons = new ();
    private readonly List<SystemAction> _configTabActions = new ();
    private static int _listeningType = 0;
    private static System.Action<int> _listeningAction = null;
    
    
    protected override void OnAwake()
    {
        requiredFeatures = new List<FeatureFlag>() { EnableModMenu }.ToArray();
        openActions = new List<MenuActions> { MenuActions.PauseGame, MenuActions.HideMenus }.ToArray();
        closeActions = new List<MenuActions> { MenuActions.UnPauseGame, MenuActions.UnHideMenus, MenuActions.EnableInput }.ToArray();
    }
    protected override void OnClose()
    {
        gameObject.GetObjectRecursively<Button>("ModMenuModMenuSelectionButtonRec").onClick.Invoke();
        for (int i = 0; i < _modContent.childCount; i++)
            Destroy(_modContent.GetChild(i).gameObject);
    }
    
    public override void AfterGameContext(GameContext gameContext)
    {
        _inputDown = Get<InputEvent>("ItemDown");
        _inputUp = Get<InputEvent>("ItemUp");
        var refScroll = _modContent.parent.parent;
        if (!refScroll.HasComponent<ScrollByMenuKeys>())
        {
            var comp = refScroll.gameObject.AddComponent<ScrollByMenuKeys>();
            comp._scrollDownInput = _inputDown;
            comp._scrollUpInput = _inputUp;
            comp._scrollPerFrame = 9f;
        }
        var gadgetScroll = _modConfigContent.parent.parent;
        if (!gadgetScroll.HasComponent<ScrollByMenuKeys>())
        {
            var comp = gadgetScroll.gameObject.AddComponent<ScrollByMenuKeys>();
            comp._scrollDownInput = _inputDown;
            comp._scrollUpInput = _inputUp;
            comp._scrollPerFrame = 9f;
        }
    }

    [HideFromIl2Cpp] void ProcessPackage(StarlightPackageInfo info,string downloadLink,bool isRotten,GameObject buttonPrefab, List<object> rottenInfo)
    {
        var obj = Instantiate(buttonPrefab, _modContent);
        _modButtons.Add(obj,info.name);
        var b = obj.GetComponent<Button>();
        b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = info.name;
        obj.SetActive(true);
        if (info.type==PackageType.Expansion)
        {
            var colorBlock = b.colors;
            colorBlock.normalColor = new Color(0.149f, 0.7176f, 0.3961f, 1);
            colorBlock.highlightedColor = new Color(0.1098f, 0.6314f, 0.2157f, 1);
            colorBlock.pressedColor = new Color(0.1371f, 0.7248f, 0.3792f, 1f);
            colorBlock.selectedColor = new Color(0.8706f, 0.5298f, 0.4216f, 1f);
            b.colors = colorBlock;
        }

        if (isRotten)
        {
            var colorBlock = b.colors;colorBlock.normalColor = new Color(0.5f, 0.5f, 0.5f, 1);
            colorBlock.highlightedColor = new Color(0.7f, 0.7f, 0.7f, 1); 
            colorBlock.pressedColor = new Color(0.3f, 0.3f, 0.3f, 1); 
            colorBlock.selectedColor = new Color(0.6f, 0.6f, 0.6f, 1); 
            b.colors = colorBlock;
        }

        if (info.icon != null)
        {
            b.transform.GetChild(1).GetComponent<Image>().sprite = info.icon;
            b.transform.GetChild(1).gameObject.SetActive(true);
        }

        b.onClick.AddListener((SystemAction)(() =>
        {
            AudioEUtil.PlaySound(MenuSound.Click);
            _themeButton.gameObject.SetActive(info.name=="Starlight Core Essentials");
            if (isRotten)
            {
                _modInfoText.text = translation("modmenu.modinfo.brokenmod", info.name);
                if (info.type==PackageType.Expansion) _modInfoText.text = translation("modmenu.modinfo.brokenexpansion", info.name);
            }
            else
            {
                _modInfoText.text = translation("modmenu.modinfo.mod", info.name);
                if (info.type==PackageType.Expansion) _modInfoText.text = translation("modmenu.modinfo.expansion", info.name);
            }
            if(!string.IsNullOrWhiteSpace(info.ID)) 
                _modInfoText.text += "\n" + translation("modmenu.modinfo.id", info.ID);
            _modInfoText.text += "\n" + translation("modmenu.modinfo.author", string.IsNullOrEmpty(info.author)?"Anonymous":info.author);

            if(info.type==PackageType.Expansion)
                _modInfoText.text += "\n" + translation("modmenu.modinfo.useprism", info.usePrism);

            if (info.coAuthors is { Length: > 0 }) _modInfoText.text += "\n" + translation("modmenu.modinfo.coauthor", string.Join(", ",info.coAuthors));
            if (info.contributors is { Length: > 0 }) _modInfoText.text += "\n" + translation("modmenu.modinfo.contributors", string.Join(", ",info.contributors));
            
            _modInfoText.text += "\n" + translation("modmenu.modinfo.version", info.version) + "\n";
            
            
            if(!string.IsNullOrWhiteSpace(info.sourceCode)) 
                _modInfoText.text += "\n" + translation("modmenu.modinfo.sourcecode", FormatLink(info.sourceCode));
            
            if(!string.IsNullOrWhiteSpace(info.nexus)) 
                _modInfoText.text += "\n" + translation("modmenu.modinfo.nexus", FormatLink(info.nexus));
            
            if(!string.IsNullOrWhiteSpace(info.discord)) 
                _modInfoText.text += "\n" + translation("modmenu.modinfo.discord", FormatLink(info.discord));


            if (!string.IsNullOrWhiteSpace(downloadLink))
                _modInfoText.text += "\n" + translation("modmenu.modinfo.link", FormatLink(downloadLink));

            if(!string.IsNullOrWhiteSpace(info.description)) 
                _modInfoText.text += "\n" + translation("modmenu.modinfo.description", info.description + "\n");

            if (isRotten&&rottenInfo!=null&rottenInfo.Count>=3)
            {
                _modInfoText.text += "\n";
                try {_modInfoText.text += "\n" + translation("modmenu.modinfo.path", rottenInfo[0]);} catch {}
                try {_modInfoText.text += "\n" + translation("modmenu.modinfo.exception", rottenInfo[1]);} catch {}
                try {_modInfoText.text += "\n" + translation("modmenu.modinfo.errormessage", rottenInfo[2]);} catch {}
            }
        }));
    }
    protected override void OnOpen()
    {
        GameObject buttonPrefab = transform.GetObjectRecursively<GameObject>("ModMenuModMenuTemplateButtonRec");
        buttonPrefab.SetActive(false);
        _modButtons = new();
        
        // Melons
        foreach (var info in StarlightPackageManager.GetAllMelonInfos())
            try { ProcessPackage(info,null,false, buttonPrefab, null); }
            catch (Exception e) { LogError(e); }
        // Expansions
        foreach (var info in StarlightPackageManager.GetAllExpansionInfos())
            try { ProcessPackage(info,null,false, buttonPrefab, null); }
            catch (Exception e) { LogError(e); }
        // Rotten
        foreach (var pair in StarlightPackageManager.GetAllRottenInfos())
            try { ProcessPackage(pair.Key,null,true, buttonPrefab, pair.Value); }
            catch (Exception e) { LogError(e); }
        
        
        
        var sortedButtons = _modButtons.Keys.ToList().OrderBy(obj =>
        {
            var text = _modButtons[obj];
            if (text == BuildInfo.Name) return " ";
            return text;
        }).ToList();

        for (int i = 0; i < sortedButtons.Count; i++)
            sortedButtons[i].transform.SetSiblingIndex(i);
        _modButtons = new ();
        
        _modContent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    string FormatLink(string url)
    {
        return $"<link=\"{url}\"><color=#2C6EC8><u>{url}</u></color></link>";
    }

    protected override void OnLateAwake()
    {
        _modContent = transform.GetObjectRecursively<Transform>("ModMenuModMenuContentRec");
        _modConfigContent = transform.GetObjectRecursively<Transform>("ModMenuModConfigurationContentRec");
        _entryTemplate = transform.GetObjectRecursively<GameObject>("ModMenuModConfigurationTemplateEntryRec");
        _headerTemplate = transform.GetObjectRecursively<GameObject>("ModMenuModConfigurationTemplateHeaderRec");
        _warningText = transform.GetObjectRecursively<GameObject>("ModMenuModConfigurationRestartWarningRec");
        toTranslate.Add(_warningText.GetComponent<TextMeshProUGUI>(),"modmenu.warning.restart");
        _modInfoText = transform.GetObjectRecursively<TextMeshProUGUI>("ModMenuModInfoTextRec");
        _modInfoText.AddComponent<ClickableTextLink>();
        foreach (string stringKey in Enum.GetNames(typeof(Key)))
            if (!string.IsNullOrEmpty(stringKey))
                if (stringKey != "None")
                {
                    if (Enum.TryParse(stringKey, out Key key)) _allPossibleUnityKeys.Add(key);
                }
        _allPossibleUnityKeys.Remove(Key.LeftCommand);
        _allPossibleUnityKeys.Remove(Key.RightCommand);
        
        foreach (string stringKey in Enum.GetNames(typeof(KeyCode)))
            if (!string.IsNullOrEmpty(stringKey))
                if (stringKey != "None")
                {
                    if (Enum.TryParse(stringKey, out KeyCode key)) _allPossibleUnityKeyCodes.Add(key);
                }
        _allPossibleUnityKeyCodes.Remove(KeyCode.LeftWindows);
        _allPossibleUnityKeyCodes.Remove(KeyCode.RightWindows);
        
        foreach (string stringKey in Enum.GetNames(typeof(LKey)))
            if (!string.IsNullOrEmpty(stringKey))
                if (stringKey != "None")
                {
                    if (Enum.TryParse(stringKey, out LKey key)) _allPossibleLKey.Add(key);
                }
        var button1 = transform.GetObjectRecursively<Image>("ModMenuModMenuSelectionButtonRec");
        button1.GetComponent<Button>().onClick.AddListener(selectCategorySound);
        button1.sprite = whitePillBg;
        var button2 = transform.GetObjectRecursively<Image>("ModMenuConfigurationSelectionButtonRec");
        button2.sprite = whitePillBg;
        button2.GetComponent<Button>().onClick.AddListener(selectCategorySound);
        button2.GetComponent<Button>().onClick.AddListener((SystemAction)(() =>
        {
            ExecuteInTicks((() =>
            {
                
                foreach (var action in _configTabActions)
                    action.Invoke();
            }),1);
        }));
        var button3 = transform.GetObjectRecursively<Image>("ModMenuRepoSelectionButtonRec");
        button3.sprite = whitePillBg;
        button3.GetComponent<Button>().onClick.AddListener(selectCategorySound);
        button3.GetComponent<Button>().onClick.AddListener((SystemAction)(() =>
        {
            if (EnableRepoMenu.HasFlag())
            {
                Close(); MenuEUtil.GetMenu<StarlightRepoMenu>().OpenC(this);
            }
            else
            {
                AudioEUtil.PlaySound(MenuSound.Error);
                StarlightTextViewer.Open(translation("feature.indevelopment"));
            }
        }));
        toTranslate.Add(button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"modmenu.category.modmenu");
        toTranslate.Add(button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"modmenu.category.modconfig");
        toTranslate.Add(button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"modmenu.category.repo");
        toTranslate.Add(transform.GetObjectRecursively<TextMeshProUGUI>("TitleTextRec"),"modmenu.title");
        
        _themeButton = transform.GetObjectRecursively<Button>("ThemeMenuButtonRec");
        _themeButton.onClick.AddListener((SystemAction)(() =>{ AudioEUtil.PlaySound(MenuSound.Click); Close(); MenuEUtil.GetMenu<StarlightThemeMenu>().OpenC(this); }));
        toTranslate.Add(_themeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"buttons.thememenu.label");
        foreach (var category in MelonPreferences.Categories)
        {
            var header = Instantiate(_headerTemplate, _modConfigContent);
            header.SetActive(true);
            header.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = category.DisplayName;
            foreach (var entry in category.Entries)
            {
                if (!entry.IsHidden)
                {
                    GameObject obj = Instantiate(_entryTemplate, _modConfigContent);
                    obj.SetActive(true);
                    var entryName = obj.GetObjectRecursively<TextMeshProUGUI>("EntryName");
                    entryName.text = entry.DisplayName;
                    if (!string.IsNullOrEmpty(entry.Description))
                        entryName.text +=
                            $"\n<size=60%>{entry.Description.Replace("\n", " ")}</size>";
                    //entryName.autoSizeTextContainer = true;
                    obj.GetObjectRecursively<TextMeshProUGUI>("Value").text = entry.GetEditedValueAsString();
                    _configTabActions.Add(() =>
                    {
                        var rectT = obj.GetComponent<RectTransform>();
                        var newValue = entryName.GetRenderedHeight() + 5;
                        if (newValue < rectT.sizeDelta.y) newValue = rectT.sizeDelta.y;
                        rectT.sizeDelta = new Vector2(rectT.sizeDelta.x, newValue);
                    });
                    if (entry.BoxedEditedValue is bool)
                    {
                        obj.GetObjectRecursively<GameObject>("EntryToggle").SetActive(true);
                        obj.GetObjectRecursively<Toggle>("EntryToggle").isOn =
                            bool.Parse(entry.GetEditedValueAsString());
                        obj.GetObjectRecursively<Toggle>("EntryToggle").onValueChanged.AddListener((Action<bool>)(
                            (isOn) =>
                            {
                                if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                                entry.BoxedEditedValue = isOn;
                                category.SaveToFile(false);
                                if (!EntriesWithActions.TryGetValue(entry, out var action))
                                    _warningText.SetActive(true);
                                else
                                {
                                    if (action != null)
                                        action.Invoke();
                                }

                                obj.GetObjectRecursively<TextMeshProUGUI>("Value").text = isOn.ToString();
                            }));

                    }
                    else if (entry.BoxedEditedValue is int)
                    {
                        obj.GetObjectRecursively<GameObject>("Value").SetActive(false);
                        obj.GetObjectRecursively<GameObject>("EntryInput").SetActive(true);
                        TMP_InputField inputField = obj.GetObjectRecursively<TMP_InputField>("EntryInput");
                        inputField.restoreOriginalTextOnEscape = false;
                        inputField.text = entry.GetEditedValueAsString();
                        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                        inputField.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                            translation("modmenu.modconfig.enterint");
                        inputField.onValueChanged.AddListener((Action<string>)(
                            (text) =>
                            {
                                if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                                if (string.IsNullOrEmpty(text))
                                    text = "0";
                                if (int.TryParse(text, out var value))
                                {
                                    entry.BoxedEditedValue = value;
                                    category.SaveToFile(false);
                                    if (!EntriesWithActions.TryGetValue(entry, out var action))
                                        _warningText.SetActive(true);
                                    else
                                    {
                                        if (action != null)
                                            action.Invoke();
                                    }
                                }
                                else
                                    inputField.text = int.MaxValue.ToString();
                            }));
                    }
                    else if (entry.BoxedEditedValue is float)
                    {
                        obj.GetObjectRecursively<GameObject>("Value").SetActive(false);
                        obj.GetObjectRecursively<GameObject>("EntryInput").SetActive(true);
                        TMP_InputField inputField = obj.GetObjectRecursively<TMP_InputField>("EntryInput");
                        inputField.restoreOriginalTextOnEscape = false;
                        inputField.text = entry.GetEditedValueAsString();
                        inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
                        inputField.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                            translation("modmenu.modconfig.enterfloat");
                        inputField.onValueChanged.AddListener((Action<string>)(
                            (text) =>
                            {
                                if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                                if (string.IsNullOrEmpty(text))
                                    text = "0.0";
                                if (float.TryParse(text, out var value))
                                {
                                    entry.BoxedEditedValue = value;
                                    category.SaveToFile(false);
                                    obj.GetObjectRecursively<TextMeshProUGUI>("Value").text = text;
                                    if (!EntriesWithActions.TryGetValue(entry, out var action))
                                        _warningText.SetActive(true);
                                    else
                                    {
                                        if (action != null)
                                            action.Invoke();
                                    }
                                }
                                else
                                    inputField.text = obj.GetObjectRecursively<TextMeshProUGUI>("Value").text;
                            }));
                    }
                    else if (entry.BoxedEditedValue is double)
                    {
                        obj.GetObjectRecursively<GameObject>("Value").SetActive(false);
                        obj.GetObjectRecursively<GameObject>("EntryInput").SetActive(true);
                        TMP_InputField inputField = obj.GetObjectRecursively<TMP_InputField>("EntryInput");
                        inputField.restoreOriginalTextOnEscape = false;
                        inputField.text = entry.GetEditedValueAsString();
                        inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
                        inputField.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                            translation("modmenu.modconfig.enterdouble");
                        inputField.onValueChanged.AddListener((Action<string>)(
                            (text) =>
                            {
                                if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                                if (string.IsNullOrEmpty(text))
                                    text = "0.0";
                                if (double.TryParse(text, out var value))
                                {
                                    entry.BoxedEditedValue = value;
                                    category.SaveToFile(false);
                                    obj.GetObjectRecursively<TextMeshProUGUI>("Value").text = text;
                                    if (!EntriesWithActions.TryGetValue(entry, out var action))
                                        _warningText.SetActive(true);
                                    else
                                    {
                                        if (action != null)
                                            action.Invoke();
                                    }
                                }
                                else
                                    inputField.text = obj.GetObjectRecursively<TextMeshProUGUI>("Value").text;
                            }));
                    }
                    else if (entry.BoxedEditedValue is string)
                    {
                        obj.GetObjectRecursively<GameObject>("Value").SetActive(false);
                        obj.GetObjectRecursively<GameObject>("EntryInput").SetActive(true);
                        TMP_InputField inputField = obj.GetObjectRecursively<TMP_InputField>("EntryInput");
                        inputField.restoreOriginalTextOnEscape = false;
                        inputField.text = entry.GetEditedValueAsString();
                        inputField.contentType = TMP_InputField.ContentType.Standard;
                        inputField.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                            translation("modmenu.modconfig.entertext");
                        inputField.onValueChanged.AddListener((Action<string>)((text) =>
                        {
                            if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                            entry.BoxedEditedValue = text;
                            category.SaveToFile(false);
                            if (!EntriesWithActions.TryGetValue(entry, out var action))
                                _warningText.SetActive(true);
                            else
                            {
                                if (action != null)
                                    action.Invoke();
                            }
                        }));
                    }
                    else if (entry.BoxedEditedValue is Key)
                    {
                        obj.GetObjectRecursively<GameObject>("Value").SetActive(false);
                        obj.GetObjectRecursively<GameObject>("Button").SetActive(true);
                        var button = obj.GetObjectRecursively<Button>("Button");
                        var textMesh = obj.GetObjectRecursively<Transform>("Button").GetChild(0).GetComponent<TextMeshProUGUI>();
                        textMesh.text = entry.GetEditedValueAsString();
                        button.onClick.AddListener((SystemAction)(() =>
                        {
                            if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                            textMesh.text = translation("modmenu.modconfig.keylistening");
                            _listeningType = 3;
                            _listeningAction = (integer) =>
                            {
                                var inputKey = (Key) integer;
                                var key = inputKey == Key.Escape ? Key.None : inputKey;
                                if (entry.BoxedEditedValue is Key)
                                {
                                    textMesh.text = key.ToString();
                                    entry.BoxedEditedValue = key;
                                    if (!EntriesWithActions.TryGetValue(entry, out var action))
                                        _warningText.SetActive(true);
                                    else
                                    {
                                        if (action != null)
                                            action.Invoke();
                                    }
                                }

                                _listeningAction = null;
                            };
                        }));
                    }
                    else if (entry.BoxedEditedValue is KeyCode)
                    {
                        obj.GetObjectRecursively<GameObject>("Value").SetActive(false);
                        obj.GetObjectRecursively<GameObject>("Button").SetActive(true);
                        var button = obj.GetObjectRecursively<Button>("Button");
                        var textMesh = obj.GetObjectRecursively<Transform>("Button").GetChild(0).GetComponent<TextMeshProUGUI>();
                        textMesh.text = entry.GetEditedValueAsString();
                        button.onClick.AddListener((SystemAction)(() =>
                        {
                            if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                            textMesh.text = translation("modmenu.modconfig.keylistening");
                            _listeningType = 2;
                            _listeningAction = (integer) =>
                            {
                                var inputKey = (KeyCode) integer;
                                var key = inputKey == KeyCode.Escape ? KeyCode.None : inputKey;
                                if (entry.BoxedEditedValue is KeyCode)
                                {
                                    textMesh.text = key.ToString();
                                    entry.BoxedEditedValue = key;
                                    if (!EntriesWithActions.TryGetValue(entry, out var action))
                                        _warningText.SetActive(true);
                                    else
                                    {
                                        if (action != null)
                                            action.Invoke();
                                    }
                                }

                                _listeningAction = null;
                            };
                        }));
                    }
                    else if (entry.BoxedEditedValue is LKey)
                    {
                        obj.GetObjectRecursively<GameObject>("Value").SetActive(false);
                        obj.GetObjectRecursively<GameObject>("Button").SetActive(true);
                        var button = obj.GetObjectRecursively<Button>("Button");
                        var textMesh = obj.GetObjectRecursively<Transform>("Button").GetChild(0).GetComponent<TextMeshProUGUI>();
                        textMesh.text = entry.GetEditedValueAsString();
                        button.onClick.AddListener((SystemAction)(() =>
                        {
                            if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                            textMesh.text = translation("modmenu.modconfig.keylistening");
                            _listeningType = 1;
                            _listeningAction = (integer) =>
                            {
                                var inputKey = (LKey) integer;
                                var key = inputKey == LKey.Escape ? LKey.None : inputKey;
                                if (entry.BoxedEditedValue is LKey)
                                {
                                    textMesh.text = key.ToString();
                                    entry.BoxedEditedValue = key;
                                    if (!EntriesWithActions.TryGetValue(entry, out var action))
                                        _warningText.SetActive(true);
                                    else
                                    {
                                        if (action != null)
                                            action.Invoke();
                                    }
                                }

                                _listeningAction = null;
                            };
                        }));
                    }
                }
            }
        }
    }


    public override void OnCloseUIPressed()
    {
        if (_listeningAction != null) return;
        if (MenuEUtil.isAnyPopUpOpen) return;
        
        Close();
    }

    protected override void OnUpdate()
    {
        if(_listeningAction !=null) switch (_listeningType)
        {
            case 1:
                foreach (LKey key in _allPossibleLKey)
                    try { if(key.OnKeyDown()) { _listeningAction.Invoke(Convert.ToInt32(key)); } } catch { }
                break;
            case 2:
                foreach (KeyCode key in _allPossibleUnityKeyCodes)
                    try { if(key.OnKeyDown()) _listeningAction.Invoke(Convert.ToInt32(key)); } catch { }
                break;
            case 3:
                foreach (Key key in _allPossibleUnityKeys)
                    try { if(Keyboard.current[key].wasPressedThisFrame) _listeningAction.Invoke(Convert.ToInt32(key)); }catch { }
                break;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.rightButton.wasPressedThisFrame ||
            Mouse.current.middleButton.wasPressedThisFrame ||
            Mouse.current.backButton.wasPressedThisFrame ||
            Mouse.current.forwardButton.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (_listeningAction != null)
                _listeningAction.Invoke(0);
        }
    }
}