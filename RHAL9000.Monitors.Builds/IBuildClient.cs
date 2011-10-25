using System.Collections.Generic;

namespace RHAL9000.Monitors.Builds
{
    public interface IBuildClient
    {
        IEnumerable<ISummary> GetAllProjectSummaries();
        IBuildProject GetProject(string projectId);
        IBuildType GetBuildType(string buildTypeId);
        IEnumerable<IBuildResult> GetCompletedBuilds();
        IBuildProgress GetInProgressBuild(string buildTypeId);
        IFullBuildResult GetCompletedBuild(string buildId);
    }
}