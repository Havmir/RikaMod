using Nanoray.PluginManager;
using Nickel;

namespace RikaMod;

internal interface IRegisterable
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}