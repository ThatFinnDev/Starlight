using System;

namespace Starlight.Enums;

internal class DebugUIEntry
{
    public string Text = "<Missing Text>";
    public Sprite Icon = null;
    public bool ClosesMenu = true;
    public Action Action;
}