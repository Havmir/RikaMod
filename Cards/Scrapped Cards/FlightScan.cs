using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class FlightScan : Card, IRegisterable
{
    public static Spr RedTrashFumesBackgroundSprite;
    
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FlightScan", "name"])
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
                    _stringString = "action.moveLeft",
                    _stringInt = "1"
                },
                new ToolTipAAttack(),
                new ADrawCard
                {
                    count = 1
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.moveLeft",
                    _stringInt = "1"
                },
                new ToolTipAAttack()
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "action.moveLeft",
                    _stringInt = "1"
                },
                new ToolTipAAttack()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 0
                });
                Console.WriteLine($"[RikaMod: FlightScan.cs] s.ship.GetWorldPos = {s.ship.GetWorldPos} | s.ship.x = {s.ship.x}");
            }
            if (upgrade == Upgrade.A)
            {

            }
            if (upgrade == Upgrade.B)
            {

            }
        }
    }
    
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (upgrade == Upgrade.None)
        {
            return new CardData
            {
                cost = 0,
                description = "On draw, <c=downside>-2 energy</c> but gain 1 <c=status>flight draw</c>.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ScootLeft,
                recycle = true
            };
        }
        if (upgrade == Upgrade.A)
        {
            return new CardData
            {
                cost = 0,
                description = "On draw, <c=downside>-1 energy</c> but gain 1 <c=status>flight draw</c>.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ScootLeft,
            };
        }
        if (upgrade == Upgrade.B)
        {
            return new CardData
            {
                cost = 1,
                description = "On draw, <c=downside>-2 energy</c> but gain 2 <c=status>flight draw</c>.",
                artTint = _artTintDefault,
                art = StableSpr.cards_ScootLeft
            };
        }
        return default;
    }
};