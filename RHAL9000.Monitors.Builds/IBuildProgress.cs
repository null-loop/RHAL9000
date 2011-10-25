using System;

namespace RHAL9000.Monitors.Builds
{
    public interface IBuildProgress : IPartialBuildResult
    {
        decimal PercentageComplete { get; }
        TimeSpan ElapsedRunTime { get; }
        TimeSpan EstimatedRunTime { get; }
        string CurrentStage { get; }
        bool Outdated { get; }
        bool ProbablyHanging { get; }
    }
}