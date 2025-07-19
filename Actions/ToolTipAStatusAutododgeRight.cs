using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusAutododgeRight : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.autododgeRight")
        ];
    }
}