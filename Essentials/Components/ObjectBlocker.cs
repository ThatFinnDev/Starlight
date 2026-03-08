using Starlight.Storage;

namespace Starlight.Components;

[InjectClass]
internal class ObjectBlocker : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject);
    }
}