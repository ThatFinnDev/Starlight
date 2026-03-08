using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Starlight.Enums;

[Serializable]
[JsonConverter(typeof(StringEnumConverter))]
public enum StarlightMenuFont
{
    Default=0,
    SR2=1, 
    Bold=2, 
    Regular=3,
    NotoSans=4
}