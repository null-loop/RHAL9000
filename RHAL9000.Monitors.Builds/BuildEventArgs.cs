using System;

namespace RHAL9000.Monitors.Builds
{
    public class BuildEventArgs : EventArgs
    {
        public BuildEventArgs(BuildModel build)
        {
            Build = build;
        }

        public BuildModel Build { get; set; }
    }
}