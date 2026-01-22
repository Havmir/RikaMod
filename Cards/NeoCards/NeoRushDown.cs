using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoRushDown : Card, IRegisterable, IHasCustomCardTraits
{
    public static Spr TempPlaceHolderArt;
    
    private static string _cardName = "RushDown";
    private int _cardcostNone = 0;
    private int _cardcostA = 0;
    private int _cardcostB = 0;
    private string _actionOneNone = "status.evade";
    private string _actionOneA = "status.evade";
    private string _actionOneB = "status.evade";
    private int _actionOneAmountNone = 2;
    private int _actionOneAmountA = 3;
    private int _actionOneAmountB = 2;
    private string _actionTwoNone = "status.maxShield";
    private string _actionTwoA = "status.maxShield";
    private string _actionTwoB = "status.maxShield";
    private int _actionTwoAmountNone = -1;
    private int _actionTwoAmountA = -1;
    private int _actionTwoAmountB = -1;
    private string _actionThreeNone = "status.loseEvadeNextTurn";
    private string _actionThreeA = "status.loseEvadeNextTurn";
    private string _actionThreeB = "status.loseEvadeNextTurn";
    private int _actionThreeAmountNone = 1;
    private int _actionThreeAmountA = 1;
    private int _actionThreeAmountB = 1;
    private string _descriptionNone = "_descriptionNone = null";
    private string _descriptionA = "_descriptionA = null";
    private string _descriptionB = "_descriptionB = null";
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private Spr? _artNone = StableSpr.cards_Dodge;
    private Spr? _artA =  StableSpr.cards_Dodge;
    private Spr? _artB =  StableSpr.cards_Dodge;
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
                rarity = Rarity.uncommon,
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
                        _stringString = _actionOneNone,
                        _stringInt = $"{_actionOneAmountNone}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionTwoNone,
                        _stringInt = $"{_actionTwoAmountNone}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionThreeNone,
                        _stringInt = $"{_actionThreeAmountNone}"
                    }
                ],
                Upgrade.A =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = _actionOneA,
                        _stringInt = $"{_actionOneAmountA}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionTwoA,
                        _stringInt = $"{_actionTwoAmountA}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionThreeA,
                        _stringInt = $"{_actionThreeAmountA}"
                    }
                ],
                Upgrade.B =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = _actionOneB,
                        _stringInt = $"{_actionOneAmountB}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionTwoB,
                        _stringInt = $"{_actionTwoAmountB}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionThreeB,
                        _stringInt = $"{_actionThreeAmountB}"
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
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionTwoNone,
                        _statusAmount = _actionTwoAmountNone
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionThreeNone,
                        _statusAmount = _actionThreeAmountNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconAStatus
                    {
                        _status = _actionOneA,
                        _statusAmount = _actionOneAmountA
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionTwoA,
                        _statusAmount = _actionTwoAmountA
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionThreeA,
                        _statusAmount = _actionThreeAmountA
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
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionThreeB,
                        _statusAmount = _actionThreeAmountB
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
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionTwoNone,
                        _statusAmount = _actionTwoAmountNone
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionThreeNone,
                        _statusAmount = _actionThreeAmountNone
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
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionTwoA,
                        _statusAmount = _actionTwoAmountA
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionThreeA,
                        _statusAmount = _actionThreeAmountA
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
                    },
                    new FalseIconAStatus
                    {
                        _status = _actionThreeB,
                        _statusAmount = _actionThreeAmountB
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
                c.Queue(new AStatus
                {
                    status = Status.evade,
                    statusAmount = _actionOneAmountNone,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.maxShield,
                    statusAmount = _actionTwoAmountNone,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.loseEvadeNextTurn,
                    statusAmount = _actionThreeAmountNone,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = Status.evade,
                    statusAmount = _actionOneAmountA,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.maxShield,
                    statusAmount = _actionTwoAmountA,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.loseEvadeNextTurn,
                    statusAmount = _actionThreeAmountNone,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = Status.evade,
                    statusAmount = _actionOneAmountB,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.maxShield,
                    statusAmount = _actionTwoAmountB,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.loseEvadeNextTurn,
                    statusAmount = _actionThreeAmountNone,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Neo{_cardName} drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Neo{_cardName}.cs] Neo{_cardName} drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    

    
    public override CardData GetData(State state)
    {
        _descriptionNone =
            $"On draw, gain {_actionOneAmountNone} <c=status>evade</c>, <c=downside>but -{_actionTwoAmountNone}</c> <c=status>shield capacity</c> & lose all evade next turn.";
        _descriptionA =
            $"On draw, gain {_actionOneAmountA} <c=status>evade</c>, <c=downside>but -{_actionTwoAmountA}</c> <c=status>shield capacity</c> & lose all evade next turn.";
        _descriptionB =
            $"On draw, gain {_actionOneAmountB} <c=status>evade</c>, <c=downside>but -{_actionTwoAmountB}</c> <c=status>shield capacity</c> & lose all evade next turn.";
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
                    exhaust = true
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
                    exhaust = true
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
                    retain = true
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
                    exhaust = true
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
                    exhaust = true
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
                    retain = true
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
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = "ffffff",
                    art = _artAlphaA,
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = "ffffff",
                    art = _artAlphaB,
                    retain = true
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
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA,
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artB,
                    retain = true
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
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA,
                    exhaust = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artB,
                    retain = true
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
                    exhaust = true
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
                    exhaust = true
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
                    retain = true
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