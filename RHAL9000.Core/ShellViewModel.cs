using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RHAL9000.Core.Configuration;

namespace RHAL9000.Core
{
    public class ShellViewModel : IShell
    {
        public IApplicationCore Core { get; set; }

        public ShellViewModel(IApplicationCore core)
        {
            Core = core;
            Outlooks = new BindableCollection<IScreen>(Core.Outlooks.Cast<IScreen>());
        }

        public IObservableCollection<IScreen> Outlooks { get; private set; }
    }
}