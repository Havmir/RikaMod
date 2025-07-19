using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipMoveRandom2 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("action.moveRandom", ["2"])
        ];
    }
}