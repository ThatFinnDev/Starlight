using Starlight.Enums;

namespace Starlight.Storage;

public struct MenuIdentifier
{
    public string translationKey { get; }
    public StarlightMenuTheme defaultTheme { get; }
    public string saveKey { get; }
    public StarlightMenuFont defaultFont { get; }

    public MenuIdentifier(string translationKey, StarlightMenuFont defaultFont, StarlightMenuTheme defaultTheme, string saveKey)
    {
        this.translationKey = translationKey;
        this.defaultFont = defaultFont;
        this.defaultTheme = defaultTheme;
        this.saveKey = saveKey;
    }
    public override string ToString() => $"MenuIdentifier {{ TranslationKey: \"{translationKey}\", DefaultFont: {defaultFont}, DefaultTheme: {defaultTheme}, SaveKey: \"{saveKey}\" }}";
    
}