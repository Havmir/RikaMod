using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipADrawCard2 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("action.drawCard", ["2"])
        ];
    }
}