using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class CardToEnergy : Card, IRegisterable
{
    private int _calculation;

    public static Spr CardToEnergyA;
    public static Spr CardToEnergyB;
    public static Spr CardToEnergyV2;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        CardToEnergyA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CardToEnergyA.png").Sprite;
        CardToEnergyB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CardToEnergyB.png").Sprite;
        CardToEnergyV2 = ModEntry.RegisterSprite(package, "assets/Card/CardToEnergyV2.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CardToEnergy", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CardToEnergy.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipADiscard1(),
                new ToolTipAEnergy1()
            ],
            Upgrade.A =>
            [
                new ToolTipADiscard1(),
                new ToolTipAEnergy2()
            ],
            Upgrade.B =>
            [
                new ToolTipADiscard2(),
                new ToolTipAEnergy2(),
                new ToolTipAStatusEnergyNextTurn2()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override void OnDraw(State s, Combat c)
    {
        /* c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Card To Energy"
        });*/ 
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);
        
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new ADiscard
                {
                    count = 1
                });
                c.Queue(new AEnergy
                {
                    changeAmount = 1
                });
            }
        
            if (upgrade == Upgrade.A)
            {
                if (upgrade == Upgrade.None)
                {
                    c.Queue(new ADiscard
                    {
                        count = 1
                    });
                    c.Queue(new AEnergy
                    {
                        changeAmount = 2
                    });
                }

                if (upgrade == Upgrade.B)
                {
                    c.Queue(new ADiscard
                    {
                        count = 2
                    });
                    c.Queue(new AEnergy
                    {
                        changeAmount = 2
                    });
                    c.Queue(new AStatus
                    {
                        status = Status.energyNextTurn,
                        statusAmount = 2,
                        targetPlayer = true
                    });
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
                    description = "On draw, discard 1 & gain 1 <c=energy>energy</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, discard 1 & gain 2 <c=energy>energy</c>.",
                    artTint = "ffffff",
                    art = CardToEnergyA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, discard 2, gain 2 <c=energy>energy</c> & 2 <c=status>energy next turn</c>.",
                    artTint = "ffffff",
                    art = CardToEnergyB
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
                    description = "On draw, discard 1 & gain 1 <c=energy>energy</c>.",
                    artTint = _artTintDefault,
                    art = CardToEnergyV2
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, discard 1 & gain 2 <c=energy>energy</c>.",
                    artTint = _artTintDefault,
                    art = CardToEnergyV2
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, discard 2, gain 2 <c=energy>energy</c> & 2 <c=status>energy next turn</c>.",
                    artTint = _artTintDefault,
                    art = CardToEnergyV2
                };
            }
        }
        return default;
    }
};