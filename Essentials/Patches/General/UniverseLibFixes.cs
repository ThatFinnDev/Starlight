using Starlight.Storage;

namespace Starlight.Patches.General;

[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibPatch1
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:ActivateUIModule");
    public static bool Prefix() => false;
}
[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibPatch2
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:AddUIModule");
    public static bool Prefix() => false;
}
[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibPatch3
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:CheckVRChatEventSystemFix");
    public static bool Prefix() => false;
}
[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibPatch4
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:EnableEventSystem");

    public static bool Prefix()
    {
        NativeEUtil.OverrideSR2Input();
        return false;
    }
}
[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibPatch5
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:FallbackEventSystemSearch");
    public static bool Prefix() => false;
}
[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibPatch6
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:Init");
    public static bool Prefix() => false;
}
[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibPatch7
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:InitPatches");
    public static bool Prefix() => false;
}
[HarmonyPatch,HarmonyDontLogOnFail]
internal static class UniverseLibPatch8
{
    static System.Reflection.MethodBase TargetMethod() => AccessTools.Method("UniverseLib.Input.EventSystemHelper:ReleaseEventSystem");
    public static bool Prefix()
    {
        NativeEUtil.DeOverrideSR2Input();
        return false;
    }
}