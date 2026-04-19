using System;
using System.Linq;
using Il2CppMonomiPark.SlimeRancher.Economy;
using Il2CppMonomiPark.SlimeRancher.Input;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppSystem.Linq;
using Il2CppTMPro;
using Starlight.Buttons;
using Starlight.Commands;
using Starlight.Components;
using Starlight.Enums;
using Starlight.Enums.Features;
using Starlight.Enums.Sounds;
using Starlight.Managers;
using Starlight.Storage;
using UnityEngine.UI;

namespace Starlight.Menus;

public class StarlightCheatMenu : StarlightMenu
{
    public new static MenuIdentifier GetMenuIdentifier() => new MenuIdentifier("cheatmenu",StarlightMenuFont.SR2,StarlightMenuTheme.Default,"CheatMenu");
    protected override bool createCommands => true;
    protected override bool inGameOnly => true;
    
    protected override void OnAwake()
    {
        requiredFeatures = new List<FeatureFlag>() { EnableCheatMenu }.ToArray();
        openActions = new List<MenuActions> { MenuActions.PauseGame, MenuActions.HideMenus }.ToArray();
        closeActions = new List<MenuActions> { MenuActions.UnPauseGame, MenuActions.UnHideMenus, MenuActions.EnableInput }.ToArray();
    }
    
    
    internal static readonly List<StarlightCheatMenuButton> cheatButtons = new();
    private readonly List<CheatMenuRefineryEntry> _refineryEntries = new();
    private readonly List<CheatMenuGadgetEntry> _gadgetEntries = new();
    private Transform _cheatButtonContent;
    private Transform _refineryContent;
    private Transform _gadgetsContent;
    private Transform _warpsContent;
    private GameObject _buttonTemplate;
    private GameObject _refineryEntryTemplate;
    private GameObject _gadgetsEntryTemplate;
    private StarlightCheatMenuButton _refillButton;
    private StarlightCheatMenuButton _noclipButton;
    private StarlightCheatMenuButton _infEnergyButton;
    private StarlightCheatMenuButton _infHealthButton;
    private StarlightCheatMenuButton _removeFogButton;
    //private StarlightCheatMenuButton _betterScreenshotButton;
    internal static bool RemoveFog;
    //internal static bool BetterScreenshot;
    private InputEvent _inputDown;
    private InputEvent _inputUp;
    
    protected override void OnClose()
    {
        gameObject.GetObjectRecursively<Button>("CheatMenuMainSelectionButtonRec").onClick.Invoke();
        _refineryContent.DestroyAllChildren();
        _gadgetsContent.DestroyAllChildren();
        _cheatButtonContent.DestroyAllChildren();
        _warpsContent.DestroyAllChildren();
    }
    
    protected override void OnOpen()
    {
        if(StarlightCounterGateManager.disableCheats)
        {
            Close();
            return;
        }
        //Refinery
        List<IdentifiableType> refineryItems = sceneContext.GadgetDirector._refineryTypeGroup.GetAllMembers().ToArray().ToList();
        refineryItems.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));
        foreach (IdentifiableType refineryItem in refineryItems)
        {
            GameObject entry = Instantiate(_refineryEntryTemplate, _refineryContent);
            entry.SetActive(true);
            entry.AddComponent<CheatMenuRefineryEntry>();
            entry.GetComponent<CheatMenuRefineryEntry>().item = refineryItem;
            entry.GetComponent<CheatMenuRefineryEntry>().OnOpen();
            _refineryEntries.Add(entry.GetComponent<CheatMenuRefineryEntry>());
        }
        //Gadgets

        List<IdentifiableType> gadgetItems = sceneContext.GadgetDirector._gadgetsGroup.GetAllMembers().ToArray().ToList();
        gadgetItems.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));
        foreach (IdentifiableType gadgetItem in gadgetItems)
        {
            GameObject entry = Instantiate(_gadgetsEntryTemplate, _gadgetsContent);
            entry.SetActive(true);
            entry.AddComponent<CheatMenuGadgetEntry>();
            entry.GetComponent<CheatMenuGadgetEntry>().item = gadgetItem;
            entry.GetComponent<CheatMenuGadgetEntry>().OnOpen();
            _gadgetEntries.Add(entry.GetComponent<CheatMenuGadgetEntry>());
        }
        
        
        //Cheat Buttons
        foreach (StarlightCheatMenuButton cheatButton in cheatButtons)
        {
            GameObject button = Instantiate(_buttonTemplate, _cheatButtonContent);
            button.SetActive(true);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cheatButton.label;
            button.GetComponent<Button>().onClick.AddListener((Action)(() =>
            {
                cheatButton.action.Invoke();
            }));
            cheatButton.textInstance = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            cheatButton.buttonInstance = button.GetComponent<Button>();
        }


        _noclipButton.textInstance.text = translation("cheatmenu.cheatbuttons.noclip" + (sceneContext.Camera.GetComponent<NoClipComponent>() == null ? "off" : "on"));
        _refillButton.textInstance.text = translation("cheatmenu.cheatbuttons.refillinv");
        if (EnableInfHealth.HasFlag()) _infHealthButton.textInstance.text = translation("cheatmenu.cheatbuttons.infhealth" + (InfiniteHealthCommand.infHealth? "on" : "off"));
        if (EnableInfEnergy.HasFlag()) _infEnergyButton.textInstance.text = translation("cheatmenu.cheatbuttons.infenergy" + (InfiniteEnergyCommand.infEnergy? "on" : "off"));
        _removeFogButton.textInstance.text = translation("cheatmenu.cheatbuttons.removeFog" + (RemoveFog? "on" : "off"));
        //_betterScreenshotButton.textInstance.text = translation("cheatmenu.cheatbuttons.betterScreenshot" + (BetterScreenshot? "on" : "off"));

        
        //Warp Buttons
        foreach (KeyValuePair<string,Warp> pair in StarlightSaveManager.data.warps.OrderBy(x => x.Key))
        {
            GameObject button = Instantiate(_buttonTemplate, _warpsContent);
            button.SetActive(true);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pair.Key;
            button.GetComponent<Button>().onClick.AddListener((Action)(() =>
                {
                    AudioEUtil.PlaySound(MenuSound.Click);
                    Close();        
                    StarlightError error = pair.Value.WarpPlayerThere();
                    switch (error)
                    {
                        case StarlightError.TeleportablePlayerNull: StarlightLogManager.SendError(translation("cmd.error.teleportableplayernull")); break;
                        case StarlightError.SRCharacterControllerNull: StarlightLogManager.SendError(translation("cmd.error.srccnull")); break;
                    }
                }));
        }

        //Cheat Slots
        int i = -1;
        var cheatSlots = transform.GetObjectRecursively<Transform>("CheatMenuSlotsContentRec");
        cheatSlots.DestroyAllChildren();
        var cheatSlotPrefab = transform.GetObjectRecursively<Transform>("CheatMenuStatsSlotTemplateEntry");
        foreach (var slot in sceneContext.PlayerState.Ammo.Slots)
        {
            i += 1;
            if (slot == null) continue;
            if (!slot.IsUnlocked) continue;
            var instance = Instantiate(cheatSlotPrefab, cheatSlots);
            instance.gameObject.SetActive(true);
            var cSlot = instance.AddComponent<CheatMenuSlot>();
            cSlot.OnOpen(i);
        }
        var currencyPrefab = transform.GetObjectRecursively<Transform>("CheatMenuStatsCurrencyRec");
        foreach (var curr in gameContext.LookupDirector.CurrencyList._currencies)
        {
            if (!curr) continue;
            var instance = Instantiate(currencyPrefab, cheatSlots);
            instance.gameObject.SetActive(true);
            var cSlot = instance.AddComponent<CheatMenuCurrency>();
            cSlot.OnOpen(curr.ReferenceId);
        }

        i = 0;
    }
    public override void OnCloseUIPressed()
    {
        if (MenuEUtil.isAnyPopUpOpen) return;
        
        Close();
    }

    public override void AfterGameContext(GameContext gameContext)
    {
        _inputDown = Get<InputEvent>("ItemDown");
        _inputUp = Get<InputEvent>("ItemUp");
        var refScroll = _refineryContent.parent.parent;
        if (!refScroll.HasComponent<ScrollByMenuKeys>())
        {
            var comp = refScroll.gameObject.AddComponent<ScrollByMenuKeys>();
            comp._scrollDownInput = _inputDown;
            comp._scrollUpInput = _inputUp;
            comp._scrollPerFrame = 9f;
        }
        var gadgetScroll = _gadgetsContent.parent.parent;
        if (!gadgetScroll.HasComponent<ScrollByMenuKeys>())
        {
            var comp = gadgetScroll.gameObject.AddComponent<ScrollByMenuKeys>();
            comp._scrollDownInput = _inputDown;
            comp._scrollUpInput = _inputUp;
            comp._scrollPerFrame = 9f;
        }
    }

    protected override void OnLateAwake()
    {
        _cheatButtonContent = transform.GetObjectRecursively<Transform>("CheatMenuCheatButtonsContentRec");
        _refineryContent = transform.GetObjectRecursively<Transform>("CheatMenuRefineryContentRec");
        _warpsContent = transform.GetObjectRecursively<Transform>("CheatMenuWarpsContentRec");
        _gadgetsContent = transform.GetObjectRecursively<Transform>("CheatMenuGadgetContentRec");
        _buttonTemplate = transform.GetObjectRecursively<GameObject>("CheatMenuTemplateButton");
        _refineryEntryTemplate = transform.GetObjectRecursively<GameObject>("CheatMenuRefineryTemplateEntry");
        _gadgetsEntryTemplate = transform.GetObjectRecursively<GameObject>("CheatMenuGadgetsTemplateEntry");
        
        CheatButtons();

        
        var button1 = transform.GetObjectRecursively<Image>("CheatMenuMainSelectionButtonRec");
        //button1.sprite = whitePillBg;
        button1.GetComponent<Button>().onClick.AddListener(selectCategorySound);
        var button2 = transform.GetObjectRecursively<Image>("CheatMenuRefinerySelectionButtonRec");
        //button2.sprite = whitePillBg;
        button2.GetComponent<Button>().onClick.AddListener(selectCategorySound);
        var button3 = transform.GetObjectRecursively<Image>("CheatMenuGadgetsSelectionButtonRec");
        //button3.sprite = whitePillBg;
        button3.GetComponent<Button>().onClick.AddListener(selectCategorySound);
        var button4 = transform.GetObjectRecursively<Image>("CheatMenuSpawnSelectionButtonRec");
        //button4.sprite = whitePillBg;
        button4.GetComponent<Button>().onClick.AddListener(selectCategorySound);
        ToTranslate.Add(button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"cheatmenu.category.main");
        ToTranslate.Add(button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"cheatmenu.category.refinery");
        ToTranslate.Add(button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"cheatmenu.category.gadgets");
        ToTranslate.Add(button4.transform.GetChild(0).GetComponent<TextMeshProUGUI>(),"cheatmenu.category.spawn");
        ToTranslate.Add(transform.GetObjectRecursively<TextMeshProUGUI>("TitleTextRec"),"cheatmenu.title");
    }
    void CheatButtons()
    {
        if (EnableInfEnergy.HasFlag()) _infEnergyButton = new StarlightCheatMenuButton(translation("cheatmenu.cheatbuttons.infenergyoff"),
            () =>
        {
            AudioEUtil.PlaySound(MenuSound.Click);
            StarlightCommandManager.ExecuteByString("infenergy", true,true);
            _infEnergyButton.textInstance.text = translation("cheatmenu.cheatbuttons.infenergy" + (InfiniteEnergyCommand.infEnergy? "on" : "off"));
        });
        if (EnableInfHealth.HasFlag()) _infHealthButton = new StarlightCheatMenuButton(translation("cheatmenu.cheatbuttons.infhealthoff"),
            () =>
        {
            AudioEUtil.PlaySound(MenuSound.Click);
            StarlightCommandManager.ExecuteByString("infhealth", true,true);
            _infHealthButton.textInstance.text = translation("cheatmenu.cheatbuttons.infhealth" + (InfiniteHealthCommand.infHealth? "on" : "off"));
            });
        _removeFogButton = new StarlightCheatMenuButton(translation("cheatmenu.cheatbuttons.removeFogoff"),
            () =>
            {
                AudioEUtil.PlaySound(MenuSound.Click);
                RemoveFog = !RemoveFog;
                _removeFogButton.textInstance.text = translation("cheatmenu.cheatbuttons.removeFog" + (RemoveFog? "on" : "off"));
            });
        /*_betterScreenshotButton = new StarlightCheatMenuButton(translation("cheatmenu.cheatbuttons.betterScreenshotoff"),
            () =>
            {
                AudioEUtil.PlaySound(MenuSound.Click);
                BetterScreenshot = !BetterScreenshot;
                _betterScreenshotButton.textInstance.text = translation("cheatmenu.cheatbuttons.betterScreenshot" + (BetterScreenshot? "on" : "off"));
            });*/
        _noclipButton = new StarlightCheatMenuButton(translation("cheatmenu.cheatbuttons.noclipoff"),
            () =>
            {
                AudioEUtil.PlaySound(MenuSound.Click);
                StarlightCommandManager.ExecuteByString("noclip", true,true);
                _noclipButton.textInstance.text = translation("cheatmenu.cheatbuttons.noclip" + (sceneContext.Camera.GetComponent<NoClipComponent>()!=null ? "on" : "off"));
            });
        _refillButton = new StarlightCheatMenuButton(translation("cheatmenu.cheatbuttons.refillinv"), () =>
        {
            AudioEUtil.PlaySound(MenuSound.Click);
            StarlightCommandManager.ExecuteByString("refillinv", true,true);
        });
        
    }
}