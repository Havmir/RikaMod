using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipADrawCard3 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("action.drawCard", ["3"])
        ];
    }
}