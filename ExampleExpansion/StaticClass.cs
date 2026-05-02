using Starlight.Enums;
using Starlight.Storage;

namespace StarlightExampleExpansion;

public static class StaticClass
{
    // Get events from EVERYWHERE
    // Add a CallOn attribute to a static void
    // Check the tooltip of the enum value to see
    // if it has any custom arguments you might wanna use
    
    [CallOn(CallEvent.AfterGameContextLoad)]
    public static void CustomTest(GameContext gameContext)
    {
        Log("GameContext has been loaded!");
        Log(gameContext.name);
    }
}