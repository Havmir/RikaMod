using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusCorrode1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.corrode", ["1"])
        ];
    }
}