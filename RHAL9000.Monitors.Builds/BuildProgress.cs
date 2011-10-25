using System;

namespace RHAL9000.Monitors.Builds
{
    public class BuildProgress : IBuildProgress
    {
        #region IBuildProgress Members

        public string Id { get; set; }
        public string Number { get; set; }
        public BuildStatus Status { get; set; }
        public Uri WebUri { get; set; }
        public bool Personal { get; set; }
        public bool History { get; set; }
        public bool Pinned { get; set; }
        public string StatusText { get; set; }
        public string BuildTypeId { get; set; }
        public DateTime Started { get; set; }
        public string AgentId { get; set; }
        public decimal PercentageComplete { get; set; }
        public TimeSpan ElapsedRunTime { get; set; }
        public TimeSpan EstimatedRunTime { get; set; }
        public string CurrentStage { get; set; }
        public bool Outdated { get; set; }
        public bool ProbablyHanging { get; set; }

        #endregion
    }
}