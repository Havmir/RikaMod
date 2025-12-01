using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class Kiteing : Card, IRegisterable
{
    public static Spr KiteingCardBackgroundSprite;
    
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Kiteing", "name"])
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
                    _stringString = $"{_kiteingStatusName}",
                    _stringInt = "1"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = $"{_kiteingStatusName}",
                    _stringInt = "1"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = $"{_kiteingStatusName}",
                    _stringInt = "2"
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
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 2
                });
                c.Queue(new AStatus
                {
                    status = RikaFlightDrawAndKiteingManager.RikaKiteing.Status,
                    statusAmount = 1,
                    targetPlayer = true
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new AStatus
                {
                    status = RikaFlightDrawAndKiteingManager.RikaKiteing.Status,
                    statusAmount = 1,
                    targetPlayer = true
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 2
                });
                c.Queue(new AStatus
                {
                    status = RikaFlightDrawAndKiteingManager.RikaKiteing.Status,
                    statusAmount = 2,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Kiteing drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Kiteing.cs] Kiteing drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    private string _kiteingStatusName = ArtManager.KiteingDisplayName;
    
    public override CardData GetData(State state)
    {
        if (upgrade == Upgrade.None)
        {
            return new CardData
            {
                cost = 2,
                description = $"On draw, <c=downside>-2 energy</c>, but gain 1      <c=status>{_kiteingStatusName}</c>.",
                artTint = _artTintDefault,
                art = KiteingCardBackgroundSprite
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 1,
                description = $"On draw, <c=downside>-1 energy</c>, but gain 1      <c=status>{_kiteingStatusName}</c>.",
                artTint = _artTintDefault,
                art = KiteingCardBackgroundSprite
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 2,
                description = $"On draw, <c=downside>-2 energy</c>, but gain 2      <c=status>{_kiteingStatusName}</c>.",
                artTint = _artTintDefault,
                art = KiteingCardBackgroundSprite
            };
        }
        return default;
    }
};