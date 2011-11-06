using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace RHAL9000.Core
{
    public class RxPollingMonitor : IPollingMonitor
    {
        private readonly IList<IRequiresPolling> _polling = new List<IRequiresPolling>();
        private readonly IDictionary<IRequiresPolling,IObservable<IRequiresPolling>> _pollingSources = new Dictionary<IRequiresPolling,IObservable<IRequiresPolling>>();
        private readonly IDictionary<IRequiresPolling,IDisposable> _pollingHandlers = new Dictionary<IRequiresPolling,IDisposable>();

        public void Run(IRequiresPolling requiresPolling)
        {
            if (_polling.Contains(requiresPolling))
                return;

            var timeSource = Observable.Generate(requiresPolling, r => r.Stopping, r => r, r => r, r => r.TimeUntilNextPoll);
            var handler = timeSource.Subscribe(s => s.Poll());

            _polling.Add(requiresPolling);
            _pollingSources.Add(requiresPolling, timeSource);
            _pollingHandlers.Add(requiresPolling, handler);
        }

        public void Stop(IRequiresPolling requiresPolling)
        {
            requiresPolling.Stopping = true;
            if (!_polling.Contains(requiresPolling))
                return;

            _polling.Remove(requiresPolling);
            _pollingSources.Remove(requiresPolling);

            var handler = _pollingHandlers[requiresPolling];

            handler.Dispose();

            _pollingHandlers.Remove(requiresPolling);
        }
    }
}