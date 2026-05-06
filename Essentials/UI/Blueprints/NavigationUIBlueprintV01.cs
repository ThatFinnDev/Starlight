using Il2CppTMPro;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class NavigationUIBlueprintV01 : UIBlueprint
{
    public List<UIBlueprint> tabs;
    public List<UIBlueprint> childrenWithButtons;
    private int _activeTab;
    private List<RectTransform> _panels = new();
    private List<RectTransform> _buttons = new();
    public Vector2 tabSize = new(1330,300);
    public float buttonHeight = 80f;
    public float topPadding = 40f;
    public float horizontalPaddingPercentage = 8f;
    public float buttonSpacing = 25f;
    public UIColor buttonTextColor = UIColor.TextButton;
    public bool buttonDisableAutoTranslation = false;
    public UIColorBlock buttonColorBlock = UIColorBlock.Buttons;
    public int buttonCornerRadius = 40;
    public FontStyles buttonFontStyle = FontStyles.Bold;

    protected override void OnRender(UITheme theme, FontTheme fontTheme, RectTransform obj)
    {

        _panels = new List<RectTransform>();
        _buttons = new List<RectTransform>();
        _activeTab = 0;
        var panelContainer = new GameObject("Panels");
        panelContainer.transform.SetParent(obj);
        CustomChildHolder = panelContainer.AddComponent<RectTransform>();
        CustomChildHolder.sizeDelta = tabSize;
        CustomChildHolder.anchoredPosition = Vector2.zero;
        
        var buttonContainer = new GameObject("Tabs");
        var bcRect = buttonContainer.AddComponent<RectTransform>();
        var bcLayout = buttonContainer.AddComponent<HorizontalLayoutGroup>();
        bcLayout.childAlignment = TextAnchor.MiddleCenter;
        bcLayout.childControlHeight = true;
        bcLayout.childControlWidth = true;
        bcLayout.spacing = buttonSpacing;
        bcLayout.padding = new RectOffset((int)(horizontalPaddingPercentage/100f * Size.x * ScaleFactor), (int)(horizontalPaddingPercentage/100f * Size.x * ScaleFactor), (int)(topPadding * ScaleFactor), 0);
        buttonContainer.transform.SetParent(obj);
        bcRect.anchorMin = new Vector2(0, 1);
        bcRect.anchorMax = new Vector2(1, 1);
        bcRect.sizeDelta = new Vector2(0, (buttonHeight+topPadding)*ScaleFactor);
        bcRect.anchoredPosition = new Vector2(0, bcRect.sizeDelta.y / -2);
        
        
        if(tabs!=null)
            for (var i = 0; i < tabs.Count; i++)
            {
                var tabIndex = i;
                var buttonUIBlueprint = new ButtonUIBlueprintV01()
                {
                    OnClick = (() => SetActiveTab(tabIndex)),
                    CornerRadius = buttonCornerRadius,
                    ButtonColors = buttonColorBlock,
                    Children =
                    [
                        new TextUIBlueprintV01()
                        {
                            color = buttonTextColor,
                            textContent = tabs[i].Name,
                            disableAutoTranslation = buttonDisableAutoTranslation,
                            alignment = TextAlignmentOptions.Center,
                            fontStyle = buttonFontStyle,
                            fontSize = 40,
                            Anchors = new Vector4(0,0,1,1),
                        }
                    ]
                };
                _buttons.Add(buttonUIBlueprint.Render(theme, fontTheme, bcRect));
                tabs[i].Anchors = new Vector4(CustomChildHolder.anchorMin.x, CustomChildHolder.anchorMin.y,CustomChildHolder.anchorMax.x,CustomChildHolder.anchorMax.y);
                tabs[i].Size = CustomChildHolder.sizeDelta;
                RectTransform panel = null;
                try
                {
                    panel = tabs[i].Render(theme, fontTheme, CustomChildHolder);
                    panel.gameObject.SetActive(i == _activeTab);
                } catch (Exception e) { LogError(e); } 
                _panels.Add(panel); 
            }

        if(childrenWithButtons!=null)
            for (var i = 0; i < childrenWithButtons.Count; i++)
                try { childrenWithButtons[i].Render(theme, fontTheme, bcRect); } catch (Exception e) { LogError(e); } 
        
        ExecuteInTicks(() => { try { _buttons[0].GetComponent<Button>().interactable = false; } catch { } },1);
    }

    private void SetActiveTab(int index)
    {
        _activeTab = index;
        for (var i = 0; i < _panels.Count; i++)
            if(_panels[i])
                _panels[i].gameObject.SetActive(i == _activeTab);
        for (var i = 0; i < _buttons.Count; i++)
            if(_buttons[i])
                _buttons[i].GetComponent<Button>().interactable = i != _activeTab;
    }
}

