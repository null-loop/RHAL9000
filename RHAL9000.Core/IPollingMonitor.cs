namespace RHAL9000.Core
{
    public interface IPollingMonitor
    {
        void Run(IRequiresPolling requiresPolling);
        void Stop(IRequiresPolling requiresPolling);
    }
}