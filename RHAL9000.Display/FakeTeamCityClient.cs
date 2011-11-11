using System;
using System.Collections.Generic;
using RHAL9000.Monitors.Builds;

namespace RHAL9000.Display
{
    internal class FakeTeamCityClient : IBuildClient
    {
        public FakeTeamCityClient(string uri, string username, string password)
        {
        }

        public IEnumerable<ISummary> GetAllProjectSummaries()
        {
            return new[] {new Summary("project1", "Continous Integration")};
        }

        public IBuildProject GetProject(string projectId)
        {
            if (projectId == "project1")
                return new BuildProject
                           {
                               Archived = false,
                               Id = "project1",
                               Name = "Continous Integration",
                               Description = "Automated builds from development branches",
                               BuildTypes = new[] {new Summary("bt1", "Proving Build"),new Summary("bt2","Metrics build"), }
                           };

            throw new NotImplementedException("Can't do " + projectId);
        }

        public IBuildType GetBuildType(string buildTypeId)
        {
            if (buildTypeId == "bt1")
            {
                return new BuildType { Id = "bt1", Name = "Proving", BuildProjectId = "project1", Description = "Proves the compilation and unit tests on the development branch" };
            }
            if (buildTypeId == "bt2")
            {
                return new BuildType { Id = "bt2", Name = "Metrics", BuildProjectId = "project1", Description = "Adds test code coverage metrics and duplication analysis" };
            }

            throw new NotImplementedException("Can't do " + buildTypeId);
        }

        public IEnumerable<IBuildResult> GetCompletedBuilds()
        {
            return new[]
                       {
                           new BuildResult {Id = "2522", Number = "2522", BuildTypeId = "bt1", Status = BuildStatus.Failure},
                           new BuildResult {Id = "2523", Number = "2523", BuildTypeId = "bt1", Status = BuildStatus.Success},
                           new BuildResult {Id = "2524", Number = "2524", BuildTypeId = "bt1", Status = BuildStatus.Success}
                       };

            throw new NotImplementedException();
        }

        public IBuildProgress GetInProgressBuild(string buildTypeId)
        {
            if (buildTypeId == "bt1")
            {
                return new BuildProgress
                           {
                               AgentId = "agent1",
                               BuildTypeId = "bt1",
                               CurrentStage = "Compiling CaternetClub.Web",
                               ElapsedRunTime = TimeSpan.FromMinutes(3),
                               EstimatedRunTime = TimeSpan.FromMinutes(9),
                               History = false,
                               Id = "2525",
                               Number = "2525",
                               Outdated = false,
                               PercentageComplete = 33,
                               ProbablyHanging = false,
                               StatusText = "Compiling CaternetClub.Web",
                               Status = BuildStatus.Unknown
                           };
            }
            return null;
        }

        public IFullBuildResult GetCompletedBuild(string buildId)
        {
            if (buildId == "2522")
            {
                return new FullBuildResult
                           {
                               Id = "2522",
                               AgentId = "agent1",
                               BuildTypeId = "bt1",
                               Finished = DateTime.Now,
                               History = false,
                               Number = "2522",
                               Personal = false,
                               Pinned = false,
                               Started = DateTime.Now.Subtract(TimeSpan.FromMinutes(10)),
                               Status = BuildStatus.Failure,
                               StatusText = "Compile error in CaternetClub.Web"
                           };
            }
            if (buildId == "2523")
            {
                return new FullBuildResult
                           {
                               Id = "2523",
                               AgentId = "agent1",
                               BuildTypeId = "bt1",
                               Finished = DateTime.Now,
                               History = false,
                               Number = "2523",
                               Personal = false,
                               Pinned = false,
                               Started = DateTime.Now.Subtract(TimeSpan.FromMinutes(10)),
                               Status = BuildStatus.Success,
                               StatusText = "2589 unit tests run, 1 ignored"
                           };
            }
            if (buildId == "2524")
            {
                return new FullBuildResult
                           {
                               Id = "2524",
                               AgentId = "agent1",
                               BuildTypeId = "bt1",
                               Finished = DateTime.Now,
                               History = false,
                               Number = "2524",
                               Personal = false,
                               Pinned = false,
                               Started = DateTime.Now.Subtract(TimeSpan.FromMinutes(10)),
                               Status = BuildStatus.Success,
                               StatusText = "2589 unit tests run, 1 ignored"
                           };
            }
            throw new NotImplementedException("Can't do " + buildId);
        }
    }
}