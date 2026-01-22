using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class RushDown : Card, IRegisterable
{
    private int _calculation;
    
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
                rarity = Rarity.uncommon,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RushDown", "name"])
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
                    _stringString = "status.evade"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.maxShield",
                    _stringInt = "-1"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.evade"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.maxShield",
                    _stringInt = "-1"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.evade"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.maxShield",
                    _stringInt = "-1"
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;

    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Haste"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AStatus
                {
                    status = Status.evade,
                    statusAmount = 2,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.maxShield,
                    statusAmount = -1,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = Status.evade,
                    statusAmount = 3,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.maxShield,
                    statusAmount = -1,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = Status.evade,
                    statusAmount = 2,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.maxShield,
                    statusAmount = -1,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Quick Dodge drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: QuickDodge.cs] Quick Dodge drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }

    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (_artmode == 1)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, gain 2 <c=status>evade</c>, <c=downside>but -1</c> <c=status>shield capacity</c>.",
                    artTint = "ffffff",
                    retain = true,
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, gain 3 <c=status>evade</c>, <c=downside>but -1</c> <c=status>shield capacity</c>.",
                    artTint = "ffffff",
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, gain 2 <c=status>evade</c>, <c=downside>but -1</c> <c=status>shield capacity</c>.",
                    artTint = "ffffff",
                    retain = true
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 2 <c=status>evade</c>, <c=downside>but   -1</c> <c=status>shield capacity</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Dodge,
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 3 <c=status>evade</c>, <c=downside>but   -1</c> <c=status>shield capacity</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Dodge,
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 2 <c=status>evade</c>, <c=downside>but   -1</c> <c=status>shield capacity</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Dodge,
                    retain = true
                };
            }
        }
        return default;
    }
};