namespace Starlight.Prism.Wrappers;

public class PrismFruitFood : PrismFood
{
    internal PrismFruitFood(IdentifiableType idType, bool isNative): base(idType, isNative)
    {
        this.IDType = idType;
        this.IsNative = isNative;
    }
}