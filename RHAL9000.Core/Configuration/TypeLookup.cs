using System;
using System.Collections.Generic;

namespace RHAL9000.Core.Configuration
{
    public abstract class TypeLookup : ITypeLookup
    {
        private readonly Dictionary<string,Type> _lookup = new Dictionary<string, Type>();

        protected void Register<T>(string key)
        {
            if (!_lookup.ContainsKey(key))
                _lookup.Add(key, typeof (T));
        }

        public Type Lookup(string key)
        {
            if (_lookup.ContainsKey(key))
                return _lookup[key];

            throw new MissingLookupException(string.Format("Missing registry for key {0}", key));
        }

        public bool Contains(string key)
        {
            return _lookup.ContainsKey(key);
        }
    }
}