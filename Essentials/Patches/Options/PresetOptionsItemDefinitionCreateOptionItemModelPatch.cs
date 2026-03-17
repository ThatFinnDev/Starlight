using Il2CppMonomiPark.SlimeRancher.Options;
using Starlight.Buttons.OptionsUI;

namespace Starlight.Patches.Options;

[HarmonyPatch(typeof(PresetOptionsItemDefinition), nameof(PresetOptionsItemDefinition.CreateOptionItemModel))]
internal static class PresetOptionsItemDefinitionCreateOptionItemModelPatch
{
    [HarmonyFinalizer]
    static Exception Finalizer(PresetOptionsItemDefinition __instance, Exception __exception)
    {
        if (!InjectOptionsButtons.HasFlag()) return __exception;
        if (__instance is CustomOptionsValuesDefinition && __instance.ReferenceId.StartsWithAny("setting.sr2eexclude","setting.starlightexclude"))
            return null;
        return __exception;
    }
}