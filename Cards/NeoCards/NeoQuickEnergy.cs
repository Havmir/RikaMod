using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoQuickEnergy : Card, IRegisterable, IHasCustomCardTraits
{
    public static Spr TempPlaceHolderArt;
    
    private static string _cardName = "QuickEnergy";
    // Make sure to update rarirty
    private int _cardcostNone = 0;
    private int _cardcostA = 0;
    private int _cardcostB = 0;
    private string _actionOneNone = "action.gainEnergy";
    private string _actionOneA = "action.gainEnergy";
    private string _actionOneB = "action.gainEnergy";
    //private string _actionTwoB = "status.energyNextTurn";
    private int _actionOneAmountNone = 3;
    private int _actionOneAmountA = 4;
    private int _actionOneAmountB = 2;
    //private int _actionTwoAmountB = 1;
    private string _descriptionNone = "_descriptionNone = null";
    private string _descriptionA = "_descriptionA = null";
    private string _descriptionB = "_descriptionB = null";
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private Spr? _artNone = StableSpr.cards_ExtraBattery;
    private Spr? _artA =  StableSpr.cards_ExtraBattery;
    private Spr? _artB =  StableSpr.cards_ExtraBattery;
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
                    // new ToolTipCompitent
                    // {
                    //     _stringString = $"{_actionTwoB}",
                    //     _stringInt = $"{_actionTwoAmountB}"
                    // }
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
                    new FalseIconAEnergy
                    {
                        _energyNumber = _actionOneAmountNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconAEnergy
                    {
                        _energyNumber = _actionOneAmountA
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconAEnergy
                    {
                        _energyNumber = _actionOneAmountB
                    },
                    // new FalseIconAStatus
                    // {
                    //     _status = _actionTwoB,
                    //     _statusAmount = _actionTwoAmountB
                    // }
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
                    new FalseIconAEnergy
                    {
                        _energyNumber = _actionOneAmountNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconAEnergy
                    {
                        _energyNumber = _actionOneAmountA
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconAEnergy
                    {
                        _energyNumber = _actionOneAmountB
                    },
                    // new FalseIconAStatus
                    // {
                    //     _status = _actionTwoB,
                    //     _statusAmount = _actionTwoAmountB
                    // }
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
                c.Queue(new AEnergy
                {
                    changeAmount = _actionOneAmountNone
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostA
                });
                c.Queue(new AEnergy
                {
                    changeAmount = _actionOneAmountA
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostB
                });
                c.Queue(new AEnergy
                {
                    changeAmount = _actionOneAmountB
                });
                // c.Queue(new AStatus
                // {
                //     status = Status.energyNextTurn,
                //     statusAmount = _actionTwoAmountB,
                //     targetPlayer = true
                // });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Neo{_cardName} drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Neo{_cardName}.cs] {_cardName} drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    

    
    public override CardData GetData(State state)
    {
        _descriptionNone =
            $"On draw, gain {_actionOneAmountNone} <c=energy>energy</c>.";
        _descriptionA =
            $"On draw, gain {_actionOneAmountA} <c=energy>energy</c>.";
        // _descriptionB =
        //     $"On draw, gain {_actionOneAmountB} <c=energy>energy</c> & {_actionTwoAmountB} <c=status>energy next turn</c>.";
        _descriptionB = 
            $"On draw, gain {_actionOneAmountB} <c=energy>energy</c>.";
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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
                    unplayable = true,
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