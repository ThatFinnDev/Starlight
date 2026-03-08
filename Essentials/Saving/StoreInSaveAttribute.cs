using System;

namespace Starlight.Saving;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class StoreInSaveAttribute : Attribute { }
