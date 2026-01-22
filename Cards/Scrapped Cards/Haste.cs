using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class Haste : Card, IRegisterable
{
    private int _calculation;

    public static Spr HasteA;
    public static Spr HasteB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        HasteA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/HasteA.png").Sprite;
        HasteB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/HasteB.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Haste", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/Haste.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.hermes",
                    _stringInt = "1"
                },
                new AStatus
                {
                    status = Status.hermes,
                    statusAmount = -1,
                    targetPlayer = true
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.hermes",
                    _stringInt = "1"
                },
                new ToolTipCompitent
                {
                    _stringString = "status.evade",
                    _stringInt = "1"
                },
                new AStatus
                {
                    status = Status.hermes,
                    statusAmount = -1,
                    targetPlayer = true
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent
                {
                    _stringString = "status.hermes",
                    _stringInt = "2"
                },
                new AStatus
                {
                    status = Status.hermes,
                    statusAmount = -2,
                    targetPlayer = true
                }
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;

    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Haste"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AStatus
                {
                    status = Status.hermes,
                    statusAmount = 1,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AStatus
                {
                    status = Status.hermes,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.evade,
                    statusAmount = 1,
                    targetPlayer = true
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = Status.hermes,
                    statusAmount = 2,
                    targetPlayer = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Haste drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Haste.cs] Haste drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
        }
    }

    private int _artmode = ArtManager.ArtNumber;
    private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (_artmode == 1)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 1 <c=status>hermes boots</c>. On play, lose 1 <c=status>hermes boots</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 1 <c=status>hermes boots</c> & 1 <c=status>evade</c>. On play, lose 1 <c=status>hermes boots</c>.",
                    artTint = "ffffff",
                    art = HasteA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, gain 2 <c=status>hermes boots</c>. On play, lose 2 <c=status>hermes boots</c>.",
                    artTint = "ffffff",
                    art = HasteB
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, + 1 <c=status>hermes boots</c>. On play, - 1 <c=status>hermes boots</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Fleetfoot
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, + 1 <c=status>hermes boots</c> & 1 <c=status>evade</c>; on play, - 1 <c=status>hermes boots</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Fleetfoot
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, + 2 <c=status>hermes boots</c>; on play, - 2 <c=status>hermes boots</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Fleetfoot
                };
            }
        }
        return default;
    }
};