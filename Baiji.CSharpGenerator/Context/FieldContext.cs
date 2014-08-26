using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTripOSS.Baiji.IDLParser.Model;

namespace CTripOSS.Baiji.CSharpGenerator.Context
{
    public class FieldContext : IComparable
    {
        public string Name { get; private set; }
        public short Id { get; private set; }
        //public string CSharpType { get; private set; }
        public string[] DocStringLines { get; private set; }
        public string CSharpName { get; private set; }
        public string CSharpPropertyName { get; private set; }
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
            string csharpName,
            string csharpPropertyName,
            bool required,
            GenType genType)
        {
            DocStringLines = docStringLines;
            Name = name;
            Id = id;
            //CSharpType = csharpType;
            CSharpName = csharpName;
            CSharpPropertyName = csharpPropertyName;
            Required = required;
            GenType = genType;
        }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((FieldContext)obj).Id);
        }
    }
}
