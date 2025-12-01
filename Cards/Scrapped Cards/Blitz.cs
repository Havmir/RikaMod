using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class Blitz : Card, IRegisterable
{
    private int _rikaCardsDrawnThisTurn;
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
                rarity = Rarity.rare,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Blitz", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/Blitz.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAAttack()
            ],
            Upgrade.A =>
            [
                new ToolTipAAttack()
            ],
            Upgrade.B =>
            [
                new ToolTipAAttack()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Blitz"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        _rikaCardsDrawnThisTurn =
            ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                for (int i = 1; i <= _rikaCardsDrawnThisTurn; i++)
                {
                    c.Queue(new AAttack
                    {
                        damage = GetDmg(s, 0),
                        fast = true
                    });
                    if (_isplaytester)
                    {
                        Console.WriteLine($"[RikaMod] Attack({i}) | _rikaCardsDrawnThisTurn: {_rikaCardsDrawnThisTurn} | ModEntry Number: {ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0)} | Turn: {c.turn}");
                    }
                }
            }
            if (upgrade == Upgrade.A)
            {
                for (int i = 1; i <= _rikaCardsDrawnThisTurn; i++)
                {
                    c.Queue(new AAttack
                    {
                        damage = GetDmg(s, 1),
                        fast = true
                    });
                    if (_isplaytester)
                    {
                        Console.WriteLine($"[RikaMod] Attack({i}) | _rikaCardsDrawnThisTurn: {_rikaCardsDrawnThisTurn} | ModEntry Number: {ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0)} | Turn: {c.turn}");
                    }
                }
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                for (int i = 1; i <= _rikaCardsDrawnThisTurn; i++)
                {
                    c.Queue(new AAttack
                    {
                        damage = GetDmg(s, 2),
                        fast = true
                    });
                    if (_isplaytester)
                    {
                        Console.WriteLine($"[RikaMod] Attack({i}) | _rikaCardsDrawnThisTurn: {_rikaCardsDrawnThisTurn} | ModEntry Number: {ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0)} | Turn: {c.turn}");
                    }
                }
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
                    //description = $"On draw, <c=downside>-1 energy</c>, but xN: Attack for <c=redd>{GetDmg(state, 1)}</c>, N = cards played this turn ({_cardsPlayedThisTurn})."
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 0)}</c> dmg per <c=f00707>Rika</c> card drawn this turn ({ModEntry.Instance.Helper.ModData.GetModDataOrDefault(state, "rikaCardsPerTurnNumber", 0)}).",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg per <c=f00707>Rika</c> card drawn this turn ({ModEntry.Instance.Helper.ModData.GetModDataOrDefault(state, "rikaCardsPerTurnNumber", 0)}).",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c>, but attack for <c=redd>{GetDmg(state, 2)}</c> dmg per <c=f00707>Rika</c> card drawn this turn ({ModEntry.Instance.Helper.ModData.GetModDataOrDefault(state, "rikaCardsPerTurnNumber", 0)}).",
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
                    cost = 0,
                    //description = $"On draw, <c=downside>-1 energy</c>, but xN: Attack for <c=redd>{GetDmg(state, 1)}</c>, N = cards played this turn ({_cardsPlayedThisTurn})."
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 0)}</c> dmg per <c=db9b79>Rika</c> card drawn this turn ({ModEntry.Instance.Helper.ModData.GetModDataOrDefault(state, "rikaCardsPerTurnNumber", 0)}).",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_MultiShot
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg per <c=db9b79>Rika</c> card drawn this turn ({ModEntry.Instance.Helper.ModData.GetModDataOrDefault(state, "rikaCardsPerTurnNumber", 0)}).",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_MultiShot
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c>, but attack for <c=redd>{GetDmg(state, 2)}</c> dmg per <c=db9b79>Rika</c> card drawn this turn ({ModEntry.Instance.Helper.ModData.GetModDataOrDefault(state, "rikaCardsPerTurnNumber", 0)}).",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_MultiShot
                };
            }
        }
        return default;
    }
};