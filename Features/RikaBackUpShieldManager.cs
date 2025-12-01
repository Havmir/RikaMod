using System;
using Nickel;
using RikaMod.External;

namespace RikaMod.Features;

public class RikaBackUpShieldManager : IKokoroApi.IV2.IStatusLogicApi.IHook
{
    
    internal static IStatusEntry rikaBackUpShield  = null! ;
    
    public RikaBackUpShieldManager()
    {
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this);
        ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnPlayerLoseHull), OnPlayerLoseHull);
    }

    public void OnPlayerLoseHull(Combat combat, State state)
    {
        if (state.ship.Get(rikaBackUpShield.Status) > 0)
        {
            combat.QueueImmediate(new AStatus
            {
                status = Status.tempShield,
                statusAmount = 2 * state.ship.Get(rikaBackUpShield.Status),
                targetPlayer = true,
                timer = 0
            });
            combat.QueueImmediate(new AStatus
            {
                status = rikaBackUpShield.Status,
                statusAmount = 0,
                targetPlayer = true,
                mode = AStatusMode.Set,
                timer = 0
            });
        }
    }
}

