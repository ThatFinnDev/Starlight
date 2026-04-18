using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Starlight.Enums;
using Starlight.Repos;
using Starlight.Storage;
using UnityEngine.InputSystem;

namespace Starlight.Managers;

internal static class StarlightSaveManager
{
    internal static StarlightSaveData data = null;
    internal static void Start()
    {
        Load();
    }
    static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
    {
        Error = (sender, args) =>
        {
            if (DebugLogging.HasFlag())
                Log($"Error: {args.ErrorContext.Error.Message}");

            // Handle broken fonts
            if (args.ErrorContext.Member is string memberName && args.ErrorContext.Path.Contains(nameof(StarlightSaveData.fonts)))
                ((Dictionary<string, StarlightMenuFont>)args.ErrorContext.OriginalObject)[memberName] = StarlightMenuFont.Default;

            // Handle broken themes
            if (args.ErrorContext.Member is string memberName2 && args.ErrorContext.Path.Contains(nameof(StarlightSaveData.themes)))
                ((Dictionary<string, StarlightMenuTheme>)args.ErrorContext.OriginalObject)[memberName2] = StarlightMenuTheme.Default;

            // Handle broken keybinds (invalid enum key)
            if (args.ErrorContext.Path.Contains(nameof(StarlightSaveData.keyBinds)) && args.ErrorContext.Member is string brokenKey)
            {
                var dict = args.ErrorContext.OriginalObject as Dictionary<LKey, string>;
                if (dict != null)
                {
                    // Try to find the invalid entry by string match (since enum parse failed)
                    var entry = dict.FirstOrDefault(kv => kv.Key.ToString() == brokenKey);
                    if (!entry.Equals(default(KeyValuePair<LKey, string>)))
                    {
                        dict.Remove(entry.Key);
                        if (DebugLogging.HasFlag())
                            Log($"Removed broken keybind: {brokenKey}");
                    }
                }
            }

            args.ErrorContext.Handled = true;
        }
    };

    internal static void Load()
    {
        var path = "";
        if (File.Exists(oldpath2)) path = oldpath2;
        if (File.Exists(oldpath1)) path = oldpath1;
        if (File.Exists(configPath)) path = configPath;

        if (string.IsNullOrWhiteSpace(path)) data = new StarlightSaveData();
        else try
            {
                data = JsonConvert.DeserializeObject<StarlightSaveData>(File.ReadAllText(path), jsonSerializerSettings);
            }
            catch (Exception e)
            {
                Log("Starlight save data is broken");
                Log(e);
                data = new StarlightSaveData();
            }
        if (File.Exists(oldpath2)) File.Delete(oldpath2);
        if (File.Exists(oldpath1)) File.Delete(oldpath1);

        if (data.keyBinds == null) data.keyBinds = new Dictionary<LKey, string>();
        if (data.warps == null) data.warps = new Dictionary<string, Warp>();
        if (data.themes == null) data.themes = new Dictionary<string, StarlightMenuTheme>();
        if (data.fonts == null) data.fonts = new Dictionary<string, StarlightMenuFont>();
        if (data.repos == null) data.repos = new List<RepoSave>();
        foreach (var pair in data.fonts)
            if (!Enum.IsDefined(typeof(StarlightMenuFont), pair.Value))
                data.fonts[pair.Key] = StarlightMenuFont.Default;
        foreach (var pair in data.themes)
            if (!Enum.IsDefined(typeof(StarlightMenuTheme), pair.Value))
                data.themes[pair.Key] = StarlightMenuTheme.Default;
        foreach (var pair in new Dictionary<LKey,String>(data.keyBinds))
            if (!Enum.IsDefined(typeof(LKey), pair.Key))
                data.keyBinds.Remove(pair.Key);
        if (data.keyBinds.ContainsKey(LKey.F11))
        {
            string cmd = data.keyBinds[LKey.F11];
            if (cmd.Contains("toggleconsole") || cmd.Contains("closeconsole") || cmd.Contains("openconsole"))
                data.keyBinds[LKey.F11] = cmd.Replace("toggleconsole", "").Replace("closeconsole", "")
                    .Replace("openconsole", "");
        }

        Save();
    }

    internal static void Save() { File.WriteAllText(configPath,JsonConvert.SerializeObject(data, Formatting.Indented)); }
    
    static string configPath => Path.Combine(StarlightEntryPoint.dataPath, "starlight.data");
    
    
    static string oldpath2 = Path.Combine(Application.persistentDataPath, "Starlight.data");
    static string oldpath1
    {
        get
        {
            try
            {
                var provider = systemContext.GetStorageProvider();
                return provider.TryCast<FileStorageProvider>().savePath + "/Starlight.data";
            }
            catch 
            {
                return Application.persistentDataPath + "/Starlight.data";
            }
        }
    }

    public class StarlightSaveData
    {
        public Dictionary<string, Warp> warps = new Dictionary<string, Warp>();
        public Dictionary<LKey, string> keyBinds = new Dictionary<LKey, string>();
        public Dictionary<string, StarlightMenuTheme> themes = new Dictionary<string, StarlightMenuTheme>();
        public Dictionary<string, StarlightMenuFont> fonts = new Dictionary<string, StarlightMenuFont>();
        public List<RepoSave> repos = new List<RepoSave>();
    }
}