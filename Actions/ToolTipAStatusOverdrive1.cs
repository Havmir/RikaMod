using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusOverdrive1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.overdrive", ["1"])
        ];
    }
}