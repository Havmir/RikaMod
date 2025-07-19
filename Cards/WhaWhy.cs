using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Actions;
using RikaMod.Features;

namespace RikaMod.Cards;

public class WhaWhy : Card, IRegisterable
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
                rarity = Rarity.uncommon,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "WhaWhy", "name"])
                .Localize,
            Art = ModEntry.RegisterSprite(package, "assets/Card/WhaWhyV2.png").Sprite
        });
    }
    
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ToolTipToothCardNone()
            ],
            Upgrade.A =>
            [
                new ToolTipToothCardA()
            ],
            Upgrade.B =>
            [
                new ToolTipToothCardB()
            ],
            _ => throw new ArgumentOutOfRangeException()
        };
    }



    
    public override void OnDraw(State s, Combat c)
    {
        /*c.Queue(new AcknowledgeRikaCardDrawn
        {
            CardName = "Wha ... Why?"
        });*/
        
        _calculation = ModEntry.Instance.Helper.ModData.GetModDataOrDefault(s, "rikaCardsPerTurnNumber", 0);
        _calculation++;
        ModEntry.Instance.Helper.ModData.SetModData(s, "rikaCardsPerTurnNumber", _calculation);
        
        int x = s.rngActions.NextInt();
        x = x % 4;
        x = x + 1;

        if (s.ship.Get(ModEntry.Instance.Rikamissing.Status) == 0)
        {
            if (x == 1 && upgrade == Upgrade.None)
            {
                c.Queue(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHeal
                {
                    healAmount = 1,
                    targetPlayer = true
                });
            }

            if (x == 2 && upgrade == Upgrade.None)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
            }

            if (x == 3 && upgrade == Upgrade.None)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = 3
                });
            }

            if (x == 4 && upgrade == Upgrade.None)
            {
                c.Queue(new AMove
                {
                    dir = -2,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
            }

            if (x == 1 && upgrade == Upgrade.A)
            {
                c.Queue(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHeal
                {
                    healAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHeal
                {
                    healAmount = 1,
                    targetPlayer = true
                });
            }

            if (x == 2 && upgrade == Upgrade.A)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true
                });
            }

            if (x == 3 && upgrade == Upgrade.A)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = 3
                });
            }

            if (x == 4 && upgrade == Upgrade.A)
            {
                c.Queue(new AMove
                {
                    dir = -4,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
            }

            if (x == 1 && upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = Status.perfectShield,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AHurt
                {
                    hurtAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.perfectShield,
                    statusAmount = -1,
                    targetPlayer = true
                });
            }

            if (x == 2 && upgrade == Upgrade.B)
            {
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true,
                    piercing = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true,
                    piercing = true
                });
                c.Queue(new AAttack
                {
                    damage = GetDmg(s, 0),
                    fast = true,
                    piercing = true
                });
            }

            if (x == 3 && upgrade == Upgrade.B)
            {
                c.Queue(new AEnergy
                {
                    changeAmount = 3
                });
                c.Queue(new ADrawCard
                {
                    count = 1
                });
            }

            if (x == 4 && upgrade == Upgrade.B)
            {
                c.Queue(new AStatus
                {
                    status = Status.ace,
                    statusAmount = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = -2,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AMove
                {
                    dir = 1,
                    targetPlayer = true
                });
                c.Queue(new AStatus
                {
                    status = Status.ace,
                    statusAmount = -1,
                    targetPlayer = true
                });
            }

            if (x == 5)
            {
                Console.WriteLine(
                    "WhaWhy.cs returned a 5 somehow ... probally best to let Havmir know, as this isn't suppose to happen!");
            }
        }
    }

    private int _artmode = ArtManager.ArtNumber;
    //private string _artTintDefault = ArtManager.ModeDefaultArtTint;
    
    public override CardData GetData(State state)
    {
        if (_artmode == 1)
        {
            if (upgrade == Upgrade.None)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, apply a random <c=tooth>Tooth</c> card effect.",
                    artTint =  "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, apply a random <c=tooth>Tooth</c> A upgraded card effect.",
                    artTint =  "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, apply a random <c=tooth>Tooth</c> B upgraded card effect.",
                    artTint =  "ffffff"
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
                    description = "On draw, apply a random <c=tooth>Tooth</c> card effect.",
                    artTint =  "ffffff"
                };
            }
            if (upgrade == Upgrade.A)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, apply a random <c=tooth>Tooth</c> A upgraded card effect.",
                    artTint =  "ffffff"
                };
            }
            if (upgrade == Upgrade.B)
            {
                return new CardData
                {
                    cost = 0,
                    description = "On draw, apply a random <c=tooth>Tooth</c> B upgraded card effect.",
                    artTint =  "ffffff"
                };
            }
        }
        return default;
    }
};
// <c=5476a7>T</c><c=93b46f>o</c><c=5476a7>o</c><c=93b46f>t</c><c=5476a7>h</c>