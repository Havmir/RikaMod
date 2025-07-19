using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Cards; 

public class ColorlessRikaSummon : Card, IRegisterable
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
                deck = Deck.colorless, 
                rarity = Rarity.common, 
                dontOffer = false, 
                upgradesTo = [Upgrade.A, Upgrade.B] 
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ColorlessRikaSummon", "name"])
                .Localize,
            Art = StableSpr.cards_Dodge
        });
    }
        public override List<CardAction> GetActions(State s, Combat c) 
    {
        return upgrade switch 
        {
            Upgrade.A => [
                new ACardOffering()
                {
                    amount = 2,
                    limitDeck = ModEntry.Instance.RikaDeck.Deck,
                    makeAllCardsTemporary = true,
                    overrideUpgradeChances = false,
                    canSkip = false,
                    inCombat = true,
                    discount = -1,
                }
            ],
            Upgrade.B => [
                new ACardOffering()
                {
                    amount = 3,
                    limitDeck = ModEntry.Instance.RikaDeck.Deck,
                    makeAllCardsTemporary = true,
                    overrideUpgradeChances = false,
                    canSkip = false,
                    inCombat = true,
                    discount = -1,
                }
            ],
            Upgrade.None => [
                new ACardOffering()
                {
                    amount = 2,
                    limitDeck = ModEntry.Instance.RikaDeck.Deck,
                    makeAllCardsTemporary = true,
                    overrideUpgradeChances = false,
                    canSkip = false,
                    inCombat = true,
                    discount = -1,
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
        
    public override CardData GetData(State state)
    {
        if (_artmode == 1)
        {
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    exhaust = true, 
                    artTint = "f00707",
                    description = "Add 1 of 2 <c=cardtrait>discount, temp</c> <c=f00707>Rika</c> cards to your hand.",
                    art = StableSpr.cards_TrashFumes
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    exhaust = true,
                    artTint = "f00707",
                    description = "Add 1 of 3 <c=cardtrait>discount, temp</c> <c=f00707>Rika</c> cards to your hand.",
                    art = StableSpr.cards_TrashFumes
                };
            }
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 1,
                    exhaust = true,
                    artTint = "f00707",
                    description = "Add 1 of 2 <c=cardtrait>discount, temp</c> <c=f00707>Rika</c> cards to your hand.",
                    art = StableSpr.cards_TrashFumes
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    exhaust = true, 
                    artTint = _artTintDefault,
                    description = "Add 1 of 2 <c=cardtrait>discount, temp</c> <c=db9b79>Rika</c> cards to your hand."
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    exhaust = true,
                    artTint = _artTintDefault,
                    description = "Add 1 of 3 <c=cardtrait>discount, temp</c> <c=db9b79>Rika</c> cards to your hand."
                };
            }
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 1,
                    exhaust = true,
                    artTint = _artTintDefault,
                    description = "Add 1 of 2 <c=cardtrait>discount, temp</c> <c=db9b79>Rika</c> cards to your hand."
                };
            }
        }
        return default;
    }
};
