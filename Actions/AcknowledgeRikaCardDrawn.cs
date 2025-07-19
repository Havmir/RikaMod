using System;

namespace RikaMod.Actions;

public class AcknowledgeRikaCardDrawn : CardAction
{
    private int _calculation;
    public string CardName = "null";
    
    public override void Begin(G g, State s, Combat c)
    {
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);
        
        //Console.WriteLine($"[RikaMod] Card: {CardName} | Rika Card #: {_calculation} | card for turn {c.turn}.");
        Console.WriteLine("[RikaMod] You're using an scraped or card I forgot to properly update to work with Blitz.");
    }
}