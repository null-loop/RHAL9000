using System;

namespace RHAL9000.Core
{
    public class AwarenessStateUpdatedEventArgs : EventArgs
    {
        public string Status { get; set; }
        public HumanAttentionRequirement AttentionRequirement { get; set; }

        public AwarenessStateUpdatedEventArgs(string status, HumanAttentionRequirement attentionRequirement)
        {
            Status = status;
            AttentionRequirement = attentionRequirement;
        }
    }
}