using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipToothCardNone : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTCard
            {
                card = new BruiseCard()
            },
            new TTCard
            {
                card = new Buckshot()
            },
            new TTCard
            {
                card = new LightningBottle()
            },
            new TTCard
            {
                card = new WaltzCard()
            }
        ];
    }
}