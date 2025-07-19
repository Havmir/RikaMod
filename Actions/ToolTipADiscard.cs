using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipADiscard : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("action.discardHand")
        ];
    }
}