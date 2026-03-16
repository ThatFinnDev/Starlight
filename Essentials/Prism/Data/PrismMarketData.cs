namespace Starlight.Prism.Data;

public struct PrismMarketData
{
    public readonly float Saturation;
    public readonly float Value;
    public readonly bool HideInMarketUI;

    public PrismMarketData(float saturation, float value)
    {
        this.Saturation = saturation;
        this.Value = value;
        this.HideInMarketUI = false;
    }
    public PrismMarketData(float saturation, float value, bool hideInMarketUI)
    {
        this.Saturation = saturation;
        this.Value = value;
        this.HideInMarketUI = hideInMarketUI;
    }
}