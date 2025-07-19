using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusDrawNextTurn2 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.drawNextTurn", ["2"])
        ];
    }
}