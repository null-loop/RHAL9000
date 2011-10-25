using System;
using System.Collections.Generic;
using RHAL9000.Core;

namespace RHAL9000.Display
{
    public abstract class SingleMonitorPollingViewModel : ModelBase, IPollingViewModel, ICalScreen
    {
        public void Configure(Dictionary<string, string> dictionary)
        {
            PollingMonitor = CreateMonitor(dictionary);
        }

        public void AddItem(object item)
        {
            throw new NotImplementedException();
        }

        protected IPollingMonitor PollingMonitor { get; set; }

        protected abstract IPollingMonitor CreateMonitor(Dictionary<string, string> dictionary);

        public void Activate()
        {
            PollingMonitor.Initialise();
        }

        public abstract void Deactivate(bool close);

        public abstract void Poll();

        public abstract TimeSpan TimeUntilNextPoll { get; }
    }
}