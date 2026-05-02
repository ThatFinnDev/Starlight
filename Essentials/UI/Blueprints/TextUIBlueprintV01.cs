using Il2CppTMPro;
using Starlight.Components;

namespace Starlight.UI.Blueprints;

public class TextUIBlueprintV01 : UIBlueprint
{
    public UIColor color = UIColor.TextGeneral;
    public Color? customColor = null;
    public string textContent = "";
    public bool disableAutoTranslation = false;
    public TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft;
    public TMP_FontAsset customFont;
    public FontStyles fontStyle;
    public float fontSize = 20f;
    public float fontAutoSizeMin = 1f;
    public float fontAutoSizeMax = 32f;
    public bool enableAutoSizing;
    public bool enableAutoSizeTextContainer;
    public bool clickableLinks = false;
    public Vector4 margins = Vector4.zero;

    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        var txt = obj.AddComponent<TextMeshProUGUI>();

        txt.alignment = alignment;
        txt.text = disableAutoTranslation?textContent:Tr(textContent);
        txt.font = customFont ?? fontTheme.DefaultFont;
        txt.color = customColor ?? theme.GetColor(color);
        txt.fontSize=fontSize*ScaleFactor;
        txt.fontSizeMin=fontAutoSizeMin*ScaleFactor;
        txt.fontSizeMax=fontAutoSizeMax*ScaleFactor;
        txt.enableAutoSizing=enableAutoSizing;
        txt.autoSizeTextContainer = enableAutoSizeTextContainer;
        txt.fontStyle = fontStyle;
        txt.margin = margins * ScaleFactor;
        if (clickableLinks)
            obj.AddComponent<ClickableTextLink>();

    }
}
