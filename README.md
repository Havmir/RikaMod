0.2.3-Beta Changelog:

"update" 0.2.2-Beta was never released (or finished)

Created RikaEnergy.cs in Features to work with the new Rika Energy Status

Updated ModEntry to allow for Rika Energy status to exist

RikaEnergyCost.cs was created in actions to actually handle the costs for the Rika Energy Status (thanks for the recommendation Arin)

Almost every non scrapped card was either updated to use RikaEnergyCost or got scrapped.

I updated and created a lot of the managers

Thanks to rft50 for helping me with the code for the Kiteing Status and a little for the FlightDraw Status.

Thanks to JyGein for helping me with the code for the FlightDraw Status.

Thanks to Urufudoggo for letting me utalize some of Weth's code to get alt starters working.

0.2.2-Beta Changelog:

Replace Hand now gives +1 card accross the board

Spare Shot now has fast = true for all attacks

Add ToolTipCompitent to make my life easier changing cards like Replace Hand ( I now only have to change 3 numbers to change 33 different values.)

Scraped Borrow Cards card

Scraped Card Swap card

0.2.1-Beta Changelog:

Changed RandomUSB to have the correct color, which involved updated en.json as well

Added Console.WriteLine for SpareStatus to help see what happened prior to getting crazy statuses

Cleaned up the code on HullLostManager

Blitz now has Console.WriteLine stuff

ArtManager can now manager a lot of Console.WriteLine things. If you want it disabled, you'd need to go to ArtManager and set _isplaytester to false. (Not sure how you would do that, but I'm looking into it.)

Every Rika artifact besides randomUSB now has Console.Writeline things