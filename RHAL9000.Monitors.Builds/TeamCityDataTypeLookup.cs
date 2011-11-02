using System;
using System.Collections.Generic;
using RHAL9000.Core;
using RHAL9000.Core.Configuration;
using RHAL9000.Monitors.Builds;

namespace RHAL9000.Testing.TDD
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