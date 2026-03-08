using System;
using Starlight.Enums;

namespace Starlight.Storage;


[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class CallOnAttribute : Attribute
{
    public CallEvent callEvent { get; }

    public CallOnAttribute(CallEvent callEvent)
    {
        this.callEvent = callEvent;
    }
}