using System;
using Il2CppMonomiPark.SlimeRancher.UI.Localization;
using Il2CppSystem.Globalization;
using Starlight.Managers;

namespace Starlight.Patches.General;

[HarmonyPatch(typeof(LocalizationDirector), nameof(LocalizationDirector.SetLocale))]
internal class ChangeLanguagePatch
{
    internal static void Postfix(LocalizationDirector __instance, UnityEngine.Localization.Locale locale)
    {
        FixLanguage(__instance, locale);
        ExecuteInTicks((() => { FixLanguage(__instance, locale);}), 10);
    }

    static void FixLanguage(LocalizationDirector director, UnityEngine.Localization.Locale curLocale)
    {
        var code = curLocale.Formatter.Cast<CultureInfo>()._name;

        LoadLanguage(code);

        StarlightLanguageManager.StarlightToSRLanguage.Clear();
        StarlightLanguageManager.AddedTranslations.Clear();
        foreach (var str in StarlightLanguageManager.StarlightReplaceOnLanguageChange)
        {
            var localized = AddTranslation(Tr(str.Key), str.Value.Item1, str.Value.Item2);

            var original = str.Value.Item3;

            original.m_TableEntryReference = localized.TableEntryReference;
            original.m_TableReference = localized.TableReference;
        }
    }
}