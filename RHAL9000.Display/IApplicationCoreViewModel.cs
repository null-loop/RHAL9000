using Caliburn.Micro;

namespace RHAL9000.Display
{
    public interface IApplicationCoreViewModel
    {
        IObservableCollection<IDataSourceViewModel> DataSources { get; }
        IObservableCollection<IOutlookViewModel> Outlooks { get; }
    }
}