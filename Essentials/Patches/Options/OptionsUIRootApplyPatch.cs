using Il2CppMonomiPark.SlimeRancher.UI.Options;

namespace Starlight.Patches.Options;

[HarmonyPatch(typeof(OptionsUIRoot), nameof(OptionsUIRoot.ApplyChanges))]
internal static class OptionsUIRootApplyPatch
{
    internal static int CustomMasterTextureLimit = -1;
    internal static int CustomMaxFPS = -1;
    private static int _realMasterTextureLimit = 0;
    private static bool _isCheckingFPS = false;

    public static void Apply()
    {
        if (CustomMasterTextureLimit == -1)
            QualitySettings.masterTextureLimit = _realMasterTextureLimit;
        else
            QualitySettings.masterTextureLimit = CustomMasterTextureLimit;
        
        if(!_isCheckingFPS) CheckCustomFPS();
    }

    public static void CheckCustomFPS()
    {
        if (CustomMaxFPS != -1)
        {
            _isCheckingFPS = true;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = CustomMaxFPS;
            ExecuteInTicks((() => CheckCustomFPS()), 5);
        }
        else _isCheckingFPS = false;
    }
    public static void Postfix()
    {
        _realMasterTextureLimit = QualitySettings.masterTextureLimit;
        Apply();
    }
}