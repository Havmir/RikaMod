using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class Tailwind : Card, IRegisterable
{
    private int _calculation;

    public static Spr TailwindA;
    public static Spr TailwindB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        TailwindA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/TailwindA.png").Sprite;
        TailwindB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/TailwindB.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Tailwind", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/Tailwind.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipADrawCard3()
            ],
            Upgrade.A =>
            [
                new ToolTipADrawCard5()
            ],
            Upgrade.B =>
            [
                new ToolTipADrawCard2()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;

    public override void OnDraw(State s, Combat c)
    {
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
                c.Queue(new ADrawCard
                {
                    count = 3
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new ADrawCard
                {
                    count = 5
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new ADrawCard
                {
                    count = 2
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Tailwind drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Tailwind.cs] Tailwind drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                    description = "On draw, <c=downside>-1 energy</c> but draw 3.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but draw 5.",
                    artTint = "ffffff",
                    art = TailwindA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, draw 2.",
                    art = TailwindB,
                    artTint = "ffffff"
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
                    description = "On draw, <c=downside>-1 energy</c> but draw 3.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Ace,
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but draw 5.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Ace
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, draw 2.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Ace
                };
            }
        }
        return default;
    }
};