using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusLockdown : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.lockdown")
        ];
    }
}