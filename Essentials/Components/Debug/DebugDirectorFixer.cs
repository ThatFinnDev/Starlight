using System;
using Il2CppMonomiPark.SlimeRancher.DebugTool;
using Starlight.Enums;
using Starlight.Storage;

namespace Starlight.Components;

[InjectClass]
public class DebugDirectorFixer : MonoBehaviour
{
    internal static DebugDirectorFixer Instance;
    internal DebugDirector director;
    void Start()
    {
        Instance = this;
        director = GetComponent<DebugDirector>();
    }

}