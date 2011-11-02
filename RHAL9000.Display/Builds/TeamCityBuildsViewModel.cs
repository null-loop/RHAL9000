using System.Reactive.Linq;
using Caliburn.Micro;
using RHAL9000.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RHAL9000.Monitors.Builds;

namespace RHAL9000.Display.Builds
{
    public class TeamCityBuildsViewModel : Screen
    {
        private IBuildMonitor BuildMonitor { get; set; }
        private IObservable<BuildModel> _eventSource;
        private IDisposable _eventHandler;

        private readonly object _projectLoadLock = new object();

        public TeamCityBuildsViewModel(IBuildMonitor buildMonitor)
        {
            if (buildMonitor == null) throw new ArgumentNullException("buildMonitor");

            BuildMonitor = buildMonitor;
        }

        private void BindBuildMonitor(IBuildMonitor buildMonitor)
        {
            _eventSource = Observable.FromEventPattern<BuildEventArgs>(buildMonitor, "BuildUpdated").Select(e => e.EventArgs.Build);
            _eventHandler = _eventSource.ObserveOnDispatcher().Subscribe(ReceiveBuild);
        }

        private ObservableCollection<BuildProjectModel> _projects;

        public ObservableCollection<BuildProjectModel> Projects
        {
            get { return _projects; }
            set { 
                _projects = value;
                NotifyOfPropertyChange(() => Projects);
            }
        }

        protected override void OnActivate()
        {
            if (_eventHandler == null)
            {
                BindBuildMonitor(BuildMonitor);
            }
        }

        protected override void OnDeactivate(bool close)
        {
            if (_eventHandler != null)
            {
                _eventHandler.Dispose();
            }
        }

        public TimeSpan TimeUntilNextPoll
        {
            get { return BuildMonitor.TimeUntilNextPoll; }
        }

        private void ReceiveBuild(BuildModel build)
        {
            if (Projects==null)
            {
                lock (_projectLoadLock)
                {
                    if (Projects == null) 
                    {
                        Projects = new ObservableCollection<BuildProjectModel>(BuildMonitor.GetAllBuildProjects());
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
