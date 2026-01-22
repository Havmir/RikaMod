using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class RecoilShot : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RecoilShot", "name"])
                .Localize,
        });
    }
    
    private int _damageNone = 2;
    private int _damageA = 3;
    private int _damageB = 5;
    private int _drawLessNextTurnNone = 1;
    private int _drawLessNextTurnA = 1;
    private int _drawLessNextTurnB = 2;
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.attackPiercing"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.drawLessNextTurn",
                    _stringInt = $"{_drawLessNextTurnNone}"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.attackPiercing"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.drawLessNextTurn",
                    _stringInt = $"{_drawLessNextTurnA}"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.attackPiercing"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.drawLessNextTurn",
                    _stringInt = $"{_drawLessNextTurnB}"
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
            CardName = "Recoil Shot"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, _damageNone),
                    fast = true,
                    piercing = true
                });
                c.Queue(new AStatus
                {
                    status = Status.drawLessNextTurn,
                    statusAmount = _drawLessNextTurnNone,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, _damageA),
                    fast = true,
                    piercing = true
                });
                c.Queue(new AStatus
                {
                    status = Status.drawLessNextTurn,
                    statusAmount = _drawLessNextTurnA,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, _damageB),
                    fast = true,
                    piercing = true
                });
                c.Queue(new AStatus
                {
                    status = Status.drawLessNextTurn,
                    statusAmount = _drawLessNextTurnB,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Spare Shot drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Spareshot.cs] Spare Shot drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                    cost = 0,
                    description = $"On draw, <c=downside>-1 energy & gain 1</c> <c=status>Draw Less Next Turn</c> but attack for <c=redd>{GetDmg(state, _damageNone)}</c> damage with <c=keyword>piercing</c>.",
                    artTint = "ffffff",
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, <c=downside>-1 energy & gain 1</c> <c=status>Draw Less Next Turn</c> but attack for <c=redd>{GetDmg(state, _damageA)}</c> damage with <c=keyword>piercing</c>.",
                    artTint = "ffffff",
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, <c=downside>-1 energy & gain 1</c> <c=status>Draw Less Next Turn</c> but attack for <c=redd>{GetDmg(state, _damageB)}</c> damage with <c=keyword>piercing</c>.",
                    artTint = "ffffff",
                    exhaust = true
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
                    description = $"On draw, <c=downside>gain {_drawLessNextTurnNone}</c> <c=status>draw less next turn</c> but attack for <c=redd>{GetDmg(state, _damageNone)}</c> <c=keyword>pierce</c> dmg.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon,
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, <c=downside>gain {_drawLessNextTurnA}</c> <c=status>draw less next turn</c> but attack for <c=redd>{GetDmg(state, _damageA)}</c> <c=keyword>pierce</c> dmg.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon,
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, <c=downside>gain {_drawLessNextTurnB}</c> <c=status>draw less next turn</c> but attack for <c=redd>{GetDmg(state, _damageB)}</c> <c=keyword>pierce</c> dmg.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon,
                    exhaust = true
                };
            }
        }
        return default;
    }
};