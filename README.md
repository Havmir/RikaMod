0.4.0-Beta Changelog:

Shield Current A is now honest with the amount of shield flow it gives

We now have access to card traits now, thanks rft50 & VladIsSlav for your help on that

Replace Hand has had some private ints swapped to public to allow for the rewind mod to work.

We added a trait to each Icon

Every prevouis card has been recoded and now lives in a new file. For these new cards, they have the same name as the old ones, just with "neo" in front of em.

Haste had to be reverted due to using icons now

Put the scrapped cards into the scrapped cards folder

Card Rotate has a small delay now (1 second)

Card Investment was cut laregly due to compition with tailwind

Swoop, the card, was added as a replacement for Card Investment and should provide myself with data on how players & Rika interact with single use on cards



0.3.5-Beta Changelog:

"_isplaytester" is now set to false in ArtManager

0.3.4-Beta Changelog:

Rika's manager for temp strafe and flight draw had its priority changed from very low to normal to fix an issue with another different mod.

0.3.3-Beta Changelog:

Rika's manager for temp strafe and flight draw had its priority changed from very low to low to fix an issue with another mod. (I also turned off the logger for that file as well)

Temp strafe is now called temp strafe instead of kiteing.

0.3.2-Beta Changelog:

The manager for the temp strafe and flight draw status should have very low priority to help with conflicts with other mods.

Card Rotate now specifies discarding random cards

0.3.1-Beta Changelog:

Quick Energy now gives an extra energy for the none and A upgrades.

Cut Roll Away

Added Rush Down to replace Roll Away

Shield Current now gives an extra shield flow for the none and A upgrades

Fixed PowerBoost's sprite being loaded twice

Cut Status Updraft, as getting boost from that card was being iffy in terms of gameplay

Added Adjust Gameplan to replace Status Updraft and to expirement with having a duel card for Rika.

Tailwind now gives an extra card for all 3 upgrades

Rika's descrioption is updated to be more in line with Vanilla's

RandomUSB's description is updated to be more in line with Vanilla's

0.3.0-Beta Changelog:

PowerBoost got buffed (I got weird with this and put it in its own file)

Card Investment now gives 2 extra draw per turn now (and now I can easily mess with values)

Flight Draw's costs are reduced by 1

Cut Fume Shot for now

Added Recoil Shot as a replacement for Fume Shot and to experiment with having Exhaust on my cards

Cut Jet Stream

Added Aggressive Gamble as a replacement for Jet Stream and to experiment with having recycle on a card

Kiting now has retain.

Power Gain's power B now gives 2 less energy next turn and is unplayable

Quick Block's upgrades now give one less temp shield each.

Quick Dodge now has 1 evade added to each upgrade & retain is added to both the none & B upgrade. Also made all upgrades cost 1 energy.

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