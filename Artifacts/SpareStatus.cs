using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using RikaMod.Features;

namespace RikaMod.Artifacts;

public class SpareStatus : Artifact, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SpareStatus", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SpareStatus", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/ThisIconIsACryForHelp.png")).Sprite
        });
    }


    
    public override void OnTurnStart(State state, Combat combat)
    {
        Pulse();
        
        int x = state.rngActions.NextInt();
        x = x % 1000;
    
        if (x >= 0 && x <= 150)
        {
            Combat combat1 = combat;
            AStatus a = new AStatus();
            a.status = Status.evade;
            a.statusAmount = 1;
            a.targetPlayer = true;
            combat1.QueueImmediate(a);
        }
        if (x >= 151 && x <= 200)
        {
            Combat combat2 = combat;
            AStatus b = new AStatus();
            b.status = Status.droneShift;
            b.statusAmount = 1;
            b.targetPlayer = true;
            combat2.QueueImmediate(b);
        }
        if (x >= 201 && x <= 210)
        {
            Combat combat3 = combat;
            AStatus c = new AStatus();
            c.status = Status.perfectShield;
            c.statusAmount = 1;
            c.targetPlayer = true;
            combat3.QueueImmediate(c);
        }
        if (x >= 211 && x <= 240)
        {
            Combat combat4 = combat;
            AStatus d = new AStatus();
            d.status = Status.serenity;
            d.statusAmount = 1;
            d.targetPlayer = true;
            combat4.QueueImmediate(d);
        }
        if (x >= 241 && x <= 280)
        {
            Combat combat5 = combat;
            AStatus e = new AStatus();
            e.status = Status.maxShield;
            e.statusAmount = 1;
            e.targetPlayer = true;
            combat5.QueueImmediate(e);
        }
        if (x >= 281 && x <= 310)
        {
            Combat combat6 = combat;
            AStatus f = new AStatus();
            f.status = Status.shard;
            f.statusAmount = 1;
            f.targetPlayer = true;
            combat6.QueueImmediate(f);
        }
        if (x >= 311 && x <= 330)
        {
            Combat combat7 = combat;
            AStatus g = new AStatus();
            g.status = Status.maxShard;
            g.statusAmount = 1;
            g.targetPlayer = true;
            combat7.QueueImmediate(g);
        }
        if (x == 331)
        {
            Combat combat8 = combat;
            AStatus h = new AStatus();
            h.status = Status.mitosis;
            h.statusAmount = 1;
            h.targetPlayer = true;
            combat8.QueueImmediate(h);
        }
        if (x == 332)
        {
            Combat combat9 = combat;
            AStatus i = new AStatus();
            i.status = Status.payback;
            i.statusAmount = 1;
            i.targetPlayer = true;
            combat9.QueueImmediate(i);
        }
        if (x >= 333 && x <= 336)
        {
            Combat combat10 = combat;
            AStatus j = new AStatus();
            j.status = Status.tempPayback;
            j.statusAmount = 1;
            j.targetPlayer = true;
            combat10.QueueImmediate(j);
        }
        if (x >= 337 && x <= 387)
        {
            Combat combat11 = combat;
            AStatus k = new AStatus();
            k.status = Status.stunCharge;
            k.statusAmount = 1;
            k.targetPlayer = true;
            combat11.QueueImmediate(k);
        }
        if (x == 388)
        {
            Combat combat12 = combat;
            AStatus m = new AStatus();
            m.status = Status.stunSource;
            m.statusAmount = 1;
            m.targetPlayer = true;
            combat12.QueueImmediate(m);
        }
        if (x == 389)
        {
            Combat combat13 = combat;
            AStatus n = new AStatus();
            n.status = Status.ace;
            n.statusAmount = 1;
            n.targetPlayer = true;
            combat13.QueueImmediate(n);
        }
        if (x >= 390 && x <= 450)
        {
            Combat combat14 = combat;
            AStatus o = new AStatus();
            o.status = Status.hermes;
            o.statusAmount = 1;
            o.targetPlayer = true;
            combat14.QueueImmediate(o);
        }
        if (x >= 451 && x <= 500)
        {
            Combat combat15 = combat;
            AStatus p = new AStatus();
            p.status = Status.drawNextTurn;
            p.statusAmount = 1;
            p.targetPlayer = true;
            combat15.QueueImmediate(p);
        }
        if (x >= 501 && x <= 550)
        {
            Combat combat16 = combat;
            AStatus q = new AStatus();
            q.status = Status.energyNextTurn;
            q.statusAmount = 1;
            q.targetPlayer = true;
            combat16.QueueImmediate(q);
        }
        if (x == 551)
        {
            Combat combat17 = combat;
            AStatus r = new AStatus();
            r.status = Status.strafe;
            r.statusAmount = 1;
            r.targetPlayer = true;
            combat17.QueueImmediate(r);
        }
        if (x == 552)
        {
            Combat combat18 = combat;
            AStatus s = new AStatus();
            s.status = Status.endlessMagazine;
            s.statusAmount = 1;
            s.targetPlayer = true;
            combat18.QueueImmediate(s);
        }
        if (x >= 553 && x <= 602)
        {
            Combat combat19 = combat;
            AStatus t = new AStatus();
            t.status = Status.overdrive;
            t.statusAmount = 1;
            t.targetPlayer = true;
            combat19.QueueImmediate(t);
        }
        if (x >= 603 && x <= 613)
        {
            Combat combat21 = combat;
            AStatus u = new AStatus();
            u.status = Status.powerdrive;
            u.statusAmount = 1;
            u.targetPlayer = true;
            combat21.QueueImmediate(u);
        }
        if (x >= 614 && x <= 619)
        {
            Combat combat22 = combat;
            AStatus v = new AStatus();
            v.status = Status.tableFlip;
            v.statusAmount = 1;
            v.targetPlayer = true;
            combat22.QueueImmediate(v);
        }
        if (x >= 620 && x <= 670)
        {
            Combat combat23 = combat;
            AStatus w = new AStatus();
            w.status = Status.bubbleJuice;
            w.statusAmount = 1;
            w.targetPlayer = true;
            combat23.QueueImmediate(w);
        }
        if (x == 671)
        {
            Combat combat24 = combat;
            AStatus y = new AStatus();
            y.status = Status.rockFactory;
            y.statusAmount = 1;
            y.targetPlayer = true;
            combat24.QueueImmediate(y);
        }
        if (x >= 672 && x <= 711)
        {
            Combat combat25 = combat;
            AStatus z = new AStatus();
            z.status = Status.autododgeRight;
            z.statusAmount = 1;
            z.targetPlayer = true;
            combat25.QueueImmediate(z);
        }
        if (x >= 712 && x <= 721)
        {
            Combat combat26 = combat;
            AStatus aa = new AStatus();
            aa.status = Status.autododgeLeft;
            aa.statusAmount = 1;
            aa.targetPlayer = true;
            combat26.QueueImmediate(aa);
        }
        if (x >= 722 && x <= 771)
        {
            Combat combat27 = combat;
            AStatus ab = new AStatus();
            ab.status = Status.boost;
            ab.statusAmount = 1;
            ab.targetPlayer = true;
            combat27.QueueImmediate(ab);
        }
        if (x >= 772 && x <= 871)
        {
            Combat combat28 = combat;
            AStatus ac = new AStatus();
            ac.status = Status.autopilot;
            ac.statusAmount = 1;
            ac.targetPlayer = true;
            combat28.QueueImmediate(ac);
        }
        if (x == 872)
        {
            Combat combat29 = combat;
            AStatus ad = new AStatus();
            ad.status = Status.cleanExhaust;
            ad.statusAmount = 1;
            ad.targetPlayer = true;
            combat29.QueueImmediate(ad);
        }
        if (x == 873)
        {
            Combat combat30 = combat;
            AStatus ae = new AStatus();
            ae.status = Status.quarry;
            ae.statusAmount = 1;
            ae.targetPlayer = true;
            combat30.QueueImmediate(ae);
        }
        if (x >= 874 && x <= 924)
        {
            Combat combat31 = combat;
            AStatus af = new AStatus();
            af.status = Status.libra;
            af.statusAmount = 1;
            af.targetPlayer = true;
            combat31.QueueImmediate(af);
        }
        if (x >= 925 && x <= 999)
        {
            Combat combat32 = combat;
            AStatus ag = new AStatus();
            ag.status = RikaFluxManager.RikaFlux.Status;
            ag.statusAmount = 1;
            ag.targetPlayer = true;
            combat32.QueueImmediate(ag);
        }
    }
}
