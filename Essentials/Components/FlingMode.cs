using Starlight.Managers;
using Starlight.Storage;
using UnityEngine.InputSystem;

namespace Starlight.Components;

[InjectClass]
internal class FlingMode : MonoBehaviour
{
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StarlightCommandManager.ExecuteByString("fling 100");
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            StarlightCommandManager.ExecuteByString("fling -100");
        }
    }
}