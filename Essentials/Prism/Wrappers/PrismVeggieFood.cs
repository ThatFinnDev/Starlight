namespace Starlight.Prism.Wrappers;

public class PrismVeggieFood : PrismFood
{
    internal PrismVeggieFood(IdentifiableType idType, bool isNative): base(idType, isNative)
    {
        this.IDType = idType;
        this.IsNative = isNative;
    }
}