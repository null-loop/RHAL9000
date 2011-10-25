using System.Reactive.Linq;
using RHAL9000.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RHAL9000.Monitors.Builds;

namespace RHAL9000.Display.Builds
{
    public class TeamCityBuildsViewModel : SingleMonitorPollingViewModel
    {
        private IObservable<BuildModel> _eventSource;
        private IDisposable _eventHandler;

        private readonly object _projectLoadLock = new object();

        private ObservableCollection<BuildProjectModel> _projects;

        public ObservableCollection<BuildProjectModel> Projects
        {
            get { return _projects; }
            set { 
                _projects = value;
                NotifyOfPropertyChange(() => Projects);
            }
        }

        protected override IPollingMonitor CreateMonitor(Dictionary<string, string> configuration)
        {
            var username = configuration["Username"];
            var password = configuration["Password"];
            var uri = configuration["Uri"];
            var buildTypeIds = new string[0];

            if (configuration.ContainsKey("BuildTypeIds"))
            {
                buildTypeIds = configuration["BuildTypeIds"].Split(',');
            }

            var client = new TeamCityClient(uri, username, password);

            var monitor = new TeamCityBuildMonitor(client, buildTypeIds);

            _eventSource = Observable.FromEventPattern<BuildEventArgs>(monitor, "BuildUpdated").Select(e=>e.EventArgs.Build);
            
            _eventHandler = _eventSource.ObserveOnDispatcher().Subscribe(ReceiveBuild);

            return monitor;
        }

        public override void Deactivate(bool close)
        {
            if (close && _eventHandler!=null)
            {
                _eventHandler.Dispose();
            }
        }

        public override void Poll()
        {
            PollingMonitor.Poll();
        }

        public override TimeSpan TimeUntilNextPoll
        {
            get { return PollingMonitor.TimeUntilNextPoll; }
        }

        private void ReceiveBuild(BuildModel build)
        {
            if (Projects==null)
            {
                lock (_projectLoadLock)
                {
                    if (Projects == null) 
                    {
                        var m = PollingMonitor as TeamCityBuildMonitor;

                        Projects = new ObservableCollection<BuildProjectModel>(m.GetAllBuildProjects());
                    }
                }
            }

            var ex = build.BuildType.Builds.Where(b => b.Id == build.Id).FirstOrDefault();

            if (ex == null)
            {
                build.BuildType.Builds.Add(build);
            }
            else
            {
                var reAdd = ex.IsRunning && !build.IsRunning;

                // update the one we've got...
                ex.CurrentStage = build.CurrentStage;
                ex.ElapsedRunTime = build.ElapsedRunTime;
                ex.EstimatedRunTime = build.EstimatedRunTime;
                ex.Finished = build.Finished;
                ex.History = build.History;
                ex.IsRunning = build.IsRunning;
                ex.Number = build.Number;
                ex.OutDated = build.OutDated;
                ex.PercentageComplete = build.PercentageComplete;
                ex.Personal = build.Personal;
                ex.Pinned = build.Pinned;
                ex.ProbablyHanging = build.ProbablyHanging;
                ex.Started = build.Started;
                ex.Status = build.Status;
                ex.StatusText = build.StatusText;

                if (reAdd)
                {
                    ex.BuildType.Builds.Remove(ex);
                    ex.BuildType.Builds.Add(ex);
                }
            }

             build.BuildType.RefreshLatest();
        }
    }
}
