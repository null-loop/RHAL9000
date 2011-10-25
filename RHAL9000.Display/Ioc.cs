using RHAL9000.Monitors.Builds;
using StructureMap;

namespace RHAL9000.Display
{
    public static class Ioc
    {
        public static void Configure()
        {
            Container = new Container(new BasicRegistry().AddViews());
        }

        public static Container Container { get; private set; }
    }
}