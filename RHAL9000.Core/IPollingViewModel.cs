using System;

namespace RHAL9000.Core
{
    public interface IPollingViewModel : IAcceptsConfiguration
    {
        void Activate();
        void Deactivate(bool close);
        void Poll();
        TimeSpan TimeUntilNextPoll { get; }
    }
}