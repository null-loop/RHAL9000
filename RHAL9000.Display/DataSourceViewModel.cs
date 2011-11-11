using RHAL9000.Core;

namespace RHAL9000.Display
{
    public class DataSourceViewModel : ModelBase, IDataSourceViewModel
    {
        private IDataSource DataSource { get; set; }

        private object _statesLock = new object();

        public DataSourceViewModel(IDataSource dataSource)
        {
            DataSource = dataSource;
            
        }

    }
}