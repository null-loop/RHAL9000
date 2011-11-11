using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RHAL9000.Core;
using RHAL9000.Core.Configuration;

namespace RHAL9000.Display
{
    public class ApplicationCoreViewModel : Screen, IApplicationCoreViewModel
    {
        public IApplicationCore Core { get; set; }
        
        private IObservableCollection<IDataSourceViewModel> _dataSources;

        protected override void OnActivate()
        {
            if (Core==null)
            {
                Core = Ioc.Container.GetInstance<IApplicationCore>();
            }
            base.OnActivate();
        }

        public ApplicationCoreViewModel(IApplicationCore core)
        {
            Core = core;
        }

        public ApplicationCoreViewModel()
        {
        }

        public IObservableCollection<IDataSourceViewModel> DataSources
        {
            get { return _dataSources; }
            private set
            {
                if (EqualityComparer<IObservableCollection<IDataSourceViewModel>>.Default.Equals(_dataSources, value)) return;

                _dataSources = value;
                NotifyOfPropertyChange(() => DataSources);
            }
        }

        private IObservableCollection<IOutlookViewModel> _outlooks;

        public IObservableCollection<IOutlookViewModel> Outlooks
        {
            get { return _outlooks; }
            set
            {
                if (EqualityComparer<IObservableCollection<IOutlookViewModel>>.Default.Equals(_outlooks, value)) return;

                _outlooks = value;
                NotifyOfPropertyChange(() => Outlooks);
            }
        }

        public string Id { get; set; }
    }
}