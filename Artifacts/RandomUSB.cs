using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Artifacts;

public class RandomUsb : Artifact, IRegisterable
{
 
    private static int _artmode = ArtManager.ArtNumber;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (_artmode == 1)
        {
            helper.Content.Artifacts.RegisterArtifact(new ArtifactConfiguration
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new ArtifactMeta
                {
                    pools = [ArtifactPool.Common],
                    owner = ModEntry.Instance.RikaDeck.Deck
                },
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "RandomUSB", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "RandomUSB", "alphadesc"]).Localize,
                Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/BadUSBIcon.png")).Sprite
            });
        }
        else
        {
            helper.Content.Artifacts.RegisterArtifact(new ArtifactConfiguration
            {
                ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new ArtifactMeta
                {
                    pools = [ArtifactPool.Common],
                    owner = ModEntry.Instance.RikaDeck.Deck
                },
                Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "RandomUSB", "name"]).Localize,
                Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "RandomUSB", "desc"]).Localize,
                Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/BadUSBIcon.png")).Sprite
            });
        }
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    
    public override void OnReceiveArtifact(State state)
    {
        state.GetCurrentQueue().QueueImmediate(new ACardOffering
        {
            amount = 5,
            limitDeck = ModEntry.Instance.RikaDeck.Deck,
            makeAllCardsTemporary = false,
            canSkip = true,
            isEvent = true
        });
        state.GetCurrentQueue().QueueImmediate(new AAddCard
        {
            card = new TrashAutoShoot{ temporaryOverride = false },
            destination = CardDestination.Deck,
            amount = 1,
            insertRandomly = true
        });
        
        if (_isplaytester)
        {
            Console.WriteLine("[RikaMod] RandomUSB picked up ~ you probally shouldn't have done that, you might have malware that overrides the safety on your ship now :3");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation("[RikaMod: RandomUSB.cs] RandomUSB picked up");
        }

    }
    
    public override List<Tooltip> GetExtraTooltips()
    {
        return new List<Tooltip>
        {
            new TTCard
            {
                card = new TrashAutoShoot{ temporaryOverride = false }
                // card = new SpareShot{ temporaryOverride = false }
            }
        };
    }
}
