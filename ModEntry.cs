using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using RikaMod.Actions;
using RikaMod.Artifacts;
using RikaMod.Cards;
using RikaMod.Cards.NeoCards;
using RikaMod.External;
using RikaMod.Features;
using TheJazMaster.MoreDifficulties;

namespace RikaMod;

internal class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony;
    internal IKokoroApi.IV2 KokoroApi;
    internal IDeckEntry RikaDeck;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations;
    
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; private set; } = null!;
    
    internal IPlayableCharacterEntryV2 Rikachar { get; }
    internal IStatusEntry Rikamissing { get; }

    internal ICardTraitEntry RikasTrait { get; }
    
    private int _artmode = ArtManager.ArtNumber;
    
    private static List<Type> _rikaCommonCardTypes = [
        // typeof(SpareShot),
        // typeof(ShieldCurrent),
        // typeof(QuickDodge),
        // typeof(QuickShift), ~ Scarped for the 0.2.3 update to make Rika have a more coheisive deck ~ 01/08/2025 Havmir
        // typeof(QuickBlock), //~ Shield Flow already does what QuickBlock does, just better 90% of the time and this card isn't that good on it's own to begin with, so I'm replacing it with other better cards.
        // typeof(CardSwap) ~ this was a little too niche,
        // typeof(PowerBoost),
        // typeof(CardInvestment),
        // typeof(PeircingShots), ~ Scarped for the 0.2.3 update to make Rika have a more coheisive deck ~ 01/08/2025 Havmir
        // typeof(BorrowCards), ~ it was never really good or useful anywhere
        // typeof(Tailwind),
        // typeof(Dive),
        // typeof(EmergencyShield),
        // typeof(QuickEnergy),
        // typeof(PowerBoostExpirement1),
        typeof(NeoDive),
        // typeof(NeoCardInvestment),
        typeof(NeoPowerBoost),
        typeof(NeoQuickBlock),
        typeof(NeoQuickDodge),
        typeof(NeoQuickEnergy),
        typeof(NeoShieldCurrent),
        typeof(NeoSpareShot),
        typeof(NeoTailwind),
        // typeof(NeoSinghShot) ~~ I feel like this needs to be better made, so I swaped it out with NeoSwoop instead for the V0.4.0 update Havmir ~ 21/01/2026
        typeof(NeoSwoop)
    ];
    private static List<Type> _rikaUncommonCardTypes = [
        // typeof(BarrelRoll),
        // typeof(ParallelStasis),scrapped for easily killing off the player ~ Havmir 22/06/20225
        // typeof(CardToEnergy),  ~ Scarped for the 0.2.3 update to make Rika have a more coheisive deck ~ 01/08/2025 Havmir
        // typeof(WhaWhy),
        // typeof(Haste),
        // typeof(EnergyInvestment), ~ Scarped for the 0.2.3 update to make Rika have a more coheisive deck ~ 01/08/2025 Havmir
        // typeof(RollAway), ~ Cut in the 0.3.1 update as it wasn't ever really a good pull to grab. 14/12/2025 Havmir
        // typeof(ShieldDraw), ~ Due to how artifacts work on the first turn, this card is bugged ~ 17/11/2025 Havmir
        // typeof(Recast),
        // typeof(FlightDraw),
        // typeof(FumeShot) ~ Too much worse compared to Peri's Frontloaded Blast ~ 12/12/2025 Havmir
        // typeof(RecoilShot),
        // typeof(RushDown),
        typeof(NeoBarrelRoll),
        typeof(NeoFlightDraw),
        typeof(NeoHaste),
        typeof(NeoRecast),
        typeof(NeoRecoilShot),
        typeof(NeoRushDown),
        typeof(NeoKiteing),
    ];
    private static List<Type> _rikaRareCardTypes = [
        // typeof(PowerGain),
        // typeof(ReplaceHand),
        // typeof(RapidAttack),
        // typeof(StatusInvestment), ~ I never picked this card when play testing & I believe it to be overly weak.
        // typeof(TradeBlows), ~ Scarped for the 0.2.3 update to make Rika have a more coheisive deck ~ 01/08/2025 Havmir
        // typeof(Blitz), ~ Scarped for the 0.2.3 update to make Rika have a more coheisive deck ~ 01/08/2025 Havmir
        // typeof(CorrosionShot), ~ Scarped for the 0.2.3 update to make Rika have a more coheisive deck ~ 01/08/2025 Havmir
        // typeof(Kiteing),
        // typeof(JetStream), ~ Too much free energy, so it got scraped in favor of Aggressive Gamble ~ 12/12/2025 Havmir
        // typeof(StatusUpdraft), ~ getting boost from this card was being iffy in terms of gameplay ~ 15/12/2025 Havmir
        // typeof(AggressiveGamble),
        // typeof(AdjustGameplan)
        typeof(NeoAdjustGameplan),
        // typeof(NeoAggressiveGamble), ~ I really liked how recycle and retain interacted with each other, but I couldn't think of a good card that suited this effect short of maybe making a custom token(s) to make the card good. ~ Havmir 18/01/2026
        typeof(NeoPowerGain),
        typeof(NeoReplaceHand),
        typeof(NeoEvadeBooster),
        typeof(NeoWhaWhy)
    ];
    private static List<Type> _rikaSpecialCardTypes = [
        typeof(ColorlessRikaSummon)
    ];
    private static IEnumerable<Type> _rikaCardTypes =
        _rikaCommonCardTypes
            .Concat(_rikaUncommonCardTypes)
            .Concat(_rikaRareCardTypes)
            .Concat(_rikaSpecialCardTypes);

    private static List<Type> _rikaCommonArtifacts = [
        typeof(SpicyBoost),
        typeof(CardRotate),
        typeof(RandomUsb),
        // typeof(SpareStatus)
    ];
    private static List<Type> _rikaBossArtifacts = [
        typeof(MoralBoost),
        typeof(BoostedDeck)
    ];
    private static IEnumerable<Type> _rikaArtifactTypes =
        _rikaCommonArtifacts
            .Concat(_rikaBossArtifacts);

    private static IEnumerable<Type> _allRegisterableTypes =
        _rikaCardTypes
            .Concat(_rikaArtifactTypes);

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new Harmony("havmir.RikaMod");
        
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;
        MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");

        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );

        if (_artmode == 1 || _artmode == 2)
        {
            RikaDeck = helper.Content.Decks.RegisterDeck("Rika", new DeckConfiguration
            {
                Definition = new DeckDef
                {
                    color = new Color("f00707"),
                    titleColor = new Color("000000")
                },

                DefaultCardArt = StableSpr.cards_colorless,
                Name = AnyLocalizations.Bind(["character", "name"]).Localize,
                BorderSprite = RegisterSprite(package, "assets/Alpha/frame_rika.png").Sprite,
                ShineColorOverride = _ => new Color(1, 0, 1)
            });
        }
        else
        {
            RikaDeck = helper.Content.Decks.RegisterDeck("Rika", new DeckConfiguration
            {
                Definition = new DeckDef
                {
                    color = new Color("db9b79"),
                    titleColor = new Color("000000")
                },

                DefaultCardArt = StableSpr.cards_colorless,
                Name = AnyLocalizations.Bind(["character", "name"]).Localize,
                BorderSprite = RegisterSprite(package, "assets/BrahminyKiteUnderWingFrame.png").Sprite,
            });
        }

        var rikaFluxicon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/RikaFlux.png"));
        RikaFluxManager.RikaFlux = helper.Content.Statuses.RegisterStatus("RikaFlux", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = true,
                color = new Color("004080"),
                icon = rikaFluxicon.Sprite
            },
            Name = AnyLocalizations.Bind(["status", "RikaFlux", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "RikaFlux", "desc"]).Localize
        });
        
        var rikaEnergyicon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/RikaEnergy.png"));
        RikaEnergyManager.RikaEnergy = helper.Content.Statuses.RegisterStatus("RikaEnergy", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = true,
                color = new Color("92dcff"),
                icon = rikaEnergyicon.Sprite
            },
            Name = AnyLocalizations.Bind(["status", "RikaEnergy", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "RikaEnergy", "desc"]).Localize
        });
        
        var rikaBackUpShieldicon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/BackUpShield.png"));
        RikaBackUpShieldManager.rikaBackUpShield = helper.Content.Statuses.RegisterStatus("BackUpShield", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = true,
                color = new Color("be00a1"),
                icon = rikaBackUpShieldicon.Sprite
            },
            Name = AnyLocalizations.Bind(["status", "BackUpShield", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "BackUpShield", "shipdesc"]).Localize
        });
        
        var rikaFlightDrawicon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/RikaFlightDraw.png"));
        RikaFlightDrawAndKiteingManager.RikaFlightDraw = helper.Content.Statuses.RegisterStatus("RikaFlightDraw", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = true,
                color = new Color("1abec7"),
                icon = rikaFlightDrawicon.Sprite
            },
            Name = AnyLocalizations.Bind(["status", "RikaFlightDraw", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "RikaFlightDraw", "desc"]).Localize
        });
        
        var rikaKiteingicon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Kiteing.png"));
        RikaFlightDrawAndKiteingManager.RikaKiteing = helper.Content.Statuses.RegisterStatus("FlightDraw",
            new StatusConfiguration
            {
                Definition = new StatusDef
                {
                    isGood = true,
                    affectedByTimestop = true,
                    color = new Color("9ccef7"),
                    icon = rikaKiteingicon.Sprite
                },
                Name = AnyLocalizations.Bind(["status", "Kiteing", "name"]).Localize,
                Description = AnyLocalizations.Bind(["status", "Kiteing", "desc"]).Localize
            });
        
        _ = new RikaFluxManager();
        _ = new RikaEnergyManager();
        _ = new RikaBackUpShieldManager();
        _ = new RikaFlightDrawAndKiteingManager();
        _ = new RikaCardsPerTurnManager();
        
        ToolTipAStatusRikaFlux.RikaFluxIcon = rikaFluxicon.Sprite;
        ToolTipCompitent.RikaBackUpShieldicon = rikaBackUpShieldicon.Sprite;
        ToolTipCompitent.RikaKiteingicon = rikaKiteingicon.Sprite;
        ToolTipCompitent.RikaEnergyIcon = rikaEnergyicon.Sprite;
        ToolTipCompitent.RikaFlightDrawIcon = rikaFlightDrawicon.Sprite;
        ToolTipCompitent.RikaFluxIcon = rikaFluxicon.Sprite;
        FalseIconAStatus.RikaFluxIcon = rikaFluxicon.Sprite;
        FalseIconAStatus.RikaFlightDrawIcon = rikaFlightDrawicon.Sprite;
        FalseIconAStatus.RikaKiteingicon = rikaKiteingicon.Sprite;
        
        ISpriteEntry rikasTraiticon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/EnergyBall.png")); 
        RikasTrait = helper.Content.Cards.RegisterTrait("RikasTrait", new()
        {
            Name = AnyLocalizations.Bind(["cardtrait", "RikasTrait", "name"]).Localize,
            Icon = (_, _) => rikasTraiticon.Sprite,
            Tooltips = (_, _) => [
                new GlossaryTooltip($"action.{Instance.Package.Manifest.UniqueName}::RikasTrait")
                {
                    Icon = rikasTraiticon.Sprite,
                    TitleColor = Colors.cardtrait,
                    Title = Localizations.Localize(["cardTrait", "RikasTrait", "name"]),
                    Description = Localizations.Localize(["cardTrait", "RikasTrait", "desc"])
                }
            ]
        });
        
        // RegisterSprite(package, "assets/RikaFlux.png").Sprite
        /*
         * All the IRegisterable types placed into the static lists at the start of the class are initialized here.
         * This snippet invokes all of them, allowing them to register themselves with the package and helper.
         */
        foreach (var type in _allRegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
        
        /*
         * Characters have required animations, recommended animations, and you have the option to add more.
         * In addition, they must be registered before the character themselves is registered.
         * The game requires you to have a neutral animation and mini animation, used for normal gameplay and the map and run start screen, respectively.
         * The game uses the squint animation for the Extra-Planar Being and High-Pitched Static events, and the gameover animation while you are dying.
         * You may define any other animations, and they will only be used when explicitly referenced (such as dialogue).
         */
        RegisterAnimation(package, "neutral", "assets/Animation/RikaNeutral", 4);
        RegisterAnimation(package, "squint", "assets/Animation/RikaSquint", 4);
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = RikaDeck.Deck.Key(),
            LoopTag = "gameover",
            Frames = [
                RegisterSprite(package, "assets/Animation/RikaGameOver.png").Sprite,
            ]
        });
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = RikaDeck.Deck.Key(),
            LoopTag = "mini",
            Frames = [
                RegisterSprite(package, "assets/Animation/RikaMini.png").Sprite,
            ]
        });

        if (_artmode == 1 || _artmode == 2)
        {
            Rikachar = helper.Content.Characters.V2.RegisterPlayableCharacter("Rika", new PlayableCharacterConfigurationV2
            {
                Deck = RikaDeck.Deck,
                BorderSprite = RegisterSprite(package, "assets/Alpha/char_frame_rika.png").Sprite,
                Starters = new StarterDeck
                {
                    cards = [
                        new NeoSpareShot(),
                        new NeoQuickDodge()
                    ]
                },
                Description = AnyLocalizations.Bind(["character", "desc"]).Localize,
                ExeCardType = typeof(ColorlessRikaSummon),
                SoloStarters = new StarterDeck
                {
                    cards = [
                        new NeoSpareShot(),
                        new NeoQuickDodge(),
                        new NeoDive(),
                        new NeoPowerBoost(),
                        new NeoQuickBlock(),
                        new NeoTailwind()
                    ]
                }
            });
        }
        else
        {
            Rikachar = helper.Content.Characters.V2.RegisterPlayableCharacter("Rika", new PlayableCharacterConfigurationV2
            {
                Deck = RikaDeck.Deck,
                BorderSprite = RegisterSprite(package, "assets/BrahminyKiteUnderWingCharFrame.png").Sprite,
                Starters = new StarterDeck
                {
                    cards = [
                        new NeoSpareShot(),
                        new NeoQuickDodge()
                    ]
                },
                Description = AnyLocalizations.Bind(["character", "desc"]).Localize,
                ExeCardType = typeof(ColorlessRikaSummon),
                SoloStarters = new StarterDeck
                {
                    cards = [
                        new NeoSpareShot(),
                        new NeoQuickDodge(),
                        new NeoDive(),
                        new NeoPowerBoost(),
                        new NeoQuickBlock(),
                        new NeoTailwind()
                    ]
                }
            });
        }
        
        MoreDifficultiesApi?.RegisterAltStarters(RikaDeck.Deck, new StarterDeck
        {
            cards = [
                new NeoDive(),
                new NeoQuickBlock()
            ]
        });
        
        Rikamissing = Rikachar.MissingStatus;
        
        _ = new HullLostManager();
        _ = new ArtManager();
        
        var redTrashFumesBackground = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Alpha/Card/RedTrashFumesBackground.png"));
        var kiteingCardBackgroundSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/KiteingCardBackground.png"));
        var jetStreamCardBackgroundSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/PoorJetStreamArt.png"));
        var statusUpdraftCardBackgroundSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/StatusUpdraftCardArt.png"));
        var powerBoostAlphaSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Alpha/Card/PowerBoost.png"));
        var powerBoostBAlphaSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Alpha/Card/PowerBoostB.png"));
        var energyBallSprite = rikasTraiticon;
        var tempPlaceHolderArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Alpha/Card/Unused/BadToothCard.png"));
        Dive.RedTrashFumesBackgroundSprite = redTrashFumesBackground.Sprite;
        ShieldDraw.RedTrashFumesBackgroundSprite = redTrashFumesBackground.Sprite;
        EmergencyShield.RedTrashFumesBackgroundSprite = redTrashFumesBackground.Sprite;
        Recast.RedTrashFumesBackgroundSprite = redTrashFumesBackground.Sprite;
        EmergencyShield.RedTrashFumesBackgroundSprite = redTrashFumesBackground.Sprite;
        Kiteing.KiteingCardBackgroundSprite = kiteingCardBackgroundSprite.Sprite;
        JetStream.JetStreamCardBackgroundSprite = jetStreamCardBackgroundSprite.Sprite;
        StatusUpdraft.StatusUpdraftCardBackgroundSprite = statusUpdraftCardBackgroundSprite.Sprite;
        PowerBoost.PowerBoostAlphaSprite = powerBoostAlphaSprite.Sprite;
        PowerBoost.PowerBoostBAlphaSprite = powerBoostAlphaSprite.Sprite;
        RikaEnergyCost.EnergyBallSprite = energyBallSprite.Sprite;
        FalseIconRikaCard.EnergyBallSprite = energyBallSprite.Sprite;
        NeoDive.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoAdjustGameplan.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoAggressiveGamble.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoBarrelRoll.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoCardInvestment.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoFlightDraw.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoHaste.TempPlaceHolderArt =  tempPlaceHolderArt.Sprite;
        NeoKiteing.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoKiteing.KiteingCardBackgroundSprite = kiteingCardBackgroundSprite.Sprite;
        NeoPowerBoost.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoPowerGain.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoQuickBlock.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoQuickDodge.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoQuickEnergy.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoRecast.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoRecoilShot.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoRushDown.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoShieldCurrent.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoSpareShot.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoTailwind.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoEvadeBooster.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoSinghShot.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
        NeoSwoop.TempPlaceHolderArt = tempPlaceHolderArt.Sprite;
    }
    


    public static ISpriteEntry RegisterSprite(IPluginPackage<IModManifest> package, string dir)
    {
        return Instance.Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile(dir));
    }

    /*
     * Animation frames are typically named very similarly, only differing by the number of the frame itself.
     * This utility method exists to easily register an animation.
     * It expects the animation to start at frame 0, up to frames - 1.
     * TODO It is advised to avoid animations consisting of 2 or 3 frames.
     */
    public static void RegisterAnimation(IPluginPackage<IModManifest> package, string tag, string dir, int frames)
    {
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Instance.RikaDeck.Deck.Key(),
            LoopTag = tag,
            Frames = Enumerable.Range(0, frames)
                .Select(i => RegisterSprite(package, dir + i + ".png").Sprite)
                .ToImmutableList()
        });
    }
}

