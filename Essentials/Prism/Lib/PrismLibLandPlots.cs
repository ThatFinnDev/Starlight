using Starlight.Prism.Data.LandPlots;
using UnityEngine.SceneManagement;

namespace Starlight.Prism.Lib;

public static class PrismLibLandPlots
{
    private static Dictionary<string, PrismLandPlotLocation> _customPlots = new ();
    private static Dictionary<string, GameObject> _rootObjects = new ();
    internal static List<LandPlotLocation> LandPlotLocations = new();

    private static GameObject GetNewLandPlotRoot(string sceneName)
    {
        if(_rootObjects.ContainsKey(sceneName))
            if (_rootObjects[sceneName] != null)
                return _rootObjects[sceneName];
        var gameObj = new GameObject("PrismLandPlotRoots-" + sceneName);
        SceneManager.MoveGameObjectToScene(gameObj, SceneManager.GetSceneByName(sceneName));
        var foundACell = false;
        foreach (var dir in GetAllInScene<CellDirector>())
        {
            if (dir.gameObject.scene.name == sceneName)
            {
                gameObj.transform.SetParent(dir.transform);
                foundACell = true;
                break;
            }
        }
        if(!foundACell)
        {
            var anyDir = GetAnyInScene<CellDirector>();
            if(anyDir==null) Log("Oh oh... A landplot is outside a CellDirector. Things are about to get sideways");
            else gameObj.transform.SetParent(anyDir.transform);
        }
        _rootObjects[sceneName] = gameObj;
        return gameObj;
    }
    internal static void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        ExecuteInTicks((() =>
        {
            if (sceneName == "MainMenuUI")
            {
                LandPlotLocations = new();
                _customPlots = new();
                _rootObjects = new();
            }
            foreach (var plot in _customPlots)
            {
                if (plot.Value.SceneName == sceneName)
                {
                    try
                    {
                        SpawnLandPlot(plot.Key, plot.Value);
                    }
                    catch (Exception e) { LogError(e); }
                }
            }
        }),2);
    }
    /// <summary>
    /// Note, custom landplots don't support drones yet!
    /// </summary>
    /// <param name="id"></param>
    /// <param name="loc"></param>
    public static void AddLandPlotLocation(string id,PrismLandPlotLocation loc)
    {
        if (!inGame) return;
        if (loc == null || string.IsNullOrWhiteSpace(id)) return;
        var scene = SceneManager.GetSceneByName(loc.SceneName);
        if (_customPlots.ContainsKey(id)) return;
         
        _customPlots.Add(id,loc);
        if (scene.isLoaded)
            SpawnLandPlot(id, loc);

    }

    public static bool HasLandPlotLocation(string id) => _customPlots.ContainsKey(id);
    public static void RemoveLandPlotLocation(string id)
    {
        if (!inGame) return;
        if (string.IsNullOrWhiteSpace(id)) return;
        if (!_customPlots.ContainsKey(id)) return;
        _customPlots.Remove(id);
        try { Object.Destroy(sceneContext.GameModel.landPlots[id].gameObj); } catch { }
        try { Object.Destroy(sceneContext.GameModel.landPlots["plot"+id].gameObj); } catch { }
        try { sceneContext.GameModel.UnregisterLandPlot(id); }catch { }
        try { sceneContext.GameModel.UnregisterLandPlot("plot"+id); }catch { }
        if (sceneContext.GameModel.landPlots.ContainsKey(id))
            sceneContext.GameModel.landPlots.Remove(id);
        if (sceneContext.GameModel.landPlots.ContainsKey("plot"+id))
            sceneContext.GameModel.landPlots.Remove(id);
    }

    static void SpawnLandPlot(string plotKey, PrismLandPlotLocation loc)
    { 
        var landplotRoot = GetNewLandPlotRoot(loc.SceneName);
        var obj = new GameObject(plotKey);
        var lpl = obj.AddComponent<LandPlotLocation>();
        LandPlotLocations.Add(lpl);
        lpl._id = "plot" + plotKey;
        obj.transform.SetParent(landplotRoot.transform);
        obj.transform.position = loc.Position;
        obj.transform.rotation = loc.Rotation;
        obj.transform.localScale = loc.Scale;
        var id = loc.DefaultPlot;
        if (id == LandPlot.Id.NONE) id = LandPlot.Id.EMPTY;        
        ExecuteInTicks(() =>
        {
            var prefab = gameContext.LookupDirector.GetPlotPrefab(id);
            var plotObj = Object.Instantiate(prefab, obj.transform);
            lpl.enabled = true;
            ExecuteInTicks(() =>
            {
                var landPlot = plotObj.GetComponent<LandPlot>();
                try { sceneContext.GameModel.RegisterLandPlot(lpl._id,obj); }catch { }
                //Yes, it's a different key on purpose
                landPlot.InitModel(sceneContext.GameModel.InitializeLandPlotModel(plotKey));
            },2);
        },2);
    }
}