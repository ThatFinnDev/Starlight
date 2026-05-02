using System;

namespace Starlight.Storage;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectIntoIL : Attribute
{
    public bool logOnFail = true;
    public InjectIntoIL() {}

    public InjectIntoIL(bool logOnFail)
    {
        this.logOnFail = logOnFail;
    }
}