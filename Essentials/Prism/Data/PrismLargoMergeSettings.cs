namespace Starlight.Prism.Data;

public class PrismLargoMergeSettings
{
    public readonly bool MergeComponents = true;
    public readonly PrismBfMergeStrategy Body;
    public readonly PrismBfMergeStrategy Face;
    public readonly PrismColorMergeStrategy BaseColors;
    public readonly PrismColorMergeStrategy TwinColors;
    public readonly PrismColorMergeStrategy SloomberColors;

    public PrismLargoMergeSettings()
    {
        Body = PrismBfMergeStrategy.Optimal;
        Face = PrismBfMergeStrategy.Optimal;
        BaseColors = PrismColorMergeStrategy.Optimal;
        TwinColors = PrismColorMergeStrategy.Optimal;
        SloomberColors = PrismColorMergeStrategy.Optimal;
    }

    public PrismLargoMergeSettings(bool mergeComponents,PrismBfMergeStrategy body, PrismBfMergeStrategy face, PrismColorMergeStrategy baseColors, PrismColorMergeStrategy twinColors, PrismColorMergeStrategy sloomberColors)
    {
        this.MergeComponents=mergeComponents;
        this.Body = body;
        this.Face = face;
        this.BaseColors = baseColors;
        this.TwinColors = twinColors;
        this.SloomberColors = sloomberColors;
    }
}   