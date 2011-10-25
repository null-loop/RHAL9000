using Caliburn.Micro;
using RHAL9000.Core;
using RHAL9000.Display.Builds;
using RHAL9000.Display.TV;
using StructureMap.Configuration.DSL;

namespace RHAL9000.Display
{
    public class BasicRegistry : Registry
    {
        public BasicRegistry()
        {
            For<IWindowManager>().Singleton().Use<WindowManager>();
            For<IEventAggregator>().Singleton().Use<EventAggregator>();
        }

        public BasicRegistry AddViews()
        {
            For<IShell>().Singleton().Use<ShellViewModel>();
            For<ICalScreen>().Use<TeamCityBuildsViewModel>().Named("TeamCityBuilds");
            For<ICalScreen>().Use<TVStreamsViewModel>().Named("TVStreams");
            return this;
        }
    }
}