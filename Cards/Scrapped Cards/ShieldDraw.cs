using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class ShieldDraw : Card, IRegisterable
{
    public static Spr ShieldDrawArt;
    public static Spr RedTrashFumesBackgroundSprite;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        ShieldDrawArt = ModEntry.RegisterSprite(package, "assets/Card/ShieldDraw.png").Sprite;
        
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.RikaDeck
                    .Deck,
                rarity = Rarity.uncommon,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ShieldDraw", "name"])
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
                    IsWeird = true,
                    WeirdCase = 1,
                    _stringString = "status.shieldAlt"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.drawNextTurn",
                    _stringInt = $"{s.ship.Get(Status.shield)}"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    IsWeird = true,
                    WeirdCase = 1,
                    _stringString = "status.shieldAlt"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.drawNextTurn",
                    _stringInt = $"{s.ship.Get(Status.shield)}"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    IsWeird = true,
                    WeirdCase = 1,
                    _stringString = "status.shieldAlt"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.drawNextTurn",
                    _stringInt = $"{s.ship.Get(Status.shield)}"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.tempShield",
                    _stringInt = $"{s.ship.Get(Status.shield)}"
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;

    private int _getShipShield = 0;
    
    public override void OnDraw(State s, Combat c)
    {
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            _getShipShield = s.ship.Get(Status.shield);
            
            if (_isplaytester)
            {
                Console.WriteLine($"[RikaMod] Shield Draw drawn | _getShipShield: {_getShipShield} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
            }
            if (_logALotOfThings)
            {
                ModEntry.Instance.Logger.LogInformation($"[RikaMod] Shield Draw drawn | _getShipShield: {_getShipShield} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
            }
            
            if (upgrade == Upgrade.None)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                Console.WriteLine($"1 _getShipShield: {_getShipShield}");
                c.Queue(new AStatus
                {
                    status = Status.shield,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true,
                    omitFromTooltips = true
                });
                Console.WriteLine($"2 _getShipShield: {_getShipShield}");
                c.Queue(new AStatus
                {
                    status = Status.drawNextTurn,
                    statusAmount = s.ship.Get(Status.shield),
                    targetPlayer = true,
                    omitFromTooltips = true,
                    xHint = s.ship.Get(Status.shield)
                });
                Console.WriteLine($"3 _getShipShield: {_getShipShield}");
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = Status.drawNextTurn,
                    statusAmount = s.ship.Get(Status.shield),
                    targetPlayer = true,
                    omitFromTooltips = true,
                    xHint = s.ship.Get(Status.shield)
                });
                c.Queue(new AStatus
                {
                    status = Status.shield,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true,
                    omitFromTooltips = true
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new AStatus
                {
                    status = Status.drawNextTurn,
                    statusAmount = s.ship.Get(Status.shield),
                    targetPlayer = true,
                    omitFromTooltips = true,
                    xHint = s.ship.Get(Status.shield)
                });
                c.Queue(new AStatus
                {
                    status = Status.tempShield,
                    statusAmount = s.ship.Get(Status.shield),
                    targetPlayer = true,
                    omitFromTooltips = true,
                    xHint = s.ship.Get(Status.shield)
                });
                c.Queue(new AStatus
                {
                    status = Status.shield,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true,
                    omitFromTooltips = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Shield Draw drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: ShieldDraw.cs] Shield Draw drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                description = $"On draw, <c=downside>-1 energy & convert shield</c> to <c=status>draw next turn</c> (<c=keyword>{state.ship.Get(Status.shield)}</c>).",
                artTint = _artTintDefault,
                art = ShieldDrawArt
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, <c=downside>convert shield</c> to <c=status>draw next turn</c> (<c=keyword>{state.ship.Get(Status.shield)}</c>).",
                artTint = _artTintDefault,
                art = ShieldDrawArt
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 1,
                description = $"On draw, <c=downside>-1 nrg, convert shield</c> to <c=status>temp sheild</c> & <c=status>draw next turn</c> (<c=keyword>{state.ship.Get(Status.shield)}</c>).",
                artTint = _artTintDefault,
                art = ShieldDrawArt
            };
        }
        return default;
    }
};