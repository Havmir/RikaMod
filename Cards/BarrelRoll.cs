using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class BarrelRoll : Card, IRegisterable
{
    private int _calculation;

    public static Spr BarrelRollA;
    public static Spr BarrelRollB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        BarrelRollA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/BarrelRollA.png").Sprite;
        BarrelRollB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/BarrelRollB.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BarrelRoll", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/BarrelRoll.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAStatusAutopilot1()
            ],
            Upgrade.A =>
            [
                new ToolTipAStatusAutopilot2()
            ],
            Upgrade.B =>
            [
                new ToolTipAStatusAutopilot1(),
                new ToolTipMoveRandom2()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Barrel Roll"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AStatus
                {
                    status = Status.autopilot,
                    statusAmount = 1,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = Status.autopilot,
                    statusAmount = 2,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = Status.autopilot,
                    statusAmount = 2,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = -2,
                    targetPlayer = true,
                    isRandom = true
                });
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
                    description = "On draw, gain 1 <c=status>autopilot</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 2 <c=status>autopilot</c>.",
                    artTint = "ffffff",
                    art = BarrelRollA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 1 <c=status>autopilot</c> & randomally move 2 spaces.",
                    artTint = "ffffff",
                    art = BarrelRollB
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
                    description = "On draw, gain 1 <c=status>autopilot</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Reroute
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 2 <c=status>autopilot</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Reroute
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 1 <c=status>autopilot</c> & randomally move 2 spaces.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Reroute
                };
            }
        }
        return default;
    }
};