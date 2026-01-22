using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoSinghShot : Card, IRegisterable, IHasCustomCardTraits
{
    public static Spr TempPlaceHolderArt;
    
    private static string _cardName = "SinghShot";
    private int _cardcostNone = 1;
    private int _cardcostA = 0;
    private int _cardcostB = 1;
    private string _actionOneNone = "action.moveRandom";
    private string _actionOneA = "action.moveRandom";
    private string _actionOneB = "action.moveRandom";
    private string _actionTwoNone = "action.moveLeft";
    private string _actionTwoA = "action.moveLeft";
    private string _actionTwoB = "action.moveRandom";
    private string _actionThreeNone = "action.attack.name";
    private string _actionThreeA = "action.attack.name";
    private string _actionThreeB = "action.attack.name";
    private int _moveOneNone = -1;
    private bool _isRandomOneNone = true;
    private int _moveOneA = -1;
    private bool _isRandomOneA = true;
    private int _moveOneB = -1;
    private bool _isRandomOneB = true;
    private int _moveTwoNone = -1;
    private bool _isRandomTwoNone = false;
    private int _moveTwoA = -1;
    private bool _isRandomTwoA = false;
    private int _moveTwoB = -1;
    private bool _isRandomTwoB = true;
    private int _damageNone = 3;
    private int _damageA = 3;
    private int _damageB = 4;
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
                        _stringString = _actionOneNone,
                        _stringInt = $"{_moveOneNone}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionTwoNone,
                        _stringInt = $"{_moveTwoNone}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionThreeNone
                    }
                ],
                Upgrade.A =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = _actionOneA,
                        _stringInt = $"{_moveOneA}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionTwoA,
                        _stringInt = $"{_moveTwoA}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionThreeA
                    }
                ],
                Upgrade.B =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = _actionOneB,
                        _stringInt = $"{_moveOneB}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionTwoB,
                        _stringInt = $"{_moveTwoB}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionThreeB
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
                    new FalseIconAMove
                    {
                        _moveNumber = _moveOneNone,
                        _isRandom = _isRandomOneNone
                    },
                    new FalseIconAMove
                    {
                        _moveNumber = _moveTwoNone,
                        _isRandom = _isRandomTwoNone
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _damageNone)
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconAMove
                    {
                        _moveNumber = _moveOneA,
                        _isRandom = _isRandomOneA
                    },
                    new FalseIconAMove
                    {
                        _moveNumber = _moveTwoA,
                        _isRandom = _isRandomTwoA
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _damageA)
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconAMove
                    {
                        _moveNumber = _moveOneB,
                        _isRandom = _isRandomOneB
                    },
                    new FalseIconAMove
                    {
                        _moveNumber = _moveTwoB,
                        _isRandom = _isRandomTwoB
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _damageB)
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
                    new FalseIconAMove
                    {
                        _moveNumber = _moveOneNone,
                        _isRandom = _isRandomOneNone
                    },
                    new FalseIconAMove
                    {
                        _moveNumber = _moveTwoNone,
                        _isRandom = _isRandomTwoNone
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _damageNone)
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostA
                    },
                    new FalseIconAMove
                    {
                        _moveNumber = _moveOneA,
                        _isRandom = _isRandomOneA
                    },
                    new FalseIconAMove
                    {
                        _moveNumber = _moveTwoA,
                        _isRandom = _isRandomTwoA
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _damageA)
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostB
                    },
                    new FalseIconAMove
                    {
                        _moveNumber = _moveOneB,
                        _isRandom = _isRandomOneB
                    },
                    new FalseIconAMove
                    {
                        _moveNumber = _moveTwoB,
                        _isRandom = _isRandomTwoB
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _damageB)
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
                c.Queue(new AMove
                {
                    dir = _moveOneNone,
                    isRandom =  _isRandomOneNone,
                    targetPlayer = true 
                });
                c.Queue(new AMove
                {
                    dir = _moveTwoNone,
                    isRandom =  _isRandomTwoNone,
                    targetPlayer = true 
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, _damageNone),
                    fast = true
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostA
                });
                c.Queue(new AMove
                {
                    dir = _moveOneA,
                    isRandom =  _isRandomOneA,
                    targetPlayer = true 
                });
                c.Queue(new AMove
                {
                    dir = _moveTwoA,
                    isRandom =  _isRandomTwoA,
                    targetPlayer = true 
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, _damageA),
                    fast = true
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostB
                });
                c.Queue(new AMove
                {
                    dir = _moveOneB,
                    isRandom =  _isRandomOneB,
                    targetPlayer = true 
                });
                c.Queue(new AMove
                {
                    dir = _moveTwoB,
                    isRandom =  _isRandomTwoB,
                    targetPlayer = true 
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, _damageB),
                    fast = true
                });
            }
        }
        
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Neo{_cardName} drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Dive.cs] Neo{_cardName} drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    

    
    public override CardData GetData(State state)
    {
        _descriptionNone =
            $"Move {Math.Abs(_moveOneNone)} randomly, move {_moveTwoNone} left & attack for <c=redd>{GetDmg(state, _damageNone)}</c> damage.";
        _descriptionA =
            $"Move {Math.Abs(_moveOneA)} randomly, move {_moveTwoA} left & attack for <c=redd>{GetDmg(state, _damageA)}</c> damage.";
        _descriptionB =
            $"Move {Math.Abs(_moveOneB)} randomly, move {_moveTwoB} randomly & attack for <c=redd>{GetDmg(state, _damageB)}</c> damage.";
        if (_artmode == 0)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = _artTintDefault,
                    art = _artNone
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = _artTintDefault,
                    art = _artA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = _artTintDefault,
                    art = _artB
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
                    art = _artAlphaNone
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = "ffffff",
                    art = _artAlphaA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = _artAlphaB
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
                    art = _artAlphaNone
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = "ffffff",
                    art = _artAlphaA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = "ffffff",
                    art = _artAlphaB
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
                    art = _artNone
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artB
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
                    art = _artNone
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artB
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
                    art = _artNone
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = _artTintDefault,
                    art = _artA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = _artTintDefault,
                    art = _artB
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