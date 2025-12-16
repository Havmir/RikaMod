using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class QuickBlock : Card, IRegisterable
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
                rarity = Rarity.common,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "QuickBlock", "name"])
                .Localize,
            Art = StableSpr.cards_Dodge,
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
                    _stringString = "status.tempShield",
                    _stringInt = $"{_tempShieldAmountNone}"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.tempShield",
                    _stringInt = $"{_tempShieldAmountA}"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.tempShield",
                    _stringInt = $"{_tempShieldAmountB}"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.Shield",
                    _stringInt = $"{_shieldAmountB}"
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private int _tempShieldAmountNone = 3;
    private int _tempShieldAmountA = 4;
    private int _tempShieldAmountB = 2;
    private int _shieldAmountB = 2;
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Quick Block"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                c.Queue(new AStatus
                {
                    status = Status.tempShield,
                    statusAmount = _tempShieldAmountNone,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                c.Queue(new AStatus
                {
                    status = Status.tempShield,
                    statusAmount = _tempShieldAmountA,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                c.Queue(new AStatus
                {
                    status = Status.tempShield,
                    statusAmount = _tempShieldAmountB,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.shield,
                    statusAmount = _shieldAmountB,
                    targetPlayer = true
                });
            }
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
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_tempShieldAmountNone} <c=status>temp shield</c>."
                };
            }

            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_tempShieldAmountA} <c=status>temp shield</c>."
                };
            }

            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_tempShieldAmountB} <c=status>temp shield</c> & {_shieldAmountB} <c=status>shield</c>."
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
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_tempShieldAmountNone} <c=status>temp shield</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Shield
                };
            }

            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_tempShieldAmountA} <c=status>temp shield</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Shield
                };
            }

            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_tempShieldAmountB} <c=status>temp shield</c> & {_shieldAmountB} <c=status>shield</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Shield
                };
            }
        }
        return default;
    }
};