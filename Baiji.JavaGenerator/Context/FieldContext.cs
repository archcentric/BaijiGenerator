using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator.Context
{
    public class FieldContext : IComparable
    {
        public string Name { get; private set; }
        public short Id { get; private set; }
        public string[] DocStringLines { get; private set; }
        public string JavaName { get; private set; }
        public string JavaGetterName { get; private set; }
        public string JavaSetterName { get; private set; }
        public bool Required { get; private set; }
        public GenType GenType { get; private set; }

        public FieldContext(string name, GenType genType)
        {
            Name = name;
            GenType = genType;
        }

        public FieldContext(string[] docStringLines,
            string name,
            short id,
            string javaName,
            string javaSetterName,
            string javaGetterName,
            bool required,
            GenType genType)
        {
            DocStringLines = docStringLines;
            Name = name;
            Id = id;
            JavaName = javaName;
            JavaSetterName = javaSetterName;
            JavaGetterName = javaGetterName;
            Required = required;
            GenType = genType;
        }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((FieldContext)obj).Id);
        }
    }
}
