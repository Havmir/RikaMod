using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoAggressiveGamble : Card, IRegisterable, IHasCustomCardTraits
{
    public static Spr TempPlaceHolderArt;
    
    private static string _cardName = "AggressiveGamble";
    private int _cardcostNone = 1;
    private int _cardcostA = 0;
    private int _cardcostB = 1;
    private string _actionOneNone = "trashAutoShoot";
    private string _actionOneA = "trashAutoShoot";
    private string _actionOneB = "trashAutoShoot";
    private string _cardAddedNone = "Safety Override";
    private string _cardAddedA = "Safety Override";
    private string _cardAddedB = "Safety Override";
    private int _actionOneAmountNone = 1;
    private int _actionOneAmountA = 1;
    private int _actionOneAmountB = 2;
    private string _descriptionNone = "_descriptionNone = null";
    private string _descriptionA = "_descriptionA = null";
    private string _descriptionB = "_descriptionB = null";
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private Spr? _artNone = StableSpr.cards_WaveBeam;
    private Spr? _artA =  StableSpr.cards_WaveBeam;
    private Spr? _artB =  StableSpr.cards_WaveBeam;
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
                        _stringString = $"{_actionOneNone}"
                    }
                ],
                Upgrade.A =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneA}"
                    }
                ],
                Upgrade.B =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneB}"
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
                    new FalseIconAAddCard
                    {
                        _card = _cardAddedNone,
                        _cardAmount = _actionOneAmountNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconAAddCard
                    {
                        _card = _cardAddedA,
                        _cardAmount = _actionOneAmountA
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconAAddCard
                    {
                        _card = _cardAddedB,
                        _cardAmount = _actionOneAmountB
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
                    new FalseIconAAddCard
                    {
                        _card = _cardAddedNone
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostA
                    },
                    new FalseIconAAddCard
                    {
                        _card = _cardAddedA
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostB
                    },
                    new FalseIconAAddCard
                    {
                        _card = _cardAddedB
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
                c.Queue(new AAddCard
                {
                    card = new TrashAutoShoot(),
                    destination = CardDestination.Deck,
                    amount = _actionOneAmountNone
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostA
                });
                c.Queue(new AAddCard
                {
                    card = new TrashAutoShoot(),
                    destination = CardDestination.Deck,
                    amount = _actionOneAmountA
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = _cardcostB
                });
                c.Queue(new AAddCard
                {
                    card = new TrashAutoShoot(),
                    destination = CardDestination.Deck,
                    amount = _actionOneAmountB
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Fume Shot drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Fumeshot.cs] Spare Shot drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }

    }
    

    
    public override CardData GetData(State state)
    {
        _descriptionNone =
            $"On draw, <c=downside>-{_cardcostNone} energy</c>, but add {_actionOneAmountNone} <c=card>{_cardAddedNone}</c> to your deck.";
        _descriptionA =
            $"On draw, add {_actionOneAmountA} <c=card>{_cardAddedA}</c> to your deck.";
        _descriptionB =
            $"On draw, <c=downside>-{_cardcostB} energy</c>, but add {_actionOneAmountB} <c=card>{_cardAddedB}</c> to your deck.";
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
                    retain = true,
                    recycle = true
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
                    retain = true,
                    recycle = true
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
                    recycle = true
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
                    retain = true,
                    recycle = true
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
                    retain = true,
                    recycle = true
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
                    recycle = true
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
                    retain = true,
                    recycle = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = "ffffff",
                    art = _artAlphaA,
                    retain = true,
                    recycle = true
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
                    recycle = true
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
                    retain = true,
                    recycle = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA,
                    retain = true,
                    recycle = true
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
                    recycle = true
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
                    retain = true,
                    recycle = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA,
                    retain = true,
                    recycle = true
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
                    recycle = true
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
                    retain = true,
                    recycle = true
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
                    retain = true,
                    recycle = true
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
                    recycle = true
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