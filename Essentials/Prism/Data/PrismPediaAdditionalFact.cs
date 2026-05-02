using UnityEngine.Localization;

namespace Starlight.Prism.Data;

public record struct PrismPediaAdditionalFact
{
    public LocalizedString Title;
    public LocalizedString Description;
    public Sprite Icon;

    public static PrismPediaAdditionalFact Create(LocalizedString title, LocalizedString description, Sprite icon)
    {
        return new PrismPediaAdditionalFact { Title = title, Description = description, Icon = icon };
    }

    public static PrismPediaAdditionalFact[] From(params PrismPediaAdditionalFact[] array) => array;
}