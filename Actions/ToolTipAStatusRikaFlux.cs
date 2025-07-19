using System.Collections.Generic;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Actions;

public class ToolTipAStatusRikaFlux : CardAction
{
    public static Spr RikaFluxIcon;
    
    public override List<Tooltip> GetTooltips(State s)
    {
        object[] objArray = new object[2]
        {
            $"{s.ship.Get(RikaFluxManager.RikaFlux.Status)}",
            null!
        };
        
        var side = "ToolTipAStatusRikaFlux";
        return
        [
            new GlossaryTooltip($"ToolTipAStatusRikaFlux::{side}")
            {
                Icon = RikaFluxIcon,
                Title = ModEntry.Instance.Localizations.Localize(["status", "RikaFlux", "name"]),
                TitleColor = Colors.status,
                Description = string.Format(ModEntry.Instance.Localizations.Localize(["status", "RikaFlux", "desc"]), objArray)
            }
        ];
    }
}