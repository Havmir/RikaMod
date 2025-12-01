using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Artifacts;

public class SpicyBoost : Artifact, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SpicyBoost", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SpicyBoost", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/TheIdeaForThisIconLookedBetterInsideMyHead.png")).Sprite
        });
    }

    public int HullLostManagersNumber(Combat combat)
    {
        return ModEntry.Instance.Helper.ModData.GetModDataOrDefault(combat, "hullLostNumber", 0);
    }
    
    private static bool _isplaytester = ArtManager.IsPlayTester;
    private static bool _logALotOfThings = ArtManager.LogALotOfThings;
    
    public override void OnPlayerLoseHull(State state, Combat combat, int amount)
    {
        Pulse();
        Combat combat1 = combat;
        AStatus a = new AStatus();
        a.status = Status.boost;
        a.statusAmount = amount;
        a.targetPlayer = true;
        combat1.QueueImmediate(a);

        if (_isplaytester)
        {
            Console.WriteLine($"[RikaMod] SpicyBoost proc for {amount}");
        }
        if (_logALotOfThings)
        {
            ModEntry.Instance.Logger.LogInformation($"[RikaMod: SpicyBoost.cs] SpicyBoost Proc | Hull lost amount: {amount} | Turn: {combat.turn}");
        }

    }
    
}
