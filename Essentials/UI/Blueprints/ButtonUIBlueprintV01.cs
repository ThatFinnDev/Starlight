using Il2CppTMPro;
using Starlight.Enums.Sounds;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class ButtonUIBlueprintV01 : UIBlueprint
{
    public Sprite sprite;
    public UIColor color = UIColor.None;
    public UIColorBlock buttonColors = UIColorBlock.Buttons;
    public SystemAction onClick = null;
    public bool interactable = true;
    public bool useClickSound = true;
    public MenuSound clickSound = MenuSound.Click;

    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        var image = obj.AddComponent<Image>();
        image.sprite = sprite;
        image.color = theme.GetColor(color);
        var button = obj.AddComponent<Button>();
        button.interactable = interactable;
        button.colors = theme.GetColorBlock(buttonColors);
        button.onClick = new Button.ButtonClickedEvent();
        if(onClick!=null)
            button.onClick.AddListener(onClick);
        if(useClickSound)
            button.onClick.AddListener((SystemAction)(()=> AudioEUtil.PlaySound(clickSound)));
    }
}
