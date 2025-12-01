using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class FumeShot : Card, IRegisterable
{
    private int _calculation;

    public static Spr SpareShotA;
    public static Spr SpareShotB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        SpareShotA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/SpareShotA.png").Sprite;
        SpareShotB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/SpareShotB.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FumeShot", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/SpareShot.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAAttack(),
                new ToolTipFumeCard()
            ],
            Upgrade.A =>
            [
                new ToolTipAAttack(),
                new ToolTipFumeCard()
            ],
            Upgrade.B =>
            [
                new ToolTipAAttack(),
                new ToolTipFumeCard()
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
            CardName = "Spare Shot"
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
                c.Queue(new AAddCard()
                {
                    card = new TrashFumes(),
                    destination = CardDestination.Deck,
                    amount = 3
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 4),
                    fast = true
                });

            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AAddCard()
                {
                    card = new TrashFumes(),
                    destination = CardDestination.Deck,
                    amount = 3
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 4),
                    fast = true
                });

            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new AAddCard()
                {
                    card = new TrashFumes(),
                    destination = CardDestination.Deck,
                    amount = 4
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 6),
                    piercing = true,
                    fast = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Fume Shot drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Fumeshot.cs] Spare Shot drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                    description = $"On draw, <c=downside>-1 energy & add 3</c> <c=card>Fumes</c><c=downside> to your deck</c>; attack for <c=redd>{GetDmg(state, 4)}</c> damage.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, <c=downside>add 3</c> <c=card>Fumes</c><c=downside> to your deck</c>; attack for <c=redd>{GetDmg(state, 4)}</c> damage.",
                    artTint = "ffffff",
                    art = SpareShotA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy & add 4</c> <c=card>Fumes</c><c=downside> to your deck</c>; attack for <c=redd>{GetDmg(state, 6)}</c> damage.",
                    artTint = "ffffff",
                    art = SpareShotB
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
                    description = $"On draw, <c=downside>-1 energy & add 3</c> <c=card>Fumes</c><c=downside> to your deck</c>; attack for <c=redd>{GetDmg(state, 4)}</c> damage.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_FumeCannon
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, <c=downside>add 3</c> <c=card>Fumes</c><c=downside> to your deck</c>; attack for <c=redd>{GetDmg(state, 4)}</c> damage.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_FumeCannon
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy & add 3</c> <c=card>Fumes</c><c=downside> to your deck</c>; attack for <c=redd>{GetDmg(state, 4)}</c> damage.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_FumeCannon
                };
            }
        }
        return default;
    }
};