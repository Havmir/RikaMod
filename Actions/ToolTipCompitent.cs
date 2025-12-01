using System;
using System.Collections.Generic;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Actions;

public class ToolTipCompitent : CardAction
{
    public string _stringString = "HavmirForgotToAddAStringForToolTipCompitent";
    public string _stringInt = "404";

    public bool IsWeird = false;
    public int WeirdCase;
    
    public static Spr RikaBackUpShieldicon;
    public static Spr RikaKiteingicon;
    public static Spr RikaEnergyIcon;
    public static Spr RikaFlightDrawIcon;
    
    public override List<Tooltip> GetTooltips(State s)
    {
        if (IsWeird == false)
        {
            if (_stringString == "EmergencyShield")
            {
                int calcuation = Convert.ToInt32(_stringInt) * 2; // This is bad code, don't do this ~ Havmir 27/08/2025
                string conversion = Convert.ToString(calcuation);
                
                object[] objArray = new object[2]
                {
                    $"{conversion}",
                    null!
                };
        
                var side = "thisisacryforhelpasIdontknowhowtoremovethis";
                return
                [
                    new GlossaryTooltip($"thisisacryforhelpasIdontknowhowtoremovethis::{side}")
                    {
                        Icon = RikaBackUpShieldicon,
                        Title = ModEntry.Instance.Localizations.Localize(["status", "BackUpShield", "name"]),
                        TitleColor = Colors.status,
                        Description = string.Format(ModEntry.Instance.Localizations.Localize(["status", "BackUpShield", "desc"]), objArray)
                    }
                ];
            }
            if (_stringString == "kite-ing")
            {
                object[] objArray = new object[2]
                {
                    _stringInt, //$"{s.ship.Get(RikaFlightDrawAndKiteingManager.RikaKiteing.Status)}",
                    null!
                };
        
                var side = "thisisacryforhelpasIdontknowhowtoremovethis";
                return
                [
                    new GlossaryTooltip($"thisisacryforhelpasIdontknowhowtoremovethis::{side}")
                    {
                        Icon = RikaKiteingicon,
                        Title = ModEntry.Instance.Localizations.Localize(["status", "Kiteing", "name"]),
                        TitleColor = Colors.status,
                        Description = string.Format(ModEntry.Instance.Localizations.Localize(["status", "Kiteing", "desc"]), objArray)
                    }
                ];
            }
            if (_stringString == "temp strafe")
            {
                object[] objArray = new object[2]
                {
                    $"{_stringInt}",
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
            if (_stringString == "RikaEnergy")
            {
                object[] objArray = new object[2]
                {
                    $"{s.ship.Get(RikaEnergyManager.RikaEnergy.Status)}",
                    null!
                };
        
                var side = "thisisacryforhelpasIdontknowhowtoremovethis";
                return
                [
                    new GlossaryTooltip($"thisisacryforhelpasIdontknowhowtoremovethis::{side}")
                    {
                        Icon = RikaEnergyIcon,
                        Title = ModEntry.Instance.Localizations.Localize(["status", "RikaEnergy", "name"]),
                        TitleColor = Colors.status,
                        Description = string.Format(ModEntry.Instance.Localizations.Localize(["status", "RikaEnergy", "desc"]), objArray)
                    }
                ];
            }
            if (_stringString == "flightDrawStatusName")
            {
                object[] objArray = new object[2]
                {
                    _stringInt, //$"{s.ship.Get(RikaFlightDrawAndKiteingManager.RikaKiteing.Status)}",
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
        }

        if (IsWeird)
        {
            if (WeirdCase == 1)
            {
                List<Tooltip> tooltips = new List<Tooltip>();
                tooltips.Add( new TTGlossary(_stringString));
                return tooltips;
            }

            if (WeirdCase == 0)
            {
                Console.WriteLine($"[RikaMod] Error: when using ToolTipComputent, WeirdCase was never set to an approprite value. Value: {WeirdCase} | _stringString: {_stringString}");
            }
        }
        
        
        {
            return [
                new TTGlossary(_stringString, _stringInt)
            ];
        }

    }
}