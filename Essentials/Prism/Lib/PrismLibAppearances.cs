using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Starlight.Prism.Data;
using Starlight.Prism.Wrappers;

namespace Starlight.Prism.Lib;
/// <summary>
/// A library of helper functions for dealing with slime appearances
/// </summary>
public static class PrismLibAppearances
{
    private const string TopColor = "_TopColor";
    private const string MiddleColor = "_MiddleColor";
    private const string BottomColor = "_BottomColor";
    private const string TwinTopColor = "_TwinTopColor";
    private const string TwinMiddleColor = "_TwinMiddleColor";
    private const string TwinBottomColor = "_TwinBottomColor";
    private const string SpecColor = "_SpecColor";
    private const string TwinSpecColor = "_TwinSpecColor";
    private const string SloomberTopColor = "_SloomberTopColor";
    private const string SloomberMiddleColor = "_SloomberMiddleColor";
    private const string SloomberBottomColor = "_SloomberBottomColor";
    private const string NoiseEdge = "_NoiseEdge";
    private const string SloomberStarMask = "_SloomberStarMask";
    private const string SloomberColorOverlay = "_SloomberColorOverlay";
    private const string StarMaskIntensityBodycoloringSloomber = "_StarMaskIntensity_BODYCOLORING_SLOOMBER";
    private const string StarTiling = "_StarTiling";
    private const string TwinEffectSize = "_TwinEffectSize";
    private const string TwinLineDivisionIntensity = "_TwinLineDivisionIntensity";
    private const string EnableVortex = "_EnableVortex";
    private const string VortexDistortionSize = "_VortexDistortionSize";
    private const string TwinVortexOffset = "_TwinVortexOffset";

    /*
    /// <summary>
    /// Switches the appearances of two slimes
    /// </summary>
    /// <param name="slimeOneDef">The first slime to switch</param>
    /// <param name="slimeTwoDef">The second slime to switch</param>
    public static void SwitchSlimeAppearances(SlimeDefinition slimeOneDef, SlimeDefinition slimeTwoDef)
    {
        var appearanceOne = slimeOneDef.AppearancesDefault[0]._structures;
        slimeOneDef.AppearancesDefault[0]._structures = slimeTwoDef.AppearancesDefault[0]._structures;
        slimeTwoDef.AppearancesDefault[0]._structures = appearanceOne;
        var appearanceSplatOne = slimeOneDef.AppearancesDefault[0]._splatColor;
        slimeOneDef.AppearancesDefault[0]._splatColor = slimeTwoDef.AppearancesDefault[0]._splatColor;
        slimeTwoDef.AppearancesDefault[0]._splatColor = appearanceSplatOne;

        var colorPalate = slimeOneDef.AppearancesDefault[0]._colorPalette;
        slimeOneDef.AppearancesDefault[0]._colorPalette = slimeTwoDef.AppearancesDefault[0]._colorPalette;
        slimeTwoDef.AppearancesDefault[0]._colorPalette = colorPalate;

        var structureIcon = slimeOneDef.AppearancesDefault[0]._icon;
        slimeOneDef.AppearancesDefault[0]._icon = slimeTwoDef.AppearancesDefault[0]._icon;
        slimeTwoDef.AppearancesDefault[0]._icon = structureIcon;
        var icon = slimeOneDef.icon;
        slimeOneDef.icon = slimeTwoDef.icon;
        slimeTwoDef.icon = icon;

        var debugIcon = slimeOneDef.debugIcon;
        slimeOneDef.debugIcon = slimeTwoDef.debugIcon;
        slimeTwoDef.debugIcon = debugIcon;

    }*/
    /// <summary>
    /// Sets the color of a slime appearance structure
    /// </summary>
    /// <param name="structure">The structure to set the color of</param>
    /// <param name="id">The id of the color to set</param>
    /// <param name="color">The color to set</param>
    public static void SetStructColor(SlimeAppearanceStructure structure, int id, Color color)
    {
        structure.DefaultMaterials[0]?.SetColor(id, color);
    }

    /// <summary>
    /// Adds a structure to a slime appearance
    /// </summary>
    /// <param name="appearance">The appearance to add the structure to</param>
    /// <param name="structure">The structure to add</param>
    public static void AddStructure(SlimeAppearance appearance, SlimeAppearanceStructure structure)
    {
        appearance.Structures=appearance.Structures.AddToNew(structure);
    }
    /// <summary>
    /// Adds a structure to a slime appearance
    /// </summary>
    /// <param name="app">The appearance to add the structure to</param>
    /// <param name="mesh">The mesh of the structure</param>
    /// <param name="rootBone">The root bone of the structure</param>
    /// <param name="parentBone">The parent bone of the structure</param>
    /// <param name="elementName">The name of the element</param>
    /// <returns>The new structure</returns>
    public static SlimeAppearanceStructure AddStructure(SlimeAppearance app, Mesh mesh, SlimeAppearance.SlimeBone rootBone, SlimeAppearance.SlimeBone parentBone, string elementName)
    {
        var structPrefab = app._structures[0]?.Element.Prefabs[0]?.gameObject.CopyObject();
        if (structPrefab == null) return null;
        structPrefab.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
        
        var structObj = structPrefab.GetComponent<SlimeAppearanceObject>();
        structObj.IgnoreLODIndex = true;
        structObj.RootBone = rootBone;
        structObj.ParentBone = parentBone;
        structObj.AttachedBones = new Il2CppStructArray<SlimeAppearance.SlimeBone>(0);
        
        var structure = new SlimeAppearanceStructure(app._structures[0]);
        structure.Element = ScriptableObject.CreateInstance<SlimeAppearanceElement>();
        structure.Element.CastsShadows = true;
        structure.Element.Name = elementName;
        structure.Element.Prefabs = new Il2CppReferenceArray<SlimeAppearanceObject>(new[]
        {
            structObj
        });
        
        app._structures = app._structures.AddToNew(structure);
        return structure;
    }
    /// <summary>
    /// Sets the base colors of a plort
    /// </summary>
    /// <param name="prismPlort">The plort to set the colors of</param>
    /// <param name="top">The top color</param>
    /// <param name="middle">The middle color</param>
    /// <param name="bottom">The bottom color</param>
    public static void SetPlortBaseColors(this PrismPlort prismPlort, Color32 top, Color32 middle, Color32 bottom)
    {
        var material = prismPlort.GetPrefab().GetComponent<MeshRenderer>().material;
        material.SetColor(TopColor, top);
        material.SetColor(MiddleColor, middle);
        material.SetColor(BottomColor, bottom);
        
    }

    /// <summary>
    /// Sets the twin colors of a plort
    /// </summary>
    /// <param name="prismPlort">The plort to set the colors of</param>
    /// <param name="top">The top color</param>
    /// <param name="middle">The middle color</param>
    /// <param name="bottom">The bottom color</param>
    public static void SetPlortTwinColors(this PrismPlort prismPlort, Color32 top, Color32 middle, Color32 bottom)
    {
        var material = prismPlort.GetPrefab().GetComponent<MeshRenderer>().material;
        material.SetColor(TwinTopColor, top);
        material.SetColor(TwinMiddleColor, middle);
        material.SetColor(TwinBottomColor, bottom);
    }
    /// <summary>
    /// Sets the base colors of a slime
    /// </summary>
    /// <param name="prismSlime">The slime to set the colors of</param>
    /// <param name="top">The top color</param>
    /// <param name="middle">The middle color</param>
    /// <param name="bottom">The bottom color</param>
    /// <param name="special">The special color</param>
    /// <param name="index">The index of the appearance</param>
    /// <param name="index2">The index of the material</param>
    /// <param name="isSs">Whether the appearance is a secret style</param>
    /// <param name="structure">The index of the structure</param>
    public static void SetSlimeBaseColorsSpecific(this PrismSlime prismSlime, Color32 top, Color32 middle, Color32 bottom, Color32 special, int index, int index2, bool isSs, int structure)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        var mat = isSs ? slimeDef.AppearancesDynamic.ToArray()[index].Structures[structure]?.DefaultMaterials[index2] : slimeDef.AppearancesDefault[index]?.Structures[structure]?.DefaultMaterials[index2];

        mat?.SetColor(TopColor, top);
        mat?.SetColor(MiddleColor, middle);
        mat?.SetColor(BottomColor, bottom);
        mat?.SetColor(SpecColor, special);
    }

    /// <summary>
    /// Sets the base colors of a slime
    /// </summary>
    /// <param name="prismSlime">The slime to set the colors of</param>
    /// <param name="top">The top color</param>
    /// <param name="middle">The middle color</param>
    /// <param name="bottom">The bottom color</param>
    public static void SetSlimeBaseColors(this PrismSlime prismSlime, Color32 top, Color32 middle, Color32 bottom)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0].Structures[i];
            var mat = a?.DefaultMaterials[0];
            mat?.SetColor(TopColor, top);
            mat?.SetColor(MiddleColor, middle);
            mat?.SetColor(BottomColor, bottom);
            mat?.SetColor(SpecColor, middle);
        }
    }

    /// <summary>
    /// Sets the twin colors of a slime
    /// </summary>
    /// <param name="prismSlime">The slime to set the colors of</param>
    /// <param name="top">The top color</param>
    /// <param name="middle">The middle color</param>
    /// <param name="bottom">The bottom color</param>
    public static void SetSlimeTwinColors(this PrismSlime prismSlime, Color32 top, Color32 middle, Color32 bottom)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0]?.Structures[i];
            var mat = a?.DefaultMaterials[0];
            mat?.SetColor(TwinTopColor, top);
            mat?.SetColor(TwinMiddleColor, middle);
            mat?.SetColor(TwinBottomColor, bottom);
            mat?.SetColor(TwinSpecColor, middle);
        }
    }
    /// <summary>
    /// Sets the sloomber colors of a slime
    /// </summary>
    /// <param name="prismSlime">The slime to set the colors of</param>
    /// <param name="top">The top color</param>
    /// <param name="middle">The middle color</param>
    /// <param name="bottom">The bottom color</param>
    public static void SetSlimeSloomberColors(this PrismSlime prismSlime, Color32 top, Color32 middle, Color32 bottom)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0]?.Structures[i];
            var mat = a?.DefaultMaterials[0];

            mat?.SetColor(SloomberTopColor, top);
            mat?.SetColor(SloomberMiddleColor, middle);
            mat?.SetColor(SloomberBottomColor, bottom);
        }
    }
    
    /// <summary>
    /// Sets the twin colors of a slime
    /// </summary>
    /// <param name="prismSlime">The slime to set the colors of</param>
    /// <param name="top">The top color</param>
    /// <param name="middle">The middle color</param>
    /// <param name="bottom">The bottom color</param>
    /// <param name="index">The index of the appearance</param>
    /// <param name="index2">The index of the material</param>
    /// <param name="isSs">Whether the appearance is a secret style</param>
    /// <param name="structure">The index of the structure</param>
    public static void SetSlimeTwinColorsSpecific(this PrismSlime prismSlime, Color32 top, Color32 middle, Color32 bottom, int index, int index2, bool isSs, int structure)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        var mat = isSs ? slimeDef.AppearancesDynamic.ToArray()[index].Structures[structure]?.DefaultMaterials[index2] : slimeDef.AppearancesDefault[index]?.Structures[structure]?.DefaultMaterials[index2];

        mat?.SetColor(TwinTopColor, top);
        mat?.SetColor(TwinMiddleColor, middle);
        mat?.SetColor(TwinBottomColor, bottom);
    }

    /// <summary>
    /// Sets the sloomber colors of a slime
    /// </summary>
    /// <param name="prismSlime">The slime to set the colors of</param>
    /// <param name="top">The top color</param>
    /// <param name="middle">The middle color</param>
    /// <param name="bottom">The bottom color</param>
    /// <param name="index">The index of the appearance</param>
    /// <param name="index2">The index of the material</param>
    /// <param name="isSs">Whether the appearance is a secret style</param>
    /// <param name="structure">The index of the structure</param>
    public static void SetSlimeSloomberColorsSpecific(this PrismSlime prismSlime, Color32 top, Color32 middle, Color32 bottom, int index, int index2, bool isSs, int structure)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        var mat = isSs ? slimeDef.AppearancesDynamic.ToArray()[index].Structures[structure]?.DefaultMaterials[index2] : slimeDef.AppearancesDefault[index]?.Structures[structure]?.DefaultMaterials[index2];

        mat?.SetColor(SloomberTopColor, top);
        mat?.SetColor(SloomberMiddleColor, middle);
        mat?.SetColor(SloomberBottomColor, bottom);
    }
    
    /// <summary>
    /// Enables the twin effect on a slime
    /// </summary>
    /// <param name="prismSlime">The slime to enable the effect on</param>
    /// <param name="index">The index of the appearance</param>
    /// <param name="index2">The index of the material</param>
    /// <param name="isSs">Whether the appearance is a secret style</param>
    /// <param name="structure">The index of the structure</param>
    /// <param name="applyTextures">Whether or not to automatically apply textures</param>
    public static void EnableTwinEffectSpecific(this PrismSlime prismSlime, int index, int index2, bool isSs, int structure, bool applyTextures = true)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        var twinMat = PrismNativeBaseSlime.Twin.GetPrismBaseSlime().GetSlimeAppearance()._structures[0]?.DefaultMaterials[0];
        var mat = isSs ? slimeDef.AppearancesDynamic.ToArray()[index].Structures[structure]?.DefaultMaterials[index2] : slimeDef.AppearancesDefault[index]?.Structures[structure]?.DefaultMaterials[index2];

        mat?.EnableKeyword("_ENABLETWINEFFECT_ON");
        
        if (applyTextures)
            mat?.SetTexture(NoiseEdge, twinMat?.GetTexture(NoiseEdge));
    }

    // todo: automatically set the effect textures from the actual twin slime
    /// <summary>
    /// Enables the twin effect on a slime
    /// </summary>
    /// <param name="prismSlime">The slime to enable the effect on</param>
    public static void EnableTwinEffect(this PrismSlime prismSlime)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        var twinMat = PrismNativeBaseSlime.Twin.GetPrismBaseSlime().GetSlimeAppearance()._structures[0]?.DefaultMaterials[0];
        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0]?.Structures[i];
            var mat = a?.DefaultMaterials[0];
            
            mat?.EnableKeyword("_ENABLETWINEFFECT_ON");
            mat?.SetTexture(NoiseEdge, twinMat?.GetTexture(NoiseEdge));
        }
        
        prismSlime.AdjustTwinEffect();
    }
    /// <summary>
    /// Enables the twin effect on a slime without automatically setting the textures
    /// </summary>
    /// <param name="prismSlime">The slime to enable the effect on</param>
    public static void EnableTwinEffectTextureless(this PrismSlime prismSlime)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();

        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0]?.Structures[i];
            var mat = a?.DefaultMaterials[0];
            
            mat?.EnableKeyword("_ENABLETWINEFFECT_ON");
        }
    }

    /// <summary>
    /// Enables the sloomber effect on a slime
    /// </summary>
    /// <param name="prismSlime">The slime to enable the effect on</param>
    public static void EnableSloomberEffect(this PrismSlime prismSlime)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        var sloomberMat = PrismNativeBaseSlime.Sloomber.GetPrismBaseSlime().GetSlimeAppearance()._structures[0]?.DefaultMaterials[0];
        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0]?.Structures[i];
            var mat = a?.DefaultMaterials[0];
            
            mat?.EnableKeyword("_BODYCOLORING_SLOOMBER");
            
            mat?.SetTexture(SloomberStarMask, sloomberMat?.GetTexture(SloomberStarMask));
            mat?.SetTexture(SloomberColorOverlay, sloomberMat?.GetTexture(SloomberColorOverlay));
        }
        
        prismSlime.AdjustSloomberSparkles();
    }
    
    /// <summary>
    /// Adjusts the settings for the sloomber sparkle effect
    /// <br/><br/>
    /// The default values for the parameters are the default for the sloomber slime.
    /// </summary>
    /// <param name="prismSlime">The slime to enable the effect on</param>
    /// <param name="size">The size for tiling.</param>
    /// <param name="intensity">The strength of the texture.</param>
    public static void AdjustSloomberSparkles(this PrismSlime prismSlime, float size = 3.26f, float intensity = 1)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();

        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0]?.Structures[i];
            var mat = a?.DefaultMaterials[0];
            
            mat?.SetFloat(StarTiling, size);
            mat?.SetFloat(StarMaskIntensityBodycoloringSloomber, intensity);
        }
    }
    
    /// <summary>
    /// Adjusts the settings for the sloomber sparkle effect
    /// <br/><br/>
    /// The default values for the parameters are the default for the sloomber slime.
    /// </summary>
    /// <param name="prismSlime">The slime to enable the effect on</param>
    /// <param name="size">The size for the effect</param>
    /// <param name="intensity">The strength of the effect.</param>
    /// <param name="vortex">Whether or not to enable the vortex</param>
    /// <param name="vortexDistortion">The vortex distortion size</param>
    /// <param name="vortexOffset">The vortex offset (use null for default)</param>
    public static void AdjustTwinEffect(this PrismSlime prismSlime, float size = 1f, float intensity = 0.8f, bool vortex = true, float vortexDistortion = 12f, Vector4? vortexOffset = null)
    {
        Vector4 vortexOffset2;
        if (vortexOffset == null) vortexOffset2 = new Vector4(0, -0.5f, 0, 0);
        else vortexOffset2 = (Vector4)vortexOffset;
        
        var slimeDef = prismSlime.GetSlimeDefinition();

        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0]?.Structures[i];
            var mat = a?.DefaultMaterials[0];
            
            mat?.SetFloat(TwinEffectSize, size);
            mat?.SetFloat(TwinLineDivisionIntensity, intensity);
            mat?.SetFloat(EnableVortex, vortex ? 1f : 0f);
            mat?.SetFloat(VortexDistortionSize, vortexDistortion);
            mat?.SetVector(TwinVortexOffset, vortexOffset2);
        }
    }
    /// <summary>
    /// Enables the sloomber effect on a slime but without automatically setting the texture
    /// </summary>
    /// <param name="prismSlime">The slime to enable the effect on</param>
    public static void EnableSloomberEffectTextureless(this PrismSlime prismSlime)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        for (int i = 0; i < slimeDef.AppearancesDefault[0]?.Structures.Count - 1; i++)
        {
            var a = slimeDef.AppearancesDefault[0].Structures[i];
            var mat = a?.DefaultMaterials[0];
            
            mat?.EnableKeyword("_BODYCOLORING_SLOOMBER");
        }
    }

    /// <summary>
    /// Disables the twin effect on a slime
    /// </summary>
    /// <param name="prismSlime">The slime to disable the effect on</param>
    /// <param name="index">The index of the appearance</param>
    /// <param name="index2">The index of the material</param>
    /// <param name="isSs">Whether the appearance is a secret style</param>
    /// <param name="structure">The index of the structure</param>
    public static void DisableTwinEffect(this PrismSlime prismSlime, int index, int index2, bool isSs, int structure)
    {
        var slimeDef = prismSlime.GetSlimeDefinition();
        Material mat;
        mat = isSs ? slimeDef.AppearancesDynamic.ToArray()[index].Structures[structure]?.DefaultMaterials[index2] : slimeDef.AppearancesDefault[index]?.Structures[structure]?.DefaultMaterials[index2];

        mat?.DisableKeyword("_ENABLETWINEFFECT_ON");
    }

    /*
    
       public static void SetPalette(this SlimeAppearance app, Material slimeMaterial, SlimeDefinition definition)
       {
           app._colorPalette = new SlimeAppearance.Palette
           {
               Ammo = definition.color,
               Bottom = slimeMaterial.GetColor("_BottomColor"),
               Middle = slimeMaterial.GetColor("_MiddleColor"),
               Top = slimeMaterial.GetColor("_TopColor"),
           };
       }
     public static void SetSlimeMatTopColor(this Material mat, Color color) => mat.SetColor("_TopColor", color);
    public static void SetSlimeMatMiddleColor(this Material mat, Color color) => mat.SetColor("_MiddleColor", color);

    public static void SetSlimeMatBottomColor(this Material mat, Color color) => mat.SetColor("_BottomColor", color);

    public static void SetSlimeMatColors(this Material material, Color32 Top, Color32 Middle, Color32 Bottom,
        Color32 Specular)
    {
        material.SetColor("_TopColor", Top);
        material.SetColor("_MiddleColor", Middle);
        material.SetColor("_BottomColor", Bottom);
        material.SetColor("_SpecColor", Specular);
    }

    public static void SetSlimeMatColors(this Material material, Color32 Top, Color32 Middle, Color32 Bottom)
    {
        material.SetColor("_TopColor", Top);
        material.SetColor("_MiddleColor", Middle);
        material.SetColor("_BottomColor", Bottom);
    }*/

}