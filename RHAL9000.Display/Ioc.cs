using RHAL9000.Core.Configuration;
using RHAL9000.Monitors.Builds;
using StructureMap;

namespace RHAL9000.Display
{
    public static class Ioc
    {
        public static void Configure(IApplicationCore core, IApplicationCoreViewModel applicationCoreViewModel)
        {
            var basicRegistry = new BasicRegistry().AddViews();

            basicRegistry.For<IApplicationCore>().Use(core);
            basicRegistry.For<IApplicationCoreViewModel>().Use(applicationCoreViewModel);

            Container = new Container(basicRegistry);
        }

        public static Container Container { get; private set; }
    }
}