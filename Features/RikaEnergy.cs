using System;
using Nickel;
using RikaMod.External;

namespace RikaMod.Features;

public class RikaEnergyManager : IKokoroApi.IV2.IStatusLogicApi.IHook
{

    internal static IStatusEntry RikaEnergy = null!;

    public RikaEnergyManager()
    {
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this);
    }

}

