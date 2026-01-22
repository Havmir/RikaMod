using System;
using System.Collections.Generic;
using Nickel;

namespace RikaMod.Actions;


public class FalseIconADrawCard : CardAction
{
    public int _drawNumber;
    
    public override void Begin(G g, State s, Combat c)
    {

    }
    
    public override Icon? GetIcon(State s)
    {
        if (_drawNumber >= 0)
        {
            return new Icon
            {
                path = StableSpr.icons_drawCard,
                number = _drawNumber,
                color = Colors.textMain
            };
        }
        else
        {
            Console.WriteLine($"[RikaMod: FalseIconADrawCard.cs] Error, _drawNumber is not a 0 or a positive integer: _drawNumber = {_drawNumber}");
        }
        return default;
    }
    
    public override List<Tooltip> GetTooltips(State s)
    {
        if (_drawNumber >= 0)
        {
            return [
                new TTGlossary("action.drawCard", [$"{_drawNumber}"])
            ];
        }
        else
        {
            Console.WriteLine($"[RikaMod: FalseIconADrawCard.cs] Error, _drawNumber is not a 0 or a positive integer: _drawNumber = {_drawNumber}");
        }
        return default!;
    }
}