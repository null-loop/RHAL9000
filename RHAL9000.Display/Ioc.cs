using System.Linq;
using RHAL9000.Core;
using RHAL9000.Core.Configuration;
using StructureMap;

namespace RHAL9000.Display
{
    public static class Ioc
    {
        public static void RegisterFromCore(IApplicationCore core, IApplicationCoreViewModel applicationCoreViewModel)
        {
            var basicRegistry = new BasicRegistry().AddViews();

            basicRegistry.For<IApplicationCore>().Use(core);
            basicRegistry.For<IApplicationCoreViewModel>().Use(applicationCoreViewModel);

            // register instances for types from core
            core.DataSources.ToList().ForEach(d => basicRegistry.For<IDataSource>().Singleton().Use(d).Named(d.Id));
            core.Outlooks.ToList().ForEach(o=>basicRegistry.For<IOutlook>().Singleton().Use(o).Named(o.Id));

            Container = new Container(basicRegistry);
        }

        public static Container Container { get; private set; }
    }
}