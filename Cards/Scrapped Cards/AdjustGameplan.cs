using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;


namespace RikaMod.Cards; 

public class AdjustGameplan : Card, IRegisterable
{
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
                upgradesTo = [Upgrade.A, Upgrade.B],
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "AdjustGameplan", "name"])
                .Localize,
        });
    }
    
    private int _shieldAmountNone = 1;
    private int _tempShieldAmountNone = 1;
    private int _shieldAmountA = 2;
    private int _tempShieldAmountA = 2;
    private int _rikaFluxAmountB = 1;
    private int _fluxAmountB = 1;
    
        public override List<CardAction> GetActions(State s, Combat c) 
    {
        return upgrade switch 
        {
            Upgrade.None => [
                new ToolTipCompitent
                {
                    _stringString = "action.attack.name",
                    disabled = flipped
                },
                new ToolTipCompitent
                {
                _stringString = "status.shield",
                _stringInt = $"{_shieldAmountNone}",
                disabled = !flipped
                },
                new ToolTipCompitent
                {
                    _stringString = "status.tempShield",
                    _stringInt = $"{_tempShieldAmountNone}",
                    disabled = !flipped
                }
            ],
            Upgrade.A => [
                new ToolTipCompitent
                {
                    _stringString = "action.attack.name",
                    disabled = flipped
                },
                new ToolTipCompitent
                {
                    _stringString = "status.shield",
                    _stringInt = $"{_shieldAmountA}",
                    disabled = !flipped
                },
                new ToolTipCompitent
                {
                    _stringString = "status.tempShield",
                    _stringInt = $"{_tempShieldAmountA}",
                    disabled = !flipped
                }
            ],
            Upgrade.B => [
                new ToolTipCompitent
                {
                    _stringString = "action.attackPiercing",
                    disabled = flipped
                },
                new ToolTipCompitent
                {
                    _stringString = "RikaFlux",
                    _stringInt = $"{_rikaFluxAmountB}",
                    disabled = !flipped
                },
                new ToolTipCompitent
                {
                    _stringString = "status.libra",
                    _stringInt = $"{_fluxAmountB}",
                    disabled = !flipped
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
        
    public override void OnDraw(State s, Combat c)
    {
        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
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
                c.Queue(new AStatus
                {
                    status = Status.shield,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = !flipped
                });
                c.Queue(new AStatus
                {
                    status = Status.tempShield,
                    statusAmount = 1,
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
            Console.WriteLine($"[RikaMod] AdjustGameplan drawn | flipped: {flipped} | | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: AdjustGameplan.cs] AdjustGameplan drawn | flipped: {flipped} | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn} | ");
        }
    }
        
    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (_artmode == 1)
        {
            if (upgrade == Upgrade.None && flipped == false)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg twice OR gain {_shieldAmountNone} <c=status>shield</c> & {_tempShieldAmountNone} <c=status>temp shield</c>.",
                    artTint = "ffffff",
                    art = StableSpr.cards_Cannon,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.None && flipped == true)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg twice OR gain {_shieldAmountNone} <c=status>shield</c> & {_tempShieldAmountNone} <c=status>temp shield</c>.",
                    artTint = "ffffff",
                    art = StableSpr.cards_Shield,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == false)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg thrice OR gain {_shieldAmountA} <c=status>shield</c> & {_tempShieldAmountA} <c=status>temp shield</c>.",
                    artTint = "ffffff",
                    art = StableSpr.cards_Cannon,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == true)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg thrice OR gain {_shieldAmountA} <c=status>shield</c> & {_tempShieldAmountA} <c=status>temp shield</c>.",
                    artTint = "ffffff",
                    art = StableSpr.cards_Shield,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == false)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 3)}</c> <c=keyword>piercing</c> dmg OR gain {_rikaFluxAmountB} <c=status>shield flow</c> & {_fluxAmountB} <c=status>flux</c>.",
                    artTint = "ffffff",
                    art = StableSpr.cards_Cannon,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == true)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 3)}</c> <c=keyword>piercing</c> dmg OR gain {_rikaFluxAmountB} <c=status>shield flow</c> & {_fluxAmountB} <c=status>flux</c>.",
                    artTint = "ffffff",
                    art = StableSpr.cards_Flux,
                    unplayable = true,
                    floppable = true
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.None && flipped == false)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg twice OR gain {_shieldAmountNone} <c=status>shield</c> & {_tempShieldAmountNone} <c=status>temp shield</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.None && flipped == true)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg twice OR gain {_shieldAmountNone} <c=status>shield</c> & {_tempShieldAmountNone} <c=status>temp shield</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Shield,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == false)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg thrice OR gain {_shieldAmountA} <c=status>shield</c> & {_tempShieldAmountA} <c=status>temp shield</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.A && flipped == true)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 1)}</c> dmg thrice OR gain {_shieldAmountA} <c=status>shield</c> & {_tempShieldAmountA} <c=status>temp shield</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Shield,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == false)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 3)}</c> <c=keyword>piercing</c> dmg OR gain {_rikaFluxAmountB} <c=status>shield flow</c> & {_fluxAmountB} <c=status>flux</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon,
                    unplayable = true,
                    floppable = true
                };
            }
            if (upgrade == Upgrade.B && flipped == true)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 3)}</c> <c=keyword>piercing</c> dmg OR gain {_rikaFluxAmountB} <c=status>shield flow</c> & {_fluxAmountB} <c=status>flux</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Flux,
                    unplayable = true,
                    floppable = true
                };
            }
        }
        return default;
    }
};

