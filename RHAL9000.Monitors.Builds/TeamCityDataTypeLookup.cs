using System.Collections.Generic;
using RHAL9000.Core.Configuration;

namespace RHAL9000.Monitors.Builds
{
    public class TeamCityDataTypeLookup : TypeLookup
    {
        public TeamCityDataTypeLookup()
        {
            Register<TeamCityBuildMonitor>("BuildMonitor");
            Register<TeamCityClient>("TeamCityClient");
            Register<BuildTypeConfiguration>("BuildType");
            Register<BuildProjectConfiguration>("BuildProject");

            Register<List<BuildTypeConfiguration>>("BuildTypes");
            Register<List<BuildProjectConfiguration>>("BuildProjects");
        }
    }
}