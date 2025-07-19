using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusEnergyNextTurn1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.energyNextTurn", ["1"])
        ];
    }
}