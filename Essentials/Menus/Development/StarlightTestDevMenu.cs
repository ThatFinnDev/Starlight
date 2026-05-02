using Il2CppInterop.Runtime.Attributes;
using Il2CppTMPro;
using Starlight.Enums;
using Starlight.Storage;
using Starlight.UI;
using Starlight.UI.Blueprints;

namespace Starlight.Menus.Development;

internal class StarlightTestDevMenu : StarlightMenu
{
    public new static MenuIdentifier GetMenuIdentifier() => new ("starlighttestdevemenu",StarlightMenuFont.Native,StarlightMenuTheme.Starlight, "TestDevMenu",true,true);

    protected override bool createCommands => true;
    protected override bool inGameOnly => false;
    protected override void OnAwake()
    {
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        requiredFeatures = [DevTestMenu];
        openActions = [MenuActions.PauseGameFalse];
        closeActions = [MenuActions.UnPauseGameFalse, MenuActions.EnableInput];
    }

    [HideFromIl2Cpp] private PanelUIBlueprintV01 blueprint => new ()
    {
        color = UIColor.Primary, cornerRadius = 90, size = new Vector2(1330, 840),
        children =
        [
            new PanelUIBlueprintV01 { color = UIColor.Accent, cornerRadius = 0, size = new Vector2(1330,10), position = new Vector2(0,-265f) },
            new PanelUIBlueprintV01 { color = UIColor.Accent, cornerRadius = 0, size = new Vector2(1330,10), position = new Vector2(0,265f) },
            new NavigationUIBlueprintV01
            {
                cornerRadius = 90, size = new Vector2(1330, 840),
                tabSize = new Vector2(1330,520),
                tabs = [
                    new PanelUIBlueprintV01
                    {
                        name = "Tab1",
                        children = [
                            new VScrollUIBlueprintV01
                            {
                                size = new Vector2(500,500), cornerRadius = 10,
                                children = [
                                    new ButtonUIBlueprintV01 { size = new Vector2(600,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 1", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 2", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(600,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 3", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(600,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 4", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(600,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 5", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 5", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 6", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 7", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 8", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 10", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 11", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 1", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                ]
                            }
                        ]
                    },
                    new PanelUIBlueprintV01
                    {
                        name = "Tab2",
                        children = [
                            new PanelUIBlueprintV01 { color = UIColor.Accent, cornerRadius = 0, size = new Vector2(10,520) },
                            new VScrollUIBlueprintV01
                            {
                                size = new Vector2(660,520), cornerRadius = 10, position = new Vector2(-332.5f,0),
                                children = [
                                    new ButtonUIBlueprintV01 { size = new Vector2(600,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 1", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                    new ButtonUIBlueprintV01 { size = new Vector2(500,70), cornerRadius = 30, children = [new TextUIBlueprintV01 { textContent = "Button 2", alignment = TextAlignmentOptions.Center, fontSize = 30, color = UIColor.TextButton }] },
                                ]
                            },
                            new TextUIBlueprintV01
                            {
                                size = new Vector2(660,520), cornerRadius = 10, position = new Vector2(332.5f,0),
                                textContent = "Youre on Panel 2", alignment = TextAlignmentOptions.TopLeft,
                                margins= new Vector4(10,10,10,10),
                            }
                        ]
                    },
                    new PanelUIBlueprintV01
                    {
                        name = "Tab3",
                        children = [
                            new SliderUIBlueprintV01()
                            {
                                size =  new Vector2(400,40),
                                defaultValue = 20,
                                minValue = 0,
                                maxValue = 100,
                            },
                            
                            new SliderUIBlueprintV01()
                            {
                                size =  new Vector2(400,40), position = new Vector2(0,-200),
                                defaultValue = 20,
                                minValue = 0,
                                maxValue = 100,
                                outputPow = 3.51f,outputMaxValue = 999999f
                            }
                        ]
                    },
                    new PanelUIBlueprintV01
                    {
                        name = "Test",
                        children = [
                            new CheckboxUIBlueprintV01()
                            {
                                size =  new Vector2(50,50), cornerRadius = 10, position = new Vector2(0,-200),
                                defaultValue=true
                            },
                            new InputUIBlueprintV01()
                            {
                                size =  new Vector2(200,50), cornerRadius = 10,
                                defaultValue = "SomeValue", placeHolderContent = "hii"
                            },
                            
                        ]
                    }
                ]
            },
            new PanelUIBlueprintV01
            {
                color = UIColor.Badge, cornerRadius = 50, size = new Vector2(530,90), position = new Vector2(0,-420f),
                children = [
                    new TextUIBlueprintV01 { textContent = "Test Menu", size = new Vector2(530,90), fontSize = 50,  alignment = TextAlignmentOptions.Center, color = UIColor.TextButton }
                ]
                
            },
        ]
    };
    
    internal static readonly LKey OpenKey = LKey.F8;
    public new static GameObject GetMenuRootObject()
    {
        var obj = new GameObject("StarlightTestDevMenu");
        var rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero; 
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        return obj;
    }

    private RectTransform _openThing;
    protected override void OnOpen()
    {
        _openThing = blueprint.Render(currentTheme, currentFontTheme, transform);
    }

    protected override void OnClose()
    {
        Destroy(_openThing.gameObject);
    }
    
    public override void OnCloseUIPressed()
    {
        if (MenuEUtil.isAnyPopUpOpen) return;
        Close();
    }
}