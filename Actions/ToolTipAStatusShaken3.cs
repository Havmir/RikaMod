using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusDrawLessNextTurn3 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.drawLessNextTurn", ["3"])
        ];
    }
}