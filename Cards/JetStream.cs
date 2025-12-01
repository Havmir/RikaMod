using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class JetStream : Card, IRegisterable
{
    public static Spr JetStreamCardBackgroundSprite;
    
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
                rarity = Rarity.rare,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "JetStream", "name"])
                .Localize,
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
                    _stringString = "RikaEnergy"
                },
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "RikaEnergy"
                },
                new ToolTipCompitent
                {
                    _stringString = "action.drawCard",
                    _stringInt = "1"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "RikaEnergy"
                }
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
                c.Queue(new AStatus
                {
                    status = RikaEnergyManager.RikaEnergy.Status,
                    statusAmount = 3,
                    targetPlayer = true
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = RikaEnergyManager.RikaEnergy.Status,
                    statusAmount = 3,
                    targetPlayer = true
                });
                c.Queue(new ADrawCard
                {
                    count = 1
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = RikaEnergyManager.RikaEnergy.Status,
                    statusAmount = 6,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Jet Stream drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: JetStream.cs] JetStream drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                description = "On draw, gain 3 <c=status>rika energy</c>.",
                artTint = _artTintDefault,
                art = JetStreamCardBackgroundSprite
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = "On draw, gain 3 <c=status>rika energy</c> then draw a card.",
                artTint = _artTintDefault,
                art = JetStreamCardBackgroundSprite
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 0,
                description = "On draw, gain 6 <c=status>rika energy</c>.",
                artTint = _artTintDefault,
                art = JetStreamCardBackgroundSprite,
                unplayable = true,
                retain = true
            };
        }
        return default;
    }
};