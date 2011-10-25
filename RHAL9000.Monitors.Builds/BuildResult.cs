using System;

namespace RHAL9000.Monitors.Builds
{
    public class BuildResult : IBuildResult
    {
        #region IBuildResult Members

        public string Id { get; set; }
        public string Number { get; set; }
        public BuildStatus Status { get; set; }
        public string BuildTypeId { get; set; }
        public Uri WebUri { get; set; }

        #endregion
    }
}