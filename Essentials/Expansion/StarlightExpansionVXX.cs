using System.ComponentModel;
using System.Reflection;
using Starlight.Storage;

namespace Starlight.Expansion;

[EditorBrowsable(EditorBrowsableState.Never)]

public abstract class StarlightExpansionVXX
{
    public abstract StarlightExpansionInfo info { get; }
    
    
    public Assembly Assembly => _assembly;
    private Assembly _assembly;
    
}