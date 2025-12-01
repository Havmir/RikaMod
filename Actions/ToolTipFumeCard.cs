using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipFumeCard : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTCard
            {
                card = new TrashFumes()
            },
        ];
    }
}