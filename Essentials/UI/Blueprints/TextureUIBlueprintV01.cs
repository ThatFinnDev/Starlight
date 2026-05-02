using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class TextureUIBlueprintV01 : UIBlueprint
{
    public Texture texture;
    public UIColor color = UIColor.None;
    public Color? customColor = null;
    public TextureUIBlueprintV01()
    {
    }
    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        var image = obj.AddComponent<RawImage>();
        image.color = customColor ?? theme.GetColor(color);
        image.texture = texture;
    }
}
