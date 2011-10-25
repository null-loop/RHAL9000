using System;

namespace RHAL9000.Monitors.Builds
{
    public interface IBuildResult
    {
        string Id { get; }
        string Number { get; }
        BuildStatus Status { get; }
        string BuildTypeId { get; }
        Uri WebUri { get; }
    }
}