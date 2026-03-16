using UnityEngine.Localization;

namespace Starlight.Prism.Data;

public struct PrismPediaDetail
{
    public LocalizedString Text;
    public PrismPediaDetailType Type;

    public static PrismPediaDetail Create(PrismPediaDetailType type, LocalizedString text)
    {
        return new PrismPediaDetail { Text = text, Type = type };
    }

    public static PrismPediaDetail[] From(params PrismPediaDetail[] array) => array;
}