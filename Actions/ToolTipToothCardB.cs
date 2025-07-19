using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipToothCardB : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTCard
            {
                card = new BruiseCard{upgrade = Upgrade.B}
            },
            new TTCard
            {
                card = new Buckshot{upgrade = Upgrade.B}
            },
            new TTCard
            {
                card = new LightningBottle{upgrade = Upgrade.B}
            },
            new TTCard
            {
                card = new WaltzCard{upgrade = Upgrade.B}
            }
        ];
    }
}