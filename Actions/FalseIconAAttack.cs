using System.Collections.Generic;
using Nickel;

namespace RikaMod.Actions;


public class FalseIconAAtack : CardAction
{
    public bool _isPeirce = false;
    public int _damageNumber;
    
    public override void Begin(G g, State s, Combat c)
    {

    }
    
    public override Icon? GetIcon(State s)
    {
        if (_isPeirce)
        {
            return new Icon
            {
                path = StableSpr.icons_attackPiercing,
                number = _damageNumber,
                color = Colors.redd
            };
        }
        if (_isPeirce == false)
        {
            return new Icon
            {
                path = StableSpr.icons_attack,
                number = _damageNumber,
                color = Colors.redd
            };
        }
        return default;
    }
    
    public override List<Tooltip> GetTooltips(State s)
    {
        if (_isPeirce)
        {
            var side = "FalseIconAAtack";
            return
            [
                new GlossaryTooltip($"FalseIconAAtack::{side}")
                {
                    Icon = StableSpr.icons_attackPiercing,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAAtack", "titlePeirce"]),
                    TitleColor = Colors.action,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAAtack", "descPeirce"], this)
                }
            ];
        }
        if (_isPeirce == false)
        {
            var side = "FalseIconAAtack";
            return
            [
                new GlossaryTooltip($"FalseIconAAtack::{side}")
                {
                    Icon = StableSpr.icons_attack,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAAtack", "title"]),
                    TitleColor = Colors.action,
                    //Description = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAAtack", "desc"], this)
                }
            ];
        }
        return default!;
    }
}