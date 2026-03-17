using System;
using Starlight.Buttons;
using Il2CppMonomiPark.SlimeRancher.UI.RanchHouse;

namespace Starlight.Patches.InGame;

[HarmonyPatch(typeof(RanchHouseMenuRoot), nameof(RanchHouseMenuRoot.Awake))]
internal static class SR2RanchUIButtonPatch
{
    internal static readonly List<CustomRanchUIButton> Buttons = new ();
    private static bool _safeLock;
    internal static bool PostSafeLock;
    internal static void Prefix(RanchHouseMenuRoot __instance)
    {
        if (!InjectRanchUIButtons.HasFlag()) return;
        if (_safeLock) { return; }
        _safeLock = true;
        foreach (var button in Buttons)
        {
            if (button.label == null || button.action == null) continue;
            try
            {
                if (!button.enabled)
                {
                    if (__instance._menuItems.Contains(button._model))
                        __instance._menuItems.Remove(button._model);
                    continue;
                }
                if (button._model != null)
                {
                    if (__instance._menuItems.Contains(button._model))
                        continue;
                    if (!__instance._menuItems.Contains(button._model))
                        __instance._menuItems.Insert(Math.Clamp(button.insertIndex,0,__instance._menuItems.Count), button._model);
                    continue;
                }
                button._model = ScriptableObject.CreateInstance<RanchHouseMenuItemModel>();
                button._model._onClick.AddListener(button.action);
                button._model.label = button.label;
                button._model.name = button.label.GetLocalizedString();
                button._model.hideFlags |= HideFlags.HideAndDontSave;

                if (!button.enabled)
                {
                    if (__instance._menuItems.Contains(button._model))
                        __instance._menuItems.Remove(button._model);
                    continue;
                }
                if (!__instance._menuItems.Contains(button._model))
                    __instance._menuItems.Insert(Math.Clamp(button.insertIndex,0,__instance._menuItems.Count), button._model);
            }
            catch (Exception e) { LogError(e); }


        }
        _safeLock = false;
    }
}