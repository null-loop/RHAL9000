using System.Collections.Generic;

namespace RHAL9000.Core
{
    public interface IAcceptsConfiguration
    {
        void Configure(Dictionary<string, string> dictionary);
    }
}