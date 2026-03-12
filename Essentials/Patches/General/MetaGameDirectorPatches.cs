using Il2CppMonomiPark.SlimeRancher;

namespace Starlight.Patches.General;


[HarmonyPatch(typeof(MetaGameDirector), nameof(MetaGameDirector.ChangeGameActivity))]
public static class MetaGameDirectorChangeGameActivityPatch
{
    public static bool Prefix()
    {
        if (StarlightEntryPoint.changedUserFolder || ForceLoadMainMenu.HasFlag()) return false;
        return true;
    }
}
[HarmonyPatch(typeof(MetaGameDirector), nameof(MetaGameDirector.SetAchievementProgress))]
public static class MetaGameDirectorSetAchievementProgressPatch
{
    public static bool Prefix()
    {
        if (StarlightEntryPoint.changedUserFolder || ForceLoadMainMenu.HasFlag()) return false;
        return true;
    }
}