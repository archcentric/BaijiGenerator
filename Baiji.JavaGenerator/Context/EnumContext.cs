using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator.Context
{
    public class EnumContext : JavaContext
    {
        public string[] DocStringLines { get; private set; }
        public string JavaPackage { get; private set; }
        public string JavaName { get; private set; }
        public List<EnumFieldContext> Fields { get; private set; }

        public EnumContext(string[] docStringLines, string javaPackage, string javaName)
        {
            DocStringLines = docStringLines;
            JavaPackage = javaPackage;
            JavaName = javaName;
            Fields = new List<EnumFieldContext>();
        }

        public void AddField(EnumFieldContext field)
        {
            Fields.Add(field);
        }
    }
}
