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
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.gainEnergy",
                    _stringInt = "2"
                },
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.gainEnergy",
                    _stringInt = "3"
                },
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.gainEnergy",
                    _stringInt = "1"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.energyNextTurn",
                    _stringInt = "1"
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
                    changeAmount = 2
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = 3
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = 1
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
                description = "On draw, gain 2 <c=energy>energy</c>.",
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
                description = "On draw, gain 3 <c=energy>energy</c>.",
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
                description = "On draw, gain 1 <c=energy>energy</c> & 1 <c=status>energy next turn</c>.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ExtraBattery,
                unplayable = true
            };
        }
        return default;
    }
};