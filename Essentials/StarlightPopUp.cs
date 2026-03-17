using System;
using System.Reflection;
using Il2CppTMPro;
using Starlight.Enums;
using Starlight.Enums.Sounds;
using Starlight.Patches.Context;
using Starlight.Storage;

namespace Starlight;
/// <summary>
/// Abstract menu class
/// </summary>
[InjectIntoIL]
public abstract class StarlightPopUp : MonoBehaviour
{
    internal Transform block;

    public static void PreAwake(GameObject obj,List<object> objects) {}
    private void DisableBlock()
    {
        if(block!=null)
            Destroy(block.gameObject);
    }
    
    public new void Close()
    {
        AudioEUtil.PlaySound(MenuSound.ClosePopup);
        DisableBlock();
        MenuEUtil.OpenPopUps.Remove(this);
        Destroy(gameObject);
    }
    public virtual void ApplyFont(TMP_FontAsset font)
    {
        foreach (var text in gameObject.GetAllChildrenOfType<TMP_Text>())
            text.font = font;
    }
    protected static void _Open(string identifier,Type type,StarlightMenuTheme theme,List<object> objects)
    {
        var asset = SystemContextPatch.bundle.LoadAsset(SystemContextPatch.getPopUpPath(identifier,theme));
        var instance = GameObject.Instantiate(asset, StarlightEntryPoint.StarlightStuff.transform);
        ExecuteInTicks((() =>
        {
            for (int i = 0; i < StarlightEntryPoint.StarlightStuff.transform.childCount; i++)
            {
                Transform child = StarlightEntryPoint.StarlightStuff.transform.GetChild(i);
                if (child.name == instance.name)
                {
                    try
                    {
                        var methodInfo = type.GetMethod(nameof(StarlightPopUp.PreAwake), BindingFlags.Static | BindingFlags.Public);
                        if (methodInfo != null)
                            methodInfo.Invoke(null, [child.gameObject, objects]);
                        child.gameObject.SetActive(true);
                    }catch (Exception e) { LogError(e); }
                }
            }
            AudioEUtil.PlaySound(MenuSound.OpenPopup);
        }), 1);
    }
    protected virtual void OnOpen() {}
    public void Awake()
    {
        MenuEUtil.OpenPopUpBlock(this);
        MenuEUtil.OpenPopUps.Add(this);
        
    }

    private void Start()
    {
        OnOpen();
    }

    protected void Update()
    {
        OnUpdate();
    } protected virtual void OnUpdate() {}
    
}

