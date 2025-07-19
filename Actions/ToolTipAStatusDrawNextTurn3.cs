using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusDrawNextTurn3 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.drawNextTurn", ["3"])
        ];
    }
}