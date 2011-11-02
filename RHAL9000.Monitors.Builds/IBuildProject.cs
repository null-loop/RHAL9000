using System;
using System.Collections.Generic;

namespace RHAL9000.Monitors.Builds
{
    public interface IBuildProject : ISummary
    {
        Uri WebUrl { get; }
        string Description { get; }
        bool Archived { get; }
        IEnumerable<ISummary> BuildTypes { get; }
    }
}