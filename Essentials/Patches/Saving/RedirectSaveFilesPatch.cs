using System.IO;

namespace Starlight.Patches.Saving;

[HarmonyPatch(typeof(SystemContext), nameof(SystemContext.GetStorageProvider))]
internal static class RedirectSaveFilesPatch
{
    private static StorageProvider _provider = null;

    private static StorageProvider provider
    {
        get
        {
            if (_provider == null)
            {
                var savePath = Path.Combine(StarlightEntryPoint.dataPath, "redirectedSaves");
                Directory.CreateDirectory(savePath);
                var prov = new FileStorageProvider(savePath);
                prov.isInitialized = true;
                _provider = prov.TryCast<StorageProvider>();
            }
            return _provider;
        }
    }
    public static bool Prefix(SystemContext __instance, ref StorageProvider __result)
    {
        if (!RedirectSaveFiles.HasFlag()) return true;
        __result = provider;
        return false; 
    }
    
}