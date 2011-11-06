using System;

namespace RHAL9000.Core
{
    public interface IDataSource : IHasAwarenessStates
    {
        string Id { get; }
    }
}