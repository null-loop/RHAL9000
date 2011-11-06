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

            // 

            return state;
        }

        public string Id { get; set; }
    }

    public class AwarenessState : IAwarenessState
    {
        public DateTime Began { get; set; }
        public DateTime? Ended { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Status { get; set; }
        public HumanAttentionRequirement AttentionRequired { get; set; }

        public event EventHandler<AwarenessStateUpdatedEventArgs> Updated;

        protected virtual void OnUpdated()
        {
            LastUpdated = DateTime.Now;

            var eh = Updated;
            var args = new AwarenessStateUpdatedEventArgs(Status, AttentionRequirement);

            if (eh != null)
                eh(this, args);
        }

        public HumanAttentionRequirement AttentionRequirement { get; set; }

        public AwarenessState(DateTime began, DateTime? ended, string status, HumanAttentionRequirement attentionRequirement)
        {
            Began = began;
            Ended = ended;
            Status = status;
            AttentionRequirement = attentionRequirement;
        }

        public void Finished(string status)
        {
            Status = status;
            Ended = DateTime.Now;
            AttentionRequirement = HumanAttentionRequirement.None;

            OnUpdated();
        }

        public void Change(string status)
        {
            Status = status;

            OnUpdated();
        }

        public void Change(string status, HumanAttentionRequirement attentionRequirement)
        {
            Status = status;
            AttentionRequired = attentionRequirement;
        }
    }
}