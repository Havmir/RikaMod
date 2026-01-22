using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class QuickEnergy : Card, IRegisterable
{
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        
        
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.RikaDeck
                    .Deck,
                rarity = Rarity.common,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "QuickEnergy", "name"])
                .Localize
        });
    }
    
    private int _energyAmountNone = 3;
    private int _energyAmountA = 4;
    private int _energyAmountB = 1;
    private int _energyNextTurnAmountB = 1;
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.gainEnergy",
                    _stringInt = $"{_energyAmountNone}"
                },
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.gainEnergy",
                    _stringInt = $"{_energyAmountA}"
                },
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.gainEnergy",
                    _stringInt = $"{_energyAmountB}"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.energyNextTurn",
                    _stringInt = $"{_energyNextTurnAmountB}"
                },
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;

    public override void OnDraw(State s, Combat c)
    {
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = _energyAmountNone
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = _energyAmountA
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = _energyAmountB
                });
                c.Queue(new AStatus
                {
                    status = Status.energyNextTurn,
                    statusAmount = _energyNextTurnAmountB,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Quick Energy drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: QuickEnergy.cs] Quick Energy drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (upgrade == Upgrade.None)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, gain {_energyAmountNone} <c=energy>energy</c>.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ExtraBattery,
                unplayable = true,
                retain = true
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, gain {_energyAmountA} <c=energy>energy</c>.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ExtraBattery,
                unplayable = true,
                retain = true
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, gain {_energyAmountB} <c=energy>energy</c> & {_energyNextTurnAmountB} <c=status>energy next turn</c>.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ExtraBattery,
                unplayable = true
            };
        }
        return default;
    }
};