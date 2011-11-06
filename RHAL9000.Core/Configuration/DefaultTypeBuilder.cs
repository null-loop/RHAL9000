using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace RHAL9000.Core.Configuration
{
    public class DefaultTypeBuilder : ITypeBuilder
    {
        private ITypeLookup[] TypeLookups { get; set; }

        public DefaultTypeBuilder(params ITypeLookup[] typeLookups)
        {
            TypeLookups = typeLookups;
        }

        private Type LookupType(string key)
        {
            var lookup = TypeLookups.Where(t => t.Contains(key)).FirstOrDefault();

            if (lookup == null)
            {
                throw new MissingLookupException(string.Format("Missing type for key {0}", key));
            }

            return lookup.Lookup(key);
        }

        public object Build(XElement element)
        {
            if (element == null) throw new ArgumentNullException("element");

            var elementType = LookupType(element.Name.LocalName);

            // look for lists
            if (typeof(IList).IsAssignableFrom(elementType))
                return BuildList(elementType, element);

            // elements -> constructor parameters
            var ctorParamTypes = element.Elements().Select(e=>LookupType(e.Name.LocalName));

            // if we've got string content but no ctor params use the string...
            var stringContent = GetStringContent(element);


            //TODO:THIS IS DIRTY! Replace with string value converter!
            if (stringContent != null && !ctorParamTypes.Any())
                if (elementType == typeof(string))
                    ctorParamTypes = new[] {typeof (char[])};
                else
                    ctorParamTypes = new[] {typeof (string)};

            var ctor = FindCtor(elementType, ctorParamTypes.ToArray());

            if (ctor == null)
                throw new MissingMemberException(string.Format("Cannot find ctor for type {0}", elementType.FullName));

            // create parameter type instances...
            var ctorParamInstances = element.Elements().Select(Build).ToArray();
            // allowing for string content only...
            if (stringContent != null && !ctorParamInstances.Any())
                if (elementType == typeof(string))
                    ctorParamInstances = new[] { stringContent.ToCharArray() };
                else
                    ctorParamInstances = new[] {stringContent};

            // create instance
            var instance = Activator.CreateInstance(elementType, ctorParamInstances);

            // attributes -> map simple types to set properties
            foreach(var property in elementType.GetProperties(BindingFlags.SetProperty | BindingFlags.Public))
            {
                var info = property;
                var attribute = element.Attributes().Where(a => a.Name == info.Name || a.Name == LoweredFirst(info.Name)).FirstOrDefault();

                if (attribute!=null)
                {
                    property.SetValue(instance, GetValue(property.PropertyType, attribute.Value), null);
                }
            }           

            return instance;
        }

        public T Build<T>(XElement element) where T : class
        {
            return Build(element) as T;
        }

        private static string GetStringContent(XElement element)
        {
            return string.IsNullOrEmpty(element.Value) ? null : element.Value;
        }

        private object BuildList(Type elementType, XElement element)
        {
            var collection = Activator.CreateInstance(elementType) as IList;

            if (collection == null)
                throw new InvalidOperationException(string.Format("Cannot cast type {0} as IList", elementType.FullName));

            foreach (var subElement in element.Elements())
                collection.Add(Build(subElement));

            return collection;
        }

        private static object GetValue(Type propertyType, string value)
        {
            if (propertyType == typeof(string))
                return value;
            if (propertyType == typeof(Int32))
                return Int32.Parse(value);
            if (propertyType == typeof(Decimal))
                return Decimal.Parse(value);
            if (propertyType == typeof(DateTime))
                return DateTime.Parse(value);
            if (propertyType == typeof(Uri))
                return new Uri(value);
            // TODO:Any other types?
            if (typeof(Enum).IsAssignableFrom(propertyType))
                return Enum.Parse(propertyType, value);

            throw new InvalidOperationException(string.Format("Cannot map string to {0}", propertyType.FullName));
        }

        private static ConstructorInfo FindCtor(Type elementType, Type[] ctorParamTypes)
        {
            ConstructorInfo foundCtor = null;

            foreach(var ctor in elementType.GetConstructors())
            {
                var parameters = ctor.GetParameters();
                if (ctorParamTypes.Count() != parameters.Count())
                    continue;

                var matches = 0;
                for(var i = 0;i!=ctorParamTypes.Count();i++)
                {
                    if (parameters[i].ParameterType.IsAssignableFrom(ctorParamTypes[i]))
                        matches++;
                }
                if (matches == ctorParamTypes.Count())
                    foundCtor = ctor;

                if (foundCtor != null) break;
            }

            return foundCtor;
        }

        private static string LoweredFirst(string name)
        {
            return name.Substring(0, 1).ToLower() + name.Substring(1, name.Length - 1);
        }
    }
}