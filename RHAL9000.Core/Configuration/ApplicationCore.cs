using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace RHAL9000.Core.Configuration
{
    public class ApplicationCore : IApplicationCore
    {
        public IEnumerable<IDataSource> DataSources { get; private set; }
        public IEnumerable<IScreen> Outlooks { get; private set; }

        public ApplicationCore(IEnumerable<IDataSource> dataSources, IEnumerable<IScreen> outlooks)
        {
            DataSources = new ObservableCollection<IDataSource>(dataSources);
            Outlooks = new ObservableCollection<IScreen>(outlooks);
        }

    }
}