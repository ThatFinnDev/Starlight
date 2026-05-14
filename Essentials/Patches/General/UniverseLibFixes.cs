using Starlight.Storage;

namespace Starlight.Patches.General;

[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibFix1
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:ReleaseEventSystem");
    public static bool Prefix()
    {
        return !MenuEUtil.isAnyMenuOpen;
    }
}
[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibFix2
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:Init");

    public static void Prefix() => NativeEUtil.ue = true;
    
}