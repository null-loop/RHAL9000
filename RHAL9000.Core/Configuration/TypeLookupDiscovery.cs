using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RHAL9000.Core.Configuration
{
    public static class TypeLookupDiscovery
    {
        public static IEnumerable<ITypeLookup> FromEntryAssembly()
        {
            return FromAssemblyAndReferences(Assembly.GetEntryAssembly());
        }

        public static IEnumerable<ITypeLookup> FromAssemblyAndReferences(Assembly assembly)
        {
            if (assembly == null) return new ITypeLookup[0];
            return assembly.GetReferencedAssemblies().SelectMany(FromAssembly).Concat(FromAssembly(assembly));
        }

        public static IEnumerable<ITypeLookup> FromAssembly(AssemblyName assemblyName)
        {
            return FromAssembly(LoadFromName(assemblyName));
        }

        public static IEnumerable<ITypeLookup> FromAssembly(Assembly assembly)
        {
            if (assembly == null) return new ITypeLookup[0];
            return assembly.GetTypes().Where(t => !t.IsInterface && !t.IsAbstract && typeof (ITypeLookup).IsAssignableFrom(t)).Select(Activator.CreateInstance).Cast<ITypeLookup>();
        }

        private static Assembly LoadFromName(AssemblyName assemblyName)
        {
            return assemblyName.CodeBase == null ? null : Assembly.LoadFrom(assemblyName.CodeBase);
        }
    }
}
