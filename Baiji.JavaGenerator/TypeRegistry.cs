using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator
{
    /// <summary>
    /// Collects all the various custom types found in the IDL definition files.
    /// </summary>
    public class TypeRegistry : IEnumerable<JavaType>
    {
        private readonly Dictionary<string, JavaType> registry = new Dictionary<string, JavaType>();

        public TypeRegistry()
        {
        }

        public void AddAll(TypeRegistry otherRegistry)
        {
            foreach (var type in otherRegistry)
            {
                Add(type);
            }
        }

        public void Add(JavaType type)
        {
            if (registry.ContainsKey(type.Key))
            {
                throw new ArgumentException(string.Format("The type {0} was already registered!", type));
            }
            registry[type.Key] = type;
        }

        public IEnumerator<JavaType> GetEnumerator()
        {
            return registry.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return registry.Values.GetEnumerator();
        }

        public JavaType FindType(string tripNamespace, string name)
        {
            if (name == null) return null;

            if (name.Contains("."))
            {
                // If the name contains a '.' it already has a namespace prepended
                return FindType(name);
            }
            else
            {
                // Otherwise, use the default namespace
                return FindType(tripNamespace + "." + name);
            }
        }

        public JavaType FindType(string key)
        {
            if (key == null) return null;

            if (!registry.ContainsKey(key))
            {
                throw new ArgumentException(string.Format("Unable to find java type with key {0} in registry!", key));
            }
            return registry[key];
        }

    }
}
