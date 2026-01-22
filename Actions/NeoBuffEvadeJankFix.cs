using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using RikaMod.Cards.NeoCards;
using RikaMod.Features;

namespace RikaMod.Actions;

public class NeoBuffEvadeJankFix : CardAction
{

    public string Cardname = "null";
    public int timesPlayedthisCombat = 0;
    public int whatIsTheuuid = 0;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    private static bool _extraLogCheck = ArtManager.ExtraCheckLog;
    
    public override void Begin(G g, State s, Combat c)
    {
        if (_extraLogCheck && _logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: NeoBuffEvadeJankFix action played Check #1 | Card Name {Cardname} | # of times played this combat + 1 = {timesPlayedthisCombat + 1} | whatIsTheuuid = {whatIsTheuuid}");
        }
        foreach (var card in s.deck.Concat(c.hand).Concat(c.discard).Concat(c.exhausted))
        {
            if (_extraLogCheck && _logALotOfThings)
            {
                ModEntry.Instance.Logger.LogInformation($"[RikaMod: NeoBuffEvadeJankFix action played Check #2 | Card Name {Cardname} | # of times played this combat + 1 = {timesPlayedthisCombat + 1} | whatIsTheuuid = {whatIsTheuuid} | card.uuid {card.uuid}");
            }
            if (card is NeoEvadeBooster specificCard)
            {
                if (_extraLogCheck && _logALotOfThings)
                {
                    ModEntry.Instance.Logger.LogInformation($"[RikaMod: NeoBuffEvadeJankFix action played Check #3 | Card Name {Cardname} | # of times played this combat + 1 = {timesPlayedthisCombat + 1} | whatIsTheuuid = {whatIsTheuuid} | card.uuid {card.uuid}");
                }
                if (specificCard.uuid == whatIsTheuuid)
                {
                    if (_extraLogCheck && _logALotOfThings)
                    {
                        ModEntry.Instance.Logger.LogInformation($"[RikaMod: NeoBuffEvadeJankFix action played Check #4 | Card Name {Cardname} | # of times played this combat + 1 = {timesPlayedthisCombat + 1} | whatIsTheuuid = {whatIsTheuuid} | card.uuid {card.uuid}");
                    }
                    specificCard._timesplayedThisCombat += 1;
                }
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] NeoBuffEvadeJankFix action played | Card Name {Cardname} | # of times played this combat + 1 = {timesPlayedthisCombat + 1} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: NeoBuffEvadeJankFix action played | Card Name {Cardname} | # of times played this combat + 1 = {timesPlayedthisCombat + 1} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
    }
}