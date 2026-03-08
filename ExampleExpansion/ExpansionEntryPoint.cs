using Starlight.Expansion;
using Starlight.Storage;

[assembly: StarlightExpansion(typeof(StarlightExampleExpansion.ExpansionEntryPoint))]

namespace StarlightExampleExpansion;

// This is required to load the expansion
//
// Alternatively, you can provide a list of types, if you have multiple expansions in one dll


public class ExpansionEntryPoint : StarlightExpansionV01
{
    public override StarlightExpansionInfo info => new StarlightExpansionInfo()
    {
        ID = "com.devname.modname",
        name = "Example Expansion",
        usePrism = true,
        version = "1.0.0",
        description = "Example description"
        //iconPath = "Assets/icon.png" //path is optionally overridable
    };

    public override void OnInitialize()
    {
        AddLanguages(EmbeddedResourceEUtil.LoadString("translations.csv"));
    }
}

