using System.Collections.Generic;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Actions;

public class ToolTipAStatusRikaBackUpShield : CardAction
{
    public static Spr RikaNackUpShieldIcon;
    
    public override List<Tooltip> GetTooltips(State s)
    {
        object[] objArray = new object[2]
        {
            $"{2 * s.ship.Get(RikaBackUpShieldManager.rikaBackUpShield.Status)}",
            null!
        };
        
        var side = "rftIDontKnowHowToRemoveThis";
        return
        [
            new GlossaryTooltip($"rftIDontKnowHowToRemoveThis::{side}")
            {
                Icon = RikaNackUpShieldIcon,
                Title = ModEntry.Instance.Localizations.Localize(["status", "BackUpShield", "name"]),
                TitleColor = Colors.status,
                Description = string.Format(ModEntry.Instance.Localizations.Localize(["status", "BackUpShield", "desc"]), objArray)
            }
        ];
    }
}