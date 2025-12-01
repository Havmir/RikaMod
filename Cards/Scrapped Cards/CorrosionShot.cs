using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class CorrosionShot : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CorrosionShot", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Alpha/Card/CorrosionShot.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipAAttack(),
                new ToolTipAStatusCorrode1()
            ],
            Upgrade.A =>
            [
                new ToolTipAAttack(),
                new ToolTipAStatusCorrode1()
            ],
            Upgrade.B =>
            [
                new ToolTipAAttack(),
                new ToolTipAStatusCorrode1()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Corrosion Shot"
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
                    changeAmount = -2
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    status = Status.corrode,
                    statusAmount = 1
                });
            }

            if (upgrade == Upgrade.A)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -1
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    status = Status.corrode,
                    statusAmount = 1
                });
            }

            if (upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = -2
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    status = Status.corrode,
                    statusAmount = 1
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
                    cost = 2,
                    description = $"On draw, <c=downside>-2 energy</c> & attack for <c=redd>{GetDmg(state, 0)}</c>; if hit, apply 1 <c=status>corrode</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> & attack for <c=redd>{GetDmg(state, 0)}</c>; if hit, apply 1 <c=status>corrode</c>.",
                    artTint = "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 2,
                    description = $"On draw, <c=downside>-2 energy</c> & attack for <c=redd>{GetDmg(state, 0)}</c>; if hit, apply 1 <c=status>corrode</c>.",
                    artTint = "ffffff",
                    buoyant = true
                };
            }
        }
        else
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 2,
                    description = $"On draw, <c=downside>-2 energy</c> & attack for <c=redd>{GetDmg(state, 0)}</c>; if hit, apply 1 <c=status>corrode</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Corrode
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 1,
                    description = $"On draw, <c=downside>-1 energy</c> & attack for <c=redd>{GetDmg(state, 0)}</c>; if hit, apply 1 <c=status>corrode</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Corrode
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 2,
                    description = $"On draw, <c=downside>-2 energy</c> & attack for <c=redd>{GetDmg(state, 0)}</c>; if hit, apply 1 <c=status>corrode</c>.",
                    artTint = _artTintDefault,
                    art = StableSpr.cards_Corrode,
                    buoyant = true
                };
            }
        }
        return default;
    }
};