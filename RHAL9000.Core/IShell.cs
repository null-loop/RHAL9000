using System;
using Caliburn.Micro;
using RHAL9000.Core.Configuration;

namespace RHAL9000.Core
{
    public interface IShell
    {
        IObservableCollection<IScreen> Outlooks { get; }
    }
}