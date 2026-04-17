using Il2CppMonomiPark.SlimeRancher.DebugTool;
using SR2E.Storage;

namespace SR2E.Components.Debug;

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