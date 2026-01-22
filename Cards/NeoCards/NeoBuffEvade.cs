using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

public class NeoEvadeBooster : Card, IRegisterable, IHasCustomCardTraits
{
    public static Spr TempPlaceHolderArt;
    
    private static string _cardName = "EvadeBooster";
    // Make sure to update rarirty
    private int _cardcostNone = 1;
    private int _cardcostA = 1;
    private int _cardcostB = 1;
    private string _actionOneNone = "status.evade";
    private string _actionOneA = "status.evade";
    private string _actionOneB = "status.evade";
    private int _actionOneAmountCapNone = 4;
    private int _actionOneAmountCapA = 6;
    private int _actionOneAmountCapB = 4;
    private string _descriptionNone = "_descriptionNone = null";
    private string _descriptionA = "_descriptionA = null";
    private string _descriptionB = "_descriptionB = null";
    private bool _textOverride = true;
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private bool _isRetainOnTheNoneUpgrade = false;
    private bool _isRecycleOnTheNoneUpgrade = false;
    private bool _isRetainOnTheAUpgrade = false;
    private bool _isRecycleOnTheAUpgrade = false;
    private bool _isRetainOnTheBUpgrade = true;
    private bool _isRecycleOnTheBUpgrade = true;
    private bool _isUnplayableOnTheBUpgrade = false;
    private Spr? _artNone = StableSpr.cards_SeekerMissileCard;
    private Spr? _artA =  StableSpr.cards_SeekerMissileCard;
    private Spr? _artB =  StableSpr.cards_SeekerMissileCard;
    private Spr _artAlphaNone = TempPlaceHolderArt;
    private Spr _artAlphaA = TempPlaceHolderArt;
    private Spr _artAlphaB = TempPlaceHolderArt;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    private bool _rikasTraitOverride = ArtManager.RikasTraitOverride;

    public int _timesplayedThisCombat = 0;
    private bool _doesHavmirNeedFlipableforTesting = false;
    
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
    
    public override void AfterWasPlayed(State state, Combat c)
    {
        ++_timesplayedThisCombat;
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
                        _stringInt = $"{_timesplayedThisCombat}"
                    }
                ],
                Upgrade.A =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneA}",
                        _stringInt = $"{_timesplayedThisCombat}"
                    }
                ],
                Upgrade.B =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = $"{_actionOneB}",
                        _stringInt = $"{_timesplayedThisCombat}"
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
                        _statusAmount = _timesplayedThisCombat
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconAStatus
                    {
                        _status = _actionOneA,
                        _statusAmount = _timesplayedThisCombat
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconAStatus
                    {
                        _status = _actionOneB,
                        _statusAmount = _timesplayedThisCombat
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
                        _statusAmount = _timesplayedThisCombat
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
                        _statusAmount = _timesplayedThisCombat
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
                        _statusAmount = _timesplayedThisCombat
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
    
    public override void OnExitCombat(State s, Combat c)
    {
        _timesplayedThisCombat = 0;
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Neo{_cardName}.cs] Neo{_cardName} OnExitCombat | Upgrade: {upgrade} | _playedThisCombat = {_timesplayedThisCombat} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn} | uuid = {uuid}");
        }
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
                    status = Status.evade,
                    statusAmount = Math.Min(_timesplayedThisCombat, _actionOneAmountCapNone),
                    targetPlayer = true
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
                    status = Status.evade,
                    statusAmount = Math.Min(_timesplayedThisCombat, _actionOneAmountCapA),
                    targetPlayer = true
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
                    status = Status.evade,
                    statusAmount = Math.Min(_timesplayedThisCombat, _actionOneAmountCapB),
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Neo{_cardName} drawn | Upgrade: {upgrade} | # of times played this combat = {_timesplayedThisCombat} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | uuid = {uuid}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Neo{_cardName}.cs] Neo{_cardName} drawn | Upgrade: {upgrade} | _playedThisCombat = {_timesplayedThisCombat} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn} | uuid = {uuid}");
        }
    }
    

    
    public override CardData GetData(State state)
    {
        _descriptionNone =
            $"Get {Math.Min(_timesplayedThisCombat, _actionOneAmountCapNone)} <c=status>evade</c>; Each time this card is played this fight, add 1 (max {_actionOneAmountCapNone}).";
        _descriptionA =
            $"Get {Math.Min(_timesplayedThisCombat, _actionOneAmountCapA)} <c=status>evade</c>; Each time this card is played this fight, add 1 (max {_actionOneAmountCapA}).";
        _descriptionB =
            $"Get {Math.Min(_timesplayedThisCombat, _actionOneAmountCapB)} <c=status>evade</c>; Each time this card is played this fight, add 1 (max {_actionOneAmountCapB}).";
        if (_artmode == 0 || _textOverride)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNone,
                    artTint = _artTintDefault,
                    art = _artNone,
                    retain = _isRetainOnTheNoneUpgrade,
                    recycle = _isRecycleOnTheNoneUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheAUpgrade,
                    recycle = _isRecycleOnTheAUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheBUpgrade,
                    recycle = _isRecycleOnTheBUpgrade,
                    unplayable = _isUnplayableOnTheBUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheNoneUpgrade,
                    recycle = _isRecycleOnTheNoneUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheAUpgrade,
                    recycle = _isRecycleOnTheAUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheBUpgrade,
                    recycle = _isRecycleOnTheBUpgrade,
                    unplayable = _isUnplayableOnTheBUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheNoneUpgrade,
                    recycle = _isRecycleOnTheNoneUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = "ffffff",
                    art = _artAlphaA,
                    retain = _isRetainOnTheAUpgrade,
                    recycle = _isRecycleOnTheAUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = "ffffff",
                    art = _artAlphaB,
                    retain = _isRetainOnTheBUpgrade,
                    recycle = _isRecycleOnTheBUpgrade,
                    unplayable = _isUnplayableOnTheBUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheNoneUpgrade,
                    recycle = _isRecycleOnTheNoneUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA,
                    retain = _isRetainOnTheAUpgrade,
                    recycle = _isRecycleOnTheAUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artB,
                    retain = _isRetainOnTheBUpgrade,
                    recycle = _isRecycleOnTheBUpgrade,
                    unplayable = _isUnplayableOnTheBUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheNoneUpgrade,
                    recycle = _isRecycleOnTheNoneUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artA,
                    retain = _isRetainOnTheAUpgrade,
                    recycle = _isRecycleOnTheAUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artB,
                    retain = _isRetainOnTheBUpgrade,
                    recycle = _isRecycleOnTheBUpgrade,
                    unplayable = _isUnplayableOnTheBUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheNoneUpgrade,
                    recycle = _isRecycleOnTheNoneUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheAUpgrade,
                    recycle = _isRecycleOnTheAUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
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
                    retain = _isRetainOnTheBUpgrade,
                    recycle = _isRecycleOnTheBUpgrade,
                    unplayable = _isUnplayableOnTheBUpgrade,
                    flippable = _doesHavmirNeedFlipableforTesting
                };
            }
        }
        return default;
    }

    public override void OnFlip(G g)
    {
        // I was unsure on if what I was doing would work, so I added this for testing. ~ Havmir 18/01/2026
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] NeoBuffEvadeJankFix Flipped | Card Name {_cardName} | # of times played this combat = {_timesplayedThisCombat} | uuid = {uuid}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: NeoBuffEvadeJankFix OnFlip Call | Card Name {_cardName} | # of times played this combat = {_timesplayedThisCombat} | uuid = {uuid}");
        }
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