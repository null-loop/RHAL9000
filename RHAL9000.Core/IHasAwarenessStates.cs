using System;
using System.Collections.Generic;

namespace RHAL9000.Core
{
    public interface IHasAwarenessStates
    {
        IEnumerable<IAwarenessState> States { get; }

        event EventHandler<HasAwarnessStatesUpdatedEventArgs> Updated;
    }
}