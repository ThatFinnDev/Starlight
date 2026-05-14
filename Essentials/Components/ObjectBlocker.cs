using Starlight.Storage;

namespace Starlight.Components;

[InjectIntoIL]
internal class ObjectBlocker : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject);
    }
}