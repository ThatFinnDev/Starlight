using System.Reflection;

namespace Starlight.Storage;

public struct StarlightExpansionInfo
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
    public string iconPath = "Assets/icon.png";


    internal string dllName = null;
    internal Assembly assembly = null;
    internal int expansionVersion = 0;
    internal Sprite icon;
    
    public StarlightExpansionInfo()
    {
        
    }
}