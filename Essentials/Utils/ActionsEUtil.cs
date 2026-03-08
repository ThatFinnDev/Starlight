using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace Starlight.Utils;

public static class ActionsEUtil
{
    internal static readonly Dictionary<SystemAction, int> ActionCounter = new ();
    public static void ExecuteInTicks(SystemAction action, int ticks)
    {
        if (action == null) return;
        if(ticks<=0)
            try { action.Invoke(); } catch (Exception e) { MelonLogger.Error(e); }
        else ActionCounter.Add((SystemAction)(() =>
        {
            try { action.Invoke(); } catch (Exception e) { MelonLogger.Error(e); }
        }),ticks);
    }
    public static void ExecuteInSeconds(SystemAction action, float seconds)
    {
        if(seconds<=0)
            try { action.Invoke(); } catch (Exception e) { MelonLogger.Error(e); }
        else MelonCoroutines.Start(Wait(seconds, action));
    }

    private static System.Collections.IEnumerator Wait(float seconds, SystemAction action)
    {
        yield return new WaitForSeconds(seconds);
        try { action.Invoke(); }catch (Exception e) { MelonLogger.Error(e); }
    }
    public static void InvokeAll(this List<SystemAction> actions) => actions.ForEach(action => action.Invoke());
    public static void InvokeAll(this List<Il2CppSystemAction> actions) => actions.ForEach(action => action.Invoke());
    public static void InvokeAll(this Il2CppSystem.Collections.Generic.List<SystemAction> actions) => actions.ToNetList().ForEach(action => action.Invoke());
    public static void InvokeAll(this Il2CppSystem.Collections.Generic.List<Il2CppSystemAction> actions) => actions.ToNetList().ForEach(action => action.Invoke());
    public static void InvokeAll(this SystemAction[] actions) => actions.ToNetList().ForEach(action => action.Invoke());
    public static void InvokeAll(this Il2CppSystemAction[] actions) => actions.ToNetList().ForEach(action => action.Invoke());
    public static void InvokeAll(this Il2CppReferenceArray<Il2CppSystemAction> actions) => actions.ToNetList().ForEach(action => action.Invoke());
    public static Il2CppSystemAction ToIl2CppAction(this SystemAction action) => action;
    public static Il2CppSystemAction ToNetAction(this SystemAction action) => action;

}