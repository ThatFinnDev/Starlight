using Il2CppTMPro;
using UnityEngine;

namespace Starlight.UI;

public class UITheme
{
    public string ThemeName;
    public float Scale = 1.0f;
    public Color PrimaryColor = Color.gray;
    public Color SecondaryColor = Color.black;
    public Color AccentColor = Color.blue;
    public Color TextColor = Color.white;
    public TMP_FontAsset DefaultFont;

    public Color GetColor(UIColor color)
    {
        switch (color)
        {
            case UIColor.Primary: return PrimaryColor;
            case UIColor.Secondary: return SecondaryColor;
            case UIColor.Accent: return AccentColor;
            case UIColor.Text: return TextColor;
            default: return PrimaryColor;
        }
    }
}
public enum UIColor 
{ Primary, Secondary, Accent, Text}