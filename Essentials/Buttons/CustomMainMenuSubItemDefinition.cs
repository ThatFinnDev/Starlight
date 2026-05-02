using Il2CppMonomiPark.SlimeRancher.UI.MainMenu.Definition;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu.Definition.ButtonBehavior;
using Starlight.Storage;

namespace Starlight.Buttons;

[InjectIntoIL]
internal class CustomMainMenuSubItemDefinition : SubMenuItemDefinition
{
    internal SystemAction customAction;
}
