using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nickel;
using RikaMod.Cards;
using RikaMod.Features;

namespace RikaMod.Actions;


public class FalseIconAAddCard : CardAction
{
    public string _card = "null";
    public int _cardAmount;
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    
    public static Spr RikaFluxIcon;
    
    public override void Begin(G g, State s, Combat c)
    {

    }
    
    public override Icon? GetIcon(State s)
    {
        if (_isplaytester && _card == "null")
        {
            Console.WriteLine($"[RikaMod] Please let Havmir know someone forgot to add a card to a move. Leftover data: _card: {_card} | __cardAmount: {_cardAmount}");
        }
        return new Icon
        {
            path = StableSpr.icons_addCard,
            number = _cardAmount,
            color = Colors.textMain
        };
    }
    
    public override List<Tooltip> GetTooltips(State s)
    {
        if (_logALotOfThings && _card == "null")
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: FalseIconAStatus.cs] Please let Havmir know someone forgot to add a status to a move. Leftover data: _card: {_card} | __cardAmount: {_cardAmount}");
        }
        if (_card == "Safety Override")
        {
            var side = "IWishIKnewHowToRemoveThis";
            return
            [
                new GlossaryTooltip($"IWishIKnewHowToRemoveThis::{side}")
                {
                    Icon = StableSpr.icons_addCard,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAAddCard", "titleAddCardDeck"]),
                    TitleColor = Colors.action,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAAddCard", "descAddCardDeck"], this)
                },
                new TTCard
                {
                    card = new TrashAutoShoot()
                },
            ];
        }
        if (_card == "Spare Shot")
        {
            var side = "IWishIKnewHowToRemoveThis";
            return
            [
                new GlossaryTooltip($"IWishIKnewHowToRemoveThis::{side}")
                {
                    Icon = StableSpr.icons_addCard,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAAddCard", "titleAddCardDeck"]),
                    TitleColor = Colors.action,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "FalseIconAAddCard", "descAddCardDeck"], this)
                },
                new TTCard
                {
                    card = new SpareShot()
                },
            ];
        }
        return default!;
    }
}