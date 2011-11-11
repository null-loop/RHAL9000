using System;
using System.Collections.Generic;
using RHAL9000.Core;

namespace RHAL9000.Monitors.Builds
{
    public abstract class DataSourceBase : IDataSource, IHasAwarenessStates
    {
        public IEnumerable<IAwarenessState> States
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<HasAwarnessStatesUpdatedEventArgs> Updated;

        protected IAwarenessState RegisterState(string status, HumanAttentionRequirement attentionRequirement)
        {
            var state = new AwarenessState(DateTime.Now, null, status, attentionRequirement);
            throw new NotImplementedException();
            // 

            return state;
        }

        public string Id { get; set; }
    }
}