using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.CSharpGenerator.Context
{
    public class EnumFieldContext
    {
        public string[] DocStringLines { get; private set; }
        public string CSharpName { get; private set; }
        public long Value { get; private set; }

        public EnumFieldContext(string[] docStringLines, string csharpName, long value)
        {
            DocStringLines = docStringLines;
            CSharpName = csharpName;
            Value = value;
        }
    }
}
