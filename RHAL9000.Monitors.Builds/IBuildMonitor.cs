using System;
using System.Collections.Generic;
using RHAL9000.Core;

namespace RHAL9000.Monitors.Builds
{
    public interface IBuildMonitor : IRequiresPolling, IRequiresInitialisation, IDataSource
    {
        event EventHandler<BuildEventArgs> BuildUpdated;
        IEnumerable<BuildProjectModel> GetAllBuildProjects();
    }
}