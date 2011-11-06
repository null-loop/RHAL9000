using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace RHAL9000.Core.Configuration
{

    public interface IApplicationCore
    {
        IEnumerable<IDataSource> DataSources { get; }
        IEnumerable<IScreen> Outlooks { get; }
    }

    //TODO:Introduce XML namespaces
}
