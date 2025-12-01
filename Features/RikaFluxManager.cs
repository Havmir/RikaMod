using System;
using Microsoft.Extensions.Logging;
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
        //ModEntry.Instance.Logger.LogInformation($"[RikaMod: RikaFluxManager | Constructor] RikaFlux.Status: {RikaFlux.Status}"); ~ I'm saving this in case I need to figure out how to debug something ~ Havmir 15/08/2025
    }

    public void OnPlayerAttack(Combat combat, State state)
    {
        //ModEntry.Instance.Logger.LogInformation($"[RikaMod: RikaFluxManager | OnPlyerAttack] RikaFlux.Status: {RikaFlux.Status} | state.ship.Get(RikaFlux.Status): {state.ship.Get(RikaFlux.Status)}"); ~ I'm saving this in case I need to figure out how to debug something ~ Havmir 15/08/2025
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

