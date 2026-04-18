using Il2CppTMPro;

namespace Starlight.UI.Blueprints;

public class InputUIBlueprint : UIBlueprint
{
    public TMP_InputField.ContentType ContentType = TMP_InputField.ContentType.Standard;

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var inputField = obj.AddComponent<TMP_InputField>();
        inputField.contentType = ContentType;

    }
}
