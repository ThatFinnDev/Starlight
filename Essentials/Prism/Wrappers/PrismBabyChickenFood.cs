namespace Starlight.Prism.Wrappers;

public class PrismBabyChickenFood : PrismFood
{
    internal PrismBabyChickenFood(IdentifiableType idType, bool isNative): base(idType, isNative)
    {
        this.IDType = idType;
        this.IsNative = isNative;
    }
}