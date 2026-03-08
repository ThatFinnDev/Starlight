using Starlight.Expansion;
using Starlight.Storage;

namespace StarlightExampleExpansion;

// This is required to load the expansion
//
// Alternatively, you can provide a list of types, if you have multiple expansions in one dll

[assembly: StarlightExpansion(typeof(ExpansionEntryPoint))]

public class ExpansionEntryPoint : StarlightExpansionV01
{
    public override StarlightExpansionInfo info => new StarlightExpansionInfo()
    {
        ID = "com.devname.modname",
        name = "Example Expansion",
        usePrism = true,
    };

    public override void OnInitialize()
    {
        AddLanguages(EmbeddedResourceEUtil.LoadString("translations.csv"));
    }
}

