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
            if (button.Label == null || button.Action == null) continue;
            try
            {
                if (!button.Enabled)
                {
                    if (__instance._menuItems.Contains(button.Model))
                        __instance._menuItems.Remove(button.Model);
                    continue;
                }
                if (button.Model != null)
                {
                    if (__instance._menuItems.Contains(button.Model))
                        continue;
                    if (!__instance._menuItems.Contains(button.Model))
                        __instance._menuItems.Insert(Math.Clamp(button.InsertIndex,0,__instance._menuItems.Count), button.Model);
                    continue;
                }
                button.Model = ScriptableObject.CreateInstance<RanchHouseMenuItemModel>();
                button.Model._onClick.AddListener(button.Action);
                button.Model.label = button.Label;
                button.Model.name = button.Label.GetLocalizedString();
                button.Model.hideFlags |= HideFlags.HideAndDontSave;

                if (!button.Enabled)
                {
                    if (__instance._menuItems.Contains(button.Model))
                        __instance._menuItems.Remove(button.Model);
                    continue;
                }
                if (!__instance._menuItems.Contains(button.Model))
                    __instance._menuItems.Insert(Math.Clamp(button.InsertIndex,0,__instance._menuItems.Count), button.Model);
            }
            catch (Exception e) { LogError(e); }


        }
        _safeLock = false;
    }
}