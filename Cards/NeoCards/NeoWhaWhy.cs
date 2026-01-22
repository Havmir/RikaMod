using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoWhaWhy : Card, IRegisterable, IHasCustomCardTraits
{
    private static Spr _customArt;
    
    private static string _cardName = "WhaWhy";
    // Make sure to update rarirty
    private int _cardcostNone = 0;
    private int _cardcostA = 0;
    private int _cardcostB = 0;
    private string _actionOneNone = "ToolTipToothCardNone";
    private string _actionOneA = "ToolTipToothCardA";
    private string _actionOneB = "ToolTipToothCardB";
    private bool _textoverride = true;
    private string _descriptionNone = "_descriptionNone = null";
    private string _descriptionA = "_descriptionA = null";
    private string _descriptionB = "_descriptionB = null";
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private Spr? _artNone = _customArt;
    private Spr? _artA =  _customArt;
    private Spr? _artB =  _customArt;
    private Spr _artAlphaNone = _customArt;
    private Spr _artAlphaA = _customArt;
    private Spr _artAlphaB = _customArt;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = "ffffff"; //ArtManager.ModeDefaultArtTint;
    private bool _rikasTraitOverride = ArtManager.RikasTraitOverride;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        _customArt = ModEntry.RegisterSprite(package, "assets/Card/WhaWhyV2.png").Sprite;
        
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
                .Localize
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        if (_artmode == 0 || _artmode == 1 || _artmode == 2 || _textoverride)
        {
            return upgrade switch
            {
                Upgrade.None =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneNone}",
                        IsWeird = true,
                        WeirdCase = 2
                    }
                ],
                Upgrade.A =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneA}",
                        IsWeird = true,
                        WeirdCase = 3
                    }
                ],
                Upgrade.B =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneB}",
                        IsWeird = true,
                        WeirdCase = 4
                    }
                ],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else if (_artmode == 3 && _textoverride == false)
        {
            return upgrade switch
            {
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else if (_artmode == 4 && _textoverride == false)
        {
            return upgrade switch
            {
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else
        {
            Console.WriteLine($"[RikaMod: Neo{_cardName}.cs] Please tell Havmir that this card is buggy when ArtMode = {_artmode}.");
        }
        return default!;
    }
    
    private string _cardRolled = "null";
    
    public override void OnDraw(State s, Combat c)
    {
        int x = s.rngActions.NextInt();
        x = x % 4;
        x = x + 1;

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (x == 1 && upgrade == Upgrade.None)
            {
                c.Queue(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHeal
                {
                    healAmount = 1,
                    targetPlayer = true
                });
            }

            if (x == 2 && upgrade == Upgrade.None)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
            }

            if (x == 3 && upgrade == Upgrade.None)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = 3
                });
            }

            if (x == 4 && upgrade == Upgrade.None)
            {
                c.Queue(new AMove
                {
                    dir = -2,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
            }

            if (x == 1 && upgrade == Upgrade.A)
            {
                c.Queue(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHeal
                {
                    healAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHeal
                {
                    healAmount = 1,
                    targetPlayer = true
                });
            }

            if (x == 2 && upgrade == Upgrade.A)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
            }

            if (x == 3 && upgrade == Upgrade.A)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = 3
                });
            }

            if (x == 4 && upgrade == Upgrade.A)
            {
                c.Queue(new AMove
                {
                    dir = -4,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
            }

            if (x == 1 && upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = Status.perfectShield,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.perfectShield,
                    statusAmount = -1,
                    targetPlayer = true
                });
            }

            if (x == 2 && upgrade == Upgrade.B)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true,
                    piercing = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true,
                    piercing = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true,
                    piercing = true
                });
            }

            if (x == 3 && upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = 3
                });
                c.Queue(new ADrawCard
                {
                    count = 1
                });
            }

            if (x == 4 && upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = Status.ace,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = -2,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.ace,
                    statusAmount = -1,
                    targetPlayer = true
                });
            }

            if (x == 5)
            {
                Console.WriteLine(
                    "WhaWhy.cs returned a 5 somehow ... probally best to let Havmir know, as this isn't suppose to happen!");
            }

            if (x == 1)
            {
                _cardRolled = "Bruise";
            }
            if (x == 2)
            {
                _cardRolled = "Buckshot";
            }
            if (x == 3)
            {
                _cardRolled = "Lightning in a bottle";
            }
            if (x == 4)
            {
                _cardRolled = "Waltz";
            }
            if (_isplaytester)
            {
                Console.WriteLine($"[RikaMod] Neo Wha ... Why drawn | Upgrade: {upgrade} | Card Rolled: {_cardRolled} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
            }
            if (_logALotOfThings)
            {
                ModEntry.Instance.Logger.LogInformation($"[RikaMod: Neo{_cardName}.cs] Nwo Wha ... Why drawn | Upgrade: {upgrade} | Card Rolled: {_cardRolled} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
            }

        }
    }
    

    
    public override CardData GetData(State state)
    {
        if (_artmode == 4)
        {
            _descriptionNone =
                "On Draw, apply a random <c=tooth>Tooth</c> card effect.";
            _descriptionA =
                "On Draw, apply a random <c=tooth>Tooth</c> A upgraded card effect.";
            _descriptionB =
                "On Draw, apply a random <c=tooth>Tooth</c> B upgraded card effect.";
        }
        else
        {
            _descriptionNone =
                "Apply a random <c=tooth>Tooth</c> card effect.";
            _descriptionA =
                "Apply a random <c=tooth>Tooth</c> A upgraded card effect.";
            _descriptionB =
                "Apply a random <c=tooth>Tooth</c> B upgraded card effect.";
        }
        if (_artmode == 0 || _textoverride)
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