using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipADiscard1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("action.discardCard", ["1"])
        ];
    }
}