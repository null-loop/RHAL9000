using System;

namespace RHAL9000.Monitors.Builds
{
    public interface IFullBuildResult : IPartialBuildResult
    {
        DateTime Finished { get; }
    }

    public interface IPartialBuildResult
    {
        string Id { get; }
        string Number { get; }
        BuildStatus Status { get; }
        Uri WebUri { get; }
        bool Personal { get; }
        bool History { get; }
        bool Pinned { get; }
        string StatusText { get; }
        string BuildTypeId { get; }
        DateTime Started { get; }
        string AgentId { get; }
    }
}