using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using MelonLoader;
using MelonLoader.Utils;
using Starlight.Enums;
using Starlight.Expansion;
using Starlight.Storage;

namespace Starlight.Managers;

public static class StarlightPackageManager
{
    private static readonly Dictionary<MelonBase, StarlightPackageInfo> MelonInfos = new ();
    
    
    public static StarlightPackageInfo? GetPackageInfoFromMelon(this MelonBase melonBase)
    {
        if (MelonInfos.TryGetValue(melonBase, out var melon)) return melon;
        var assembly = melonBase.MelonAssembly.Assembly;
        if (StarlightEntryPoint.Expansions.Keys.ToList().Contains(assembly))
            return null;
        var info = new StarlightPackageInfo()
        {
            ID = "melon." + TruncateForID(melonBase.Info.Author) + "." + TruncateForID(melonBase.Info.Name),
            name = melonBase.Info.Name,
            author = melonBase.Info.Author,
            version = melonBase.Info.Version,
            dllName = new FileInfo(assembly.Location).Name,
            mainClass = melonBase,
            type = PackageType.MelonMod
        };
        if (melonBase is MelonPlugin)
            info.type = PackageType.MelonPlugin;
        var desc = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
        if (desc != null)
            info.description = desc.Description;
        foreach (var meta in assembly.GetCustomAttributes<AssemblyMetadataAttribute>())
        {
            if (meta == null) continue;
            if (string.IsNullOrWhiteSpace(meta.Key)) continue;
            if (string.IsNullOrWhiteSpace(meta.Value)) continue;
            switch (meta.Key)
            {
                case StarlightModInfoAttributes.SourceCode: info.sourceCode = meta.Value; break;
                case StarlightModInfoAttributes.Nexus: info.nexus = meta.Value; break;
                case StarlightModInfoAttributes.Discord: info.discord = meta.Value; break;
                case StarlightModInfoAttributes.DisplayVersion: info.version = meta.Value; break;
                case StarlightModInfoAttributes.CoAuthors: info.coAuthors = meta.Value.Split(", "); break;
                case StarlightModInfoAttributes.Contributors: info.contributors = meta.Value.Split(", "); break;
                case StarlightModInfoAttributes.IconB64:
                    try
                    {
                        info.icon = ConvertEUtil.Base64ToTexture2D(meta.Value).Texture2DToSprite();
                    }
                    catch
                    {
                        // ignored
                    }

                    break;
            }
        }

        if (info.icon)
        { try { info.icon = EmbeddedResourceEUtil.LoadSprite("icon.png", assembly).CopyWithoutMipmaps(); }
            catch
            {
                // ignored
            }
            if (info.icon)
            { try { info.icon = EmbeddedResourceEUtil.LoadSprite("Assets.icon.png", assembly).CopyWithoutMipmaps(); }
                catch
                {
                    // ignored
                }
            }
        }
        MelonInfos.Add(melonBase,info);
        return info;
    }
    public static StarlightPackageInfo? GetPackageInfoFromExpansion(this StarlightExpansionVXX expansion)
    {
        foreach (var pair in StarlightEntryPoint.Expansions.Values.ToList())
            try
            {
                foreach (var pair2 in pair.Item1)
                    try { if (pair2.Key == expansion) return pair2.Value; }
                    catch (Exception e) { LogError(e); }
            }
            catch (Exception e) { LogError(e); }

        return null;
    }


    public static List<Assembly> GetAllPackageAssemblies()
    {
        var assemblies = StarlightEntryPoint.Expansions.Keys.ToList();
        foreach (var melonBase in MelonBase.RegisteredMelons)
        {
            if(!assemblies.Contains(melonBase.MelonAssembly.Assembly))
                assemblies.Add(melonBase.MelonAssembly.Assembly);
        }
        return assemblies;
    }
    
    public static Dictionary<StarlightPackageInfo,List<object>> GetAllRottenInfos()
    {
        var list = new Dictionary<StarlightPackageInfo,List<object>>();
        foreach (var group in StarlightEntryPoint.BrokenExpansions)
        {
            try
            {
                list.Add(new StarlightPackageInfo()
                {
                    type = PackageType.Expansion, name = group.Item1, assembly = group.Item2, version = "<unknown>",
                    dllName = group.Item1
                },new List<object>() { group.Item2.Location, group.Item3,  group.Item4 });
            }
            catch (Exception e) { LogError(e); }
        }

        foreach (var loadedAssembly in MelonAssembly.LoadedAssemblies) foreach (dynamic rotten in loadedAssembly.RottenMelons)
        {
            // Do it this way to support ML 0.7.1 and newer versions
            try
            {
                Assembly assembly = null; string exception = null; string errorMessage = null;
                try
                {
                    assembly = rotten.assembly;
                    exception = rotten.exception?.ToString();
                    errorMessage = rotten.errorMessage;
                }
                catch { try {
                        assembly = rotten.Assembly.assembly;
                        exception = rotten.exception?.ToString();
                        errorMessage = rotten.errorMessage;
                    } catch {
                        // ignored
                    }
                }
                if (assembly == null) break;
                list.Add(new StarlightPackageInfo(){type = PackageType.Expansion, name = new FileInfo(assembly.Location).Name,assembly = assembly, version = "<unknown>", dllName = new FileInfo(assembly.Location).Name},new List<object>() { assembly.Location, exception, errorMessage });

            }
            catch (Exception e) { LogError(e); }
        }
        return list;
    }
    public static List<StarlightPackageInfo> GetAllMelonInfos()
    {
        var list = new List<StarlightPackageInfo>();
        foreach (var melonBase in MelonBase.RegisteredMelons)
        {
            try
            {
                // ReSharper disable once PossibleInvalidOperationException
                list.Add(GetPackageInfoFromMelon(melonBase).Value);
            }
            catch (Exception e) { LogError(e); }
        }

        return list;
    }
    public static List<StarlightPackageInfo> GetAllExpansionInfos()
    {
        var list = new List<StarlightPackageInfo>();
        foreach (var pair in StarlightEntryPoint.Expansions.Values.ToList())
            try
            {
                foreach (var pair2 in pair.Item1)
                    try { list.Add(pair2.Value); }
                    catch (Exception e) { LogError(e); }
            }
            catch (Exception e) { LogError(e); }
        return list;
    }
    public static List<StarlightExpansionVXX> GetAllExpansions()
    {
        var list = new List<StarlightExpansionVXX>();
        foreach (var pair in StarlightEntryPoint.Expansions.Values.ToList())
            try
            {
                foreach (var pair2 in pair.Item1)
                    try { list.Add(pair2.Key); }
                    catch (Exception e) { LogError(e); }
            }
            catch (Exception e) { LogError(e); }
        return list;
    }
    public static List<MelonBase> GetAllMelons()
    {
        var list = new List<MelonBase>();
        foreach (var mBase in MelonBase.RegisteredMelons)
        {
            try
            {
                var assembly = mBase.MelonAssembly.Assembly;
                if (StarlightEntryPoint.Expansions.Keys.ToList().Contains(assembly))
                    continue;
                list.Add(mBase);
            }
            catch (Exception e) { LogError(e); }
        }
        return list;
    }

    
    
    public static StarlightPackageInfo? GetPackageInfoFromID(string id)
    {
        var infos = new List<StarlightPackageInfo>();
        if (id.StartsWith("melon.")) infos = GetAllMelonInfos();
        else GetAllExpansionInfos();
        foreach (var info in infos)
            if (info.ID == id)
                return info;
        return null;
    }
    public static MelonBase GetMelonFromID(string id)
    {
        if (!id.StartsWith("melon.")) return null;
        foreach (var info in GetAllMelonInfos())
            if (info.ID == id)
                return info.mainClass as MelonBase;
        return null;
    }
    public static StarlightExpansionVXX GetExpansionFromID(string id)
    {
        if (id.StartsWith("melon.")) return null;
        foreach (var info in GetAllMelonInfos())
            if (info.ID == id)
                return info.mainClass as StarlightExpansionVXX;
        return null;
    }
    public static StarlightExpansionVXX GetExpansionV1FromID(string id)
    {
        if (id.StartsWith("melon.")) return null;
        foreach (var info in GetAllMelonInfos())
            if (info.ID == id)
                if(info.mainClass is StarlightExpansionV01 e)
                    return e;
        return null;
    }
    
    
    public static void LoadExpansions(string dllPath)
    {
        if (!AllowExpansions.HasFlag()) return;
        var baseType = typeof(StarlightExpansionVXX);
        if (!ContainsExpansions(dllPath)) return;
        var assembly = Assembly.LoadFrom(dllPath);
        foreach (var meta in assembly.GetCustomAttributes<AssemblyMetadataAttribute>())
            if(meta.Key==StarlightModInfoAttributes.MinimumStarlightVersion)
            {
                StarlightEntryPoint.BrokenExpansions.Add((new FileInfo(assembly.Location).Name, assembly,
                    $"You need a newer version of Starlight installed! A minimum of <b>Starlight {meta.Value}</b> is required!",
                    "Starlight too old!"));
                return;
            }
        var hInstance = new HarmonyLib.Harmony(dllPath);
        var types = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(StarlightLoadExpansionAttribute), false).Any()).ToList();
        var instances = new Dictionary<StarlightExpansionVXX,StarlightPackageInfo>();
        if (types is { Count: > 0 })
            foreach (var type in types)
                if (baseType.IsAssignableFrom(type) && type != baseType)
                {
                    var instance = (StarlightExpansionVXX)Activator.CreateInstance(type);
                    bool success = true;
                    var message = "";
                    string errorMessage = null;
                    var info = instance.StarlightInternal_GetInfo;
                    info.assembly = assembly;
                    info.dllName = new FileInfo(assembly.Location).Name;
                    info.type = PackageType.Expansion;
                    info.mainClass = instance;
                    try
                    {
                        if (string.IsNullOrWhiteSpace(info.ID) && (!string.IsNullOrEmpty(info.ID) && info.ID.All(char.IsLetterOrDigit))) 
                            message += "\nThe expansion's ID is invalid! It needs to be a non-empty alphanumeric string! For example: \"com.devname.expansionanme\"";
                        if (string.IsNullOrWhiteSpace(info.name))
                            message += "\nThe expansion's name is invalid! It needs to be a non-empty string! For example: \"Blue Slimes\"";
                    }
                    catch (Exception e)
                    {
                        success = false;
                        LogError(e);
                        LogError($"Couldn't load the expansion \"{type.FullName}\" in the dll at \"{dllPath}\" due to an unknown error!");
                        errorMessage = e.Message;
                    }

                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        success = false;
                        LogError($"Couldn't load the expansion \"{type.FullName}\" in the dll at \"{dllPath}\"!");
                        LogError("The expansion info is invalid!");
                        LogError(message);
                    }

                    try { info.icon = EmbeddedResourceEUtil.LoadSprite(info.iconPath, assembly).CopyWithoutMipmaps(); }
                    catch (Exception e) { LogError($"Couldn't load the icon of expansion \"{type.FullName}\" in the dll at \"{dllPath}\"!"); }

                    if (instance is StarlightExpansionV01)
                    {
                        if (!AllowExpansionsV1.HasFlag())
                        {
                            success = false;
                            message += "\nExpansionV1s are disabled!";
                        }
                        info.expansionVersion = 1;
                    }
                    /*else if (instance is StarlightExpansionV02)
                    {
                        if (!AllowExpansionsV2.HasFlag())
                        {
                            success = false;
                            message += "\nExpansionV2s are disabled!";
                        }
                        info.expansionVersion = 2;
                    }*/
                    else
                    {
                        success = false;
                        message += "\nInvalid expansion version!";
                    }
                    

                    if (success)
                    {
                        if (AllowPrism.HasFlag() && info.usePrism)
                            StarlightEntryPoint.shouldEnablePrism = true;
                        if (instance is StarlightExpansionV01 v01) StarlightEntryPoint.ExpansionV01S.Add(v01);
                        baseType.GetField("_assembly", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(instance, assembly);
                        baseType.GetField("_harmonyInstance", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(instance, hInstance);
                        instances.Add(instance, info);
                    }
                    else StarlightEntryPoint.BrokenExpansions.Add((type.FullName, assembly, message, errorMessage));
                }

        if(instances.Count>0)
            StarlightEntryPoint.Expansions.Add(assembly, (instances, hInstance));
    }

    internal static void LoadAllExpansions()
    {
        if (!AllowExpansions.HasFlag()) return;
        
        foreach (var dllPath in Directory.GetFiles(MelonEnvironment.ModsDirectory))
        {
            if(dllPath.EndsWith(".dll"))
                try { LoadExpansions(dllPath); }
                catch (Exception e) { LogError(e); }
        }
        
    }
    public static bool ContainsExpansions(string dllPath)
    {
        var context = new AssemblyLoadContext("StarlightUnloadableContext", isCollectible: true);
        bool isExpansion;
        try 
        {
            var assembly = context.LoadFromAssemblyPath(dllPath);
            isExpansion = assembly.GetTypes().Any(t => Attribute.IsDefined(t, typeof(StarlightLoadExpansionAttribute)));
        }
        finally 
        {
            context.Unload();
        }
        for (int i = 0; i < 10 && GC.GetTotalMemory(false) > 0; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        return isExpansion;
    }







    static string TruncateForID(string input) => Regex.Replace(input, "[^a-zA-Z0-9]", "").ToLower();
    
    
}