using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusShield1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.shield", ["1"])
        ];
    }
}