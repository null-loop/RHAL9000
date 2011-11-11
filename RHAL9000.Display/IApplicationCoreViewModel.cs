using Caliburn.Micro;
using RHAL9000.Core.Configuration;

namespace RHAL9000.Display
{
    public interface IApplicationCoreViewModel : IOutlook
    {
        IObservableCollection<IDataSourceViewModel> DataSources { get; }
        IObservableCollection<IOutlookViewModel> Outlooks { get; }
    }
}