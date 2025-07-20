
namespace RikaMod.Features;

public class HullLostManager
{

    public void OnCombatStart(Combat combat)
    {
        ModEntry.Instance.Helper.ModData.SetModData(combat, "hullLostNumber", 0);
    }
    
    public void OnTurnStart(Combat combat)
    {
        ModEntry.Instance.Helper.ModData.SetModData(combat, "hullLostNumber", 0);
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
    }

    public HullLostManager()
    {
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(HullLostManager.OnCombatStart), OnCombatStart);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(HullLostManager.OnTurnStart), OnTurnStart);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(HullLostManager.OnPlayerLoseHull), OnPlayerLoseHull);
    }
}