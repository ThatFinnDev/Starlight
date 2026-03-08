using System;

namespace Starlight.Storage;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
internal class InjectClass : Attribute
{
    internal InjectClass(){}
}