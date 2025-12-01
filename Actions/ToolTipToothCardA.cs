using System;
using System.Collections.Generic;

namespace RikaMod.Actions;

public class ToolTipToothCardA : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new TTCard
            {
                card = new BruiseCard{upgrade = Upgrade.A}
            },
            new TTCard
            {
                card = new Buckshot{upgrade = Upgrade.A}
            },
            new TTCard
            {
                card = new LightningBottle{upgrade = Upgrade.A}
            },
            new TTCard
            {
                card = new WaltzCard{upgrade = Upgrade.A}
            }
        ];
    }
}