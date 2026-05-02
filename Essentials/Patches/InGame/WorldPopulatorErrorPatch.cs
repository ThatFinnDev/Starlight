using System;
using System.Linq;
using System.Reflection;

namespace Starlight.Patches.InGame;

public static class WorldPopulatorErrorPatch
{
    // IEnumerators get turned into classes and have weird suffixes like _d_5_
    // In the interest of not fixing this every version and in between Store versions, I made this
    // We love Il2CPP
    public static void Apply(HarmonyLib.Harmony harmony)
    {
        var worldPopulatorType = typeof(WorldPopulator);
        var nestedTypes = worldPopulatorType.GetNestedTypes();

        var patchableMethods = new[]
            { "PopulateRanch", "Populate_", "PopulateActors", "PopulateDrones", "PopulateGadgets" };

        foreach (var nestedType in nestedTypes)
            if (patchableMethods.Any(m => nestedType.Name.Contains(m)))
            {
                var moveNextMethod = nestedType.GetMethod("MoveNext");
                if (moveNextMethod != null)
                {
                    var prefixMethod = typeof(WorldPopulatorErrorPatch).GetMethod(nameof(Prefix), BindingFlags.Static | BindingFlags.Public);
                    harmony.Patch(moveNextMethod, new HarmonyMethod(prefixMethod));
                }
            }
    }

    public static void Prefix(object __instance)
    {
        if (!ShowWorldPopulatorErrors.HasFlag()) return;

        var instanceType = __instance.GetType();
        var onFailField = instanceType.GetField("onFail", BindingFlags.Instance | BindingFlags.Public);
        if (onFailField == null)
        {
            LogError($"Could not find 'onFail' field in {instanceType.FullName}");
            return;
        }
        var coroutineName = instanceType.Name;
        var onFailAction = onFailField.GetValue(__instance) as Action<Il2CppSystem.Exception>;

        Action<Il2CppSystem.Exception> newAction = (exception) =>
        {
            LogError($"Coroutine exception in {coroutineName}:\n{exception.ToString()}");
        };

        if (IgnoreWorldPopulatorErrors.HasFlag())
            onFailField.SetValue(__instance, newAction);
        else
            onFailField.SetValue(__instance, onFailAction + newAction);

    }
}

