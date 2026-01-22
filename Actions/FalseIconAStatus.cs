using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Actions;


public class FalseIconAStatus : CardAction
{
    public string _status = "null";
    public int _statusAmount;
    public bool _isVanilla = true;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    private string _kiteingStatusName = ArtManager.KiteingDisplayName;
    
    public static Spr RikaFluxIcon;
    public static Spr RikaFlightDrawIcon;
    public static Spr RikaKiteingicon;
    
    public override void Begin(G g, State s, Combat c)
    {

    }
    
    public override Icon? GetIcon(State s)
    {
        if (_isplaytester && _status== "null")
        {
            Console.WriteLine($"[RikaMod] Please let Havmir know someone forgot to add a status to a move. Leftover data: _status: {_status} | _statusAmount: {_statusAmount}");
        }
        if (_status == "status.shield")
        {
            return new Icon
            {
                path = StableSpr.icons_shield,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.tempShield")
        {
            return new Icon
            {
                path = StableSpr.icons_tempShield,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "RikaFlux")
        {
            return new Icon
            {
                path = RikaFluxIcon,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.libra")
        {
            return new Icon
            {
                path = StableSpr.icons_libra,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.autopilot")
        {
            return new Icon
            {
                path = StableSpr.icons_autopilot,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "flightDrawStatusName")
        {
            return new Icon
            {
                path = RikaFlightDrawIcon,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.drawNextTurn")
        {
            return new Icon
            {
                path = StableSpr.icons_drawNextTurn,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == _kiteingStatusName)
        {
            return new Icon
            {
                path = RikaKiteingicon,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.hermes")
        {
            return new Icon
            {
                path = StableSpr.icons_hermes,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.evade")
        {
            return new Icon
            {
                path = StableSpr.icons_evade,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.overdrive")
        {
            return new Icon
            {
                path = StableSpr.icons_overdrive,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.powerdrive")
        {
            return new Icon
            {
                path = StableSpr.icons_powerdrive,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.powerdrive")
        {
            return new Icon
            {
                path = StableSpr.icons_powerdrive,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.energyLessNextTurn")
        {
            return new Icon
            {
                path = StableSpr.icons_energyLessNextTurn,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.energyNextTurn")
        {
            return new Icon
            {
                path = StableSpr.icons_energyNextTurn,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.drawLessNextTurn")
        {
            return new Icon
            {
                path = StableSpr.icons_drawLessNextTurn,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.maxShield")
        {
            return new Icon
            {
                path = StableSpr.icons_maxShield,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        if (_status == "status.loseEvadeNextTurn")
        {
            return new Icon
            {
                path = StableSpr.icons_loseEvadeNextTurn,
                number = _statusAmount,
                color = Colors.textMain
            };
        }
        return default;
    }
    
    public override List<Tooltip> GetTooltips(State s)
    {
        if (_isVanilla)
        {
            return [
                new TTGlossary($"{_status}", [$"{_statusAmount}"])
            ];
        }
        if (_status == "RikaFlux")
        {
            object[] objArray = new object[2]
            {
                $"{_statusAmount}",
                null!
            };
            var side = "ToolTipAStatusRikaFlux";
            return
            [
                new GlossaryTooltip($"ToolTipAStatusRikaFlux::{side}")
                {
                    Icon = RikaFluxIcon,
                    Title = ModEntry.Instance.Localizations.Localize(["status", "RikaFlux", "name"]),
                    TitleColor = Colors.status,
                    Description = string.Format(ModEntry.Instance.Localizations.Localize(["status", "RikaFlux", "desc"]), objArray)
                }
            ];
        }
        if (_status == "flightDrawStatusName")
        {
            object[] objArray = new object[2]
            {
                _statusAmount, //$"{s.ship.Get(RikaFlightDrawAndKiteingManager.RikaKiteing.Status)}",
                null!
            };
        
            var side = "thisisacryforhelpasIdontknowhowtoremovethis";
            return
            [
                new GlossaryTooltip($"thisisacryforhelpasIdontknowhowtoremovethis::{side}")
                {
                    Icon = RikaFlightDrawIcon,
                    Title = ModEntry.Instance.Localizations.Localize(["status", "RikaFlightDraw", "name"]),
                    TitleColor = Colors.status,
                    Description = string.Format(ModEntry.Instance.Localizations.Localize(["status", "RikaFlightDraw", "desc"]), objArray)
                }
            ];
        }
        if (_status == _kiteingStatusName)
        {
            object[] objArray = new object[2]
            {
                $"{_statusAmount}",
                null!
            };
        
            var side = "thisisacryforhelpasIdontknowhowtoremovethis";
            return
            [
                new GlossaryTooltip($"thisisacryforhelpasIdontknowhowtoremovethis::{side}")
                {
                    Icon = RikaKiteingicon,
                    Title = ModEntry.Instance.Localizations.Localize(["status", "TempStrafe", "name"]),
                    TitleColor = Colors.status,
                    Description = string.Format(ModEntry.Instance.Localizations.Localize(["status", "TempStrafe", "desc"]), objArray)
                }
            ]; 
        }
        // Otherwise:
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: FalseIconAStatus.cs] Please let Havmir know someone forgot to add a status to a move. Leftover data: _status: {_status} | _statusAmount: {_statusAmount} | _isVanilla: {_isVanilla}");
        }
        return default!;
    }
}