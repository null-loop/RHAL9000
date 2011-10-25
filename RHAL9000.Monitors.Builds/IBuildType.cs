using System;
using System.Collections.Generic;

namespace RHAL9000.Monitors.Builds
{
    public interface IBuildType : ISummary
    {
        Uri WebUri { get; }
        string Description { get; }
        bool Paused { get; }
        string BuildProjectId { get; }
        Dictionary<string, string> RunParameters { get; }
    }

    public class BuildType : IBuildType
    {
        #region IBuildType Members

        public string Name { get; set; }
        public string Id { get; set; }
        public Uri WebUri { get; set; }
        public string Description { get; set; }
        public bool Paused { get; set; }
        public string BuildProjectId { get; set; }
        public Dictionary<string, string> RunParameters { get; set; }

        #endregion
    }
}