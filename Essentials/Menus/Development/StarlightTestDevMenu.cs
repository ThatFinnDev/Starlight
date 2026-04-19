using Starlight.Enums;
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
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        requiredFeatures = [DevTestMenu];
        openActions = [MenuActions.PauseGameFalse];
        closeActions = [MenuActions.UnPauseGameFalse, MenuActions.EnableInput];
    }

    private readonly PanelUIBlueprint _blueprint = new ()
    {
        color = UIColor.Primary, CornerRadius = 90, Size = new Vector2(1330, 840),
        Children =
        [
            new PanelUIBlueprint()
            {
                color = UIColor.Secondary, CornerRadius = 0, Size = new Vector2(1330,580),
                Children = [
                    new PanelUIBlueprint() { color = UIColor.Accent, CornerRadius = 0, Size = new Vector2(1330,10), Position = new Vector2(0,-290f) },
                    new PanelUIBlueprint() { color = UIColor.Accent, CornerRadius = 0, Size = new Vector2(1330,10), Position = new Vector2(0,290f) },
                ]
            },
            new TextUIBlueprint()
            {
                Content = "TestLabel"
            },
        ]
    };
    
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
        return obj;
    }

    private RectTransform _openThing;
    protected override void OnOpen()
    {
        var theme = new UITheme();
        if(ColorUtility.TryParseHtmlString("#1B1B1DFF", out var color)) theme.PrimaryColor = color;
        if(ColorUtility.TryParseHtmlString("#303846FF", out var color2)) theme.SecondaryColor = color2;
        if(ColorUtility.TryParseHtmlString("#2C6EC8FF", out var color3)) theme.AccentColor = color3;
        _openThing = _blueprint.Render(theme, transform);
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