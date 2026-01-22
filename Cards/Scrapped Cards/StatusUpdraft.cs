using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class StatusUpdraft : Card, IRegisterable
{
    public static Spr StatusUpdraftCardBackgroundSprite;
    
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "StatusUpdraft", "name"])
                .Localize,
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent()
                {
                    _stringString = "status.boost",
                    _stringInt = "1"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent()
                {
                    _stringString = "status.boost",
                    _stringInt = "2"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent()
                {
                    _stringString = "status.boost",
                    _stringInt = "3"
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
                    cardCost = 1
                });
                c.Queue(new AStatus
                {
                    status = Status.boost,
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
                    status = Status.boost,
                    statusAmount = 2,
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
                    status = Status.boost,
                    statusAmount = 3,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Status Updraft drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: StatusUpdraft.cs] Status Updraft drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (upgrade == Upgrade.None)
        {
            return new CardData
            {
                cost = 1,
                description = "On draw, <c=downside>-1 energy</c> but gain 1 <c=status>boost</c>.",
                retain = true,
                artTint = _artTintDefault,
                art = StatusUpdraftCardBackgroundSprite
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 1,
                description = "On draw, <c=downside>-1 energy</c> but gain 2 <c=status>boost</c>.",
                retain = true,
                artTint = _artTintDefault,
                art = StatusUpdraftCardBackgroundSprite
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 2,
                description = "On draw, <c=downside>-2 energy</c> but gain 3 <c=status>boost</c>.",
                retain = true,
                artTint = _artTintDefault,
                art = StatusUpdraftCardBackgroundSprite
            };
        }
        return default;
    }
};