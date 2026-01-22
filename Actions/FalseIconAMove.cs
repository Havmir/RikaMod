using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Actions;


public class FalseIconAMove : CardAction
{

    public int _moveNumber;
    public bool _isRandom = false;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    
    public override void Begin(G g, State s, Combat c)
    {

    }
    
    public override Icon? GetIcon(State s)
    {
        if (_moveNumber < 0 && _isRandom == false)
        {
            return new Icon
            {
                path = StableSpr.icons_moveLeft,
                number = Math.Abs(_moveNumber) + s.ship.Get(Status.hermes),
                color = Colors.textMain
            };
        }
        if (_moveNumber == 0 && _isRandom == false)
        {
            Console.WriteLine("[RikaMod: FalseIconAMove.cs] Please Inform Havmir that FalseIconAMove.cs had a 0 get returend, which was unexpected ...");
            return new Icon
            {
                path = StableSpr.icons_wall,
                number = null,
                color = Colors.textMain
            };
        }
        if (_moveNumber > 0 && _isRandom == false)
        {
            return new Icon
            {
                path = StableSpr.icons_moveRight,
                number = Math.Abs(_moveNumber) + s.ship.Get(Status.hermes),
                color = Colors.textMain
            };
        }
        if (_isRandom)
        {
            return new Icon
            {
                path = StableSpr.icons_moveRandom,
                number = Math.Abs(_moveNumber) + s.ship.Get(Status.hermes),
                color = Colors.textMain
            };
        }
        return default;
    }
    
    public override List<Tooltip> GetTooltips(State s)
    {
        if (_moveNumber < 0 && _isRandom == false)
        {
            var side = "FalseIconAMove";
            return
            [
                new GlossaryTooltip($"FalseIconAMove::{side}")
                {
                    Icon = StableSpr.icons_moveLeft,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAMove", "titleLeft"]),
                    TitleColor = Colors.action,
                    Description = string.Format(ModEntry.Instance.Localizations.Localize(["action", "FalseIconAMove", "descLeft"]), $"{Math.Abs(_moveNumber) + s.ship.Get(Status.hermes)}")
                }
            ];
        }
        if (_moveNumber == 0 && _isRandom == false)
        {
            var side = "FalseIconAMove";
            return
            [
                new GlossaryTooltip($"FalseIconAMove::{side}")
                {
                    Icon = StableSpr.icons_wall,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAMove", "titleZero"]),
                    TitleColor = Colors.action,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAMove", "descZero"], $"{Math.Abs(_moveNumber)}")
                }
            ];
        }
        if (_moveNumber > 0 && _isRandom == false)
        {
            var side = "FalseIconAMove";
            return
            [
                new GlossaryTooltip($"FalseIconAMove::{side}")
                {
                    Icon = StableSpr.icons_moveRight,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAMove", "titleRight"]),
                    TitleColor = Colors.action,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAMove", "descRight"], $"{Math.Abs(_moveNumber) + s.ship.Get(Status.hermes)}")
                }
            ];
        }
        if (_isRandom)
        {
            return [
                new TTGlossary("action.moveRandom", [$"{_moveNumber}"])
            ];
        }
        return default!;
    }
}