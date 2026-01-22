using System;
using System.Collections.Generic;
using Nickel;

namespace RikaMod.Actions;


public class FalseIconAEnergy : CardAction
{
    public int _energyNumber;
    
    public override void Begin(G g, State s, Combat c)
    {

    }
    
    public override Icon? GetIcon(State s)
    {
        if (_energyNumber >= 0)
        {
            return new Icon
            {
                path = StableSpr.icons_gainEnergy,
                number = _energyNumber,
                color = Colors.textMain
            };
        }
        else
        {
            Console.WriteLine($"[RikaMod: FalseIconAEnergy.cs] Error, _energyNumber is not a 0 or a positive integer: _energyNumber = {_energyNumber}");
        }
        return default;
    }
    
    public override List<Tooltip> GetTooltips(State s)
    {
        if (_energyNumber >= 0)
        {
            return [
                new TTGlossary("action.gainEnergy", [$"{_energyNumber}"])
            ];
        }
        else
        {
            Console.WriteLine($"[RikaMod: FalseIconAEnergy.cs] Error, _energyNumber is not a 0 or a positive integer: _energyNumber = {_energyNumber}");
        }
        return default!;
    }
}