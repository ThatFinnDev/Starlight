namespace Starlight.Prism.Wrappers;

public class PrismChickenFood : PrismFood
{
    internal PrismChickenFood(IdentifiableType idType, bool isNative): base(idType, isNative)
    {
        this.IDType = idType;
        this.IsNative = isNative;
    }
}