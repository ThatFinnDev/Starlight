namespace Starlight.Patches.InGame;

[HarmonyPatch(typeof(PlayerState), nameof(PlayerState.InitModel))]
public class PlayerStateInitModelPatch
{
    internal static void Postfix(PlayerState __instance)
    {
        ExecuteInTicks((() =>
        {
            try
            {
                if(__instance.GetCurrHealth()<0)
                    __instance.SetHealth(__instance.GetMaxHealth());
            } catch {}
            try
            {
                if(__instance.GetCurrEnergy()<0)
                    __instance.SetEnergy(__instance.GetMaxEnergy());
            } catch {}
        }),2);
    }
}