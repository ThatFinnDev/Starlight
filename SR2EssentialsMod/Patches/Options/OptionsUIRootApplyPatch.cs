using Il2CppMonomiPark.SlimeRancher.UI.Options;

namespace SR2E.Patches.Options;

[HarmonyPatch(typeof(OptionsUIRoot), nameof(OptionsUIRoot.ApplyChanges))]
internal static class OptionsUIRootApplyPatch
{
    internal static int realMasterTextureLimit = 0;
    internal static int customMasterTextureLimit = -1;
    internal static int customMaxFPS = -1;
    private static bool isCheckingFPS = false;

    public static void Apply()
    {
        if (customMasterTextureLimit == -1)
            QualitySettings.masterTextureLimit = realMasterTextureLimit;
        else
            QualitySettings.masterTextureLimit = customMasterTextureLimit;
        
        if(!isCheckingFPS) CheckCustomFPS();
    }

    public static void CheckCustomFPS()
    {
        if (customMaxFPS != -1)
        {
            isCheckingFPS = true;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = customMaxFPS;
            ExecuteInTicks((() => CheckCustomFPS()), 5);
        }
        else isCheckingFPS = false;
    }
    public static void Postfix()
    {
        realMasterTextureLimit = QualitySettings.masterTextureLimit;
        Apply();
    }
}