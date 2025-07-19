using System;

namespace RikaMod.Features;

public class RikaCardsPerTurnManager
{

    public void OnCombatStart(State state)
    {
        ModEntry.Instance.Helper.ModData.SetModData(state, "rikaCardsPerTurnNumber", 0);
        Console.WriteLine("[RIkaMod] RCPT Manager: OnCombatStart set rikaCardsPerTurnNumber = 0");
    }
    
    public void OnTurnStart(State state)
    {
        ModEntry.Instance.Helper.ModData.SetModData(state, "rikaCardsPerTurnNumber", 0);
        Console.WriteLine("[RIkaMod] RCPT Manager: OnTurnStart set rikaCardsPerTurnNumber = 0");
    }

    public RikaCardsPerTurnManager()
    {
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(HullLostManager.OnCombatStart), OnCombatStart);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(HullLostManager.OnTurnStart), OnTurnStart);
    }
}