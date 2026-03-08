using System.IO;
using Il2CppTMPro;

namespace Starlight.Assets;

public static class StarlightFontAssets
{
    public static TMP_FontAsset SlimeRancherHemispheres;
    
    public static TMP_FontAsset? RegularLexend;
    public static TMP_FontAsset BoldLexend;
    
    public static TMP_FontAsset NormalTahoma;
    
    public static TMP_FontAsset NotoSans;

    // i didn't finish this
    
    public static void OnSceneWasLoaded(string sceneName)
    {
        switch (sceneName)
        {
            case "GameCore":
            {
                SlimeRancherHemispheres = FontEUtil.FontFromGame("Runsell Type - HemispheresCaps2");
                RegularLexend ??= FontEUtil.FontFromGame("Lexend-Regular (Latin)");
                BoldLexend ??= FontEUtil.FontFromGame("Lexend-Bold (Latin)");
                NormalTahoma ??= FontEUtil.FontFromOS("Tahoma");
                break;
            }
        }
        
        if (NotoSans == null)
        {
            var settings = Get<TMP_Settings>("TMP Settings");
            if (settings == null) return;
            var tempPath = Path.Combine(StarlightEntryPoint.tmpDataPath, "tmpFallbackFont.ttf");
            File.WriteAllBytes(tempPath, EmbeddedResourceEUtil.LoadResource("Asset                   s.NotoSans.ttf"));
            var tempFont = new Font(tempPath);
            StarlightEntryPoint.NotoSansFont = TMP_FontAsset.CreateFontAsset(tempFont);
            //settings.m_fallbackFontAssets.Add(fallBackFont);, creates issues for some reason :(
            settings.m_warningsDisabled = true;
        }

        foreach (var fontAsset in GetAll<TMP_FontAsset>())
        {
            if (fontAsset == StarlightEntryPoint.NotoSansFont) continue;
            if (!fontAsset.fallbackFontAssetTable.Contains(StarlightEntryPoint.NotoSansFont))
                fontAsset.fallbackFontAssetTable.Add(StarlightEntryPoint.NotoSansFont);
        }
    }
}