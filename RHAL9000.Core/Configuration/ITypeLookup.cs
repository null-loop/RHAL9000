using System;

namespace RHAL9000.Core.Configuration
{
    public interface ITypeLookup
    {
        Type Lookup(string key);
        bool Contains(string key);
    }
}