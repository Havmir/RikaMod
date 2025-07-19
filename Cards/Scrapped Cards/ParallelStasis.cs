using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;

namespace RikaMod.Cards;

public class ParallelStasis : Card, IRegisterable
{

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
                rarity = Rarity.uncommon,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ParallelStasis", "name"])
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
                new ToolTipAStatusLockdown(),
                new ToolTipAStatusOutgoing()
            ],
            Upgrade.A =>
            [
                new ToolTipAStatusLockdown(),
                new ToolTipAStatusOutgoing()
            ],
            Upgrade.B =>
            [
                new ToolTipAStatusLockdown(),
                new ToolTipAStatusOutgoing()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        if (upgrade == Upgrade.None)
        {
            c.Queue(new AEnergy
            {
                changeAmount = -1
            });
            c.Queue(new AStatus
            {
                status = Status.lockdown,
                statusAmount = 1,
            });
            c.Queue(new AStatus
            {
                status = Status.lockdown,
                statusAmount = 1,
                targetPlayer = true
            });
        }
        if (upgrade == Upgrade.A)
        {
            c.Queue(new AStatus
            {
                status = Status.lockdown,
                statusAmount = 1,
            });
            c.Queue(new AStatus
            {
                status = Status.lockdown,
                statusAmount = 1,
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
                status = Status.lockdown,
                statusAmount = 2,
            });
            c.Queue(new AStatus
            {
                status = Status.lockdown,
                statusAmount = 2,
                targetPlayer = true
            });
        }
    }

    public override CardData GetData(State state)
    {
        if (upgrade == Upgrade.None)
        {
            return new CardData
            {
                cost = 1,
                description = "On draw, <c=downside>-1 energy</c>. Enemy gains 1 <c=status>engine lock</c>. <c=downside>You gain 1</c> <c=status>engine lock</c>."
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = "On draw, enemy gains 1 <c=status>engine lock</c>. <c=downside>You gain 1</c> <c=status>engine lock</c>."
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 0,
                description = "On draw, <c=downside>-1 energy</c>. Enemy gains 2 <c=status>engine lock</c>. <c=downside>You gain 2</c> <c=status>engine lock</c>."
            };
        }
        return default;
    }
};