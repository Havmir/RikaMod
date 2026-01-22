using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards.NeoCards;

// I'd like to thank Shockah & rft50 for helping me make the Harmony Patch to allow for an interaction where you can preserve the flipped state of Rika Cards :D ~ Havmir 18/01/2026eds

public class NeoAdjustGameplan : Card, IRegisterable, IHasCustomCardTraits
{
    public static Spr TempPlaceHolderArt;
    
    private static string _cardName = "AdjustGameplan";
    private int _cardcostNone = 0;
    private int _cardcostA = 0;
    private int _cardcostB = 0;
    private int _attackDamageNone = 1;
    private int _attackDamageA = 1;
    private int _attackDamageB = 3;
    private string _statusOneNone = "status.shield";
    private string _statusOneA = "status.shield";
    private string _statusOneB = "RikaFlux";
    private string _statusTwoNone = "status.tempShield";
    private string _statusTwoA = "status.tempShield";
    private string _statusTwoB = "status.libra";
    //Remember to change the OnDraw manually ~ Havmir 31/12/2025
    private int _shieldNone = 1;
    private int _shieldA = 2;
    private int _shieldFlowB = 1;
    private int _tempShieldNone = 1;
    private int _tempShieldA = 2;
    private int _fluxB = 1;
    private string _descriptionNotFlippedNone = "_descriptionNotFlippedNone = null";
    private string _descriptionFlippedNone = "_descriptionFlippedNone = null";
    private string _descriptionNotFlippedA = "_descriptionNotFlippedA = null";
    private string _descriptionFlippedA = "_descriptionFlippedA = null";
    private string _descriptionNotFlippedB = "_descriptionNotFlippedB = null";
    private string _descriptionFlippedB = "_descriptionFlippedB = null";
    private bool _haveRikasTraitNone = true;
    private bool _haveRikasTraitA = true;
    private bool _haveRikasTraitB = true;
    private Spr? _artNotFlippedNone = StableSpr.cards_Cannon;
    private Spr? _artFlippedNone = StableSpr.cards_Shield;
    private Spr? _artNotFlippedA =  StableSpr.cards_Cannon;
    private Spr? _artFlippedA =  StableSpr.cards_Shield;
    private Spr? _artNotFlippedB =  StableSpr.cards_Cannon;
    private Spr? _artFlippedB =  StableSpr.cards_Shield;
    private Spr _artNotFlippedAlphaNone = TempPlaceHolderArt;
    private Spr _artFlippedAlphaNone = TempPlaceHolderArt;
    private Spr _artNotFlippedAlphaA = TempPlaceHolderArt;
    private Spr _artFlippedAlphaA = TempPlaceHolderArt;
    private Spr _artNotFlippedAlphaB = TempPlaceHolderArt;
    private Spr _artFlippedAlphaB = TempPlaceHolderArt;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    private bool _rikasTraitOverride = ArtManager.RikasTraitOverride;

    private bool _flipMemory = false;
    
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
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.ReturnCardsToDeck)),
            prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(CardRewardEdgeCaseFlip))
        );
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
                        _stringString = "action.attack.name",
                        _stringInt = $"{_attackDamageNone}",
                        disabled = flipped
                    },
                    new ToolTipCompitent
                    {
                        _stringString = $"{_statusOneNone}",
                        _stringInt = $"{_shieldNone}",
                        disabled = !flipped
                    },
                    new ToolTipCompitent
                    {
                        _stringString = $"{_statusTwoNone}",
                        _stringInt = $"{_tempShieldNone}",
                        disabled = !flipped
                    }
                ],
                Upgrade.A =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = "action.attack.name",
                        _stringInt = $"{_attackDamageA}",
                        disabled = flipped
                    },
                    new ToolTipCompitent
                    {
                        _stringString = $"{_statusOneA}",
                        _stringInt = $"{_shieldA}",
                        disabled = !flipped
                    },
                    new ToolTipCompitent
                    {
                        _stringString = $"{_statusTwoA}",
                        _stringInt = $"{_tempShieldA}",
                        disabled = !flipped
                    }
                ],
                Upgrade.B =>
                [
                    new ToolTipCompitent
                    {
                        _stringString = "action.attackPiercing",
                        _stringInt = $"{_attackDamageB}",
                        disabled = flipped
                    },
                    new ToolTipCompitent
                    {
                        _stringString = $"{_statusOneB}",
                        _stringInt = $"{_shieldFlowB}",
                        disabled = !flipped
                    },
                    new ToolTipCompitent
                    {
                        _stringString = $"{_statusTwoB}",
                        _stringInt = $"{_fluxB}",
                        disabled = !flipped
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
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageNone),
                        disabled = flipped
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageNone),
                        disabled = flipped
                    },
                    new ADummyAction(),
                    new FalseIconAStatus
                    {
                        _status = _statusOneNone,
                        _statusAmount = _shieldNone,
                        disabled = !flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusTwoNone,
                        _statusAmount = _tempShieldNone,
                        disabled = !flipped
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageA),
                        disabled = flipped
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageA),
                        disabled = flipped
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageA),
                        disabled = flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusOneA,
                        _statusAmount = _shieldA,
                        disabled = !flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusTwoA,
                        _statusAmount = _tempShieldA,
                        disabled = !flipped
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconAAtack
                    {
                        _isPeirce = true,
                        _damageNumber = GetDmg(s, _attackDamageB),
                        disabled = flipped
                    },
                    new ADummyAction(),
                    new FalseIconAStatus
                    {
                        _status = _statusOneB,
                        _statusAmount = _shieldFlowB,
                        disabled = !flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusTwoB,
                        _statusAmount = _fluxB,
                        disabled = !flipped
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
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageNone),
                        disabled = flipped
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageNone),
                        disabled = flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusOneNone,
                        _statusAmount = _shieldNone,
                        disabled = !flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusTwoNone,
                        _statusAmount = _tempShieldNone,
                        disabled = !flipped
                    }
                ],
                Upgrade.A =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostA
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageA),
                        disabled = flipped
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageA),
                        disabled = flipped
                    },
                    new FalseIconAAtack
                    {
                        _damageNumber = GetDmg(s, _attackDamageA),
                        disabled = flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusOneA,
                        _statusAmount = _shieldA,
                        disabled = !flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusTwoA,
                        _statusAmount = _tempShieldA,
                        disabled = !flipped
                    }
                ],
                Upgrade.B =>
                [
                    new FalseIconRikaCard
                    {
                        cardCost = _cardcostB
                    },
                    new FalseIconAAtack
                    {
                        _isPeirce = true,
                        _damageNumber = GetDmg(s, _attackDamageB),
                        disabled = flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusOneB,
                        _statusAmount = _shieldFlowB,
                        disabled = !flipped
                    },
                    new FalseIconAStatus
                    {
                        _status = _statusTwoB,
                        _statusAmount = _fluxB,
                        disabled = !flipped
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
        _flipMemory = flipped;
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, _attackDamageNone),
                    fast = true,
                    disabled = flipped
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, _attackDamageNone),
                    fast = true,
                    disabled = flipped
                });
                c.Queue(new AStatus
                {
                    status = Status.shield,
                    statusAmount = _shieldNone,
                    targetPlayer = true,
                    disabled = !flipped
                });
                c.Queue(new AStatus
                {
                    status = Status.tempShield,
                    statusAmount = _tempShieldNone,
                    targetPlayer = true,
                    disabled = !flipped
                });
            }
            if (upgrade == Upgrade.A)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true,
                    disabled = flipped
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true,
                    disabled = flipped
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true,
                    disabled = flipped
                });
                c.Queue(new AStatus
                {
                    status = Status.shield,
                    statusAmount = 2,
                    targetPlayer = true,
                    disabled = !flipped
                });
                c.Queue(new AStatus
                {
                    status = Status.tempShield,
                    statusAmount = 2,
                    targetPlayer = true,
                    disabled = !flipped
                });
            }
            if (upgrade == Upgrade.B)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 3),
                    fast = true,
                    disabled = flipped,
                    piercing = true
                });
                c.Queue(new AStatus
                {
                    status = RikaFluxManager.RikaFlux.Status,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = !flipped
                });
                c.Queue(new AStatus
                {
                    status = Status.libra,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = !flipped
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] NeoAdjustGameplan drawn | flipped: {flipped} | | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: NeoAdjustGameplan.cs] AdjustGameplan drawn | flipped: {flipped} | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn} | ");
        }
    }
    

    
    public override CardData GetData(State state)
    {
        _flipMemory = flipped;
        _descriptionNotFlippedNone =
            $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg twice OR gain {_shieldNone} <c=status>shield</c> & {_tempShieldNone} <c=status>temp shield</c>.";
        _descriptionFlippedNone = _descriptionNotFlippedNone;
        _descriptionNotFlippedA =
            $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg thrice OR gain {_shieldA} <c=status>shield</c> & {_tempShieldA} <c=status>temp shield</c>.";
        _descriptionFlippedA = _descriptionNotFlippedA;
        _descriptionNotFlippedB =
            $"On draw, attack for <c=redd>{GetDmg(state, 3)}</c> <c=keyword>piercing</c> dmg OR gain {_shieldFlowB} <c=status>shield flow</c> & {_fluxB} <c=status>flux</c>.";
        _descriptionFlippedB = _descriptionNotFlippedB;
        if (_artmode == 0)
        {
            if (upgrade == Upgrade.None && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNotFlippedNone,
                    artTint = _artTintDefault,
                    art = _artNotFlippedNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.None && flipped)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionFlippedNone,
                    artTint = _artTintDefault,
                    art = _artFlippedNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionNotFlippedA,
                    artTint = _artTintDefault,
                    art = _artNotFlippedA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionNotFlippedA,
                    artTint = _artTintDefault,
                    art = _artFlippedA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionNotFlippedB,
                    artTint = _artTintDefault,
                    art = _artNotFlippedB,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionNotFlippedB,
                    artTint = _artTintDefault,
                    art = _artFlippedB,
                    floppable = true,
                    unplayable = true
                };
            }
        }
        else if (_artmode == 1)
        {
            if (upgrade == Upgrade.None && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNotFlippedNone,
                    artTint = "ffffff",
                    art = _artNotFlippedAlphaNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.None && flipped)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionFlippedNone,
                    artTint = "ffffff",
                    art = _artFlippedAlphaNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionNotFlippedA,
                    artTint = "ffffff",
                    art = _artNotFlippedAlphaA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionFlippedA,
                    artTint = "ffffff",
                    art = _artFlippedAlphaA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionNotFlippedB,
                    artTint = "ffffff",
                    art = _artNotFlippedAlphaB,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionFlippedB,
                    artTint = "ffffff",
                    art = _artFlippedAlphaB,
                    floppable = true,
                    unplayable = true
                };
            }
        }
        else if (_artmode == 2)
        {
            if (upgrade == Upgrade.None && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    artTint = "ffffff",
                    art = _artNotFlippedAlphaNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.None && flipped)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    artTint = "ffffff",
                    art = _artFlippedAlphaNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = "ffffff",
                    art = _artNotFlippedAlphaA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = "ffffff",
                    art = _artFlippedAlphaA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = "ffffff",
                    art = _artNotFlippedAlphaB,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = "ffffff",
                    art = _artFlippedAlphaB,
                    floppable = true,
                    unplayable = true
                };
            }
        }
        else if (_artmode == 3 ||  _artmode == 4)
        {
            if (upgrade == Upgrade.None && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    artTint = _artTintDefault,
                    art = _artNotFlippedNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.None && flipped)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    artTint = _artTintDefault,
                    art = _artFlippedNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artNotFlippedA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    artTint = _artTintDefault,
                    art = _artFlippedA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artNotFlippedB,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    artTint = _artTintDefault,
                    art = _artFlippedB,
                    floppable = true,
                    unplayable = true
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.None && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionNotFlippedNone,
                    artTint = _artTintDefault,
                    art = _artNotFlippedNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.None && flipped)
            {
                return new CardData
                {
                    cost = _cardcostNone,
                    description = _descriptionFlippedNone,
                    artTint = _artTintDefault,
                    art = _artFlippedNone,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionNotFlippedA,
                    artTint = _artTintDefault,
                    art = _artNotFlippedA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.A && flipped)
            {
                return new CardData
                {
                    cost = _cardcostA,
                    description = _descriptionNotFlippedA,
                    artTint = _artTintDefault,
                    art = _artFlippedA,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == false)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionNotFlippedB,
                    artTint = _artTintDefault,
                    art = _artNotFlippedB,
                    floppable = true,
                    unplayable = true
                };
            }
            if (upgrade == Upgrade.B && flipped)
            {
                return new CardData
                {
                    cost = _cardcostB,
                    description = _descriptionNotFlippedB,
                    artTint = _artTintDefault,
                    art = _artFlippedB,
                    floppable = true,
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
    
    public static void CardRewardEdgeCaseFlip(State state, Combat __instance)
    {
        foreach (var card in state.deck.Concat(__instance.hand).Concat(__instance.discard).Concat(__instance.exhausted))
        {
            if (card is NeoAdjustGameplan specificCard)
            {
                specificCard._flipMemory = specificCard.flipped;
            }
        }
    }
    
    public override void OnFlip(G g)
    {
        _flipMemory = flipped;
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod Neo{_cardName}.cs] OnFlip | _flipMemory = {_flipMemory} | uuid = {uuid} | flipped = {flipped} | upgrade = {upgrade}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Neo{_cardName}.cs] OnFlip | _flipMemory = {_flipMemory} | uuid = {uuid} | flipped = {flipped} | upgrade = {upgrade}");
        }
    }
    
    public override void OnExitCombat(State s, Combat c)
    {
        flipped = _flipMemory;
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod Neo{_cardName}.cs] OnExitCombat | _flipMemory = {_flipMemory} | uuid = {uuid} | flipped = {flipped} | upgrade = {upgrade}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Neo{_cardName}.cs] OnExitCombat | _flipMemory = {_flipMemory} | uuid = {uuid} | flipped = {flipped} | upgrade = {upgrade}");
        }
    }
};