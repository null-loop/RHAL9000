using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RHAL9000.Core;

namespace RHAL9000.Display.TV
{
    public class TVStreamsViewModel : ModelBase, IPollingViewModel, ICalScreen
    {
        private List<StreamInformationConfiguration> _backlog = new List<StreamInformationConfiguration>();
        private StreamInformation _currentStream;
        private int _defaultTime = 60 * 100;

        public TVStreamsViewModel()
        {
            UpcomingStreams = new ObservableCollection<StreamInformation>();
        }

        public void Configure(Dictionary<string, string> dictionary)
        {
            var names = dictionary["StreamNames"].Split(',');
            var uris = dictionary["StreamUris"].Split(',');

            for(var i = 0; i!=names.Length;i++)
            {
                _backlog.Add(new StreamInformationConfiguration() { Name = names[i], LifeTime = TimeSpan.FromSeconds(_defaultTime), Uri = new Uri(uris[i]) });
            }
        }

        public void AddItem(object item)
        {
            throw new NotImplementedException();
        }

        public void Activate()
        {
            Poll();
        }

        public void Deactivate(bool close)
        {
        }

        public void Poll()
        {
            if (UpcomingStreams.Count == 0)
                FillFromBacklog();

            if (CurrentStream == null || CurrentStream.Until < DateTime.Now)
                MoveToNextStream();
        }

        private void FillFromBacklog()
        {
            StreamInformation current = null;

            foreach(var config in _backlog)
            {
                current = current == null ? 
                    new StreamInformation() { From = DateTime.Now, Until = DateTime.Now.Add(config.LifeTime), Name = config.Name, Uri = config.Uri } : 
                    new StreamInformation() { From = current.Until, Until = current.Until.Add(config.LifeTime), Name= config.Name, Uri = config.Uri };

                UpcomingStreams.Add(current);
            }
        }

        private void MoveToNextStream()
        {
            CurrentStream = UpcomingStreams.First();

            UpcomingStreams.Remove(CurrentStream);
        }

        public TimeSpan TimeUntilNextPoll
        {
            get { return TimeSpan.FromSeconds(5); }
        }

        public ObservableCollection<StreamInformation> UpcomingStreams { get; private set; }

        public StreamInformation CurrentStream
        {
            get { return _currentStream; }
            set
            {
                SetField(ref _currentStream, value, () => CurrentStream);
            }
        }
    }
}
