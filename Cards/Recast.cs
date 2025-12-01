using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class Recast : Card, IRegisterable
{
    private static Spr _customArt;
    public static Spr RedTrashFumesBackgroundSprite;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        _customArt = ModEntry.RegisterSprite(package, "assets/Card/Recast.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Recast", "name"])
                .Localize,
            Art = RedTrashFumesBackgroundSprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.searchCardNew.hand",
                    _stringInt = "hand" // Yep, this is totally an int ~ Havmir 02/08/2025
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.searchCardNew.hand",
                    _stringInt = "hand"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.searchCardNew.hand",
                    _stringInt = "hand"
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;

    public override void OnDraw(State s, Combat c)
    {
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new ACardSelect
                {
                    browseAction = new PutDiscardedCardOnTopOfDrawPile(), //Totally using this as intended ~ Havmor 02/08/2025
                    browseSource = CardBrowse.Source.Hand
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new ACardSelect
                {
                    browseAction = new PutDiscardedCardOnTopOfDrawPile(),
                    browseSource = CardBrowse.Source.Hand
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new ACardSelect
                {
                    browseAction = new PutDiscardedCardOnTopOfDrawPile(),
                    browseSource = CardBrowse.Source.Hand
                });
                c.Queue(new ACardSelect
                {
                    browseAction = new PutDiscardedCardOnTopOfDrawPile(),
                    browseSource = CardBrowse.Source.Hand
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Recast drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Recast.cs] Recast drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (upgrade == Upgrade.None)
        {
            return new CardData
            {
                cost = 1,
                description = "On draw, <c=downside>-1 energy</c> but move a card from hand to the top of the draw pile.",
                artTint = _artTintDefault,
                art = _customArt
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = "On draw, move a card from hand to the top of the draw pile.",
                artTint = _artTintDefault,
                art = _customArt
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 1,
                description = "On draw, <c=downside>-1 nrg</c> but move 2 cards from hand to the top of the draw pile.",
                artTint = _artTintDefault,
                art = _customArt
            };
        }
        return default;
    }
};