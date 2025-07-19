using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class BorrowCards : Card, IRegisterable
{
    private int _calculation;
    
    public static Spr BorrowCardsB;

    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        BorrowCardsB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/BorrowCardsB.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BorrowCards", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/BorrowCards.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAStatusDrawLessNextTurn2(),
                new ToolTipADrawCard2()
            ],
            Upgrade.A =>
            [
                new ToolTipAStatusDrawLessNextTurn2(),
                new ToolTipADrawCard2()
            ],
            Upgrade.B =>
            [
                new ToolTipAStatusDrawLessNextTurn3(),
                new ToolTipADrawCard3()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Borrow Cards"
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
                    status = Status.drawLessNextTurn,
                    statusAmount = 2,
                    targetPlayer = true
                });
                c.Queue(new ADrawCard
                {
                    count = 2
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = Status.drawLessNextTurn,
                    statusAmount = 2,
                    targetPlayer = true
                });
                c.Queue(new ADrawCard
                {
                    count = 2
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
                    status = Status.drawLessNextTurn,
                    statusAmount = 3,
                    targetPlayer = true
                });
                c.Queue(new ADrawCard
                {
                    count = 3
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
                    description = "On draw, <c=downside>-1 energy & gain 2</c> <c=status>shaken</c>, but draw 2.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, <c=downside>gain 2</c> <c=status>shaken</c>, but draw 2.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy & gain 3</c> <c=status>shaken</c>, but draw 3.",
                    artTint = "ffffff",
                    art = BorrowCardsB
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
                    description = "On draw, <c=downside>-1 energy & gain 2</c> <c=status>shaken</c>, but draw 2.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_QuickThinking
                    
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, <c=downside>gain 2</c> <c=status>shaken</c>, but draw 2.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_QuickThinking
                    
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy & gain 3</c> <c=status>shaken</c>, but draw 3.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_QuickThinking
                };
            }
        }
        return default;
    }
};