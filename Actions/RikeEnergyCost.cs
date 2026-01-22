using System;
using System.Collections.Generic;
using Nickel;
using RikaMod.Cards;
using RikaMod.Features;

namespace RikaMod.Actions;



public class RikaEnergyCost : CardAction
{
    public static Spr EnergyBallSprite;

    public int rikaEnergyCheck;
    public int cardCost;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private bool _consoleWriteLineOn = _isplaytester;
    
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        rikaEnergyCheck = s.ship.Get(RikaEnergyManager.RikaEnergy.Status);
        
        if (rikaEnergyCheck >= cardCost)
        {
            c.QueueImmediate(new AStatus
            {
                status = RikaEnergyManager.RikaEnergy.Status,
                statusAmount = -1 * cardCost,
                targetPlayer = true
            });
        }
        else if (rikaEnergyCheck == 0)
        {
            c.QueueImmediate(new AEnergy
            {
                changeAmount = -1 * cardCost
            });
        }
        else if (rikaEnergyCheck < cardCost && rikaEnergyCheck > 0)
        {
            c.QueueImmediate(new AEnergy
            {
                changeAmount = rikaEnergyCheck - cardCost
            });
            c.QueueImmediate(new AStatus
            {
                status = RikaEnergyManager.RikaEnergy.Status,
                statusAmount = rikaEnergyCheck - cardCost,
                targetPlayer = true
            });
        }
        else if (cardCost == 0)
        {
            /*if (_consoleWriteLineOn)
            {
                Console.WriteLine("[RikaMod] Havmir likely did something silly with a 0 cost card for you to see this. Please check to see if there is any updates and report this error. Error Code: RikaEnergyCost.cs");
            }*/
        }
        else
        {
            Console.WriteLine($"[RikaMod] This is a catch all check in case something went wrong with RikaEnergyCost.cs, so please let Havmir know if you saw this with the following information | Card Cost:{cardCost} | rikaEnergyCheck: {rikaEnergyCheck}.");
        }
        
        return default;
    }


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