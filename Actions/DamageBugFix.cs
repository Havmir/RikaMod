using System;
using System.Collections.Generic;
using Nickel;
using RikaMod.Cards;
using RikaMod.Features;

namespace RikaMod.Actions;



public class DamageBugFix : CardAction
{
    public int whatIsTheDamage = 0;
    public bool IsFast = false;
    public bool IsPeirce = false;
    
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        c.QueueImmediate(new AAttack
        {
            damage = Card.GetActualDamage(s, whatIsTheDamage - 999),
            fast = IsFast,
            piercing = IsPeirce
        });
        return default;
    }
}