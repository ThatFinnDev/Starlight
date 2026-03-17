using System;
using System.Linq;
using System.Text.RegularExpressions;
using Il2CppTMPro;
using Starlight.Enums;
using Starlight.Enums.Features;
using Starlight.Managers;
using Starlight.Storage;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Starlight.Menus;

public class StarlightConsole : StarlightMenu
{
    public new static MenuIdentifier GetMenuIdentifier() => new ("console",StarlightMenuFont.Regular, StarlightMenuTheme.Black, "Console");

    protected override bool createCommands => true;
    protected override bool inGameOnly => false;

    internal static readonly LKey OpenKey = LKey.F11;
    internal static readonly LMultiKey OpenKey2 = new (LKey.Tab, LKey.LeftControl);
    internal Transform consoleContent;
    private TMP_InputField _commandInput;
    private GameObject _autoCompleteEntryPrefab;
    private Transform _autoCompleteContent;
    private GameObject _autoCompleteScrollView;
    private GameObject _messagePrefab;
    private int _selectedAutoComplete;
    private List<string> _commandHistory;
    private int _commandHistoryIdx = -1;
    private Scrollbar _scrollbar;
    private bool _shouldResetTime = false;
    private bool _scrollCompletlyDown;
    
    protected override void OnAwake()
    {
        requiredFeatures = new List<FeatureFlag>() { EnableConsole }.ToArray();
        openActions = new List<MenuActions> { MenuActions.PauseGameFalse, MenuActions.DisableInput }.ToArray();
        closeActions = new List<MenuActions> { MenuActions.UnPauseGameFalse, MenuActions.EnableInput }.ToArray();
    }

    protected override void OnStart()
    {
        SendMessage(translation("console.helloworld"));
        SendMessage(translation("console.info"));
        StarlightLogManager.OnSendMessage += SendMessage;
        StarlightLogManager.OnSendWarning += SendWarning;
        StarlightLogManager.OnSendError += SendError;
    }

    public void Send(string message, Color32 color)
    {
        if (!EnableConsole.HasFlag()) return;
        if (!StarlightEntryPoint.MenusFinished) return;
        try
        {
            if (message.Contains("\n"))
            {
                foreach (string singularLine in message.Split('\n')) SendMessage(singularLine);
                return;
            }
            GameObject instance = Instantiate(_messagePrefab, consoleContent);
            instance.gameObject.SetActive(true);
            instance.transform.GetChild(0).gameObject.SetActive(true);
            instance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = message;
            instance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = color;
            _scrollbar.value = 0f;
            _scrollCompletlyDown = true;
        } catch { }
    }

    private void SendMessage(string message) => Send(message, Color.white);
    private void SendError(string message) => Send(message, Color.red);
    private void SendWarning(string message) => Send(message, Color.yellow);
    


    protected override void OnClose()
    {
        _autoCompleteContent.DestroyAllChildren();
    }

    protected override void OnOpen()
    {
        RefreshAutoComplete(_commandInput.text);
    }


    void RefreshAutoComplete(string text)
    {
        _autoCompleteContent.parent.parent.GetComponent<ScrollRect>().enabled = true; // Make sure that the component is enabled            

        if (_selectedAutoComplete > _autoCompleteContent.childCount - 1)
            _selectedAutoComplete = 0;
        _autoCompleteContent.DestroyAllChildren();
        if (string.IsNullOrWhiteSpace(text))
        {
            ExecuteInTicks((() =>
            {
                _autoCompleteScrollView.SetActive(_autoCompleteContent.childCount != 0);
            }), 1);
            return;
        }

        if (text.Contains(" "))
        {
            string cmd = text.Substring(0, text.IndexOf(' '));
            if (StarlightCommandManager.commands.ContainsKey(cmd))
            {
                var argString = text;
                var split = argString.Split(' ').ToList();
                split.RemoveAt(0);
                int argIndex = split.Count - 1;
                string[] args = null;
                if (split.Count != 0)
                    args = split.ToArray();
                string containing = "";
                if (args != null) containing = args[argIndex];
                List<string> possibleAutoCompletes = null;
                try { possibleAutoCompletes = StarlightCommandManager.commands[cmd].GetAutoComplete(argIndex, args); } catch (Exception e) { LogError($"Error in command auto complete!\n{e}"); }
                if (possibleAutoCompletes != null)
                {
                    possibleAutoCompletes = possibleAutoCompletes.Where(s => s.ToUpper().Contains(containing.ToUpper()))
                        .OrderBy(s => !s.ToUpper().StartsWith(containing.ToUpper()))
                        .ToList();
                    if (possibleAutoCompletes.Count == 0)
                        possibleAutoCompletes = null;
                }
                if (possibleAutoCompletes != null)
                {
                    int maxPredictions = MAX_AUTOCOMPLETE.Get();
                    int predicted = 0;
                    foreach (string argument in possibleAutoCompletes)
                    {
                        if (predicted >= maxPredictions) break;
                        predicted++;
                        GameObject instance = Instantiate(_autoCompleteEntryPrefab, _autoCompleteContent);
                        TextMeshProUGUI textMesh = instance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        if (string.IsNullOrEmpty(containing))
                            textMesh.text =
                                "<alpha=#FF>" + argument +
                                "<alpha=#67>"; // "alpha=#FF" is the normal argument, and the "alpha=#75" is the uncompleted part. DO NOT CHANGE THE SYSTEM, ONLY THE ALPHA VALUES!!!
                        else
                        {
                            textMesh.text = "<alpha=#67>"+new Regex(Regex.Escape(containing), RegexOptions.IgnoreCase).Replace(
                                argument,
                                "<alpha=#FF>" + 
                                argument.Substring(argument.ToUpper().IndexOf(containing.ToUpper(), StringComparison.Ordinal),containing.Length) 
                                + "<alpha=#67>", 1);
                        }
                        instance.SetActive(true);
                        instance.GetComponent<Button>().onClick.AddListener((Action)(() =>
                        {
                            _commandInput.text = cmd;

                            if (args != null)
                            {
                                for (int i = 0; i < args.Length - 1; i++)
                                    _commandInput.text += " " + args[i];
                                _commandInput.text += " " + argument;
                            }

                            _commandInput.MoveToEndOfLine(false, false);
                        }));
                    }
                }
            }
        }
        else
            foreach (var valuePair in StarlightCommandManager.commands)
                if (valuePair.Key.StartsWith(text) && !valuePair.Value.Hidden)
                {
                    GameObject instance = Object.Instantiate(_autoCompleteEntryPrefab, _autoCompleteContent);
                    instance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = valuePair.Key;
                    instance.SetActive(true);
                    instance.GetComponent<Button>().onClick.AddListener((Action)(() =>
                    {
                        _commandInput.text = valuePair.Key;
                        _commandInput.MoveToEndOfLine(false, false);
                    }));
                }

        _autoCompleteScrollView.SetActive(_autoCompleteContent.childCount != 0);
        _autoCompleteContent.parent.parent.GetComponent<ScrollRect>().enabled = false;

    }


    protected override void OnLateAwake()
    {
        _commandHistory = new List<string>();

        consoleContent = transform.GetObjectRecursively<Transform>("ConsoleMenuConsoleContentRec");
        _messagePrefab = transform.GetObjectRecursively<GameObject>("ConsoleMenuTemplateMessageRec");
        _commandInput = transform.GetObjectRecursively<TMP_InputField>("ConsoleMenuCommandInputRec");
        _scrollbar = transform.GetObjectRecursively<Scrollbar>("ConsoleMenuConsoleScrollbarRec");
        _autoCompleteContent = transform.GetObjectRecursively<Transform>("ConsoleMenuAutoCompleteContentRec");
        _autoCompleteEntryPrefab = transform.GetObjectRecursively<GameObject>("ConsoleMenuTemplateAutoCompleteEntryRec");
        _autoCompleteScrollView = transform.GetObjectRecursively<GameObject>("ConsoleMenuAutoCompleteScrollRectRec");
        //autoCompleteScrollView.GetComponent<ScrollRect>().enabled = false;
        //autoCompleteScrollView.SetActive(false);

        _commandInput.onValueChanged.AddListener((Action<string>)((text) =>
        {
            if (text.Contains("\n")) _commandInput.text = text.Replace("\n", "");
            RefreshAutoComplete(text);
        }));
        
        foreach (Transform child in transform.parent.GetChildren())
            child.gameObject.SetActive(false);
        
        _messagePrefab.SetActive(false);
        
        Send("Hello Console!", Color.green);
    }


    protected override void OnUpdate()
    {
        try { if (consoleContent.childCount >= MAX_CONSOLELINES.Get())
            Destroy(consoleContent.GetChild(0).gameObject);
        } catch { }

        _commandInput.ActivateInputField();
        if (_scrollCompletlyDown)
            if (_scrollbar.value != 0)
            {
                _scrollbar.value = 0f;
                _scrollCompletlyDown = false;
            }

        if (LKey.Tab.OnKeyDown())
        {
            if (_autoCompleteContent.childCount != 0)
                try
                {
                    _autoCompleteContent.GetChild(_selectedAutoComplete).GetComponent<Button>().onClick
                        .Invoke();
                    _selectedAutoComplete = 0;
                }
                catch { }

        }

        if (LKey.KeypadEnter.OnKeyDown())
            if (_commandInput.text != "")
                Execute();

        if (_commandHistoryIdx != -1 && !_autoCompleteScrollView.active)
        {
            if (LKey.UpArrow.OnKeyDown())
            {
                _commandInput.text = _commandHistory[_commandHistoryIdx];
                _commandInput.MoveToEndOfLine(false, false);
                RefreshAutoComplete(_commandInput.text);
                _commandHistoryIdx -= 1;
                _autoCompleteScrollView.SetActive(false);
            }
        }

        if (_autoCompleteContent.childCount != 0 && _autoCompleteScrollView.active)
        {
            if (LKey.DownArrow.OnKeyDown())
                NextAutoComplete();

            if (LKey.UpArrow.OnKeyDown())
                PrevAutoComplete();
        }

        if (_selectedAutoComplete == _autoCompleteContent.childCount)
        {
            _selectedAutoComplete = 0;
        }

        if (_scrollbar != null)
        {
            float value = Mouse.current.scroll.ReadValue().y;
            if (Mouse.current.scroll.ReadValue().y != 0)
                _scrollbar.value =
                    Mathf.Clamp(
                        _scrollbar.value + ((value > 0.01 ? StarlightEntryPoint.consoleMaxSpeed : value < -0.01 ? -StarlightEntryPoint.consoleMaxSpeed : 0) *
                                            _scrollbar.size), 0, 1f);

        }

        try
        {
            if (_autoCompleteContent.childCount != 0)
            {
                _autoCompleteContent.GetChild(_selectedAutoComplete).GetComponent<Image>().color =
                    new Color32(255, 211, 0, 120);
                if (_selectedAutoComplete > MAX_AUTOCOMPLETEONSCREEN.Get())
                    _autoCompleteContent.position = new Vector3(_autoCompleteContent.position.x,
                        ((744f / 1080f) * Screen.height) - (27 * MAX_AUTOCOMPLETEONSCREEN.Get()) +
                        (27 * _selectedAutoComplete),
                        _autoCompleteContent.position.z);

                else
                    _autoCompleteContent.position = new Vector3(_autoCompleteContent.position.x,
                        ((744f / 1080f) * Screen.height),
                        _autoCompleteContent.position.z);
            }
        }
        catch
        {
        }
    }


    void NextAutoComplete()
    {
        if (!isOpen) return;
        _selectedAutoComplete += 1;
        if (_selectedAutoComplete > _autoCompleteContent.childCount - 1)
        {
            _selectedAutoComplete = 0;
            _autoCompleteContent.GetChild(_autoCompleteContent.childCount - 1).GetComponent<Image>().color =
                new Color32(0, 0, 0, 25);
            _autoCompleteContent.GetChild(_selectedAutoComplete).GetComponent<Image>().color =
                new Color32(255, 211, 0, 120);
        }
        else
        {
            _autoCompleteContent.GetChild(_selectedAutoComplete - 1).GetComponent<Image>().color =
                new Color32(0, 0, 0, 25);
            _autoCompleteContent.GetChild(_selectedAutoComplete).GetComponent<Image>().color =
                new Color32(255, 211, 0, 120);
        }
    }

    void PrevAutoComplete()
    {
        if (!isOpen) return;
        _selectedAutoComplete -= 1;

        if (_selectedAutoComplete < 0)
        {
            _selectedAutoComplete = _autoCompleteContent.childCount - 1;
            _autoCompleteContent.GetChild(0).GetComponent<Image>().color = new Color32(0, 0, 0, 25);
            _autoCompleteContent.GetChild(_selectedAutoComplete).GetComponent<Image>().color =
                new Color32(255, 211, 0, 120);
        }
        else
        {
            _autoCompleteContent.GetChild(_selectedAutoComplete + 1).GetComponent<Image>().color =
                new Color32(0, 0, 0, 25);
            _autoCompleteContent.GetChild(_selectedAutoComplete).GetComponent<Image>().color =
                new Color32(255, 211, 0, 120);
        }
    }

    void Execute()
    {
        if (!EnableConsole.HasFlag()) return;
        string cmds = _commandInput.text;
        _commandHistory.Add(cmds);
        _commandHistoryIdx = _commandHistory.Count - 1;
        _commandInput.text = "";
        _autoCompleteContent.DestroyAllChildren();
        StarlightCommandManager.ExecuteByString(cmds);
    }


}
