using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.CSharpGenerator.Context
{
    public class MethodContext
    {
        public string[] DocStringLines { get; private set; }
        public string Name { get; private set; }
        public bool Oneway { get; private set; }
        public string CSharpMethodName { get; private set; }
        public string CSharpReturnType { get; private set; }
        public string CSharpArgumentName { get; private set; }
        public string CSharpArgumentType { get; private set; }

        public MethodContext(string[] docStringLines, string name, bool oneway, string csharpMethodName, string csharpReturnType, 
            string csharpArgumentName, string csharpArgumentType)
        {
            DocStringLines = docStringLines;
            Name = name;
            Oneway = oneway;
            CSharpMethodName = csharpMethodName;
            CSharpReturnType = csharpReturnType;
            CSharpArgumentName = csharpArgumentName;
            CSharpArgumentType = csharpArgumentType;
        }
    }
}
