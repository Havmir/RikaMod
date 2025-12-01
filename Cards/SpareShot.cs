using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class SpareShot : Card, IRegisterable
{
    private int _calculation;

    public static Spr SpareShotA;
    public static Spr SpareShotB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        SpareShotA = ModEntry.RegisterSprite(package, "assets/Alpha/Card/SpareShotA.png").Sprite;
        SpareShotB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/SpareShotB.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "SpareShot", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/SpareShot.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAAttack()
            ],
            Upgrade.A =>
            [
                new ToolTipAAttack()
            ],
            Upgrade.B =>
            [
                new ToolTipAAttackPierce()
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
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 2),
                    fast = true
                });

            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 2),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    fast = true
                });

            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 2),
                    piercing = true,
                    fast = true
                });
            }
        }
        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] Spare Shot drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: Spareshot.cs] Spare Shot drawn | Upgrade: {upgrade} | Rikamissing.Status = {s.ship.Get(ModEntry.Instance.Rikamissing.Status)} | Turn: {c.turn}");
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
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 2)}</c> damage.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 2)}</c> damage then <c=redd>{GetDmg(state, 1)}</c> damage.",
                    artTint = "ffffff",
                    art = SpareShotA
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 2)}</c> damage with <c=keyword>piercing</c>.",
                    artTint = "ffffff",
                    art = SpareShotB
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
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 2)}</c> damage.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 2)}</c> damage then <c=redd>{GetDmg(state, 1)}</c> damage.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 2)}</c> damage with <c=keyword>piercing</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Cannon
                };
            }
        }
        return default;
    }
};