using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusTempshield1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.tempShield", ["1"])
        ];
    }
}