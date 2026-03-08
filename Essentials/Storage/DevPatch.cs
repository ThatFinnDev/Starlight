using System;

namespace Starlight.Storage;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DevPatch : Attribute
{
    public DevPatch(){}
}