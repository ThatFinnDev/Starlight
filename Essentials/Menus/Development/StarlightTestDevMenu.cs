using Starlight.Enums;
using Starlight.Enums.Features;
using Starlight.Storage;
using Starlight.UI;
using Starlight.UI.Blueprints;

namespace Starlight.Menus.Development;

internal class StarlightTestDevMenu : StarlightMenu
{
    public new static MenuIdentifier GetMenuIdentifier() => new ("starlighttestdevemenu",StarlightMenuFont.SR2,StarlightMenuTheme.Default,
        "TestDevMenu");

    protected override bool createCommands => true;
    protected override bool inGameOnly => false;
    protected override void OnAwake()
    {
        requiredFeatures = [DevTestMenu];
        openActions = [MenuActions.PauseGameFalse];
        closeActions = [MenuActions.UnPauseGameFalse, MenuActions.EnableInput];
    }

    private readonly BackgroundPanelUIBlueprint _blueprint = new ()
    {
        Children =
        [
            new TextUIBlueprint()
            {
                Content = "TestLabel"
            },
        ]
    };
    
    // In reality it's tab
    internal static readonly LKey OpenKey = LKey.F8;
    public new static GameObject GetMenuRootObject()
    {
        var obj = new GameObject("StarlightTestDevMenu");
        var rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero; 
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.localPosition = Vector2.zero;
        return obj;
    }

    private RectTransform _openThing;
    protected override void OnOpen()
    {
        _openThing = _blueprint.Render(new UITheme(){BackgroundPanelSprite = EmbeddedResourceEUtil.LoadSprite("Assets.MenuBG.png")}, transform);
    }

    protected override void OnClose()
    {
        Destroy(_openThing.gameObject);
    }
    
    public override void OnCloseUIPressed()
    {
        if (MenuEUtil.isAnyPopUpOpen) return;
        Close();
    }
}