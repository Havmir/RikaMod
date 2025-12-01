using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class Dive : Card, IRegisterable
{
    public static Spr RedTrashFumesBackgroundSprite;
    
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Dive", "name"])
                .Localize,
            Art = RedTrashFumesBackgroundSprite
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
                    _stringString = "action.moveLeft",
                    _stringInt = "1"
                },
                new ToolTipAAttack()
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.moveLeft",
                    _stringInt = "1"
                },
                new ToolTipAAttack()
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.moveLeft",
                    _stringInt = "1"
                },
                new ToolTipAAttack()
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
                c.Queue(new AMove
                {
                    dir = -1,
                    targetPlayer = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 3),
                    fast = true
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new AMove
                {
                    dir = -1,
                    targetPlayer = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 3),
                    fast = true
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new AMove
                {
                    dir = -1,
                    targetPlayer = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 4),
                    fast = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Dive drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Dive.cs] BarrelRoll drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                description = $"On draw, <c=downside>-1 energy</c> but move 1 left & attack for <c=redd>{GetDmg(state, 3)}</c> damage.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ScootLeft
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, move 1 left & attack for <c=redd>{GetDmg(state, 3)}</c> damage.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ScootLeft
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 1,
                description = $"On draw, <c=downside>-1 energy</c> but move 1 left & attack for <c=redd>{GetDmg(state, 4)}</c> damage.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ScootLeft
            };
        }
        return default;
    }
};