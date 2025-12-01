using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using Nickel;
using RikaMod.External;

namespace RikaMod.Features;

// Thanks to rft50, JyGein & Shockah for helping me learn how to use Harmony to get these behaviors coded in

public class RikaFlightDrawAndKiteingManager : IKokoroApi.IV2.IStatusLogicApi.IHook
{
    
    internal static IStatusEntry RikaFlightDraw  = null! ;
    internal static IStatusEntry RikaKiteing  = null! ;
    
    public RikaFlightDrawAndKiteingManager()
    {
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this);
        //Harmony.DEBUG = true; //~ readd this if Harmony is being weird (only readd for your end though) Havmir 08/08/2025
        ModEntry.Instance.Logger.LogInformation("Please let Havmir know that Harmony.DEBUG was left on in RikaFlightDrawAndKiteingManager.");
        ModEntry.Instance.Helper.Utilities.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AMove), nameof(AMove.Begin)), 
            transpiler: new HarmonyMethod(GetType(), nameof(AMovePatch)));
    }
    
    private static void HavmirsRikaFlightDrawCustomFunction(Ship ship, Combat combat)
    {
        if (ship.Get(RikaFlightDraw.Status) > 0)
        {
            combat.QueueImmediate(new ADrawCard
            {
                count = ship.Get(RikaFlightDraw.Status)
            });
        }
    }
    
    public static IEnumerable<CodeInstruction> AMovePatch(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(
                    ILMatches.Ldloc<Ship>(originalMethod).CreateLdlocInstruction(out var aCreateLdlocInstruction).ExtractLabels(out var firstExtractLabelsOutVar),
                    ILMatches.LdcI4(Status.strafe),
                    ILMatches.AnyCall,
                    ILMatches.AnyLdcI4,
                    ILMatches.Ble,
                    ILMatches.AnyLdarg.CreateLabel(il , out var anyLdargVar))
                .Insert(    
                    SequenceMatcherPastBoundsDirection.Before,
                    SequenceMatcherInsertionResultingBounds.IncludingInsertion,
                    [
                        aCreateLdlocInstruction.Value.WithLabels(firstExtractLabelsOutVar), // loads the arguments for the function (ship (to read the status))
                        new CodeInstruction(OpCodes.Ldarg_3), // loads the arguments for the function (combat (to queue the action)))
                        new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(HavmirsRikaFlightDrawCustomFunction))), // to call the actual function
                        aCreateLdlocInstruction.Value, //This is to switch fron RikaFlightDraw to RikaKiteing. This does not what WithLabels, as we already used aCreateLdlocInstruction in this insert.
                        new CodeInstruction(OpCodes.Ldc_I4, (int) RikaKiteing.Status), //This tells the code to look for RikaKiteing Status
                        new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.Get))), //This tells the code to look for prevouisally mentioned status on a ship.
                        new CodeInstruction(OpCodes.Ldc_I4_0), // IDK, this is probally related to the IL code side of things
                        new CodeInstruction(OpCodes.Bgt, anyLdargVar)]) // IDK ... TODO, fix these notes https://discord.com/channels/806989214133780521/1138540954761035827/1410099573031960658
                .Find(
                    ILMatches.AnyLdcI4,
                    ILMatches.Instruction(OpCodes.Ldnull), 
                    ILMatches.Call("GetActualDamage") ) 
                .Insert(
                    SequenceMatcherPastBoundsDirection.Before,
                    SequenceMatcherInsertionResultingBounds.IncludingInsertion,
                    [aCreateLdlocInstruction.Value, 
                        new CodeInstruction(OpCodes.Ldc_I4, (int) RikaKiteing.Status),
                        new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.Get))),
                        new CodeInstruction(OpCodes.Add)])
                .AllElements();
                

        }
        catch (Exception e)
        {
            Console.WriteLine("[RikaMod] Flight draw and kiteing manager's transpiler failed, see next line(s) for more details:");
            Console.WriteLine($"{e}");
            throw;
        }
    }
    
    public bool HandleStatusTurnAutoStep(IKokoroApi.IV2.IStatusLogicApi.IHook.IHandleStatusTurnAutoStepArgs args)
    {
        if (args.Status != RikaFlightDraw.Status && args.Status != RikaKiteing.Status)  
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

