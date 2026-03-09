using System.Reflection;
using Starlight.Enums;

namespace Starlight.Storage;

public record struct StarlightPackageInfo
{
    public string ID = null;
    public string name = null;
    public string description = null;
    public string version = null;
    public string author = null;
    public string[] coAuthors = null;
    public string[] contributors = null;
    public string sourceCode = null;
    public string nexus = null;
    public string discord = null;
    public bool usePrism = false;
    public string iconPath = "Assets.icon.png";

    public string GetDllName() => dllName;
    public Assembly GetAssembly() => assembly;
    public int GetExpansionVersion() => expansionVersion;
    public Sprite GetIcon() => icon;
    public PackageType GetPackageType() => type;
    public dynamic GetEntrypoint() => mainClass;
    internal dynamic mainClass = null;
    internal string dllName = null;
    internal Assembly assembly = null;
    internal int expansionVersion = 0;
    internal Sprite icon;
    internal PackageType type = PackageType.Expansion;
    
    public StarlightPackageInfo()
    {
        
    }
}