using Starlight.Enums;

namespace Starlight.Managers;

public static class StarlightBindingManger
{
    /// <summary>
    /// Bind a command to a key
    /// </summary>
    /// <param name="key">The key that should be bound</param>
    /// <param name="command">The command that should be executed</param>
    public static void BindKey(LKey key, string command)
    {
        if (StarlightSaveManager.data.keyBinds.ContainsKey(key)) StarlightSaveManager.data.keyBinds[key] += ";" + command;
        else StarlightSaveManager.data.keyBinds.Add(key, command);
        StarlightSaveManager.Save();
    }
    /// <summary>
    /// Unbinds every command currently bound to a key
    /// </summary>
    /// <param name="key">The key which should be unbound</param>
    public static void UnbindKey(LKey key)
    {
        if (StarlightSaveManager.data.keyBinds.ContainsKey(key)) StarlightSaveManager.data.keyBinds.Remove(key);
        StarlightSaveManager.Save();
    }
    /// <summary>
    /// Get every command separated by a semicolon which is bound to a key
    /// </summary>
    /// <param name="key">The key to be checked</param>
    /// <returns>The command</returns>
    public static string GetBind(LKey key)
    {
        if (StarlightSaveManager.data.keyBinds.ContainsKey(key)) return StarlightSaveManager.data.keyBinds[key]; return null;
    }

    /// <summary>
    /// Returns true if a key has a command bound to it
    /// </summary>
    /// <param name="key">The key to be checked</param>
    /// <returns>bool</returns>
    public static bool isKeyBound(LKey key) => StarlightSaveManager.data.keyBinds.ContainsKey(key);
    
        
    internal static void Update()
    {
        try
        {
            foreach (KeyValuePair<LKey,string> keyValuePair in StarlightSaveManager.data.keyBinds)
            {
                if (keyValuePair.Key.OnKeyDown())
                {
                    if (StarlightWarpManager.warpTo == null)
                        StarlightCommandManager.ExecuteByString(keyValuePair.Value, true);
                }
            }
        }
        catch (Exception e) {LogError(e);}
    }
}