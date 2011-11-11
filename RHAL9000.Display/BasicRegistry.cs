using Caliburn.Micro;
using RHAL9000.Core;
using StructureMap.Configuration.DSL;

namespace RHAL9000.Display
{
    public class BasicRegistry : Registry
    {
        public BasicRegistry()
        {
            For<IWindowManager>().Singleton().Use<WindowManager>();
            For<IEventAggregator>().Singleton().Use<EventAggregator>();
            For<IPollingMonitor>().Singleton().Use<RxPollingMonitor>();
            For<IShell>().Singleton().Use<ShellViewModel>();
        }

        public BasicRegistry AddViews()
        {
            // structuremap-for-use
            // structuremap-for-use-named
            // structuremap-for-singleton-use

            //For<IScreen>().Use<TeamCityBuildsViewModel>().Named("TeamCityBuilds");
            //For<IScreen>().Use<TVStreamsViewModel>().Named("TVStreams");

            return this;
        }
    }
}