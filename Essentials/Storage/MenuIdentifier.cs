using Starlight.Enums;

namespace Starlight.Storage;

public readonly struct MenuIdentifier(
    string translationKey,
    StarlightMenuFont defaultFont,
    StarlightMenuTheme defaultTheme,
    string saveKey)
{
    public string translationKey { get; } = translationKey;
    public StarlightMenuTheme defaultTheme { get; } = defaultTheme;
    public string saveKey { get; } = saveKey;
    public StarlightMenuFont defaultFont { get; } = defaultFont;

    public override string ToString() => $"MenuIdentifier {{ TranslationKey: \"{translationKey}\", DefaultFont: {defaultFont}, DefaultTheme: {defaultTheme}, SaveKey: \"{saveKey}\" }}";
    
}