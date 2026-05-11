namespace Starlight.Prism.Wrappers;

public class PrismNectarFood : PrismFood
{
    internal PrismNectarFood(IdentifiableType idType, bool isNative): base(idType, isNative)
    {
        this.IDType = idType;
        this.IsNative = isNative;
    }
}