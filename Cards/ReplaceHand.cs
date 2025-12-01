using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class ReplaceHand : Card, IRegisterable
{

    // Thanks to Jaz & Angder for helping me with coding up the once per turn effect.
    
    private int _turncount;
    private int _whenused = 9999;
    private int _whenBused = 9999;
    private int _usedthisturn;
    
    private int _calculation;

    public override void OnExitCombat(State s, Combat c)
    {
        _turncount = 0;
        _whenused = 9999;
        _whenBused = 9999;
        _usedthisturn = 0;
    }
    
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ReplaceHand", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/ReplaceHand.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipADiscard(),
                new ToolTipCompitent
                {
                    _stringString = "action.drawCard",
                    _stringInt = $"{_replaceHandDrawAmountNone}"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipADiscard(),
                new ToolTipCompitent
                {
                    _stringString = "action.drawCard",
                    _stringInt = $"{_replaceHandDrawAmountA}"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipADiscard(),
                new ToolTipCompitent
                {
                    _stringString = "action.drawCard",
                    _stringInt = $"{_replaceHandDrawAmountB}"
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private int _replaceHandDrawAmountNone = 6;
    private int _replaceHandDrawAmountA = 8;
    private int _replaceHandDrawAmountB = 4;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Replace Hand"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);
        
        _turncount = c.turn;

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None && _turncount != _whenused)
            {
                c.Queue(new ADiscard());
                c.Queue(new ADrawCard
                {
                    count = _replaceHandDrawAmountNone
                });
                _whenused = c.turn;
            }

            if (upgrade == Upgrade.A && _turncount != _whenused)
            {
                c.Queue(new ADiscard());
                c.Queue(new ADrawCard
                {
                    count = _replaceHandDrawAmountA
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
                    count = _replaceHandDrawAmountB
                });
                _whenBused = c.turn;
                _usedthisturn++;
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Replace Hand drawn | Upgrade: {upgrade} | Turn: {c.turn} | _turncount = {_turncount} | _usedthisturn = {_usedthisturn} | _whenused = {_whenused} | _whenBused = {_whenBused} | rikaCardsPerTurnNumber = {ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0)} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: ReplaceHand.cs] Replace Hand drawn | Upgrade: {upgrade} | Turn: {c.turn} | _turncount = {_turncount} | _usedthisturn = {_usedthisturn} | _whenused = {_whenused} | _whenBused = {_whenBused} | rikaCardsPerTurnNumber = {ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0)} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }

    }

    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (_artmode == 1)
        { 
            if (upgrade == Upgrade.None && _turncount != _whenused)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw & once per turn, discard your hand and draw {_replaceHandDrawAmountNone}. <c=heal>(not drawn)</c>",
                artTint = "ffffff"
            };
        }
        if (upgrade == Upgrade.None && _turncount == _whenused)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw & once per turn, discard your hand and draw {_replaceHandDrawAmountNone}. <c=damage>(drawn)</c>",
                artTint = "ffffff",
                art = ReplaceHandDrawn
            };
        }
        if (upgrade == Upgrade.A && _turncount != _whenused)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw & once per turn, discard your hand and draw {_replaceHandDrawAmountA}. <c=heal>(not drawn)</c>",
                artTint = "ffffff",
                art = ReplaceHandA
            };
        }
        if (upgrade == Upgrade.A && _turncount == _whenused)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw & once per turn, discard your hand and draw {_replaceHandDrawAmountA}. <c=damage>(drawn)</c>",
                artTint = "ffffff",
                art = ReplaceHandDrawn
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 0)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB0
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 1)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB1
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 2)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB2
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 3)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB3
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 4)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB4
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 5)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB5
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 6)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB6
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 7)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB7
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 8)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB8
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 9)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB9
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 10)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB10
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 11)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB11
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 12)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB12
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 13)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB13
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 14)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB14
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 15)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB15
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 16)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB16
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 17)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB17
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 18)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB18
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 19)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                artTint = "ffffff",
                art = ReplaceHandB19
            };
        }
        if (upgrade == Upgrade.B && _usedthisturn == 20)
        {
            return new CardData
            {
                cost = 0,
                description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=damage>{_usedthisturn}/20</c>)",
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
                    cost = 0,
                    description = $"On draw & once per turn, discard your hand and draw {_replaceHandDrawAmountNone}. <c=heal>(not drawn)</c>",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ThinkTwice
                };
            }
            if (upgrade == Upgrade.None && _turncount == _whenused)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw & once per turn, discard your hand and draw {_replaceHandDrawAmountNone}. <c=damage>(drawn)</c>",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ThinkTwice
                };
            }
            if (upgrade == Upgrade.A && _turncount != _whenused)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw & once per turn, discard your hand and draw {_replaceHandDrawAmountA}. <c=heal>(not drawn)</c>",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ThinkTwice
                };
            }
            if (upgrade == Upgrade.A && _turncount == _whenused)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw & once per turn, discard your hand and draw {_replaceHandDrawAmountA}. <c=damage>(drawn)</c>",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ThinkTwice
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn <= 19)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=heal>{_usedthisturn}/20</c>)",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ThinkTwice
                };
            }
            if (upgrade == Upgrade.B && _usedthisturn == 20)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, discard your hand and draw {_replaceHandDrawAmountB}. (Max 20 times a turn; <c=damage>{_usedthisturn}/20</c>)",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_ThinkTwice
                };
            }
        }
        return default;
    }
};