using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class CardInvestment : Card, IRegisterable
{
    private int _calculation;

    public static Spr CardInvestmentB;
    public static Spr CardInvestmentV2;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        CardInvestmentB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CardInvestmentB.png").Sprite;
        CardInvestmentV2 = ModEntry.RegisterSprite(package, "assets/Card/CardInvestmentV2.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CardInvestment", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CardInvestment.png").Sprite
        });
    }
    
    private int _darwNextTurnAmountNone = 4;
    private int _darwNextTurnAmountA = 4;
    private int _darwNextTurnAmountB = 5;
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.drawNextTurn",
                    _stringInt = $"{_darwNextTurnAmountNone}"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.drawNextTurn",
                    _stringInt = $"{_darwNextTurnAmountA}"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.drawNextTurn",
                    _stringInt = $"{_darwNextTurnAmountB}"
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;

    
    public override void OnDraw(State s, Combat c)
    {
        /* c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Card Investment"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);
        
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
                    status = Status.drawNextTurn,
                    statusAmount = _darwNextTurnAmountNone,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = Status.drawNextTurn,
                    statusAmount = _darwNextTurnAmountA,
                    targetPlayer = true
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
                    statusAmount = _darwNextTurnAmountB,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] CardInvestment drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: CardInvestment.cs] CardInvestment drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_darwNextTurnAmountNone} <c=status>draw next turn</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, gain {_darwNextTurnAmountA} <c=status>draw next turn</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_darwNextTurnAmountB} <c=status>draw next turn</c>.",
                    artTint = "ffffff",
                    art = CardInvestmentB
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_darwNextTurnAmountNone} <c=status>draw next turn</c>.",
                    artTint = _artTintDefault,
                    art = CardInvestmentV2
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, gain {_darwNextTurnAmountA} <c=status>draw next turn</c>.",
                    artTint = _artTintDefault,
                    art = CardInvestmentV2
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_darwNextTurnAmountB} <c=status>draw next turn</c>.",
                    artTint = _artTintDefault,
                    art = CardInvestmentV2
                };
            }
        }
        return default;
    }
};