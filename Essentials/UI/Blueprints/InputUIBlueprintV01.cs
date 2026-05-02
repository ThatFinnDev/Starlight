using Il2CppTMPro;
using Starlight.Components.AssetBundle;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class InputUIBlueprintV01 : UIBlueprint
{
    public UIColor textColor = UIColor.TextGeneral;
    public Color? customTextColor = null;
    public UIColor backgroundColor = UIColor.Primary;
    public Color? customBackgroundColor = null;
    public Sprite customCheckSprite;
    public TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard;
    public TMP_InputField.LineType lineType = TMP_InputField.LineType.SingleLine;
    public string placeHolderContent = "";
    public bool disablePlaceHolderAutoTranslation = false;
    public string defaultValue = "";
    public float fontSize = 20f;
    public bool restoreOriginalTextOnEscape = true;
    public TMP_FontAsset customFont;
    public System.Action<string> onValueChanged = null;
    public System.Action<string> onEndEdit = null;
    public System.Action<string> onSelect = null;
    public System.Action<string> onDeselect = null;
    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {
        ignoreCorners = true;
        var inputObject = TMP_DefaultControls.CreateInputField(new TMP_DefaultControls.Resources());
        inputObject.transform.SetParent(obj);
        if(cornerRadius>0)
        {
            var sortGroup = inputObject.AddComponent<SortingGroup>();
            sortGroup.enabled = false;
            sortGroup.sortingOrder = Mathf.FloorToInt(cornerRadius * ScaleFactor);
            inputObject.AddComponent<RoundedUIImage>().CornerRadius = cornerRadius * ScaleFactor;
        }
        var inputRect = inputObject.GetComponent<RectTransform>();
        inputRect.anchorMin = Vector2.zero;
        inputRect.anchorMax = new Vector2(1, 1);
        inputRect.offsetMin = Vector2.zero;
        inputRect.offsetMax = Vector2.zero;
        inputRect.anchoredPosition = Vector2.zero;
        
        var inputField = inputObject.GetComponent<TMP_InputField>();
        inputField.contentType = contentType;
        inputField.text = defaultValue;
        inputField.lineType = lineType;
        inputField.restoreOriginalTextOnEscape = restoreOriginalTextOnEscape;
        
        var txt = inputField.textComponent;
        txt.fontSize = fontSize*ScaleFactor;
        txt.font = customFont ?? fontTheme.DefaultFont;
        txt.color = customTextColor ?? theme.GetColor(textColor);
        
        var placeholder = inputField.placeholder.GetComponent<TextMeshProUGUI>();
        placeholder.text = disablePlaceHolderAutoTranslation?placeHolderContent:Tr(placeHolderContent);
        placeholder.fontSize = fontSize*ScaleFactor;
        placeholder.font = customFont ?? fontTheme.DefaultFont;
        placeholder.color = (customTextColor ?? theme.GetColor(textColor))*new Color(0.2f,0.2f,0.2f,.5f);
        
        var backgroundImage = inputObject.GetComponent<Image>();
        backgroundImage.color = customBackgroundColor ?? theme.GetColor(backgroundColor);
        backgroundImage.sprite = customCheckSprite;
        
        if(onValueChanged!=null) inputField.onValueChanged.AddListener(onValueChanged);
        if(onEndEdit!=null) inputField.onEndEdit.AddListener(onEndEdit);
        if(onSelect!=null) inputField.onSelect.AddListener(onSelect);
        if(onDeselect!=null) inputField.onDeselect.AddListener(onDeselect);
    }
}
