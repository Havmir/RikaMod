using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusDroneshift : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.droneShift")
        ];
    }
}