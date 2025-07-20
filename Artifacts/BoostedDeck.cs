using System;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Artifacts;

public class BoostedDeck : Artifact, IRegisterable
{
    
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        
        helper.Content.Artifacts.RegisterArtifact(new ArtifactConfiguration
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new ArtifactMeta
            {
                pools = [ArtifactPool.Boss],
                owner = ModEntry.Instance.RikaDeck.Deck
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "BoostedDeck", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "BoostedDeck", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/BoostedDeck.png")).Sprite
        });
    }

    private static bool _isplaytester = ArtManager.IsPlayTester;
    
    public override void OnPlayerDeckShuffle(State state, Combat combat)
    {
        Combat combat1 = combat;
        AStatus a = new AStatus();
        a.status = Status.boost;
        a.statusAmount = 1;
        a.targetPlayer = true;
        combat1.QueueImmediate(a);
        Pulse();

        if (_isplaytester)
        {
            Console.WriteLine("[RikaMod] BoostedDeck proc");
        }
    }

}
