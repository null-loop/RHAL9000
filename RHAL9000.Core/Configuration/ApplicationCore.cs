using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RHAL9000.Core.Configuration
{
    public class ApplicationCore : IApplicationCore
    {
        public IEnumerable<IDataSource> DataSources { get; private set; }
        public IEnumerable<IOutlook> Outlooks { get; private set; }

        public ApplicationCore(IEnumerable<IDataSource> dataSources, IEnumerable<IOutlook> outlooks)
        {
            DataSources = new ObservableCollection<IDataSource>(dataSources);
            Outlooks = new ObservableCollection<IOutlook>(outlooks);
        }

    }
}