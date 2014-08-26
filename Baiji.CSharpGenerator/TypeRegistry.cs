using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.CSharpGenerator
{
    /// <summary>
    /// Collects all the various custom types found in the IDL definition files.
    /// </summary>
    public class TypeRegistry : IEnumerable<CSharpType>
    {
        private readonly Dictionary<string, CSharpType> registry = new Dictionary<string, CSharpType>();

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

        public void Add(CSharpType type)
        {
            if (registry.ContainsKey(type.Key))
            {
                throw new ArgumentException(string.Format("The type {0} was already registered!", type));
            }
            registry[type.Key] = type;
        }

        public IEnumerator<CSharpType> GetEnumerator()
        {
            return registry.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return registry.Values.GetEnumerator();
        }

        public CSharpType FindType(string tripNamespace, string name)
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

        public CSharpType FindType(string key)
        {
            if (key == null) return null;

            if (!registry.ContainsKey(key))
            {
                throw new ArgumentException(string.Format("Unable to find csharp type with key {0} in registry!", key));
            }
            return registry[key];
        }

    }
}
