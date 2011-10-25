using System;

namespace RHAL9000.Core
{
    public interface IPollingMonitor
    {
        void Initialise();
        void Poll();
        TimeSpan TimeUntilNextPoll { get; }
    }
}