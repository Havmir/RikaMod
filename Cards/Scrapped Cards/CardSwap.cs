using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class CardSwap : Card, IRegisterable
{
    private int _calculation;

    public static Spr CardSwapA;
    public static Spr CardSwapB;
    public static Spr CardSwapV2;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        CardSwapA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CardSwapA.png").Sprite;
        CardSwapB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CardSwapB.png").Sprite;
        CardSwapV2 = ModEntry.RegisterSprite(package, "assets/Card/CardSwapV2.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CardSwap", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CardSwap.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipADiscard1(),
                new ToolTipADrawCard1()
            ],
            Upgrade.A =>
            [
                new ToolTipADiscard1(),
                new ToolTipADrawCard2()
            ],
            Upgrade.B =>
            [
                new ToolTipADiscard2(),
                new ToolTipADrawCard2()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Card Swap"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new ADiscard
                {
                    count = 1
                });
                c.Queue(new ADrawCard
                {
                    count = 1
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new ADiscard
                {
                    count = 1
                });
                c.Queue(new ADrawCard
                {
                    count = 2
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new ADiscard
                {
                    count = 2
                });
                c.Queue(new ADrawCard
                {
                    count = 2
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
                    cost = 0,
                    description = "On draw, discard 1 random card then draw 1.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, discard 1 random card then draw 2.",
                    artTint = "ffffff",
                    art = CardSwapA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, discard 2 random cards then draw 2.",
                    artTint = "ffffff",
                    art = CardSwapB
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
                    description = "On draw, discard 1 random card then draw 1.",
                    artTint = _artTintDefault,
                    art = CardSwapV2
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, discard 1 random card then draw 2.",
                    artTint = _artTintDefault,
                    art = CardSwapV2
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, discard 2 random cards then draw 2.",
                    artTint = _artTintDefault,
                    art = CardSwapV2
                };
            }
        }
        return default;
    }
};