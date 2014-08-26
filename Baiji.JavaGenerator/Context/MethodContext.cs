using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator.Context
{
    public class MethodContext
    {
        public string Name { get; private set; }
        public bool Oneway { get; private set; }
        public string[] DocStringLines { get; private set; }
        public string JavaMethodName { get; private set; }
        public string JavaReturnType { get; private set; }
        public string JavaArgumentName { get; private set; }
        public string JavaArgumentType { get; private set; }

        public MethodContext(string[] docStringLines, string name, bool oneway, string javaMethodName, string javaReturnType,
            string javaArgumentName, string javaArgumentType)
        {
            DocStringLines = docStringLines;
            Name = name;
            Oneway = oneway;
            JavaMethodName = javaMethodName;
            JavaReturnType = javaReturnType;
            JavaArgumentName = javaArgumentName;
            JavaArgumentType = javaArgumentType;
        }
    }
}
