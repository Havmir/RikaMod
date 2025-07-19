using System;
using Nickel;
using RikaMod.External;

namespace RikaMod.Features;

public class RikaFluxManager : IKokoroApi.IV2.IStatusLogicApi.IHook
{
    
    internal static IStatusEntry RikaFlux  = null! ;
    
    public RikaFluxManager()
    {
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this);
        ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnPlayerAttack), OnPlayerAttack);
    }

    public void OnPlayerAttack(Combat combat, State state)
    {
        if (state.ship.Get(RikaFlux.Status) > 0)
        {
            combat.QueueImmediate(new AStatus
            {
                status = Status.shield,
                statusAmount = state.ship.Get(RikaFlux.Status),
                targetPlayer = true,
                timer = 0
            });
        }
    }
    
    public bool HandleStatusTurnAutoStep(IKokoroApi.IV2.IStatusLogicApi.IHook.IHandleStatusTurnAutoStepArgs args)
    {
        if (args.Status != RikaFlux.Status)
            return false;
        if (args.Timing != IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnStart)
            return false;
        if (args.Amount == 0)
            return false;

        args.Amount = 0;
        // args.Amount = Math.Max(args.Amount - 1, 0);
            return false;
    }
}

