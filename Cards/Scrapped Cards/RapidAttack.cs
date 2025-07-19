using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;

namespace RikaMod.Cards;

public class RapidAttack : Card, IRegisterable
{
    private int _cardsPlayedThisTurn;
    
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RapidAttack", "name"])
                .Localize,
            Art = StableSpr.cards_TrashFumes,
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
    
    public override void OnDraw(State s, Combat c)
    {
        _cardsPlayedThisTurn = c.cardsPlayedThisTurn;
        if (upgrade == Upgrade.None)
        {
            for (int i = 1; i <= _cardsPlayedThisTurn; i++)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg (s, 0),
                    fast = true
                });
            }
        }
        if (upgrade == Upgrade.A)
        {
            for (int i = 1; i <= _cardsPlayedThisTurn; i++)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg (s, 1),
                    fast = true
                });
            }
        }   
        if (upgrade == Upgrade.B)
        {
            c.Queue(new AEnergy
            {
                changeAmount = -1
            });
            for (int i = 1; i <= _cardsPlayedThisTurn; i++)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg (s, 2),
                    fast = true
                });
            }
        }
    }

    public override CardData GetData(State state)
    {
        if (upgrade == Upgrade.None)
        {
            return new CardData
            {
                cost = 0,
                //description = $"On draw, <c=downside>-1 energy</c>, but xN: Attack for <c=redd>{GetDmg(state, 1)}</c>, N = cards played this turn ({_cardsPlayedThisTurn})."
                description = $"On draw, attack N times for <c=redd>{GetDmg(state, 0)}</c> damage, N = cards played this turn ({_cardsPlayedThisTurn})."
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, attack N times for <c=redd>{GetDmg(state, 1)}</c> damage, N = cards played this turn ({_cardsPlayedThisTurn})."
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 1,
                description = $"On draw, <c=downside>-1 energy</c>, but xN: Attack for <c=redd>{GetDmg(state, 2)}</c>, N = cards played this turn ({_cardsPlayedThisTurn})."
            };
        }
        return default;
    }
};