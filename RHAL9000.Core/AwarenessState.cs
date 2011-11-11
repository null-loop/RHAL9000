using System;

namespace RHAL9000.Core
{
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