using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoDive : Card, IRegisterable, IHasCustomCardTraits
{
    public static Spr TempPlaceHolderArt;
    
    private static string _cardName = "Dive";
    private int _cardcostNone = 1;
    private int _cardcostA = 0;
    private int _cardcostB = 1;
    private int _moveNone = -1;
    private int _moveA = -1;
    private int _moveB = -1;
    private int _damageNone = 3;
    private int _damageA = 3;
    private int _damageB = 4;
    private string _descriptionNone = "_descriptionNone = null";
    private string _descriptionA = "_descriptionA = null";
    private string _descriptionB = "_descriptionB = null";
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private Spr? _artNone = StableSpr.cards_ScootLeft;
    private Spr? _artA =  StableSpr.cards_ScootLeft;
    private Spr? _artB =  StableSpr.cards_ScootLeft;
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
                        _stringString = "action.moveLeft",
                        _stringInt = $"{_moveNone}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = "action.attack.name"
                    }
                ],
                Upgrade.A =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = "action.moveLeft",
                        _stringInt = $"{_moveA}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = "action.attack.name"
                    }
                ],
                Upgrade.B =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = "action.moveLeft",
                        _stringInt = $"{_moveB}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = "action.attack.name"
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
                        _moveNumber = _moveNone
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
                        _moveNumber = _moveA
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
                        _moveNumber = _moveB
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
                        _moveNumber = _moveNone
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
                        _moveNumber = _moveA
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
                        _moveNumber = _moveB
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
                    dir = _moveNone,
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
                    dir = _moveA,
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
                    dir = _moveB,
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
            Console.WriteLine($"[RikaMod] Dive drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Dive.cs] BarrelRoll drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }
    

    
    public override CardData GetData(State state)
    {
        _descriptionNone =
            $"On draw, <c=downside>-{_cardcostNone} energy</c> but move {Math.Abs(_moveNone)} left & attack for <c=redd>{GetDmg(state, _damageNone)}</c> damage.";
        _descriptionA =
            $"On draw, move {Math.Abs(_moveA)} left & attack for <c=redd>{GetDmg(state, _damageA)}</c> damage.";
        _descriptionB =
            $"On draw, <c=downside>-{_cardcostB} energy</c> but move {Math.Abs(_moveB)} left & attack for <c=redd>{GetDmg(state, _damageB)}</c> damage.";
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