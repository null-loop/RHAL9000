using Caliburn.Micro;
using RHAL9000.Core;
using RHAL9000.Display.Builds;
using RHAL9000.Display.TV;
using RHAL9000.Monitors.Builds;
using StructureMap;

namespace RHAL9000.Display
{
    public static class Ioc
    {
        public static void Configure()
        {
            Container = new Container();
            Container.Configure(x=>
                                    {
                                        x.For<IWindowManager>().Singleton().Use<WindowManager>();
                                        x.For<IEventAggregator>().Singleton().Use<EventAggregator>();
                                        x.For<IShell>().Singleton().Use<ShellViewModel>();
                                        x.For<ICalScreen>().Use<TeamCityBuildsViewModel>().Named("TeamCityBuilds");
                                        x.For<ICalScreen>().Use<TVStreamsViewModel>().Named("TVStreams");
                                    });

        }

        public static Container Container { get; private set; }
    }
}