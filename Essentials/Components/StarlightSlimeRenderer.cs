using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Starlight.Patches.Context;
using Starlight.Storage;

namespace Starlight.Components;

[InjectIntoIL]
public class StarlightSlimeRenderer : MonoBehaviour
{
    private GameObject _renderedObj;

    private bool _oldUnscaledTime;
    public bool unscaledTime
    {
        get
        {
            try { if(_renderedObj.GetObjectRecursively<Animator>("Appearance").updateMode==AnimatorUpdateMode.UnscaledTime) return true; }
            catch {}
            return false;
        }
        set
        {
            _oldUnscaledTime = value;
            if(value)
                try { _renderedObj.GetObjectRecursively<Animator>("Appearance").updateMode=AnimatorUpdateMode.UnscaledTime; }
                catch {}
            else 
                try { _renderedObj.GetObjectRecursively<Animator>("Appearance").updateMode=AnimatorUpdateMode.Normal; }
                catch {}
        }
    }

    public void DestroyRender()
    {
        if(_renderedObj) Destroy(_renderedObj);
    }
    public void RenderAppearance(SlimeAppearance appearance,float scale = 1.0f)
    {
        if(_renderedObj) Destroy(_renderedObj);
        _renderedObj = Instantiate(GameContextPatch.SlimeRendererPrefab,transform);
        var app = _renderedObj.transform.GetChild(0).gameObject.GetComponent<SlimeAppearanceApplicator>();
        app.Appearance = appearance;
        app.AppearanceObjectProvider = GameContextPatch.RendererPool;
        _renderedObj.SetActive(true);
        app.transform.localScale = new Vector3(scale, scale, scale);
        app.ApplyAppearance();
        app.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        SetLayerRecursive(_renderedObj, gameObject.layer);
        unscaledTime = _oldUnscaledTime;
    }
    void SetLayerRecursive(GameObject obj, int newLayer)
    {
        if (null == obj) return;
        obj.layer = newLayer;
        foreach (var child in obj.transform.GetChildren())
        {
            if (null == child) continue;
            SetLayerRecursive(child.gameObject, newLayer);
        }
    }
}