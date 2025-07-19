using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusShield2 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.shield", ["2"])
        ];
    }
}