using System.Collections.Generic;

namespace RHAL9000.Core
{
    public class ViewConfiguration
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public IEnumerable<ViewConfiguration> SubViews { get; set; }
    }
}