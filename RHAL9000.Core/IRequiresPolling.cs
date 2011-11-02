using System;

namespace RHAL9000.Core
{
    public interface IRequiresPolling
    {
        void Poll();
        TimeSpan TimeUntilNextPoll { get; }
        bool Stopping { get; set; }
    }
}