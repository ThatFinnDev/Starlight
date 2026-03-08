using System;
using Starlight.Storage;

namespace Starlight.Expansion;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public class StarlightExpansionAttribute : Attribute
{
    public Type[] types;

    public StarlightExpansionAttribute(Type type)
    {
        this.types = [type];
    }
    public StarlightExpansionAttribute(Type[] types)
    {
        this.types = types;
    }
}
