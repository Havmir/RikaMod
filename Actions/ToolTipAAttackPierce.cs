using System.Collections.Generic;
using Nickel;

namespace RikaMod.Actions;

public class ToolTipAAttackPierce : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTGlossary("action.attackPiercing")
        ];
    }
}