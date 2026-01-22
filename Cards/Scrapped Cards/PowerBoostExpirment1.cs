using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class PowerBoostExpirement1 : Card, IRegisterable
{
    private int _calculation;

    public static Spr PowerBoostAlphaSprite;
    public static Spr PowerBoostBAlphaSprite;
    public static Spr PowerBoostB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        PowerBoostB = PowerBoostBAlphaSprite;
        
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
            Art = PowerBoostAlphaSprite
        });
    }
    
    private int _overdriveAmountNone = 2;
    private int _overdriveAmountA = 2;
    private int _overdriveAmountB = 3;
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.overdrive",
                    _stringInt = $"{_overdriveAmountNone}"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.overdrive",
                    _stringInt = $"{_overdriveAmountA}"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.overdrive",
                    _stringInt = $"{_overdriveAmountB}"
                }
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
            CardName = "Power Boost"
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
                    cardCost = 1
                });
                c.Queue(new AStatus
                {
                    status = Status.overdrive,
                    statusAmount = _overdriveAmountNone,
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
                    statusAmount = _overdriveAmountA,
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
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 2
                });
                c.Queue(new AStatus
                {
                    status = Status.overdrive,
                    statusAmount = _overdriveAmountB,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerBoost",
                    Statusamount = 2
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Power Boost Expirenment 1 drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: PowerBoost.cs] Power Boost Expirenment 1 drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_overdriveAmountNone} <c=status>overdrive</c>.",
                    artTint = "ffffff",
                    retain = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, gain {_overdriveAmountA} <c=status>overdrive</c>.",
                    artTint = "ffffff",
                    retain = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 2,
                    description = $"On draw, <c=downside>-2 energy</c> but gain {_overdriveAmountB} <c=status>overdrive</c>.",
                    artTint = "ffffff",
                    art = PowerBoostB,
                    retain = true,
                    unplayable = true
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
                    description = $"On draw, <c=downside>-1 energy</c> but gain {_overdriveAmountNone} <c=status>overdrive</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Overdrive,
                    retain = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, gain {_overdriveAmountA} <c=status>overdrive</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Overdrive,
                    retain = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 2,
                    description = $"On draw, <c=downside>-2 energy</c> but gain {_overdriveAmountB} <c=status>overdrive</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Overpower,
                    retain = true,
                    unplayable = true
                };
            }
        }
        return default;
    }
};