using System;
using System.Collections.Generic;
using Nickel;

namespace RikaMod.Actions;


public class FalseIconADiscard : CardAction
{
    public int _discardNumber;
    public bool _discardHand = false;
    
    public override void Begin(G g, State s, Combat c)
    {

    }
    
    public override Icon? GetIcon(State s)
    {
        if (_discardHand)
        {
            return new Icon
            {
                path = StableSpr.icons_discardHand
            };
        }
        else if (_discardNumber >= 0 && _discardHand == false)
        {
            return new Icon
            {
                path = StableSpr.icons_discardCard,
                number = _discardNumber,
                color = Colors.textMain
            };
        }
        else
        {
            Console.WriteLine($"[RikaMod: FalseIconADiscard.cs] Error, _discardNumber is not a 0 or a positive integer or something else: _discardNumber = {_discardNumber} | _discardHand = {_discardHand}");
        }
        return default;
    }
    
    public override List<Tooltip> GetTooltips(State s)
    {
        if (_discardHand)
        {
            return [
                new TTGlossary("action.discardHand")
            ];
        }
        else if (_discardNumber >= 0 && _discardHand == false)
        {
            return [
                new TTGlossary("action.discardCard", [$"{_discardNumber}"])
            ];
        }
        else
        {
            Console.WriteLine($"[RikaMod: FalseIconADiscard.cs] Error, _discardNumber is not a 0 or a positive integer or something else: _discardNumber = {_discardNumber} | _discardHand = {_discardHand}");
        }
        return default!;
    }
}