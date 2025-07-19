using System.Collections.Generic;
using Nickel;

namespace RikaMod.Actions;

public class ToolTipAAttack : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("action.attack.name")
        ];
    }
}