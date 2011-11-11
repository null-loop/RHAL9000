using System;

namespace RHAL9000.Core
{
    public class HasAwarnessStatesUpdatedEventArgs : EventArgs
    {
        public HasAwarnessStatesUpdatedEventArgs(bool added, bool removed, IAwarenessState state)
        {
            if ((added && removed) || (!added && !removed))
                throw new InvalidOperationException("added and removed cannot both be true or false");

            Added = added;
            Removed = removed;
            State = state;
        }

        public IAwarenessState State { get; set; }

        public bool Removed { get; set; }

        public bool Added { get; set; }
    }
}