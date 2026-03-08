using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Il2CppInterop.Runtime.Attributes;
using Il2CppMonomiPark.SlimeRancher.Input;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppTMPro;
using Starlight.Components;
using Starlight.Enums;
using Starlight.Enums.Features;
using Starlight.Enums.Sounds;
using Starlight.Expansion;
using Starlight.Managers;
using Starlight.Popups;
using Starlight.Storage;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

namespace Starlight.Menus;

public class StarlightModMenu : StarlightMenu
{
    public new static MenuIdentifier GetMenuIdentifier() => new MenuIdentifier("modmenu",StarlightMenuFont.SR2,StarlightMenuTheme.Default,"ModMenu");
    protected override bool createCommands => true;
    protected override bool inGameOnly => false;
    
    protected override void OnAwake()
    {
        requiredFeatures = new List<FeatureFlag>() { EnableModMenu }.ToArray();
        openActions = new List<MenuActions> { MenuActions.PauseGame, MenuActions.HideMenus }.ToArray();
        closeActions = new List<MenuActions> { MenuActions.UnPauseGame, MenuActions.UnHideMenus, MenuActions.EnableInput }.ToArray();
    }
    
    
    internal static Dictionary<MelonPreferences_Entry, SystemAction> entriesWithActions = new ();
    TextMeshProUGUI modInfoText;
    GameObject entryTemplate;
    GameObject headerTemplate;
    GameObject warningText;
    Texture2D modMenuTabImage;
    List<Key> allPossibleUnityKeys = new List<Key>();
    private List<KeyCode> allPossibleUnityKeyCodes = new List<KeyCode>();
    private List<LKey> allPossibleLKey = new List<LKey>();
    TextMeshProUGUI themeMenuText;
    Button themeButton;
    private Transform modContent;
    private Transform modConfigContent;

    protected override void OnClose()
    {
        gameObject.GetObjectRecursively<Button>("ModMenuModMenuSelectionButtonRec").onClick.Invoke();
        for (int i = 0; i < modContent.childCount; i++)
            Object.Destroy(modContent.GetChild(i).gameObject);
    }
    
    private InputEvent inputDown;
    private InputEvent inputUp;
    public override void AfterGameContext(GameContext gameContext)
    {
        inputDown = Get<InputEvent>("ItemDown");
        inputUp = Get<InputEvent>("ItemUp");
        var refScroll = modContent.parent.parent;
        if (!refScroll.HasComponent<ScrollByMenuKeys>())
        {
            var comp = refScroll.gameObject.AddComponent<ScrollByMenuKeys>();
            comp._scrollDownInput = inputDown;
            comp._scrollUpInput = inputUp;
            comp._scrollPerFrame = 9f;
        }
        var gadgetScroll = modConfigContent.parent.parent;
        if (!gadgetScroll.HasComponent<ScrollByMenuKeys>())
        {
            var comp = gadgetScroll.gameObject.AddComponent<ScrollByMenuKeys>();
            comp._scrollDownInput = inputDown;
            comp._scrollUpInput = inputUp;
            comp._scrollPerFrame = 9f;
        }
    }

    private Dictionary<GameObject, string> modButtons = new ();
    [HideFromIl2Cpp] void ProcessMelon(StarlightExpansionInfo info,string downloadLink,bool isExpansion,bool isRotten,GameObject buttonPrefab, List<object> rottenInfo)
    {
        var obj = Instantiate(buttonPrefab, modContent);
        modButtons.Add(obj,info.name);
        var b = obj.GetComponent<Button>();
        b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = info.name;
        obj.SetActive(true);
        if (isExpansion)
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
            themeButton.gameObject.SetActive(info.name=="Starlight");
            if (isRotten)
            {
                modInfoText.text = translation("modmenu.modinfo.brokenmod", info.name);
                if (isExpansion) modInfoText.text = translation("modmenu.modinfo.brokenexpansion", info.name);
            }
            else
            {
                modInfoText.text = translation("modmenu.modinfo.mod", info.name);
                if (isExpansion) modInfoText.text = translation("modmenu.modinfo.expansion", info.name);
            }
            if(!string.IsNullOrWhiteSpace(info.ID)) 
                modInfoText.text += "\n" + translation("modmenu.modinfo.id", info.ID);
            modInfoText.text += "\n" + translation("modmenu.modinfo.author", info.author);

            if(isExpansion)
                modInfoText.text += "\n" + translation("modmenu.modinfo.useprism", info.usePrism);

            if (info.coAuthors is { Length: > 0 }) modInfoText.text += "\n" + translation("modmenu.modinfo.coauthor", string.Join(", ",info.coAuthors));
            if (info.contributors is { Length: > 0 }) modInfoText.text += "\n" + translation("modmenu.modinfo.contributors", string.Join(", ",info.contributors));
            
            modInfoText.text += "\n" + translation("modmenu.modinfo.version", info.version) + "\n";
            
            
            if(!string.IsNullOrWhiteSpace(info.sourceCode)) 
                modInfoText.text += "\n" + translation("modmenu.modinfo.sourcecode", info.sourceCode);
            
            if(!string.IsNullOrWhiteSpace(info.nexus)) 
                modInfoText.text += "\n" + translation("modmenu.modinfo.nexus", info.nexus);
            
            if(!string.IsNullOrWhiteSpace(info.discord)) 
                modInfoText.text += "\n" + translation("modmenu.modinfo.discord", info.discord);


            if (!string.IsNullOrWhiteSpace(downloadLink))
                modInfoText.text += "\n" + translation("modmenu.modinfo.link", FormatLink(downloadLink));

            if(!string.IsNullOrWhiteSpace(info.description)) 
                modInfoText.text += "\n" + translation("modmenu.modinfo.description", info.description + "\n");

            if (isRotten&&rottenInfo!=null&rottenInfo.Count>=3)
            {
                modInfoText.text += "\n";
                try {modInfoText.text += "\n" + translation("modmenu.modinfo.path", rottenInfo[0]);} catch {}
                try {modInfoText.text += "\n" + translation("modmenu.modinfo.exception", rottenInfo[1]);} catch {}
                try {modInfoText.text += "\n" + translation("modmenu.modinfo.errormessage", rottenInfo[2]);} catch {}
            }
        }));
    }
    protected override void OnOpen()
    {
        GameObject buttonPrefab = transform.GetObjectRecursively<GameObject>("ModMenuModMenuTemplateButtonRec");
        buttonPrefab.SetActive(false);
        modButtons = new();
        //Load broken Melons
        foreach (var loadedAssembly in MelonAssembly.LoadedAssemblies) foreach (dynamic rotten in loadedAssembly.RottenMelons)
        {
            // Do it this way to support ML 0.7.1 and newer versions
            try
            {
                Assembly assembly = null;
                string exception = null;
                string errorMessage = null;
                try
                {
                    assembly = rotten.assembly;
                    exception = rotten.exception?.ToString();
                    errorMessage = rotten.errorMessage;
                }
                catch
                {
                    try
                    {
                        assembly = rotten.Assembly.assembly;
                        exception = rotten.exception?.ToString();
                        errorMessage = rotten.errorMessage;
                    }
                    catch { }
                }
                if (assembly == null) break;
                
                string melonName = "";
                try {melonName = assembly.FullName; } catch {}
                if (string.IsNullOrEmpty(melonName)) melonName = translation("modmenu.modinfo.brokentitle");
                ProcessMelon(new StarlightExpansionInfo(){name = melonName,assembly = assembly, dllName = new FileInfo(assembly.Location).Name},null,false,true,buttonPrefab,new List<object>() { assembly.Location, exception, errorMessage });

            }
            catch (Exception e) { MelonLogger.Error(e); }
        }
        //Load Melons
        foreach (var melonBase in MelonBase.RegisteredMelons)
        {
            try
            {
                var assembly = melonBase.MelonAssembly.Assembly;
                if (StarlightEntryPoint.Expansions.Keys.ToList().Contains(assembly))
                    continue;
                var info = new StarlightExpansionInfo()
                {
                    name = melonBase.Info.Name,
                    author = melonBase.Info.Author,
                    version = melonBase.Info.Version,
                    dllName = new FileInfo(assembly.Location).Name
                };
            
                var desc = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
                if (desc != null)
                    info.description = desc.Description;
                foreach (var meta in assembly.GetCustomAttributes<AssemblyMetadataAttribute>())
                {
                    if (meta == null) continue;
                    if (string.IsNullOrWhiteSpace(meta.Key)) continue;
                    if (string.IsNullOrWhiteSpace(meta.Value)) continue;
                    switch (meta.Key)
                    {
                        case StarlightModInfoAttributes.SourceCode: info.sourceCode = meta.Value; break;
                        case StarlightModInfoAttributes.Nexus: info.nexus = meta.Value; break;
                        case StarlightModInfoAttributes.Discord: info.discord = meta.Value; break;
                        case StarlightModInfoAttributes.DisplayVersion: info.version = meta.Value; break;
                        case StarlightModInfoAttributes.CoAuthors: info.coAuthors = meta.Value.Split(", "); break;
                        case StarlightModInfoAttributes.Contributors: info.contributors = meta.Value.Split(", "); break;
                        case StarlightModInfoAttributes.IconB64: try { info.icon = ConvertEUtil.Base64ToTexture2D(meta.Value).Texture2DToSprite(); } catch { } break;
                    }
                }

                if (info.icon != null)
                {
                    try { info.icon = EmbeddedResourceEUtil.LoadSprite("icon.png",assembly).CopyWithoutMipmaps(); }
                    catch { }
                    if (info.icon != null)
                    {
                        try { info.icon = EmbeddedResourceEUtil.LoadSprite("Assets.icon.png", assembly).CopyWithoutMipmaps(); }
                        catch { }
                    }
                }
                ProcessMelon(info, melonBase.Info.DownloadLink, false, false, buttonPrefab, null);
            }
            catch (Exception e) { MelonLogger.Error(e); }
        }

        //Load Expansions
        foreach (var pair in StarlightEntryPoint.Expansions.Values.ToList())
        {
            try
            {
                foreach (var pair2 in pair.Item1)
                {
                    try
                    { 
                        ProcessMelon(pair2.Value,null, true,false, buttonPrefab, null);
                    }
                    catch (Exception e) { MelonLogger.Error(e); }
                }
            }
            catch (Exception e) { MelonLogger.Error(e); }
        }
        //Load broken Expansions
        foreach (var group in StarlightEntryPoint.BrokenExpansions)
        {
            try
            {
                ProcessMelon(new StarlightExpansionInfo(){name = group.Item1,assembly = group.Item2, dllName = new FileInfo(group.Item2.Location).Name},null, true,true,
                    buttonPrefab, new List<object>() { group.Item2.Location, group.Item3,  group.Item4 });
            }
            catch (Exception e) { MelonLogger.Error(e); }
        }
        
        
        
        
        var sortedButtons = modButtons.Keys.ToList().OrderBy(obj =>
        {
            var text = modButtons[obj];
            if (text == "Starlight") return " ";
            return text;
        }).ToList();

        for (int i = 0; i < sortedButtons.Count; i++)
            sortedButtons[i].transform.SetSiblingIndex(i);
        modButtons = new ();
        
        modContent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    string FormatLink(string url)
    {
        return $"<link=\"{url}\"><color=#2C6EC8><u>{url}</u></color></link>";
    }

    private List<SystemAction> configTabActions = new ();
    protected override void OnLateAwake()
    {
        modContent = transform.GetObjectRecursively<Transform>("ModMenuModMenuContentRec");
        modConfigContent = transform.GetObjectRecursively<Transform>("ModMenuModConfigurationContentRec");
        entryTemplate = transform.GetObjectRecursively<GameObject>("ModMenuModConfigurationTemplateEntryRec");
        headerTemplate = transform.GetObjectRecursively<GameObject>("ModMenuModConfigurationTemplateHeaderRec");
        warningText = transform.GetObjectRecursively<GameObject>("ModMenuModConfigurationRestartWarningRec");
        toTranslate.Add(warningText.GetComponent<TextMeshProUGUI>(),"modmenu.warning.restart");
        modInfoText = transform.GetObjectRecursively<TextMeshProUGUI>("ModMenuModInfoTextRec");
        modInfoText.AddComponent<ClickableTextLink>();
        foreach (string stringKey in System.Enum.GetNames(typeof(Key)))
            if (!string.IsNullOrEmpty(stringKey))
                if (stringKey != "None")
                {
                    Key key;
                    if (Key.TryParse(stringKey, out key))
                        if (key != null)
                            allPossibleUnityKeys.Add(key);
                }
        allPossibleUnityKeys.Remove(Key.LeftCommand);
        allPossibleUnityKeys.Remove(Key.RightCommand);
        
        foreach (string stringKey in System.Enum.GetNames(typeof(KeyCode)))
            if (!string.IsNullOrEmpty(stringKey))
                if (stringKey != "None")
                {
                    KeyCode key;
                    if (KeyCode.TryParse(stringKey, out key))
                        if (key != null)
                            allPossibleUnityKeyCodes.Add(key);
                }
        allPossibleUnityKeyCodes.Remove(KeyCode.LeftWindows);
        allPossibleUnityKeyCodes.Remove(KeyCode.RightWindows);
        
        foreach (string stringKey in System.Enum.GetNames(typeof(LKey)))
            if (!string.IsNullOrEmpty(stringKey))
                if (stringKey != "None")
                {
                    LKey key;
                    if (LKey.TryParse(stringKey, out key))
                        if (key != null)
                            allPossibleLKey.Add(key);
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
                
                foreach (var action in configTabActions)
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
        
        themeButton = transform.GetObjectRecursively<Button>("ThemeMenuButtonRec");
        themeButton.onClick.AddListener((SystemAction)(() =>{ AudioEUtil.PlaySound(MenuSound.Click); Close(); MenuEUtil.GetMenu<StarlightThemeMenu>().OpenC(this); }));
        toTranslate.Add(themeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"buttons.thememenu.label");
        foreach (MelonPreferences_Category category in MelonPreferences.Categories)
        {
            GameObject header = Instantiate(headerTemplate, modConfigContent);
            header.SetActive(true);
            header.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = category.DisplayName;
            foreach (MelonPreferences_Entry entry in category.Entries)
            {
                if (!entry.IsHidden)
                {
                    GameObject obj = Instantiate(entryTemplate, modConfigContent);
                    obj.SetActive(true);
                    var entryName = obj.GetObjectRecursively<TextMeshProUGUI>("EntryName");
                    entryName.text = entry.DisplayName;
                    if (!string.IsNullOrEmpty(entry.Description))
                        entryName.text +=
                            $"\n<size=60%>{entry.Description.Replace("\n", " ")}</size>";
                    //entryName.autoSizeTextContainer = true;
                    obj.GetObjectRecursively<TextMeshProUGUI>("Value").text = entry.GetEditedValueAsString();
                    configTabActions.Add(() =>
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
                        obj.GetObjectRecursively<Toggle>("EntryToggle").onValueChanged.AddListener((System.Action<bool>)(
                            (isOn) =>
                            {
                                if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                                entry.BoxedEditedValue = isOn;
                                category.SaveToFile(false);
                                if (!entriesWithActions.ContainsKey(entry))
                                    warningText.SetActive(true);
                                else
                                {
                                    var action = entriesWithActions[entry];
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
                        inputField.onValueChanged.AddListener((System.Action<string>)(
                            (text) =>
                            {
                                if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                                if (string.IsNullOrEmpty(text))
                                    text = "0";
                                int value;
                                if (int.TryParse(text, out value))
                                {
                                    entry.BoxedEditedValue = value;
                                    category.SaveToFile(false);
                                    if (!entriesWithActions.ContainsKey(entry))
                                        warningText.SetActive(true);
                                    else
                                    {
                                        var action = entriesWithActions[entry];
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
                        inputField.onValueChanged.AddListener((System.Action<string>)(
                            (text) =>
                            {
                                if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                                if (string.IsNullOrEmpty(text))
                                    text = "0.0";
                                float value;
                                if (float.TryParse(text, out value))
                                {
                                    entry.BoxedEditedValue = value;
                                    category.SaveToFile(false);
                                    obj.GetObjectRecursively<TextMeshProUGUI>("Value").text = text;
                                    if (!entriesWithActions.ContainsKey(entry))
                                        warningText.SetActive(true);
                                    else
                                    {
                                        var action = entriesWithActions[entry];
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
                        inputField.onValueChanged.AddListener((System.Action<string>)(
                            (text) =>
                            {
                                if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                                if (string.IsNullOrEmpty(text))
                                    text = "0.0";
                                double value;
                                if (double.TryParse(text, out value))
                                {
                                    entry.BoxedEditedValue = value;
                                    category.SaveToFile(false);
                                    obj.GetObjectRecursively<TextMeshProUGUI>("Value").text = text;
                                    if (!entriesWithActions.ContainsKey(entry))
                                        warningText.SetActive(true);
                                    else
                                    {
                                        var action = entriesWithActions[entry];
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
                        inputField.onValueChanged.AddListener((System.Action<string>)((text) =>
                        {
                            if(isOpen) AudioEUtil.PlaySound(MenuSound.Click);
                            entry.BoxedEditedValue = text;
                            category.SaveToFile(false);
                            if (!entriesWithActions.ContainsKey(entry))
                                warningText.SetActive(true);
                            else
                            {
                                var action = entriesWithActions[entry];
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
                            listeningType = 3;
                            listeningAction = (integer) =>
                            {
                                var inputKey = (Key) integer;
                                Key key = inputKey == Key.Escape ? Key.None : inputKey;
                                if (key == null)
                                {
                                    textMesh.text = entry.GetEditedValueAsString();
                                }
                                else
                                {
                                    if (entry.BoxedEditedValue is Key)
                                    {
                                        textMesh.text = key.ToString();
                                        entry.BoxedEditedValue = key;
                                        if (!entriesWithActions.ContainsKey(entry))
                                            warningText.SetActive(true);
                                        else
                                        {
                                            var action = entriesWithActions[entry];
                                            if (action != null)
                                                action.Invoke();
                                        }
                                    }
                                }

                                listeningAction = null;
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
                            listeningType = 2;
                            listeningAction = (integer) =>
                            {
                                KeyCode inputKey = (KeyCode) integer;
                                KeyCode key = inputKey == KeyCode.Escape ? KeyCode.None : inputKey;
                                if (key == null)
                                {
                                    textMesh.text = entry.GetEditedValueAsString();
                                }
                                else
                                {
                                    if (entry.BoxedEditedValue is KeyCode)
                                    {
                                        textMesh.text = key.ToString();
                                        entry.BoxedEditedValue = key;
                                        if (!entriesWithActions.ContainsKey(entry))
                                            warningText.SetActive(true);
                                        else
                                        {
                                            var action = entriesWithActions[entry];
                                            if (action != null)
                                                action.Invoke();
                                        }
                                    }
                                }

                                listeningAction = null;
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
                            listeningType = 1;
                            listeningAction = (integer) =>
                            {
                                LKey inputKey = (LKey) integer;
                                LKey key = inputKey == LKey.Escape ? LKey.None : inputKey;
                                if (key == null)
                                {
                                    textMesh.text = entry.GetEditedValueAsString();
                                }
                                else
                                {
                                    if (entry.BoxedEditedValue is LKey)
                                    {
                                        textMesh.text = key.ToString();
                                        entry.BoxedEditedValue = key;
                                        if (!entriesWithActions.ContainsKey(entry))
                                            warningText.SetActive(true);
                                        else
                                        {
                                            var action = entriesWithActions[entry];
                                            if (action != null)
                                                action.Invoke();
                                        }
                                    }
                                }

                                listeningAction = null;
                            };
                        }));
                    }
                }
            }
        }
    }

    private static int listeningType = 0;
    static System.Action<int> listeningAction = null;

    public override void OnCloseUIPressed()
    {
        if (listeningAction != null) return;
        if (MenuEUtil.isAnyPopUpOpen) return;
        
        Close();
    }

    protected override void OnUpdate()
    {
        if(listeningAction !=null) switch (listeningType)
        {
            case 1:
                foreach (LKey key in allPossibleLKey)
                    try { if(key.OnKeyDown()) { listeningAction.Invoke(Convert.ToInt32(key)); } } catch { }
                break;
            case 2:
                foreach (KeyCode key in allPossibleUnityKeyCodes)
                    try { if(key.OnKeyDown()) listeningAction.Invoke(Convert.ToInt32(key)); } catch { }
                break;
            case 3:
                foreach (Key key in allPossibleUnityKeys)
                    try { if(Keyboard.current[key].wasPressedThisFrame) listeningAction.Invoke(Convert.ToInt32(key)); }catch { }
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
            if (listeningAction != null)
                listeningAction.Invoke(0);
        }
    }
}