using System;
using System.Collections.Generic;
using System.Linq;

namespace RHAL9000.Monitors.Builds
{
    public class TeamCityBuildMonitor : IBuildMonitor
    {
        //TODO:Monitor multiple builds.

        private readonly List<string> _completedBuildIds = new List<string>();

        private List<BuildProjectModel> _buildProjects;
        private IBuildProgress _inProgressBuild;

        public TeamCityBuildMonitor(IBuildClient client, params string[] buildTypeIds)
        {
            Client = client;
            BuildTypeIds = buildTypeIds;
        }

        private IBuildClient Client { get; set; }
        private string[] BuildTypeIds { get; set; }

        public bool HasInProgressBuild
        {
            get { return _inProgressBuild != null; }
        }

        #region IBuildMonitor Members

        public void Initialise()
        {
            LoadBuildProjectCache();
            Poll();
        }

        public void Poll()
        {
            if (HasInProgressBuild)
                UpdateInProgressBuildStatus();

            CheckForNewBuilds();
        }

        public TimeSpan TimeUntilNextPoll
        {
            get
            {
                if (HasInProgressBuild) return TimeSpan.FromSeconds(10);
                else return TimeSpan.FromSeconds(10);
            }
        }

        public event EventHandler<BuildEventArgs> BuildUpdated;

        public IEnumerable<BuildProjectModel> GetAllBuildProjects()
        {
            return _buildProjects;
        }

        #endregion

        private void LoadBuildProjectCache()
        {
            IEnumerable<ISummary> projectSummaries = Client.GetAllProjectSummaries();
            IBuildProject[] projects = projectSummaries.Select(p => Client.GetProject(p.Id)).ToArray();

            IEnumerable<IBuildType> types = null;

            if (BuildTypeIds.Count() > 0)
            {
                types =
                    projects.SelectMany(p => p.BuildTypes).Where(s => BuildTypeIds.Contains(s.Id)).Select(
                        t => Client.GetBuildType(t.Id)).ToArray();
            }
            else
            {
                types = projects.SelectMany(p => p.BuildTypes).Select(t => Client.GetBuildType(t.Id)).ToArray();
            }

            _buildProjects = MergeProjectsAndTypes(projects, types).ToList();
        }

        private IEnumerable<BuildProjectModel> MergeProjectsAndTypes(IEnumerable<IBuildProject> projects,
                                                                     IEnumerable<IBuildType> types)
        {
            // filter projects to only those with a type...
            IEnumerable<IBuildProject> requiredProjects =
                projects.Where(p => types.Count(t => t.BuildProjectId == p.Id) > 0);

            BuildProjectModel[] r = requiredProjects.Select(project => new BuildProjectModel
                                    {
                                        Id = project.Id,
                                        Name = project.Name,
                                        WebUri = project.WebUri,
                                        Description = project.Description,
                                        Archived = project.Archived,
                                        BuildTypes =
                                            types.Where(t => project.BuildTypes.Any(bt => bt.Id == t.Id))
                                            .Select(b => new BuildTypeModel
                                                            {
                                                                Id = b.Id,
                                                                Name = b.Name,
                                                                Description = b.Description,
                                                                Paused = b.Paused,
                                                                RunParameters = b.RunParameters,
                                                                WebUri = b.WebUri
                                                            }).ToList()
                                    }).ToArray();

            // set refs from types back to projects

            foreach (BuildProjectModel project in r)
            {
                foreach (BuildTypeModel buildType in project.BuildTypes)
                {
                    buildType.BuildProject = project;
                }
            }

            return r;
        }

        private void CheckForNewBuilds()
        {
            //TODO:Sort out the inprogress builds.
            if (_inProgressBuild == null)
            {
                // enumerate all the build types checking for an inprogress build...
                foreach (BuildTypeModel buildType in _buildProjects.SelectMany(p => p.BuildTypes))
                {
                    IBuildProgress inProgressBuild = Client.GetInProgressBuild(buildType.Id);

                    if (inProgressBuild != null)
                    {
                        _inProgressBuild = inProgressBuild;
                        OnBuildUpdated(CreateBuildModel(_inProgressBuild));
                    }
                }
            }

            // get all completed builds and trigger events
            IEnumerable<IBuildResult> completedBuilds = Client.GetCompletedBuilds();

            IEnumerable<IBuildResult> unseenBuilds =
                completedBuilds == null ? new IBuildResult[0] :
                completedBuilds.Where(
                    c =>
                    !_completedBuildIds.Contains(c.Id) &&
                    (BuildTypeIds.Length == 0 || BuildTypeIds.Contains(c.BuildTypeId)));

            foreach (IBuildResult unseenBuild in unseenBuilds)
            {
                IFullBuildResult detailedUnseenBuild = Client.GetCompletedBuild(unseenBuild.Id);

                _completedBuildIds.Add(unseenBuild.Id);

                OnBuildUpdated(CreateBuildModel(detailedUnseenBuild));
            }
        }

        private void UpdateInProgressBuildStatus()
        {
            // get the latest version of the _inprogressBuild...

            IBuildProgress latestBuild = Client.GetInProgressBuild(_inProgressBuild.BuildTypeId);

            if (latestBuild == null)
            {
                // build has finished!
                _inProgressBuild = null;
            }
            else
            {
                _inProgressBuild = latestBuild;

                OnBuildUpdated(CreateBuildModel(_inProgressBuild));
            }
        }

        private BuildModel CreateBuildModel(IFullBuildResult source)
        {
            return new BuildModel
                       {
                           Id = source.Id,
                           Number = source.Number,
                           Status = source.Status,
                           WebUri = source.WebUri,
                           Personal = source.Personal,
                           History = source.History,
                           Pinned = source.Pinned,
                           StatusText = source.StatusText,
                           BuildType =
                               _buildProjects.SelectMany(b => b.BuildTypes).Where(t => t.Id == source.BuildTypeId).
                               FirstOrDefault(),
                           Started = source.Started,
                           Finished = source.Finished,
                           AgentId = source.AgentId,
                           PercentageComplete = null,
                           ElapsedRunTime = source.Finished - source.Started,
                           EstimatedRunTime = null,
                           CurrentStage = null,
                           OutDated = null,
                           ProbablyHanging = false,
                           IsRunning = false
                       };
        }

        private BuildModel CreateBuildModel(IBuildProgress source)
        {
            return new BuildModel
                       {
                           Id = source.Id,
                           Number = source.Number,
                           Status = source.Status,
                           WebUri = source.WebUri,
                           Personal = source.Personal,
                           History = source.History,
                           Pinned = source.Pinned,
                           StatusText = source.StatusText,
                           BuildType =
                               _buildProjects.SelectMany(b => b.BuildTypes).Where(t => t.Id == source.BuildTypeId).
                               FirstOrDefault(),
                           Started = source.Started,
                           Finished = null,
                           AgentId = source.AgentId,
                           PercentageComplete = source.PercentageComplete,
                           ElapsedRunTime = source.ElapsedRunTime,
                           EstimatedRunTime = source.EstimatedRunTime,
                           CurrentStage = source.CurrentStage,
                           OutDated = source.Outdated,
                           ProbablyHanging = source.ProbablyHanging,
                           IsRunning = true
                       };
        }

        protected virtual void OnBuildUpdated(BuildModel build)
        {
            EventHandler<BuildEventArgs> eh = BuildUpdated;
            if (eh != null) eh(this, new BuildEventArgs(build));
        }
    }
}