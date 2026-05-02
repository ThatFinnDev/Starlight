using System;
using Il2CppTMPro;
using Starlight.Components.AssetBundle;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class SliderUIBlueprintV01 : UIBlueprint
{
    public UIColor backgroundColor = UIColor.Accent;
    public Color? customBackgroundColor = null;
    public UIColor handleColor = UIColor.Badge;
    public Color? customHandleColor = null;
    public TMP_FontAsset customHandleFont;
    
    public UIColor handleTextColor = UIColor.TextGeneral;
    public Color? customHandleTextColor = null;
    public float minValue = 0;
    public float maxValue = 100;
    public float defaultValue = 0;
    public bool wholeNumbers = true;
    public bool allowDisplayingDecimalNumbers = true;
    public float handleWidth = 60f;
    public float outputPow = 1f;
    public float outputMaxValue = float.MaxValue;
    private float _oldValue = 0f;
    public System.Action<float> onValueChanged = null;

    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        var slider = obj.AddComponent<Slider>();

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = Mathf.Clamp(defaultValue,minValue,maxValue);
        _oldValue = slider.value;
        slider.wholeNumbers = wholeNumbers;
        
        var bColor = customBackgroundColor ?? theme.GetColor(backgroundColor);
        var background = new GameObject("Background");
        var backgroundRect = background.AddComponent<RectTransform>();
        background.transform.SetParent(obj, false);
        backgroundRect.sizeDelta = new Vector2(size.x*ScaleFactor, size.y*ScaleFactor / 2.5f);
        backgroundRect.anchoredPosition = Vector2.zero;
        var backgroundImage = background.AddComponent<Image>();
        backgroundImage.color = bColor;
        var backgroundSortGroup = background.AddComponent<SortingGroup>();
        backgroundSortGroup.enabled = false;
        backgroundSortGroup.sortingOrder = Mathf.FloorToInt(5 * ScaleFactor);
        backgroundSortGroup.AddComponent<RoundedUIImage>().CornerRadius = 5 * ScaleFactor;
        
        var fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(obj);
        var fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.sizeDelta = backgroundRect.sizeDelta;
        fillAreaRect.anchorMin = new Vector2(1, .75f);
        fillAreaRect.anchorMax = new Vector2(0, .25f);
        fillAreaRect.anchoredPosition = Vector2.zero;
        
        var fill = new GameObject("Fill");
        var fillRect = fill.AddComponent<RectTransform>();
        fillRect.sizeDelta = backgroundRect.sizeDelta;
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(1, 1);
        fillRect.anchoredPosition = Vector2.zero;
        fill.transform.SetParent(fillArea.transform);
        var fillImage = fill.AddComponent<Image>();
        fillImage.color = bColor*new Color(0.75f,0.75f,0.75f,1f);
        slider.fillRect = fillRect;

        var handleSlideArea = new GameObject("Handle Slide Area");
        var handleSlideAreaRect = handleSlideArea.AddComponent<RectTransform>();
        handleSlideAreaRect.anchorMin = Vector2.zero;
        handleSlideAreaRect.anchorMax = new Vector2(1, 1);
        handleSlideAreaRect.offsetMin = new Vector2(0, 0);
        handleSlideAreaRect.offsetMax = new Vector2(0, 0);
        handleSlideArea.transform.SetParent(obj);
        handleSlideAreaRect.anchoredPosition = new Vector2(-(handleWidth / 4f)*ScaleFactor,0);
        handleSlideAreaRect.sizeDelta = new Vector2((-handleWidth - (handleWidth / 2f))*ScaleFactor, 0);
        
        var handle = new GameObject("Handle");
        var handleRect = handle.AddComponent<RectTransform>();
        handle.transform.SetParent(handleSlideArea.transform);
        handleRect.offsetMin = new Vector2(-(handleWidth/2f), -10)*ScaleFactor;
        handleRect.offsetMax = new Vector2(handleWidth, 10)*ScaleFactor;
        var handleGroup = handle.AddComponent<SortingGroup>();
        handleGroup.enabled = false;
        handleGroup.sortingOrder = Mathf.FloorToInt(5 * ScaleFactor);
        handleGroup.AddComponent<RoundedUIImage>().CornerRadius = 5 * ScaleFactor;
        
        
        var handleImage = handle.AddComponent<Image>();
        handleImage.color = customHandleColor ?? theme.GetColor(handleColor);
        slider.targetGraphic = handleImage;
        slider.handleRect = handleRect;
        
        
        var text = new GameObject("Text");
        text.transform.SetParent(handle.transform);
        var textRect = text.AddComponent<RectTransform>();
        textRect.sizeDelta = Vector2.zero;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = new Vector2(1, 1);
        textRect.anchoredPosition = Vector2.zero;
        
        
        var txt = text.AddComponent<TextMeshProUGUI>();
        txt.margin = new Vector4(3, 6, 3, 6) * ScaleFactor;
        txt.font = customHandleFont ?? fontTheme.DefaultFont;
        txt.enableAutoSizing = true;
        txt.fontSizeMax = 999f;
        txt.fontSizeMin = 1f;
        txt.alignment = TextAlignmentOptions.Center;
        txt.color = customHandleTextColor ?? theme.GetColor(handleTextColor);
        txt.SetText(slider.value.ToString());
        var call = (System.Action<float>)((output) =>
        {
            var newValue = Mathf.Clamp((float)Math.Pow(output, outputPow), minValue, outputMaxValue);
            if (wholeNumbers) newValue = Mathf.FloorToInt(newValue);
            var newText = newValue.ToString();
            if (newText.EndsWith(".0")||!allowDisplayingDecimalNumbers) newText = Mathf.FloorToInt(newValue).ToString();
            txt.SetText(newText);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if(onValueChanged!=null&&_oldValue!=newValue)
            {
                _oldValue = newValue;
                onValueChanged.Invoke(newValue);
            }
        });
        call.Invoke(slider.value);
        slider.onValueChanged.AddListener(call);
    }

}