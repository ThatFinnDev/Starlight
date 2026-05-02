using System;

namespace Starlight.Enums;

internal class DebugUIEntry
{
    public string text = "<Missing Text>";
    public Sprite icon = null;
    public bool closesMenu = true;
    public Action action;
}