using Starlight.Enums;

namespace Starlight.Storage;

public readonly struct MenuIdentifier(
    string translationKey,
    StarlightMenuFont defaultFont,
    StarlightMenuTheme defaultTheme,
    string saveKey, bool useNewStyle=false, bool supportsThemes=true)
{
    public string translationKey { get; } = translationKey.ToLower();
    public StarlightMenuTheme defaultTheme { get; } = defaultTheme;
    public bool useNewStyle { get; } = useNewStyle;
    public bool supportsThemes { get; } = supportsThemes;
    public string saveKey { get; } = saveKey.ToLower();
    public StarlightMenuFont defaultFont { get; } = defaultFont;

    public override string ToString() => $"MenuIdentifier {{ TranslationKey: \"{translationKey}\", DefaultFont: {defaultFont}, DefaultTheme: {defaultTheme}, SaveKey: \"{saveKey}, UseNewStyle: \"{useNewStyle}, SupportsThemes: \"{supportsThemes}\" }}";
    
}