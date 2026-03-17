using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Il2CppInterop.Runtime.Injection;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppTMPro;
using UnityEngine.UI;
using Il2CppMonomiPark.ScriptedValue;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.UI.ButtonBehavior;
using MelonLoader;
using MelonLoader.Utils;
using Starlight.Expansion;
using Starlight.Components;
using Starlight.Enums;
using Starlight.Managers;
using Starlight.Menus;
using Starlight.Menus.Debug;
using Starlight.Patches.General;
using Starlight.Patches.InGame;
using Starlight.Prism;
using Starlight.Prism.Lib;
using Starlight.Storage;

namespace Starlight;


// Starlight Build information. Please do not edit anything other than version numbers.
public static class BuildInfo
{
    public const string Name = "Starlight Core Essentials";
    public const string Description = "Essential stuff for Slime Rancher 2";
    public const string Author = "ThatFinn";
    public const string CoAuthors = "YLohkuhl";
    public const string Contributors = "PinkTarr, shizophrenicgopher, Atmudia";
    public const string CodeVersion = "4.0.0";
    public const string DownloadLink = "https://starlight.sr2.dev/";
    public const string SourceCode = "https://github.com/ThatFinnDev/Starlight";
    public const string Nexus = "https://www.nexusmods.com/slimerancher2/mods/60";
    public const string Discord = "https://discord.gg/a7wfBw5feU";

    /// <summary>
    /// Should be the same as CodeVersion unless this is non release build.<br />
    /// For alpha versions, add "-alpha.buildnumber" e.g 3.0.0-alpha.5<br />
    /// For beta versions, add "-beta.buildnumber" e.g 3.0.0-beta.12<br />
    /// For dev versions, use "-dev". Do not add a build number!<br />
    /// Add "+metadata" only in dev builds!
    /// </summary>
    public const string DisplayVersion = "4.0.0-dev";

    // Allow Metadata, Check Update Link
    internal static readonly Dictionary<string, (bool, string)> PreInfo = new()
    {
        { "release", (false, "https://api.starlight.sr2.dev/branch/release") },
        { "alpha", (false, "https://api.starlight.sr2.dev/branch/alpha") },
        { "beta", (false, "https://api.starlight.sr2.dev/branch/beta") },
        { "dev", (true, "") }
    };
}
public class StarlightEntryPoint : MelonMod
{
    private static string _starlightFolderName = "Starlight";
    internal static bool changedUserFolder { get; private set; }
    internal static readonly Dictionary<Assembly, (Dictionary<StarlightExpansionVXX,StarlightPackageInfo>, HarmonyLib.Harmony)> Expansions = new();
    internal static readonly List<StarlightExpansionV01> ExpansionV01S = new();
    internal static readonly List<(string, Assembly, string, string)> BrokenExpansions = new();


    internal static bool GameContextStarted;
    
    internal static TMP_FontAsset Sr2FontAsset;
    internal static TMP_FontAsset NormalFont;
    internal static TMP_FontAsset RegularFont;
    internal static TMP_FontAsset BoldFont;
    internal static TMP_FontAsset NotoSansFont;
    
    internal static string UpdateBranch = "release";
    
    internal static bool MenusFinished = false;

    internal static readonly List<BaseUI> BaseUIAddSliders = new();
    internal static Dictionary<StarlightMenu, Dictionary<string, object>> Menus = new();
    private static MelonPreferences_Category _prefs;
    
    internal static bool MainMenuLoaded;
    internal static GameObject StarlightStuff;
    internal static ScriptedBool SaveSkipIntro;
    internal static bool AddedButtons = false;
    
    internal static string dataPath => Path.Combine(MelonEnvironment.UserDataDirectory, _starlightFolderName);
    internal static string tmpDataPath => Path.Combine(dataPath, ".tmp");
    internal static string flagDataPath => Path.Combine(dataPath, "flags");
    internal static string customVolumeProfilesPath => Path.Combine(dataPath, "customVolumeProfiles");

    internal static bool EarlyRegistered = false;

    public static bool isPrismInUse { get; private set; }
    internal static bool ShouldEnablePrism;

    private static readonly MelonLogger.Instance UnityLog = new("Unity");
    
    internal static string MelonVersion = "undefined";
    
    internal static StarlightEntryPoint Instance;

    internal static string onSaveLoadCommand => _prefs.GetEntry<string>("onSaveLoadCommand").Value;
    internal static string onMainMenuLoadCommand => _prefs.GetEntry<string>("onMainMenuLoadCommand").Value;
    internal static bool starlightLogToMlLog => _prefs.GetEntry<bool>("StarlightLogToMLLog").Value;
    internal static bool mLLogToStarlightLog => _prefs.GetEntry<bool>("mLLogToStarlightLog").Value;
    internal static bool autoUpdate => _prefs.GetEntry<bool>("autoUpdate").Value;
    internal static bool disableFixSaves => _prefs.GetEntry<bool>("disableFixSaves").Value;
    internal static float consoleMaxSpeed => _prefs.GetEntry<float>("consoleMaxSpeed").Value;
    internal static float noclipAdjustSpeed => _prefs.GetEntry<float>("noclipAdjustSpeed").Value;
    internal static float noclipSpeedMultiplier => _prefs.GetEntry<float>("noclipSpeedMultiplier").Value;
    internal static bool enableDebugDirector => _prefs.GetEntry<bool>("enableDebugDirector").Value;
 
    public override void OnEarlyInitializeMelon()
    {
        Instance = this;
        if (!IsDisplayVersionValid())
        {
            Log("Version Code is broken!");
            Unregister();
            return;
        }
        string[] launchArgs = Environment.GetCommandLineArgs();
        var usedArgs = new List<string>();
        foreach (var arg in launchArgs)
            if (arg.StartsWith("-starlight.") && arg.Contains("="))
            {
                var split = arg.Split("=");
                if (split.Length != 2) continue;
                if (usedArgs.Contains(split[0])) continue;
                usedArgs.Add(split[0]);
                switch (split[0])
                {
                    case "-starlight.id":
                        var id = 0;
                        try { id = int.Parse(split[1]); } catch { }
                        if (id != 0)
                        {
                            _starlightFolderName += id;
                            changedUserFolder = true;
                        }
                        break;
                }
            }

        if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
        if (!Directory.Exists(tmpDataPath)) Directory.CreateDirectory(tmpDataPath);
        if (!Directory.Exists(flagDataPath)) Directory.CreateDirectory(flagDataPath);
        if (!Directory.Exists(customVolumeProfilesPath)) Directory.CreateDirectory(customVolumeProfilesPath);
        
        InitFlagManager();
        StarlightPackageManager.LoadAllExpansions();
        
        StarlightCallEventManager.LoadAssemblies(Expansions.Keys.ToList());
        PatchIl2CppDetourMethodPatcher.InstallSecondPart(HarmonyInstance);
    }

    public override void OnInitializeMelon()
    {
        _prefs = MelonPreferences.CreateCategory("Starlight", "Starlight");
        var path = MelonAssembly.Assembly.Location + ".old";
        if (File.Exists(path)) File.Delete(path);
        RefreshPrefs();

        InjectIl2CppComponents();
        
        
        if (!ShouldEnablePrism)
            try { ShouldEnablePrism = _prefs.GetEntry<bool>("forceUsePrism").Value; } catch { }

        if (ShouldEnablePrism) isPrismInUse = true;
        if (!AllowPrism.HasFlag()) isPrismInUse = false;
        PatchGame(HarmonyInstance,MelonAssembly.Assembly);
        
        try { WorldPopulatorErrorPatch.Apply(HarmonyInstance); }
        catch (Exception e) { LogError(e); }
        foreach (var pair in Expansions)
            PatchGame(pair.Value.Item2,pair.Key);
        

        Application.add_logMessageReceived(new Action<string, string, LogType>(AppLogUnity));
        try { AddLanguages(EmbeddedResourceEUtil.LoadString("translations.csv")); }
        catch (Exception e) { LogError(e); }

        foreach (var expansion in ExpansionV01S)
            try { expansion.OnInitialize(); }
            catch (Exception e) { LogError(e); }
    }
    
    
    
    public override void OnLateInitializeMelon()
    {
        if (Get<GameObject>("StarlightPrefabHolder"))
        {
            PrefabHolder = Get<GameObject>("StarlightPrefabHolder");
        }
        else
        {
            PrefabHolder = new GameObject();
            PrefabHolder.SetActive(false);
            PrefabHolder.name = "StarlightPrefabHolder";
            Object.DontDestroyOnLoad(PrefabHolder);
        }

        if (LKeyInputAcquirer.Instance == null)
        {
            var ia = new GameObject();
            ia.AddComponent<LKeyInputAcquirer>();
            ia.AddComponent<KeyCodeInputAcquirer>();
            if (RestoreDebugAbilities.HasFlag()) ia.AddComponent<DevelopmentBuildText>();
            if (RestoreDebugDevConsole.HasFlag()) ia.AddComponent<DevConsoleFixer>();
            ia.name = "StarlightInputAcquirer";
            Object.DontDestroyOnLoad(ia);
        }

        if (CheckForUpdates.HasFlag()) StartCoroutine(StarlightUpdateManager.GetBranchJson());

        foreach (var expansion in ExpansionV01S)
            try { expansion.OnLateInitializeMelon(); }
            catch (Exception e) { LogError(e); }
    }
    
    
    

    private static bool IsDisplayVersionValid()
    {
        if (!BuildInfo.DisplayVersion.Contains(BuildInfo.CodeVersion)) return false;
        /*Semver2 Regex*/
        var semVerRegex = new Regex(@"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*))?(?:\+(?<build>[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*))?$");
        var match = semVerRegex.Match(BuildInfo.DisplayVersion);
        /*Not Semver2*/
        if (!match.Success) return false;
        var metadata = match.Groups["build"].Value;
        var hasMetadata = !string.IsNullOrEmpty(metadata);
        var preReleaseAndBuild = match.Groups["prerelease"].Value;
        if (string.IsNullOrEmpty(preReleaseAndBuild)) return !hasMetadata; /*release and no meta*/
        /*No release -> continue*/
        var dotIndex = preReleaseAndBuild.IndexOf('.');
        if (!(dotIndex != -1 && preReleaseAndBuild.LastIndexOf('.') == dotIndex && dotIndex != 0 &&
              dotIndex != preReleaseAndBuild.Length - 1))
            if (preReleaseAndBuild != "dev")
                return false; /*Has no dot to indicate buildnumber*/
        var preRelease = preReleaseAndBuild != "dev" ? preReleaseAndBuild.Substring(0, dotIndex) : "dev";
        /*Check pre and meta*/
        var valid = false;
        foreach (var triple in BuildInfo.PreInfo)
            if (preRelease == triple.Key && triple.Key != "release")
            {
                if (!triple.Value.Item1 && hasMetadata) return false; //Has meta even though it's not allowed
                valid = true;
                UpdateBranch = triple.Key;
                break;
            }

        if (!valid) return false;
        if (preRelease == "dev") return true;
        /*Check buildnumber*/
        var buildnumber = preReleaseAndBuild.Substring(dotIndex + 1);
        if (int.TryParse(buildnumber, out _)) return true;
        return false; /*buildnumber is no int*/
    }
    
    private static void RefreshPrefs()
    {
        if (AllowAutoUpdate.HasFlag())
            if (!_prefs.HasEntry("autoUpdate"))
                _prefs.CreateEntry("autoUpdate", false, "Update Starlight automatically");
        if (DevMode.HasFlag())
            if (AllowPrism.HasFlag())
                if (!_prefs.HasEntry("forceUsePrism"))
                    _prefs.CreateEntry("forceUsePrism", false, "Force enable prism",
                        "It's automatically enabled if expansions need it. This will just force it.");
        if (!_prefs.HasEntry("disableFixSaves"))
            _prefs.CreateEntry("disableFixSaves", false, "Disable save fixing", false).AddNullAction();
        if (!_prefs.HasEntry("enableDebugDirector"))
            _prefs.CreateEntry("enableDebugDirector", false, "Enable debug menu", false).AddAction(() =>
            {
                StarlightDebugUI.isEnabled = enableDebugDirector;
            });
        if (!_prefs.HasEntry("mLLogToStarlightLog"))
            _prefs.CreateEntry("mLLogToStarlightLog", false, "Send MLLogs to console", false).AddNullAction();
        if (!_prefs.HasEntry("StarlightLogToMLLog"))
            _prefs.CreateEntry("StarlightLogToMLLog", false, "Send console messages to MLLogs", false).AddNullAction();
        if (!_prefs.HasEntry("onSaveLoadCommand"))
            _prefs.CreateEntry("onSaveLoadCommand", "", "Command to execute, when save is loaded", false)
                .AddNullAction();
        if (!_prefs.HasEntry("onMainMenuLoadCommand"))
            _prefs.CreateEntry("onMainMenuLoadCommand", "", "Command to execute, when main menu is loaded",
                false).AddNullAction();
        if (!_prefs.HasEntry("noclipSpeedMultiplier"))
            _prefs.CreateEntry("noclipSpeedMultiplier", 2f, "NoClip sprint speed multiplier", false).AddNullAction();
        if (!_prefs.HasEntry("noclipAdjustSpeed"))
            _prefs.CreateEntry("noclipAdjustSpeed", 235f, "NoClip scroll speed", false).AddNullAction();
        if (!_prefs.HasEntry("consoleMaxSpeed"))
            _prefs.CreateEntry("consoleMaxSpeed", 0.75f, "Console scroll speed", false).AddNullAction();
        //if(DevMode.HasFlag()) if (!prefs.HasEntry("testLKey")) prefs.CreateEntry("testLKey", LKey.None, "Test LKey", false).AddNullAction();
    }


    // Original logging code from Atmudia, heavily adapted
    private static void AppLogUnity(string message, string trace, LogType type)
    {
        if (!ShowUnityErrors.HasFlag()) return;
        if (message.Equals(string.Empty)) return;
        var toDisplay = message;
        if (trace.StartsWith("TMPro.TextMeshProUGUI.Rebuild (UnityEngine.UI.CanvasUpdate update)")) return;
        if (!string.IsNullOrWhiteSpace(trace))
        {
            toDisplay += "\n" + trace;
            if (message.StartsWith(
                    "Coroutine couldn't be started because the the game object 'EngagementPrompt' is inactive!") &&
                trace.StartsWith("MonomiPark.SlimeRancher.Platform.StandaloneContext:InitializePlatformForCurrentUser"))
                return;
        }

        toDisplay = Regex.Replace(toDisplay, @"\[INFO]\s|\[ERROR]\s|\[WARNING]\s", "");
        switch (type)
        {
            case LogType.Assert: UnityLog.Error(toDisplay); break;
            case LogType.Exception: UnityLog.Error(toDisplay); break;
            case LogType.Log: UnityLog.Msg(toDisplay); break;
            case LogType.Error: UnityLog.Error(toDisplay); break;
            case LogType.Warning: UnityLog.Warning(toDisplay); break;
        }
    }

    private void InjectIl2CppComponents()
    {
        var types = AccessTools.GetTypesFromAssembly(MelonAssembly.Assembly);
        foreach (var type in types)
        {
            if (type == null) continue;
            try
            {
                if (type.GetCustomAttribute<InjectIntoIL>() == null) continue;
                if (!ClassInjector.IsTypeRegisteredInIl2Cpp(type))
                    ClassInjector.RegisterTypeInIl2Cpp(type, new RegisterTypeOptions() { LogSuccess = false });
            }
            catch (Exception e)
            {
                LogError(e);
                LogError($"Failed to inject {type.FullName}: {e.Message}");
            }
        }
    }

    private void PatchGame(HarmonyLib.Harmony harmony,Assembly assembly)
    {
        var types = AccessTools.GetTypesFromAssembly(assembly);
        var devPatches = DevMode.HasFlag();
        foreach (var type in types)
        {
            if (type == null) continue;
            try
            {
                var isPrismPatch = type.GetCustomAttribute<PrismPatch>() != null;
                if (!isPrismInUse && isPrismPatch) continue;
                if (!devPatches && type.GetCustomAttribute<DevPatch>() != null) continue;
                var classPatches = HarmonyMethodExtensions.GetFromType(type);
                if (classPatches.Count > 0)
                {
                    var processor = harmony.CreateClassProcessor(type);
                    processor.Patch();
                }
            }
            catch (Exception e)
            {
                LogError(e);
                LogError($"Failed to patch {type.FullName}: {e.Message}");
            }
        }
    }

    



    public override void OnApplicationQuit()
    {
        try { if (systemContext.SceneLoader.IsCurrentSceneGroupGameplay()) autoSaveDirector.SaveGame(); } catch { }

        foreach (var expansion in ExpansionV01S)
            try { expansion.OnApplicationQuit(); }
            catch (Exception e) { LogError(e); }
    }

    internal static void CheckFallBackFont()
    {
        try
        {
            if (!NotoSansFont)
            {
                var settings = Get<TMP_Settings>("TMP Settings");
                if (!settings) return;
                var tempPath = Path.Combine(tmpDataPath, "tmpFallbackFont.ttf");
                File.WriteAllBytes(tempPath, EmbeddedResourceEUtil.LoadResource("Assets.NotoSans.ttf"));
                var tempFont = new Font(tempPath);
                NotoSansFont = TMP_FontAsset.CreateFontAsset(tempFont);
                //settings.m_fallbackFontAssets.Add(fallBackFont);, creates issues for some reason :(
                settings.m_warningsDisabled = true;
            }

            foreach (var fontAsset in GetAll<TMP_FontAsset>())
            {
                if (fontAsset == NotoSansFont) continue;
                if (!fontAsset.fallbackFontAssetTable.Contains(NotoSansFont))
                    fontAsset.fallbackFontAssetTable.Add(NotoSansFont);
            }
        } catch { }
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        CheckFallBackFont();
        if (DebugLogging.HasFlag()) Log("OnLoaded Scene: " + sceneName);

        try { StarlightWarpManager.OnSceneLoaded(); }
        catch (Exception e) { LogError(e); }

        if (sceneName is "StandaloneStart" or "CompanyLogo" or "LoadScene")
            try
            {
                if (MenuEUtil.isAnyMenuOpen) MenuEUtil.CloseOpenMenu();
                if (MenuEUtil.isAnyPopUpOpen) ;
                MenuEUtil.CloseOpenPopUps();
            } catch { }

        switch (sceneName)
        {
            case "MainMenuUI":
                StarlightVolumeProfileManager.OnMainMenuUILoad();
                //For some reason there are 2 configurations? And due to Il2CPP, just patching the Getter via Harmony isn't sufficient
                foreach (var configuration in GetAll<AutoSaveDirectorConfiguration>())
                    configuration._saveSlotCount = SAVESLOT_COUNT.Get();

                NativeEUtil.CustomTimeScale = 1f;
                Time.timeScale = 1;
                try
                {
                    var b = Get<ButtonBehaviorViewHolder>("SaveGameSlotButton");
                    ExecuteInTicks(() =>
                    {
                        if (b != null)
                        {
                            var l = b.gameObject.GetObjectRecursively<LayoutElement>("Icon");
                            l.minWidth = l.preferredWidth;
                        }
                    }, 3);
                }
                catch (Exception e)
                {
                    LogError(e);
                }

                /*try
                {
                    var scroll = StarlightStuff.GetObjectRecursively<Scrollbar>("saveFilesSliderRec");
                    var styler = scroll.AddComponent<ScrollbarStyler>();
                    foreach (var sstyler in GetAll<ScrollbarStyler>())
                    {
                        if (sstyler._style == null) continue;
                        styler._style = sstyler._style;
                        scroll.colors = sstyler.GetComponent<Scrollbar>().colors;
                    }
                }
                catch (Exception e)
                {
                    LogError(e);
                    LogError("There was a problem applying styles to the save slider!");
                }*/
                break;
        }

        if (isPrismInUse)
            try { PrismShortcuts.OnSceneWasLoaded(buildIndex, sceneName); }
            catch (Exception e) { LogError(e); }
        
        switch (sceneName)
        {
            case "StandaloneEngagementPrompt":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnStandaloneEngagementPromptLoad(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "PlayerCore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnPlayerCoreLoad(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "UICore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnUICoreInitialize(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "MainMenuUI":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnMainMenuUIInitialize(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "LoadScene":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnLoadSceneInitialize(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "ZoneCore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnZoneCoreInitialized(); }
                    catch (Exception e) { LogError(e); }

                break;
        }

        foreach (var expansion in ExpansionV01S)
            try { expansion.OnSceneWasLoaded(buildIndex, sceneName); }
            catch (Exception e) { LogError(e); }

        switch (sceneName)
        {
            case "StandaloneEngagementPrompt": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneStandaloneEngagementPromptLoad); break;
            case "PlayerCore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnScenePlayerCoreLoad); break;
            case "UICore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneUICoreLoad); break;
            case "MainMenuUI": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneMainMenuUILoad); break;
            case "LoadScene": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneLoadSceneLoad); break;
            case "ZoneCore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneZoneCoreLoad); break;
        }

        StarlightCommandManager.OnSceneWasLoaded(buildIndex, sceneName);
        StarlightCounterGateManager.OnSceneWasLoaded(buildIndex, sceneName);
        if (isPrismInUse)
            PrismLibLandPlots.OnSceneWasLoaded(buildIndex, sceneName);
    }

    internal static void CheckForTime()
    {
        if (!inGame) return;
        try
        {
            if (Time.timeScale != 0 && !Mathf.Approximately(Time.timeScale, NativeEUtil.CustomTimeScale))
                Time.timeScale = NativeEUtil.CustomTimeScale;
        } catch { }

        ExecuteInSeconds(CheckForTime, 1);
    }

    internal static void SendFontError(string name)
    {
        LogError($"The font '{name}' couldn't be loaded!");
    }

    internal static void SetupFonts()
    {
        if (Sr2FontAsset == null) Sr2FontAsset = FontEUtil.FontFromGame("Runsell Type - HemispheresCaps2");
        if (RegularFont == null) RegularFont = FontEUtil.FontFromGame("Lexend-Regular (Latin)");
        if (BoldFont == null) BoldFont = FontEUtil.FontFromGame("Lexend-Bold (Latin)");
        if (NormalFont == null) NormalFont = FontEUtil.FontFromOS("Tahoma");

        foreach (var pair in Menus) pair.Key.ReloadFont();
    }


    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        if (DebugLogging.HasFlag()) Log("WasInitialized Scene: " + sceneName);
        if (isPrismInUse)
            try { PrismShortcuts.OnSceneWasInitialized(buildIndex, sceneName); }
            catch (Exception e) { LogError(e); }

        if (sceneName == "MainMenuUI")
        {
            StarlightOptionsButtonManager.inGameSave = null;
            MainMenuLoaded = true;
        }

        switch (sceneName)
        {
            case "StandaloneEngagementPrompt":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnStandaloneEngagementPromptInitialize(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "PlayerCore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnPlayerCoreInitialize(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "UICore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnUICoreInitialize(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "MainMenuUI":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnMainMenuUIInitialize(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "LoadScene":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnLoadSceneInitialize(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "ZoneCore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnZoneCoreInitialized(); }
                    catch (Exception e) { LogError(e); }
                break;
        }

        foreach (var expansion in ExpansionV01S)
            try { expansion.OnSceneWasInitialized(buildIndex, sceneName); }
            catch (Exception e) { LogError(e); }

        switch (sceneName)
        {
            case "StandaloneEngagementPrompt": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneStandaloneEngagementPromptInitialize); break;
            case "PlayerCore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnScenePlayerCoreInitialize); break;
            case "UICore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneUICoreInitialize); break;
            case "MainMenuUI": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneMainMenuUIInitialize); break;
            case "LoadScene": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneLoadSceneInitialize); break;
            case "ZoneCore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneZoneCoreInitialize); break;
        }

        StarlightCommandManager.OnSceneWasInitialized(buildIndex, sceneName);
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (DebugLogging.HasFlag()) Log("OnUnloaded Scene: " + sceneName);
        if (sceneName == "MainMenuUI") MainMenuLoaded = false;

        switch (sceneName)
        {
            case "StandaloneEngagementPrompt":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnStandaloneEngagementPromptUnload(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "PlayerCore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnPlayerCoreUnload(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "UICore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnUICoreUnload(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "MainMenuUI":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnMainMenuUIUnload(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "LoadScene":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnLoadSceneUnload(); }
                    catch (Exception e) { LogError(e); }
                break;
            case "ZoneCore":
                foreach (var expansion in ExpansionV01S)
                    try { expansion.OnZoneCoreUnloaded(); }
                    catch (Exception e) { LogError(e); }
                break;
        }

        foreach (var expansion in ExpansionV01S)
            try { expansion.OnSceneWasUnloaded(buildIndex, sceneName); }
            catch (Exception e) { LogError(e); }

        switch (sceneName)
        {
            case "StandaloneEngagementPrompt": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneStandaloneEngagementPromptUnload); break;
            case "PlayerCore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnScenePlayerCoreUnload); break;
            case "UICore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneUICoreUnload); break;
            case "MainMenuUI": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneMainMenuUIUnload); break;
            case "LoadScene": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneLoadSceneUnload); break;
            case "ZoneCore": StarlightCallEventManager.ExecuteStandard(CallEvent.OnSceneZoneCoreUnload); break;
        }

        StarlightCommandManager.OnSceneWasUnloaded(buildIndex, sceneName);
    }
    
    public override void OnUpdate()
    {
        try
        {
            foreach (var ui in new List<BaseUI>(BaseUIAddSliders))
            {
                if (!ui) continue;
                
                var scrollView = ui.gameObject.GetObjectRecursively<GameObject>("ButtonsScrollView");
                if (scrollView == null) continue;
                var rect = scrollView.GetComponent<ScrollRect>();
                if (rect.verticalScrollbar == null)
                {
                    rect.vertical = true;
                    var scrollBar = Object.Instantiate(StarlightStuff.GetObjectRecursively<Scrollbar>("saveFilesSliderRec"), rect.transform);
                    rect.verticalScrollbar = scrollBar;
                    rect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
                    scrollBar.GetComponent<RectTransform>().localPosition += new Vector3(Screen.width / 250f, 0, 0);
                }
                BaseUIAddSliders.Remove(ui);
            }

            if (MenusFinished)
            {
                try
                {
                    if (StarlightConsole.OpenKey.OnKeyDown() || StarlightConsole.OpenKey2.OnKeyDown())
                        MenuEUtil.GetMenu<StarlightConsole>().Toggle();
                }
                catch (Exception e) { LogError(e); }

                try { StarlightCommandManager.Update(); }
                catch (Exception e) { LogError(e); }

                try { StarlightBindingManger.Update(); }
                catch (Exception e) { LogError(e); }

                if (DevMode.HasFlag()) 
                    try {StarlightDebugUI.DebugStatsManager.Update(); }
                    catch (Exception e) { LogError(e); }
                
                if (RestoreDebugDebugUI.HasFlag())
                    try { if (StarlightNativeDebugUI.openKey.OnKeyDown()) MenuEUtil.GetMenu<StarlightNativeDebugUI>().Toggle(); }
                    catch (Exception e) { LogError(e); }

                foreach (var pair in Menus)
                    try { pair.Key.AlwaysUpdate(); }
                    catch (Exception e) { LogError(e); }
            }
            if (ActionCounter.Count > 0)
                foreach (var pair in new Dictionary<Action, int>(ActionCounter))
                    if (pair.Value < 1)
                    {
                        try { pair.Key.Invoke(); }
                        catch (Exception e) { LogError(e); }

                        ActionCounter.Remove(pair.Key);
                    }
                    else ActionCounter[pair.Key]--;
        }
        catch (Exception e) { LogError(e); }

        foreach (var expansion in ExpansionV01S)
            try { expansion.OnUpdate(); }
            catch (Exception e) { LogError(e); }
    }
    
    public override void OnFixedUpdate()
    {
        foreach (var expansion in ExpansionV01S)
            try { expansion.OnFixedUpdate(); }
            catch (Exception e) { LogError(e); }
    }

    public override void OnGUI()
    {
        foreach (var expansion in ExpansionV01S)
            try { expansion.OnGUI(); }
            catch (Exception e) { LogError(e); }
    }

    public override void OnLateUpdate()
    {
        foreach (var expansion in ExpansionV01S)
            try { expansion.OnLateUpdate(); }
            catch (Exception e) { LogError(e); }
    }
}