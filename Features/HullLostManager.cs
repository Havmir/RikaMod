
using System;
using Microsoft.Extensions.Logging;

namespace RikaMod.Features;

public class HullLostManager
{

    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    
    public void OnCombatStart(Combat combat)
    {
        ModEntry.Instance.Helper.ModData.SetModData(combat, "hullLostNumber", 0);
        if (_isplaytester)
        {
            Console.WriteLine("[RikaMod] Combat start!");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation("[RikaMod: HullLostManager.cs] Combat start!");
        }
    }
    
    public void OnTurnStart(Combat combat)
    {
        ModEntry.Instance.Helper.ModData.SetModData(combat, "hullLostNumber", 0);
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Turn {combat.turn} start!");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: HullLostManager.cs] Turn {combat.turn} start!");
        }
    }

    public void OnPlayerLoseHull(Combat combat, int amount)
    {
        if (combat.isPlayerTurn)
        {
            int hullLostNumber;
            hullLostNumber = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(combat, "hullLostNumber", 0);
            hullLostNumber += amount;
            ModEntry.Instance.Helper.ModData.SetModData(combat, "hullLostNumber", hullLostNumber);
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Hull Lost Manager detected {amount} of hull lost on turn {combat.turn}.");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: HullLostManager.cs] Hull Lost Manager detected {amount} of hull lost on turn {combat.turn}.");
        }
    }

    public HullLostManager()
    {
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(HullLostManager.OnCombatStart), OnCombatStart);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(HullLostManager.OnTurnStart), OnTurnStart);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(HullLostManager.OnPlayerLoseHull), OnPlayerLoseHull);
    }
}