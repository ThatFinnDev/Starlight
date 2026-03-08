using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Starlight.Enums;

[Serializable]
[JsonConverter(typeof(StringEnumConverter))]
public enum StarlightMenuTheme
{
    Default=0, 
    SR2E=1, 
    Black=2
}