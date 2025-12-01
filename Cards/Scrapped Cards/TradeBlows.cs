using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class TradeBlows : Card, IRegisterable
{
    private int _calculation;

    public static Spr TradeBlowsB;
    public static Spr TradeBlowsV2;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        TradeBlowsB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/TradeBlowsB.png").Sprite;
        TradeBlowsV2 = ModEntry.RegisterSprite(package, "assets/Card/TradeBlowsV2.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "TradeBlows", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/TradeBlows.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAStatusTempPayback1()
            ],
            Upgrade.A =>
            [
                new ToolTipAStatusTempPayback1()
            ],
            Upgrade.B =>
            [
                new ToolTipAStatusTempPayback1(),
                new ToolTipAStatusOutgoing()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Trade Blows"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AEnergy()
                {
                    changeAmount = -1
                });
                c.Queue(new AStatus()
                {
                    status = Status.tempPayback,
                    statusAmount = 1,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus()
                {
                    status = Status.tempPayback,
                    statusAmount = 1,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy()
                {
                    changeAmount = -2
                });
                c.Queue(new AStatus()
                {
                    status = Status.tempPayback,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AStatus()
                {
                    status = Status.tempPayback,
                    statusAmount = 1,
                    targetPlayer = false
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
                    description = "On draw, <c=downside>-1 energy</c> but gain 1 <c=status>temporary payback</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 1 <c=status>temporary payback</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 2,
                    description = "On draw, <c=downside>-2 energy</c> but both ships gain 1 <c=status>temporary payback</c>.",
                    artTint = "ffffff",
                    art = TradeBlowsB
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
                    description = "On draw, <c=downside>-1 energy</c> but gain 1 <c=status>temporary payback</c>.",
                    artTint = _artTintDefault,
                    art = TradeBlowsV2
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 1 <c=status>temporary payback</c>.",
                    artTint = _artTintDefault,
                    art = TradeBlowsV2
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 2,
                    description = "On draw, <c=downside>-2 energy</c> but both ships gain 1 <c=status>temporary payback</c>.",
                    artTint = _artTintDefault,
                    art = TradeBlowsV2
                };
            }
        }
        return default;
    }
};