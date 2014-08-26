using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator.Context
{
    public class EnumFieldContext
    {
        public string[] DocStringLines { get; private set; }
        public string JavaName { get; private set; }
        public long Value { get; private set; }

        public EnumFieldContext(string[] docStringLines, string javaName, long value)
        {
            DocStringLines = docStringLines;
            JavaName = javaName;
            Value = value;
        }
    }
}
