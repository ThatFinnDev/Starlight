using Il2CppMonomiPark.SlimeRancher.UI.Pause;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starlight.Storage;

namespace Starlight.Buttons;

[InjectClass]
internal class CustomPauseItemModel : ResumePauseItemModel
{
    internal SystemAction action;
    public override void InvokeBehavior()
    {
        action.Invoke();
        return;
    }
}
