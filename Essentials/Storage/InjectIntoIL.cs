using System;

namespace Starlight.Storage;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectIntoIL : Attribute
{
    public readonly bool LOGOnFail = true;
    public InjectIntoIL() {}

    public InjectIntoIL(bool logOnFail)
    {
        this.LOGOnFail = logOnFail;
    }
}