using System;
using System.Collections.Generic;

namespace RHAL9000.Monitors.Builds
{
    public class BuildProject : Summary, IBuildProject
    {
        #region IBuildProject Members

        public Uri WebUri { get; set; }
        public string Description { get; set; }
        public bool Archived { get; set; }
        public IEnumerable<ISummary> BuildTypes { get; set; }

        #endregion
    }
}