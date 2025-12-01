using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class PowerGain : Card, IRegisterable
{
    private int _calculation;

    public static Spr PowerGainB;
    public static Spr PowerGainBv2;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        PowerGainB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/PowerGainB.png").Sprite;
        PowerGainBv2 = ModEntry.RegisterSprite(package, "assets/Card/PowerGainBV2.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PowerGain", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/PowerGain.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAStatusPowerdrive1()
            ],
            Upgrade.A =>
            [
                new ToolTipAStatusPowerdrive1()
            ],
            Upgrade.B =>
            [
                new ToolTipAStatusPowerdrive2()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;

    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Power Gain"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 3
                });
                c.Queue(new AStatus
                {
                    status = Status.powerdrive,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerGain",
                    Statusamount = 1
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 2
                });
                c.Queue(new AStatus
                {
                    status = Status.powerdrive,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerGain",
                    Statusamount = 1
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 4
                });
                c.Queue(new AStatus
                {
                    status = Status.powerdrive,
                    statusAmount = 2,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerGain",
                    Statusamount = 2
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Power Gain drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: PowerGain.cs] Power Gain drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                    cost = 3,
                    description = "On draw, <c=downside>-3 energy</c> but gain 1 <c=status>powerdrive</c>.",
                    retain = true,
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 2,
                    description = "On draw, <c=downside>-2 energy</c> but gain 1 <c=status>powerdrive</c>.",
                    retain = true,
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 4,
                    description = "On draw, <c=downside>-4 energy</c> but gain 2 <c=status>powerdrive</c>.",
                    retain = true,
                    artTint = "ffffff",
                    art = PowerGainB
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 3,
                    description = "On draw, <c=downside>-3 energy</c> but gain 1 <c=status>powerdrive</c>.",
                    retain = true,
                    artTint = _artTintDefault,
                    art = StableSpr.cards_PowerPlay
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 2,
                    description = "On draw, <c=downside>-2 energy</c> but gain 1 <c=status>powerdrive</c>.",
                    retain = true,
                    artTint = _artTintDefault,
                    art = StableSpr.cards_PowerPlay
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 4,
                    description = "On draw, <c=downside>-4 energy</c> but gain 2 <c=status>powerdrive</c>.",
                    retain = true,
                    artTint = _artTintDefault,
                    art = PowerGainBv2
                };
            }
        }
        return default;
    }
};