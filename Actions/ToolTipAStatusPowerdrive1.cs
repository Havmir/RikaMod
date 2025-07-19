using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusPowerdrive1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.powerdrive", ["1"])
        ];
    }
}