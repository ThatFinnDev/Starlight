using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using Il2CppMonomiPark.SlimeRancher.UI;
using SR2E.Patches.Context;
using SR2E.Storage;
using UnityEngine.UI;

namespace SR2E.Patches.General;


[HarmonyPatch(typeof(BootstrapCompanyLogoScene), nameof(BootstrapCompanyLogoScene.Start))]
internal static class BootstrapCompanyLogoScenePatch
{
    public static void Prefix(BootstrapCompanyLogoScene __instance)
    {
        GameObject obj = new GameObject("MLIcon", typeof(RectTransform).il2cppTypeof(), typeof(Image).il2cppTypeof());
        Image img = obj.GetComponent<Image>();
        img.sprite = EmbeddedResourceEUtil.LoadSprite("Assets.mlIcon.png").CopyWithoutMipmaps();
        if (false) //temp fix
        {
            
            if(SystemContextPatch.Bundle==null) SystemContextPatch.Bundle = EmbeddedResourceEUtil.LoadIl2CppBundle("Assets.srtwoessentials.assetbundle");
            //Loading it by path doesn't work
            foreach (var asset in SystemContextPatch.Bundle.LoadAllAssets())
                if (asset.TryCast<Texture2D>() != null && asset.name=="mlIcon")
                {
                    img.sprite = asset.Cast<Texture2D>().Texture2DToSprite();
                    break;
                }
        }
        img.preserveAspect = true;
        var rt = obj.GetComponent<RectTransform>();
        rt.SetParent(__instance.transform, false);
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 0);
        rt.anchoredPosition = new Vector2(10f,10f);
        rt.sizeDelta = new Vector2(100, 100);
        rt.SetParent(__instance.transform, false);

        obj.transform.SetParent(__instance.companyLogo.gameObject.GetComponent<RectTransform>(), true);
    }

}