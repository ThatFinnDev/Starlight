namespace Starlight.Prism.Data.Market;

public struct PrismMarketData
{
    public readonly float Saturation;
    public readonly float Value;
    public readonly bool HideInMarketUI;
    public readonly bool UseSaturationAsRangeForValue = false;

    public PrismMarketData(float saturation, float value)
    {
        this.Saturation = saturation;
        this.Value = value;
        this.HideInMarketUI = false;
    }
    public PrismMarketData(float saturation, float value, bool hideInMarketUI, bool useSaturationAsRangeForValue)
    {
        this.Saturation = saturation;
        this.Value = value;
        this.HideInMarketUI = hideInMarketUI;
        this.UseSaturationAsRangeForValue = useSaturationAsRangeForValue;
    }
}