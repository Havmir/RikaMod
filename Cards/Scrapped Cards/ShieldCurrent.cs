using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class ShieldCurrent : Card, IRegisterable
{
    private int _calculation;

    public static Spr ShieldCurrentB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        ShieldCurrentB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ShieldCurrentB.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ShieldCurrent", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ShieldCurrent.png").Sprite
        });
    }

    private int _rikaFluxAmountNone = 2;
    private int _rikaFluxAmountA = 2;
    private int _libraAmountB = 1;
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "RikaFlux",
                    _stringInt = $"{_rikaFluxAmountNone}"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "RikaFlux",
                    _stringInt = $"{_rikaFluxAmountA}"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.libra",
                    _stringInt = $"{_libraAmountB}"
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
            CardName = "Shield Current"
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
                    status = RikaFluxManager.RikaFlux.Status,
                    statusAmount = _rikaFluxAmountNone,
                    targetPlayer = true
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = RikaFluxManager.RikaFlux.Status,
                    statusAmount = _rikaFluxAmountA,
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
                    status = Status.libra,
                    statusAmount = _libraAmountB,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Shield Current drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: ShieldCurrent.cs] Shield Current drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_rikaFluxAmountNone} <c=status>shield flow</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, gain {_rikaFluxAmountNone} <c=status>shield flow</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_libraAmountB} <c=status>flux</c>.",
                    artTint = "ffffff",
                    art = ShieldCurrentB
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
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_rikaFluxAmountNone} <c=status>shield flow</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Flux
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, gain {_rikaFluxAmountNone} <c=status>shield flow</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Flux
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_libraAmountB} <c=status>flux</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Flux
                };
            }  
        }
        return default;
    }
};