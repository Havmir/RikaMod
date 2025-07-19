using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class PeircingShots : Card, IRegisterable
{
    private int _calculation;
    
    public static Spr PeircingShotsB;
    
    public static void
        Register(IPluginPackage<IModManifest> package,
            IModHelper helper)
    {
        PeircingShotsB = ModEntry.RegisterSprite(package, "assets/Alpha/Card/PeircingShotsB.png").Sprite;
        
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PeircingShots", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/PeircingShots.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAAttackPierce()
            ],
            Upgrade.A =>
            [
                new ToolTipAAttackPierce()
            ],
            Upgrade.B =>
            [
                new ToolTipAAttackPierce()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Peircing Shots"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (upgrade == Upgrade.None)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    piercing = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    piercing = true
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    piercing = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    piercing = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 1),
                    piercing = true
                });
            }
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
                    description = $"On draw, <c=downside>-1 energy</c> but attack for <c=redd>{GetDmg(state, 1)}</c> damage with <c=keyword>piercing</c> twice.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 2)}</c> damage with <c=keyword>piercing</c> twice.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but attack for <c=redd>{GetDmg(state, 1)}</c> damage with <c=keyword>piercing</c> thrice.",
                    artTint = "ffffff",
                    art = PeircingShotsB
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
                    description = $"On draw, <c=downside>-1 energy</c> but attack for <c=redd>{GetDmg(state, 1)}</c> damage with <c=keyword>piercing</c> twice.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_WaveBeam
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = $"On draw, attack for <c=redd>{GetDmg(state, 2)}</c> damage with <c=keyword>piercing</c> twice.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_WaveBeam
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> but attack for <c=redd>{GetDmg(state, 1)}</c> damage with <c=keyword>piercing</c> thrice.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_WaveBeam
                };
            }
        }
        return default;
    }
};