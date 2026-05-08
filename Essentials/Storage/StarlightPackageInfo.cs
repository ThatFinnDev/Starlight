using System.Reflection;
using Starlight.Enums;

namespace Starlight.Storage;

public record struct StarlightPackageInfo
{
    public string ID = null;
    public string Name = null;
    public string Description = null;
    public string Version = null;
    public string Author = null;
    public string[] CoAuthors = null;
    public string[] Contributors = null;
    public string[] Dependencies = null;
    public string SourceCode = null;
    public string Nexus = null;
    public string Discord = null;
    public bool UsePrism = false;
    public string IconPath = "Assets.icon.png";
    public ExpansionLoadTime LoadTime = ExpansionLoadTime.Startup;
    public ExpansionUnloadTime UnloadTime = ExpansionUnloadTime.Never;
    public MultiplayerRequirement MultiplayerRequirement = MultiplayerRequirement.ServerAndClient;

    public string GetDllName() => DLLName;
    public Assembly GetAssembly() => RunningAssembly;
    public int GetExpansionVersion() => expansionVersion;
    public Sprite GetIcon() => icon;
    public PackageType GetPackageType() => type;
    public dynamic GetEntrypoint() => MainClass;
    internal dynamic MainClass = null;
    internal string DLLName = null;
    internal Assembly RunningAssembly = null;
    internal int expansionVersion = 0;
    internal Sprite icon;
    internal PackageType type = PackageType.Expansion;
    
    public StarlightPackageInfo()
    {
        
    }
}