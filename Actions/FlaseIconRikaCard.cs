using System;
using System.Collections.Generic;
using Nickel;
using RikaMod.Cards;
using RikaMod.Features;

namespace RikaMod.Actions;



public class FalseIconRikaCard : CardAction
{
    public static Spr EnergyBallSprite;
    
    public int cardCost;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private bool _consoleWriteLineOn = _isplaytester;
    


    public override Icon? GetIcon(State s)
    {
        return new Icon
        {
            path = EnergyBallSprite,
            number = null,
        };
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        if (cardCost > 0)
        {
            var side = "RikaEnergyCost";
            return
            [
                new GlossaryTooltip($"RikaEnergyCost::{side}")
                {
                    Icon = EnergyBallSprite,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "RikaEnergyCost", "title"]),
                    TitleColor = Colors.card,
                    Description = string.Format(ModEntry.Instance.Localizations.Localize(["action", "RikaEnergyCost", "descNotFree"]), $"{cardCost}")
                }
            ];
        }
        if (cardCost == 0)
        {
            var side = "RikaEnergyCost";
            return
            [
                new GlossaryTooltip($"RikaEnergyCost::{side}")
                {
                    Icon = EnergyBallSprite,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "RikaEnergyCost", "title"]),
                    TitleColor = Colors.card,
                    Description = string.Format(ModEntry.Instance.Localizations.Localize(["action", "RikaEnergyCost", "descFree"]))
                }
            ];
        }
        return default!;
    }

}