using Il2CppTMPro;

namespace Starlight.UI.Blueprints;

public class TextUIBlueprint : UIBlueprint
{
    public string Content;
    public bool DisableAutoTranslation = false;
    public TMP_FontAsset CustomFont;

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var txt = obj.AddComponent<TextMeshProUGUI>();

        txt.text = DisableAutoTranslation?Content:translation(Content);
        txt.font = CustomFont ?? theme.DefaultFont;
        txt.color = theme.TextColor;

    }
}
