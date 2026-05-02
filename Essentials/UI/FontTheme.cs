using Il2CppTMPro;
using Starlight.Enums;

namespace Starlight.UI;

public class FontTheme
{
    public FontTheme() {}

    private FontTheme(TMP_FontAsset font)
    {
        this._defaultFont = font;
    }
    private TMP_FontAsset _defaultFont;

    public TMP_FontAsset DefaultFont
    {
        get
        {
            if(!_defaultFont)
            {
                if(!StarlightEntryPoint.NotoSansFont) StarlightEntryPoint.CheckFallBackFont();
                return StarlightEntryPoint.NotoSansFont;
            }
            return _defaultFont;
        }
        set => _defaultFont = value;
    }
    
    internal static FontTheme GetTheme(StarlightMenuFont theme)
    {
        switch (theme)
        {
            case StarlightMenuFont.Default: return new FontTheme(StarlightEntryPoint.NormalFont);
            case StarlightMenuFont.Native: return new FontTheme(StarlightEntryPoint.Sr2FontAsset);
            case StarlightMenuFont.Bold: return new FontTheme(StarlightEntryPoint.BoldFont);
            case StarlightMenuFont.NotoSans: return new FontTheme(StarlightEntryPoint.NotoSansFont);
        }
        return null;
    }
}