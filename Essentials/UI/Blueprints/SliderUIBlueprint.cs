using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class SliderUIBlueprint : UIBlueprint
{
    public float MinValue = 0;
    public float MaxValue = 1;
    public float Value;

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var slider = obj.AddComponent<Slider>();

        slider.minValue = MinValue;
        slider.maxValue = MaxValue;
        slider.value = Value;

        var background = new GameObject("Background");
        background.transform.SetParent(obj);
        var backgroundImage = background.AddComponent<Image>();
        backgroundImage.color = theme.SecondaryColor;

        var fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(obj);
        var fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform);
        var fillImage = fill.AddComponent<Image>();
        fillImage.color = theme.AccentColor;
        slider.fillRect = fill.GetComponent<RectTransform>();

        var handleSlideArea = new GameObject("Handle Slide Area");
        handleSlideArea.transform.SetParent(obj);
        var handle = new GameObject("Handle");
        handle.transform.SetParent(handleSlideArea.transform);
        var handleImage = handle.AddComponent<Image>();
        handleImage.color = theme.AccentColor;
        slider.targetGraphic = handleImage;
        slider.handleRect = handle.GetComponent<RectTransform>();
    }

}