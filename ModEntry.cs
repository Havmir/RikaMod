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
using RikaMod.External;
using RikaMod.Features;

namespace RikaMod;

internal class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony;
    internal IKokoroApi.IV2 KokoroApi;
    internal IDeckEntry RikaDeck;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations;
    
    internal IPlayableCharacterEntryV2 Rikachar { get; }
    internal IStatusEntry Rikamissing { get; }

    internal ICardTraitEntry RikasTrait { get; }
    
    private int _artmode = ArtManager.ArtNumber;
    
    /*
     * The following lists contain references to all types that will be registered to the game.
     * All cards and artifacts must be registered before they may be used in the game.
     * In theory only one collection could be used, containing all registrable types, but it is seperated this way for ease of organization.
     */
    private static List<Type> _rikaCommonCardTypes = [
        typeof(SpareShot),
        typeof(ShieldCurrent),
        typeof(QuickDodge),
        typeof(QuickShift),
        // typeof(QuickBlock), ~ Shield Flow already does what QuickBlock does, just better 90% of the time and this card isn't that good on it's own to begin with, so I'm replacing it with other better cards.
        typeof(CardSwap),
        typeof(PowerBoost),
        typeof(CardInvestment),
        typeof(PeircingShots),
        typeof(BorrowCards),
    ];
    private static List<Type> _rikaUncommonCardTypes = [
        typeof(BarrelRoll),
        // typeof(ParallelStasis),scrapped for easily killing off the player ~ Havmir 22/06/20225
        typeof(CardToEnergy),
        typeof(WhaWhy),
        typeof(Haste),
        typeof(EnergyInvestment),
        typeof(RollAway),
        typeof(Tailwind)
    ];
    private static List<Type> _rikaRareCardTypes = [
        typeof(PowerGain),
        typeof(ReplaceHand),
        // typeof(RapidAttack),
        // typeof(StatusInvestment), ~ I never picked this card when play testing & I believe it to be overly weak.
        typeof(TradeBlows),
        typeof(Blitz),
        typeof(CorrosionShot),
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
        typeof(SpareStatus)
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
        
        /*
         * Some mods provide an API, which can be requested from the ModRegistry.
         * The following is an example of a required dependency - the code would have unexpected errors if Kokoro was not present.
         * Dependencies can (and should) be defined within the nickel.json file, to ensure proper load mod load order.
         */ 
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;

        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );

        if (_artmode == 1)
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
                color = new Color("72b5f5"),
                icon = rikaFluxicon.Sprite
            },
            Name = AnyLocalizations.Bind(["status", "RikaFlux", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "RikaFlux", "desc"]).Localize
        });
        
        _ = new RikaFluxManager();
        _ = new RikaCardsPerTurnManager();
        
        ToolTipAStatusRikaFlux.RikaFluxIcon = rikaFluxicon.Sprite;
        
        ISpriteEntry rikasTraiticon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/TikasTraitIcon.png")); 
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

        if (_artmode == 1)
        {
            Rikachar = helper.Content.Characters.V2.RegisterPlayableCharacter("Rika", new PlayableCharacterConfigurationV2
            {
                Deck = RikaDeck.Deck,
                BorderSprite = RegisterSprite(package, "assets/Alpha/char_frame_rika.png").Sprite,
                Starters = new StarterDeck
                {
                    cards = [
                        new SpareShot(),
                        new ShieldCurrent()
                    ]
                },
                Description = AnyLocalizations.Bind(["character", "desc"]).Localize
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
                        new SpareShot(),
                        new ShieldCurrent()
                    ]
                },
                Description = AnyLocalizations.Bind(["character", "desc"]).Localize
            });
        }

        Rikamissing = Rikachar.MissingStatus;
        
        _ = new HullLostManager();
        
        _ = new ArtManager();
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

