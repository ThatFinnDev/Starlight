using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppMonomiPark.SlimeRancher.UI.Framework.Components;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu;

namespace Starlight.Utils;

public static class NativeEUtil
{
    public static void TryHideMenus()
    {
        if (StarlightEntryPoint.MainMenuLoaded)
        {
            try
            {
                var ui = GetAnyInScene<MainMenuLandingRootUI>();
                if (ui)
                {
                    ui.gameObject.SetActive(false);
                    ui.enabled = false;
                    ui.Close(true);
                }
            }  
            catch (Exception e) { LogError(e); }
        }

        if (inGame)
        {
            try { GetAnyInScene<PauseMenuRoot>()?.HideUI(); }
            catch
            {
                // ignored
            }
        }
    }

    public static void TryPauseAndHide()
    {
        if (inGame&&Object.FindObjectOfType<PauseMenuRoot>())
        {
            TryHideMenus();
            TryPauseGame(false);
            if (inGame) HudUI.Instance.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            TryPauseGame();
            TryHideMenus();
        }
    }

    public static void TryPauseGame(bool usePauseMenu = true)
    {
        if (StarlightEntryPoint.MainMenuLoaded)
            Time.timeScale = 0;
        
        try { systemContext.SceneLoader.TryPauseGame(); }
        catch
        {
            // ignored
        }

        if (usePauseMenu)
            try { sceneContext.PauseMenuDirector.PauseGame(); }
            catch
            {
                // ignored
            }
    }

    public static void TryUnPauseGame(bool usePauseMenu = true)
    {

        if (StarlightEntryPoint.MainMenuLoaded)
            Time.timeScale = 1;
        
        try { systemContext.SceneLoader.UnpauseGame(); }
        catch
        {
            // ignored
        }

        if (usePauseMenu)
            try { sceneContext.PauseMenuDirector.UnPauseGame(); }
            catch
            {
                // ignored
            }
    }

    public static void TryUnHideMenus()
    {
        try
        {
            if (StarlightEntryPoint.MainMenuLoaded)
            {
                try
                {
                    foreach (var container in GetAllInScene<UIDisplayContainer>()!)
                        if (container.TargetContainer.name == "MainMenuRoot" && container.name == "MainMenuRoot")
                        {
                            try
                            {

                                container.OnEnable();
                                break;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                }
                catch
                {
                    // ignored
                }
            }
            if (inGame) HudUI.Instance.transform.GetChild(0).gameObject.SetActive(true);
        }
        catch
        {
            // ignored
        }
    }

    private static float _customTimeScale = 1f;

    // ReSharper disable once InconsistentNaming
    public static float CustomTimeScale
    {
        get => _customTimeScale;
        set
        {
            _customTimeScale = value;
            if (value < 0.01f) _customTimeScale = 0.01f;
            if (value > 2000f) _customTimeScale = 2000f;
            StarlightEntryPoint.CheckForTime();
        }
    }

    // ReSharper disable once InconsistentNaming
    public static void TryDisableSR2Input()
    {
        try
        {
            gameContext.InputDirector._paused.Map.Disable();
            gameContext.InputDirector._mainGame.Map.Disable();
        }
        catch
        {
            // ignored
        }
    }

    // ReSharper disable once InconsistentNaming
    public static void TryEnableSR2Input()
    {
        try
        {
            gameContext.InputDirector._paused.Map.Enable();
            gameContext.InputDirector._mainGame.Map.Enable();
        }
        catch
        {
            // ignored
        }
    }
}