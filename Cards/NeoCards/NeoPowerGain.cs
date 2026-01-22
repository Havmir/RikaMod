using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoPowerGain : Card, IRegisterable, IHasCustomCardTraits
{
    public static Spr TempPlaceHolderArt;
    public static Spr PowerGainBv2;
    
    private static string _cardName = "PowerGain";
    // Make sure to update rarirty
    private int _cardcostNone = 3;
    private int _cardcostA = 2;
    private int _cardcostB = 4;
    private string _actionOneNone = "status.powerdrive";
    private string _actionOneA = "status.powerdrive";
    private string _actionOneB = "status.powerdrive";
    private string _actionTwoB = "status.energyLessNextTurn";
    private int _actionOneAmountNone = 1;
    private int _actionOneAmountA = 1;
    private int _actionOneAmountB = 2;
    private int _actionTwoAmountB = 2;
    private string _descriptionNone = "_descriptionNone = null";
    private string _descriptionA = "_descriptionA = null";
    private string _descriptionB = "_descriptionB = null";
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private Spr? _artNone = StableSpr.cards_PowerPlay;
    private Spr? _artA =  StableSpr.cards_PowerPlay;
    private Spr? _artB =  PowerGainBv2;
    private Spr _artAlphaNone = TempPlaceHolderArt;
    private Spr _artAlphaA = TempPlaceHolderArt;
    private Spr _artAlphaB = TempPlaceHolderArt;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    private bool _rikasTraitOverride = ArtManager.RikasTraitOverride;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", $"{_cardName}", "name"])
                .Localize,
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        if (_artmode == 0 || _artmode == 1 || _artmode == 2)
        {
            return upgrade switch
            {
                Upgrade.None =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneNone}",
                        _stringInt = $"{_actionOneAmountNone}"
                    }
                ],
                Upgrade.A =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneA}",
                        _stringInt = $"{_actionOneAmountA}"
                    }
                ],
                Upgrade.B =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneB}",
                        _stringInt = $"{_actionOneAmountB}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionTwoB}",
                        _stringInt = $"{_actionTwoAmountB}"
                    }
                ],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else if (_artmode == 3)
        {
            return upgrade switch
            {
                Upgrade.None =>
                [
                    new FalseIconAStatus
                    {
                        _status = _actionOneNone,
                        _statusAmount = _actionOneAmountNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconAStatus
                    {
                        _status = _actionOneA,
                        _statusAmount = _actionOneAmountA
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconAStatus
                    {
                        _status = _actionOneB,
                        _statusAmount = _actionOneAmountB
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionTwoB,
                        _statusAmount = _actionTwoAmountB
                    }
                ],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else if (_artmode == 4)
        {
            return upgrade switch
            {
                Upgrade.None =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostNone
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionOneNone,
                        _statusAmount = _actionOneAmountNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostA
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionOneA,
                        _statusAmount = _actionOneAmountA
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostB
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionOneB,
                        _statusAmount = _actionOneAmountB
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionTwoB,
                        _statusAmount = _actionTwoAmountB
                    }
                ],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else
        {
            Console.WriteLine($"[RikaMod: Neo{_cardName}.cs] Please tell Havmir that this card is buggy when ArtMode = {_artmode}.");
        }
        return default!;
    }
    

    
    public override void OnDraw(State s, Combat c)
    {
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostNone
                });
                c.Queue(new AStatus
                {
                    status = Status.powerdrive,
                    statusAmount = _actionOneAmountNone,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerGain",
                    Statusamount = _actionOneAmountNone
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostA
                });
                c.Queue(new AStatus
                {
                    status = Status.powerdrive,
                    statusAmount = _actionOneAmountA,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerGain",
                    Statusamount = _actionOneAmountA
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostB
                });
                c.Queue(new AStatus
                {
                    status = Status.powerdrive,
                    statusAmount = _actionOneAmountB,
                    targetPlayer = true
                });
                c.Queue(new PowerGainOrPowerBoostJankFix
                {
                    Cardname = "PowerGain",
                    Statusamount = _actionOneAmountB
                });
                c.Queue(new AStatus
                {
                    status = Status.energyLessNextTurn,
                    statusAmount = _actionTwoAmountB,
                    targetPlayer = true
                });

            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Nep{_cardName} drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Nep{_cardName}.cs] {_cardName} drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    

    
    public override CardData GetData(State state)
    {
        _descriptionNone =
            $"On draw, <c=downside>-{_cardcostNone} energy</c> but gain {_actionOneAmountNone} <c=status>overdrive</c>.";
        _descriptionA =
            $"On draw, gain {_actionOneAmountA} <c=status>overdrive</c>.";
        _descriptionB =
            $"On draw, <c=downside>-{_cardcostB} energy</c> but gain {_actionOneAmountB} <c=status>overdrive</c>.";
        if (_artmode == 0)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = _artTintDefault,
                    art = _artNone,
                    retain = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = _artTintDefault,
                    art = _artA,
                    retain = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = _artTintDefault,
                    art = _artB,
                    retain = true,
                    unplayable = true
                };
            }
        }
        else if (_artmode == 1)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = "ffffff",
                    art = _artAlphaNone,
                    retain = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = "ffffff",
                    art = _artAlphaA,
                    retain = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = _artAlphaB,
                    retain = true,
                    unplayable = true
                };
            }
        }
        else if (_artmode == 2)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    artTint = "ffffff",
                    art = _artAlphaNone,
                    retain = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = "ffffff",
                    art = _artAlphaA,
                    retain = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = "ffffff",
                    art = _artAlphaB,
                    retain = true,
                    unplayable = true
                };
            }
        }
        else if (_artmode == 3)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    artTint = _artTintDefault,
                    art = _artNone,
                    retain = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA,
                    retain = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artB,
                    retain = true,
                    unplayable = true
                };
            }
        }
        else if (_artmode == 4)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    artTint = _artTintDefault,
                    art = _artNone,
                    retain = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA,
                    retain = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artB,
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
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = _artTintDefault,
                    art = _artNone,
                    retain = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = _artTintDefault,
                    art = _artA,
                    retain = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = _artTintDefault,
                    art = _artB,
                    retain = true,
                    unplayable = true
                };
            }
        }
        return default;
    }
    

    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
    {
        if (_artmode == 3 || _artmode == 4 || _rikasTraitOverride)
        {
            if (upgrade == Upgrade.None && _haveRikasTraitNone || upgrade == Upgrade.A && _haveRikasTraitA || upgrade == Upgrade.B && _haveRikasTraitB)
            {
                return new HashSet<ICardTraitEntry> { ModEntry.Instance.RikasTrait };
            }
        }
        return default!;
    }
};