using System;
using System.Collections.Generic;

namespace RHAL9000.Monitors.Builds
{
    public interface IBuildProject : ISummary
    {
        Uri WebUri { get; }
        string Description { get; }
        bool Archived { get; }
        IEnumerable<ISummary> BuildTypes { get; }
    }
}