using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace RikaMod.Artifacts;

public class RandomUsb : Artifact, IRegisterable
{
    
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
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
    
    public override void OnReceiveArtifact(State state)
    {
        state.GetCurrentQueue().QueueImmediate(new ACardOffering
        {
            amount = 5,
            limitDeck = ModEntry.Instance.RikaDeck.Deck,
            makeAllCardsTemporary = false,
            canSkip = true
        });
        state.GetCurrentQueue().QueueImmediate(new AAddCard
        {
            card = new TrashAutoShoot{ temporaryOverride = false },
            destination = CardDestination.Deck,
            amount = 1,
            insertRandomly = true
        });
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
