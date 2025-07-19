﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class PowerBoost : Card, IRegisterable
{
    private int _calculation;

    public static Spr PowerBoostB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        PowerBoostB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/PowerBoostB.png").Sprite;
        
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.RikaDeck
                    .Deck,
                rarity = Rarity.common,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PowerBoost", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/PowerBoost.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAStatusOverdrive1()
            ],
            Upgrade.A =>
            [
                new ToolTipAStatusOverdrive1()
            ],
            Upgrade.B =>
            [
                new ToolTipAStatusOverdrive2()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Power Boost"
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
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerBoost",
                    Statusamount = 1
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerBoost",
                    Statusamount = 1
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
                    status = Status.overdrive,
                    statusAmount = 2,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerBoost",
                    Statusamount = 2
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
                    description = "On draw, <c=downside>-1 energy</c> but gain 1 <c=status>overdrive</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 1 <c=status>overdrive</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but gain 2 <c=status>overdrive</c>.",
                    artTint = "ffffff",
                    art = PowerBoostB
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
                    description = "On draw, <c=downside>-1 energy</c> but gain 1 <c=status>overdrive</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Overdrive
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 1 <c=status>overdrive</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Overdrive
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c> but gain 2 <c=status>overdrive</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Overpower
                };
            }
        }
        return default;
    }
};