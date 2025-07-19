using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusEvade : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.evade")
        ];
    }
}