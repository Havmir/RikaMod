using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class AggressiveGamble : Card, IRegisterable
{
    private int _calculation;
    
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "AggressiveGamble", "name"])
                .Localize,
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipCompitent()
                {
                    _stringString = "trashAutoShoot"
                }
            ],
            Upgrade.A =>
            [
                new ToolTipCompitent()
                {
                    _stringString = "trashAutoShoot"
                }
            ],
            Upgrade.B =>
            [
                new ToolTipCompitent()
                {
                    _stringString = "trashAutoShoot"
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
            CardName = "Spare Shot"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new AAddCard
                {
                    card = new TrashAutoShoot(),
                    destination = CardDestination.Deck,
                    amount = 1
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AAddCard
                {
                    card = new TrashAutoShoot(),
                    destination = CardDestination.Deck,
                    amount = 1
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new RikaEnergyCost
                {
                    cardCost = 1
                });
                c.Queue(new AAddCard
                {
                    card = new TrashAutoShoot(),
                    destination = CardDestination.Deck,
                    amount = 2
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
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c>, but add 1 <c=card>Safety Override</c> to your deck.",
                    artTint = "ffffff",
                    retain = true,
                    recycle = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, add 1 <c=card>Safety Override</c> to your deck.",
                    artTint = "ffffff",
                    retain = true,
                    recycle = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c>, but add 2 <c=card>Safety Overrides</c> to your deck.",
                    artTint = "ffffff",
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
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c>, but add 1 <c=card>Safety Override</c> to your deck.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_WaveBeam,
                    retain = true,
                    recycle = true
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, <c=downside>-1 energy</c>, but add 1 <c=card>Safety Override</c> to your deck.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_WaveBeam,
                    retain = true,
                    recycle = true
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = "On draw, <c=downside>-1 energy</c>, but add 2 <c=card>Safety Overrides</c> to your deck.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_WaveBeam,
                    retain = true,
                    recycle = true
                };
            }
        }
        return default;
    }
};