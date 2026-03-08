using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Starlight.Storage;

namespace Starlight.Expansion;

[EditorBrowsable(EditorBrowsableState.Never)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract class StarlightExpansionVXX
{
    public abstract StarlightExpansionInfo info { get; }
    
    
    public Assembly Assembly => _assembly;
    private Assembly _assembly;
    public HarmonyLib.Harmony HarmonyInstance => _harmonyInstance;
    private HarmonyLib.Harmony _harmonyInstance;
    
}