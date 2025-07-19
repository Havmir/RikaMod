using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace RikaMod.Artifacts;

public class MoralBoost : Artifact, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MoralBoost", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MoralBoost", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/QueerSleeves.png")).Sprite
        });
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        Combat combat1 = combat;
        AStatus a = new AStatus();
        a.status = Status.boost;
        a.statusAmount = 1;
        a.targetPlayer = true;
        combat1.QueueImmediate(a);
        Pulse();
    }

}
