using System;

namespace RHAL9000.Core
{
    public interface IAwarenessState
    {
        DateTime Began { get; }
        DateTime LastUpdated { get; }
        DateTime? Ended { get; }

        string Status { get; }
        HumanAttentionRequirement AttentionRequired { get; }
        //TODO:PersonalityAttentionRequirement

        event EventHandler<AwarenessStateUpdatedEventArgs> Updated;

        void Finished(string status);
        void Change(string status);
        void Change(string status, HumanAttentionRequirement attentionRequirement);
    }
}