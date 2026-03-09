using Starlight.Expansion;
using Starlight.Storage;


namespace StarlightExampleExpansion;

// This is required to load the expansion
// One mod.dll can have multiple Starlight expansions
[StarlightLoadExpansion()]
public class ExpansionEntryPoint : StarlightExpansionV01
{
    protected override StarlightPackageInfo info => new ()
    {
        ID = "com.devname.modname",
        name = "Example Expansion",
        author = "YourName",
        description = "Example description",
        version = "1.0.0",
        /// Optionally use these arguments
        
        //coAuthors = new []{"coauthor1","coauthor2"},
        //contributors = new []{"contributor1","contributor2"},
        //sourceCode = "https://gitsite.com/SomeUserName/SomeRepo",
        //nexus = "https://www.nexusmods.com/slimerancher2/mods/0",
        //discord = "https://discord.gg/someserver",
        //usePrism = true,
        //iconPath = "Assets/otherpath.png" // By default, the path is "Assets/icon.png"
    };

    public override void OnInitialize()
    {
        AddLanguages(EmbeddedResourceEUtil.LoadString("translations.csv"));
    }
}

