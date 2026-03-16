using Starlight.Saving;

namespace Starlight.Prism.Data.LandPlots;

public class PrismLandPlotLocation : SubSave
{
    [StoreInSave] public Vector3 Position;
    [StoreInSave] public Quaternion Rotation;
    [StoreInSave] public Vector3 Scale;
    [StoreInSave] public string SceneName;
    [StoreInSave] public LandPlot.Id DefaultPlot;

    public PrismLandPlotLocation(Vector3 position, string sceneName, LandPlot.Id defaultPlot)
    {
        this.Position = position;
        this.Scale = new Vector3(1,1,1);
        this.SceneName = sceneName;
        this.DefaultPlot = defaultPlot;
    }
    public PrismLandPlotLocation(Vector3 position, Quaternion rotation, string sceneName, LandPlot.Id defaultPlot)
    {
        this.Position = position;
        this.Rotation = rotation;
        this.Scale = new Vector3(1,1,1);
        this.SceneName = sceneName;
        this.DefaultPlot = defaultPlot;
    }
    public PrismLandPlotLocation(Vector3 position, Quaternion rotation, Vector3 scale, string sceneName, LandPlot.Id defaultPlot)
    {
        this.Position = position;
        this.Rotation = rotation;
        this.Scale = scale;
        this.SceneName = sceneName;
        this.DefaultPlot = defaultPlot;
    }
    public PrismLandPlotLocation() {}
}