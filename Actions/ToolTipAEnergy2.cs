using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAEnergy2 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("action.gainEnergy", ["2"])
        ];
    }
}