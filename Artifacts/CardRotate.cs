using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Artifacts;

public class CardRotate : Artifact, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CardRotate", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CardRotate", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/BadArtifactIcon.png")).Sprite
        });
    }

    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    
    public override void OnTurnStart(State state, Combat combat)
    {
        Combat combat2 = combat;
        ADrawCard b = new ADrawCard();
        b.count = 1;
        combat2.QueueImmediate(b);
        
        Combat combat1 = combat;
        ADiscard a = new ADiscard();
        a.count = 1;
        combat1.QueueImmediate(a);
        
        Combat combat0 = combat;
        ADiscard z = new ADiscard();
        z.count = 0;
        z.timer = 1;
        combat0.QueueImmediate(z);
        Pulse();

        if (_isplaytester)
        {
            Console.Write("[RikaMod] Card Rotate Proc");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: CardRotate.cs] Card Rotate Proc | Turn: {combat.turn}");
        }
    }
}
