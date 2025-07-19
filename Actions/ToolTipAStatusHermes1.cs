using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusHermes1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.hermes", ["1"])
        ];
    }
}