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
    
    private int _drawCardAmountNone = 4;
    private int _drawCardAmountA = 6;
    private int _drawCardAmountB = 3;
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.drawCard",
                    _stringInt = $"{_drawCardAmountNone}"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.drawCard",
                    _stringInt = $"{_drawCardAmountA}"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.drawCard",
                    _stringInt = $"{_drawCardAmountB}"
                }
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
                    count = _drawCardAmountNone
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
                    count = _drawCardAmountA
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new ADrawCard
                {
                    count = _drawCardAmountB
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
                    description = $"On draw, <c=downside>-1 energy</c> but draw {_drawCardAmountNone}.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but draw {_drawCardAmountA}.",
                    artTint = "ffffff",
                    art = TailwindA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, draw {_drawCardAmountB}.",
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
                    description = $"On draw, <c=downside>-1 energy</c> but draw {_drawCardAmountNone}.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Ace,
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but draw {_drawCardAmountA}.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Ace
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, draw {_drawCardAmountB}.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Ace
                };
            }
        }
        return default;
    }
};