using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.CSharpGenerator.Context
{
    public class EnumContext : CSharpContext
    {
        public string CSharpNamespace { get; private set; }
        public string[] DocStringLines { get; private set; }
        public string CSharpName { get; private set; }
        public List<EnumFieldContext> Fields { get; private set; }

        public EnumContext(string[] docStringLines, string csharpNamespace, string csharpName)
        {
            DocStringLines = docStringLines;
            CSharpNamespace = csharpNamespace;
            CSharpName = csharpName;
            Fields = new List<EnumFieldContext>();
        }

        public void AddField(EnumFieldContext field)
        {
            Fields.Add(field);
        }
    }
}
