using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoReplaceHand : Card, IRegisterable, IHasCustomCardTraits
{
    private static string _cardName = "ReplaceHand";
    private int _cardcostNone = 0;
    private int _cardcostA = 0;
    private int _cardcostB = 0;
    private string _actionOneNone = "action.discardHand";
    private string _actionOneA = "action.discardHand";
    private string _actionOneB = "action.discardHand";
    private int _actionOneAmountNone = 10;
    private int _actionOneAmountA = 10;
    private int _actionOneAmountB = 10;
    private string _actionTwoNone = "action.drawCard";
    private string _actionTwoA = "action.drawCard";
    private string _actionTwoB = "action.drawCard";
    private int _actionTwoAmountNone = 4;
    private int _actionTwoAmountA = 6;
    private int _actionTwoAmountB = 2;
    private string _descriptionNone = "_descriptionNone = null";
    private string _descriptionA = "_descriptionA = null";
    private string _descriptionB = "_descriptionB = null";
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private Spr? _artNone = StableSpr.cards_ThinkTwice;
    private Spr? _artA =  StableSpr.cards_ThinkTwice;
    private Spr? _artB =  StableSpr.cards_ThinkTwice;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    private bool _rikasTraitOverride = ArtManager.RikasTraitOverride;
    
    private int _turncount;
    private int _whenused = 9999;
    private int _whenBused = 9999;
    private int _usedthisturn;

    public static Spr BaseArt;
    public static Spr ReplaceHandDrawn;
    public static Spr ReplaceHandA;
    public static Spr ReplaceHandB0;
    public static Spr ReplaceHandB1;
    public static Spr ReplaceHandB2;
    public static Spr ReplaceHandB3;
    public static Spr ReplaceHandB4;
    public static Spr ReplaceHandB5;
    public static Spr ReplaceHandB6;
    public static Spr ReplaceHandB7;
    public static Spr ReplaceHandB8;
    public static Spr ReplaceHandB9;
    public static Spr ReplaceHandB10;
    public static Spr ReplaceHandB11;
    public static Spr ReplaceHandB12;
    public static Spr ReplaceHandB13;
    public static Spr ReplaceHandB14;
    public static Spr ReplaceHandB15;
    public static Spr ReplaceHandB16;
    public static Spr ReplaceHandB17;
    public static Spr ReplaceHandB18;
    public static Spr ReplaceHandB19;
    public static Spr ReplaceHandB20;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        BaseArt = ModEntry.RegisterSprite(package, "assets/Card/ReplaceHand.png").Sprite;
        ReplaceHandDrawn = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandDrawn.png").Sprite;
        ReplaceHandA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandA.png").Sprite;
        ReplaceHandB0 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB0.png").Sprite;
        ReplaceHandB1 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB1.png").Sprite;
        ReplaceHandB2 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB2.png").Sprite;
        ReplaceHandB3 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB3.png").Sprite;
        ReplaceHandB4 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB4.png").Sprite;
        ReplaceHandB5 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB5.png").Sprite;
        ReplaceHandB6 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB6.png").Sprite;
        ReplaceHandB7 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB7.png").Sprite;
        ReplaceHandB8 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB8.png").Sprite;
        ReplaceHandB9 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB9.png").Sprite;
        ReplaceHandB10 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB10.png").Sprite;
        ReplaceHandB11 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB11.png").Sprite;
        ReplaceHandB12 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB12.png").Sprite;
        ReplaceHandB13 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB13.png").Sprite;
        ReplaceHandB14 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB14.png").Sprite;
        ReplaceHandB15 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB15.png").Sprite;
        ReplaceHandB16 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB16.png").Sprite;
        ReplaceHandB17 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB17.png").Sprite;
        ReplaceHandB18 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB18.png").Sprite;
        ReplaceHandB19 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB19.png").Sprite;
        ReplaceHandB20 = ModEntry.RegisterSprite(package, "assets/Alpha/Card/ReplaceHandB/ReplaceHandB20.png").Sprite;
        
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
                        _stringString = _actionOneNone,
                        _stringInt = $"{_actionOneAmountNone}"
                    },
                    new ToolTipCompitent
                    {
                        _stringString = _actionTwoNone,
                        _stringInt = $"{_actionTwoAmountNone}"
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
                    new FalseIconADiscard
                    {
                        _discardHand = true
                    },
                    new FalseIconADrawCard
                    {
                        _drawNumber = _actionTwoAmountNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconADiscard
                    {
                        _discardHand = true
                    },
                    new FalseIconADrawCard
                    {
                        _drawNumber = _actionTwoAmountA
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconADiscard
                    {
                        _discardHand = true
                    },
                    new FalseIconADrawCard
                    {
                        _drawNumber = _actionTwoAmountB
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
                    new FalseIconADiscard
                    {
                        _discardHand = true
                    },
                    new FalseIconADrawCard
                    {
                        _drawNumber = _actionTwoAmountNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostA
                    },
                    new FalseIconADiscard
                    {
                        _discardHand = true
                    },
                    new FalseIconADrawCard
                    {
                        _drawNumber = _actionTwoAmountA
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostB
                    },
                    new FalseIconADiscard
                    {
                        _discardHand = true
                    },
                    new FalseIconADrawCard
                    {
                        _drawNumber = _actionTwoAmountB
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
        _turncount = c.turn;

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None && _turncount != _whenused)
            {
                c.Queue(new ADiscard());
                c.Queue(new ADrawCard
                {
                    count = _actionTwoAmountNone
                });
                _whenused = c.turn;
            }

            if (upgrade == Upgrade.A && _turncount != _whenused)
            {
                c.Queue(new ADiscard());
                c.Queue(new ADrawCard
                {
                    count = _actionTwoAmountA
                });
                _whenused = c.turn;
            }

            if (_turncount > _whenBused)
            {
                _usedthisturn = 0;
            }

            if (upgrade == Upgrade.B && _usedthisturn <= 19)
            {
                c.Queue(new ADiscard());
                c.Queue(new ADrawCard
                {
                    count = _actionTwoAmountB
                });
                _whenBused = c.turn;
                _usedthisturn++;
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Neo{_cardName} drawn | Upgrade: {upgrade} | Turn: {c.turn} | _turncount = {_turncount} | _usedthisturn = {_usedthisturn} | _whenused = {_whenused} | _whenBused = {_whenBused} | rikaCardsPerTurnNumber = {ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0)} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Neo{_cardName}.cs] Neo{_cardName} drawn | Upgrade: {upgrade} | Turn: {c.turn} | _turncount = {_turncount} | _usedthisturn = {_usedthisturn} | _whenused = {_whenused} | _whenBused = {_whenBused} | rikaCardsPerTurnNumber = {ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0)} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
    }
    

    
    public override CardData GetData(State state)
    {
        if (_artmode == 0 || _artmode == 1 || _artmode == 3 || _artmode == 4)
        {
            if (_turncount != _whenused)
            {
                _descriptionNone = $"Once per turn, discard your hand and draw {_actionTwoAmountNone}. <c=heal>(not drawn)</c>";
                _descriptionA = $"Once per turn, discard your hand and draw {_actionTwoAmountA}. <c=heal>(not drawn)</c>";
            }
            if (_turncount == _whenused)
            {
                _descriptionNone = $"Once per turn, discard your hand and draw {_actionTwoAmountNone}. <c=damage>(drawn)</c>";
                _descriptionA = $"Once per turn, discard your hand and draw {_actionTwoAmountA}. <c=damage>(drawn)</c>";
            }
            if (_usedthisturn <= 19)
            {
                _descriptionB = $"Discard your hand and draw {_actionTwoAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)";
            }
            if (_usedthisturn == 20)
            {
                _descriptionB = $"Discard your hand and draw {_actionTwoAmountB}. (Max 20 times a turn; <c=damage>{_usedthisturn}/20</c>)";
            }
        }
        if (_artmode == 4)
        {
            if (_turncount != _whenused)
            {
                _descriptionNone = $"On draw & once per turn, discard your hand and draw {_actionTwoAmountNone}. <c=heal>(not drawn)</c>";
                _descriptionA = $"On draw & once per turn, discard your hand and draw {_actionTwoAmountA}. <c=heal>(not drawn)</c>";
            }
            if (_turncount == _whenused)
            {
                _descriptionNone = $"On draw & once per turn, discard your hand and draw {_actionTwoAmountNone}. <c=damage>(drawn)</c>";
                _descriptionA = $"On draw & once per turn, discard your hand and draw {_actionTwoAmountA}. <c=damage>(drawn)</c>";
            }
            if (_usedthisturn <= 19)
            {
                _descriptionB = $"On Draw, discard your hand and draw {_actionTwoAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)";
            }
            if (_usedthisturn == 20)
            {
                _descriptionB = $"On draw, discard your hand and draw {_actionTwoAmountB}. (Max 20 times a turn; <c=damage>{_usedthisturn}/20</c>)";
            }
        }
        if (_artmode == 2)
        {
            _descriptionNone = "";
            _descriptionA = "";
            _descriptionB = "";
        }
        
        if (_artmode == 0 || _artmode == 3 || _artmode == 4)
        {
            if (upgrade == Upgrade.None && _turncount != _whenused)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = _artTintDefault,
                    art = _artNone
                };
            }
            if (upgrade == Upgrade.None && _turncount == _whenused)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = _artTintDefault,
                    art = _artNone
                };
            }
            if (upgrade == Upgrade.A && _turncount != _whenused)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = _artTintDefault,
                    art = _artA
                };
            }
            if (upgrade == Upgrade.A && _turncount == _whenused)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = _artTintDefault,
                    art = _artA
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn <= 19)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = _artTintDefault,
                    art = _artB
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 20)
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
        else if (_artmode == 1 || _artmode == 2)
        {
            if (upgrade == Upgrade.None && _turncount != _whenused)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = "ffffff",
                    art = BaseArt
                };
            }
            if (upgrade == Upgrade.None && _turncount == _whenused)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = "ffffff",
                    art = ReplaceHandDrawn
                };
            }
            if (upgrade == Upgrade.A && _turncount != _whenused)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = "ffffff",
                    art = ReplaceHandA
                };
            }
            if (upgrade == Upgrade.A && _turncount == _whenused)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = "ffffff",
                    art = ReplaceHandDrawn
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 0)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB0
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 1)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB1
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 2)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB2
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 3)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB3
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 4)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB4
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 5)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB5
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 6)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB6
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 7)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB7
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 8)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB8
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 9)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB9
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 10)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB10
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 11)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB11
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 12)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB12
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 13)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB13
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 14)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB14
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 15)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB15
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 16)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB16
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 17)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB17
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 18)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB18
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 19)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB19
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 20)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = "ffffff",
                    art = ReplaceHandB20
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.None && _turncount != _whenused)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = _artTintDefault,
                    art = _artNone
                };
            }
            if (upgrade == Upgrade.None && _turncount == _whenused)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = _artTintDefault,
                    art = _artNone
                };
            }
            if (upgrade == Upgrade.A && _turncount != _whenused)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = _artTintDefault,
                    art = _artA
                };
            }
            if (upgrade == Upgrade.A && _turncount == _whenused)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionA,
                    artTint = _artTintDefault,
                    art = _artA
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn <= 19)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionB,
                    artTint = _artTintDefault,
                    art = _artB
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 20)
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
    
    public override void OnExitCombat(State s, Combat c)
    {
        _turncount = 0;
        _whenused = 9999;
        _whenBused = 9999;
        _usedthisturn = 0;
    }
};