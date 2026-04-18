using Il2CppTMPro;
using UnityEngine.UI;

namespace Starlight.UI.Blueprints;

public class NavbarUIBlueprint : UIBlueprint
{
    public NavBarUITab[] Tabs;
    private int _activeTab = 0;
    private readonly List<RectTransform> _panels = new();

    protected override void OnRender(UITheme theme, RectTransform obj)
    {
        var tabGroup = obj.AddComponent<HorizontalLayoutGroup>();
        tabGroup.spacing = 10;
        tabGroup.childAlignment = TextAnchor.UpperCenter;

        var buttonContainer = new GameObject("Tabs");
        CustomChildHolder = buttonContainer.AddComponent<RectTransform>();
        buttonContainer.transform.SetParent(obj);

        var panelContainer = new GameObject("Panels");
        panelContainer.transform.SetParent(obj);
        CustomChildHolder = panelContainer.AddComponent<RectTransform>();
        for (var i = 0; i < Tabs.Length; i++)
        {
            var tabIndex = i;
            var buttonObj = new GameObject(Tabs[i].Name);
            buttonObj.transform.SetParent(buttonContainer.transform);
            var button = buttonObj.AddComponent<Button>();
            var text = buttonObj.AddComponent<TextMeshProUGUI>();
            text.text = Tabs[i].Name;
            text.font = theme.DefaultFont;
            text.color = theme.TextColor;
            text.alignment = TextAlignmentOptions.Center;

            button.onClick.AddListener((SystemAction)(() => SetActiveTab(tabIndex)));

            var panel = Tabs[i].PanelUI.Render(theme, CustomChildHolder);
            panel.gameObject.SetActive(i == _activeTab);
            _panels.Add(panel);
        }
    }

    private void SetActiveTab(int index)
    {
        _activeTab = index;
        for (var i = 0; i < _panels.Count; i++)
            _panels[i].gameObject.SetActive(i == _activeTab);
    }
}

[System.Serializable]
public class NavBarUITab
{
    public string Name;
    public PanelUIBlueprint PanelUI;
}
