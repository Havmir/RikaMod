using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class EnergyInvestment : Card, IRegisterable
{
    private int _calculation;
    
    public static Spr EnergyInvestmentA;

    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        EnergyInvestmentA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/EnergyInvestmentA.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "EnergyInvestment", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/EnergyInvestment.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAStatusEnergyNextTurn2()
            ],
            Upgrade.A =>
            [
                new ToolTipAStatusEnergyNextTurn3()
            ],
            Upgrade.B =>
            [
                new ToolTipAStatusEnergyNextTurn2()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Energy Investment"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                c.Queue(new AStatus
                {
                    status = Status.energyNextTurn,
                    statusAmount = 3,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                c.Queue(new AStatus
                {
                    status = Status.energyNextTurn,
                    statusAmount = 4,
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
                    status = Status.energyNextTurn,
                    statusAmount = 3,
                    targetPlayer = true
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
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but gain 2 <c=status>energy next turn</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but gain 3 <c=status>energy next turn</c>.",
                    artTint = "ffffff",
                    art = EnergyInvestmentA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but gain 2 <c=status>energy next turn</c>.",
                    buoyant = true,
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
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but gain 2 <c=status>energy next turn</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ExtraBattery
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but gain 3 <c=status>energy next turn</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ExtraBattery
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but gain 2 <c=status>energy next turn</c>.",
                    buoyant = true,
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ExtraBattery
                };
            } 
        }
        return default;
    }
};