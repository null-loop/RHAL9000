using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using RHAL9000.Core;

namespace RHAL9000.Monitors.Builds
{
    public class BuildTypeModel : ModelBase
    {
        private BuildProjectModel _BuildProject;
        private string _Description;
        private string _Id;

        private string _Name;
        private bool _Paused;
        private Dictionary<string, string> _RunParameters;
        private Uri _WebUri;

        public BuildTypeModel()
        {
            Builds = new ObservableCollection<BuildModel>();
            LatestBuilds = new ObservableCollection<BuildModel>();
            Builds.CollectionChanged += Builds_CollectionChanged;
        }

        public string Id
        {
            get { return _Id; }
            set { SetField(ref _Id, value, () => Id); }
        }

        public string Name
        {
            get { return _Name; }
            set { SetField(ref _Name, value, () => Name); }
        }

        public string Description
        {
            get { return _Description; }
            set { SetField(ref _Description, value, () => Description); }
        }

        public bool Paused
        {
            get { return _Paused; }
            set { SetField(ref _Paused, value, () => Paused); }
        }

        public Dictionary<string, string> RunParameters
        {
            get { return _RunParameters; }
            set { SetField(ref _RunParameters, value, () => RunParameters); }
        }

        public Uri WebUri
        {
            get { return _WebUri; }
            set { SetField(ref _WebUri, value, () => WebUri); }
        }

        public BuildProjectModel BuildProject
        {
            get { return _BuildProject; }
            set { SetField(ref _BuildProject, value, () => BuildProject); }
        }

        public ObservableCollection<BuildModel> Builds { get; set; }

        public ObservableCollection<BuildModel> LatestBuilds { get; set; }

        private void Builds_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshLatest();
        }

        public void RefreshLatest()
        {
            RefilterLatestBuilds(2);
        }

        private void RefilterLatestBuilds(int count)
        {
            BuildModel[] sorted = Builds.OrderByDescending(b => b.Started).ToArray();
            IEnumerable<BuildModel> running = sorted.Where(b => b.IsRunning);
            IEnumerable<BuildModel> finished = sorted.Where(b => !b.IsRunning).Take(count);

            var newLatest = new List<BuildModel>();

            foreach (BuildModel r in running)
                newLatest.Add(r);
            foreach (BuildModel f in finished)
                newLatest.Add(f);

            // compare the two...
            for (int i = 0; i != newLatest.Count; i++)
            {
                BuildModel desired = newLatest[i];

                if (i >= LatestBuilds.Count)
                {
                    LatestBuilds.Add(desired);
                }
                else
                {
                    BuildModel actual = LatestBuilds[i];

                    if (desired != actual)
                        LatestBuilds[i] = desired;
                }
            }

            // if there's any more in LatestBuilds than in newLatest remove them...
            while (LatestBuilds.Count > newLatest.Count)
            {
                LatestBuilds.RemoveAt(LatestBuilds.Count - 1);
            }
        }
    }
}