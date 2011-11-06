using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RHAL9000.Core;
using RHAL9000.Core.Configuration;

namespace RHAL9000.Display
{
    public class ApplicationCoreViewModel : ModelBase, IApplicationCoreViewModel
    {
        private IApplicationCore Core { get; set; }

        public ApplicationCoreViewModel(IApplicationCore core)
        {
            Core = core;
            DataSources = new BindableCollection<IDataSourceViewModel>(core.DataSources.Select(ds => new DataSourceViewModel(ds)));
        }


        private IObservableCollection<IDataSourceViewModel> _dataSources;

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
    }
}