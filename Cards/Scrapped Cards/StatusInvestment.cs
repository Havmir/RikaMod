using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;

namespace RikaMod.Cards;

public class StatusInvestment : Card, IRegisterable
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
                rarity = Rarity.rare,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "StatusInvestment", "name"])
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
        c.Queue(new AcknowledgeRikaCardDrawn());
        if (upgrade == Upgrade.None)
        {
            c.Queue(new AEnergy
            {
                changeAmount = -1
            });
            c.Queue(new AStatus
            {
                status = Status.drawNextTurn,
                statusAmount = 1,
                targetPlayer = true
            });
            c.Queue(new AStatus
            {
                status = Status.boost,
                statusAmount = 1,
                targetPlayer = true
            });
        }
        if (upgrade == Upgrade.A)
        {
            c.Queue(new AStatus
            {
                status = Status.drawNextTurn,
                statusAmount = 1,
                targetPlayer = true
            });
            c.Queue(new AStatus
            {
                status = Status.boost,
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
                status = Status.drawNextTurn,
                statusAmount = 1,
                targetPlayer = true
            });
            c.Queue(new AStatus
            {
                status = Status.boost,
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
                description =
                    "On draw, <c=downside>-1 energy</c>, but gain 1 <c=status>draw next turn</c> & 1 <c=status>boost</c>."
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = "On draw, but gain 1 <c=status>draw next turn> & 1 <c=status>boost</c>."
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 1,
                description = "On draw, <c=downside>-1 energy</c>, but gain 1 <c=status>draw next turn> & 2 <c=status>boost</c>."
            };
        }
        return default;
    }
};