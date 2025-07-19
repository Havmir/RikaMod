using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusTempPayback1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.tempPayback", ["1"])
        ];
    }
}