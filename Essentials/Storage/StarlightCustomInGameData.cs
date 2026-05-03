using Starlight.Managers;
using Starlight.Saving;

namespace Starlight.Storage;

internal class StarlightCustomInGameData : RootSave
{
    [StoreInSave] public StarlightOptionsButtonManager.CustomOptionsInGameSave OptionsSave = new ();
    [StoreInSave] public Dictionary<string,StarlightLandPlotLocation> CustomPlots = new ();
}