using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class AlignerUIBlueprint : UIBlueprint
{
    public bool IsVertical;

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        if (IsVertical) obj.AddComponent<VerticalLayoutGroup>();
        else obj.AddComponent<HorizontalLayoutGroup>();
    }
}
