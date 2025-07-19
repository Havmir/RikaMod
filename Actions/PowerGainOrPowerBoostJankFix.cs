using System;

namespace RikaMod.Actions;

public class PowerGainOrPowerBoostJankFix : CardAction
{

    public string Cardname = "null";
    public int Statusamount = 1;
    
    public override void Begin(G g, State s, Combat c)
    {
        switch (Cardname)
        {
            case "PowerBoost":
            {
                timer = 0.0;
                foreach (CardAction cardAction in c.cardActions)
                {
                    if (cardAction is AAttack aattack && !aattack.targetPlayer && !aattack.fromDroneX.HasValue)
                        aattack.damage += Statusamount + s.ship.Get(Status.boost);
                }

                break;
            }
            case "PowerGain":
            {
                timer = 0.0;
                foreach (CardAction cardAction in c.cardActions)
                {
                    if (cardAction is AAttack aattack && !aattack.targetPlayer && !aattack.fromDroneX.HasValue)
                        aattack.damage += Statusamount + s.ship.Get(Status.boost);
                }

                break;
            }
            case "null":
                Console.WriteLine($"[RikaMod] Error, I probally forgot to assign a value to the Cardname field when using the PowerGainOrPowerBoostJankFix action (yes, that is how some of my code is named). If you could let me know about this error, so I could fix it, that would be great ~ Havmir");
                break;
            default:
                Console.WriteLine($"[RikaMod] Error, unexpected action resulted in attack damage calculations to likely be mishandled. Cardname field: {Cardname} | Statusamount field: {Statusamount} ~ Havmir");
                break;
        }
    }
}