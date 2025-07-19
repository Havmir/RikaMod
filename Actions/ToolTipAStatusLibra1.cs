using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusLibra1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.libra", ["1"])
        ];
    }
}