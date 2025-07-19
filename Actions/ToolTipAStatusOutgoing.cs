using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusOutgoing : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("actionMisc.outgoing")
        ];
    }
}