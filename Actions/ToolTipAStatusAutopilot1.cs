using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipAStatusAutopilot1 : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("status.autopilot", ["1"])
        ];
    }
}