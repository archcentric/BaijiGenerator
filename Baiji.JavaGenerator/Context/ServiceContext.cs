using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator.Context
{
    public class ServiceContext : JavaContext
    {
        public string[] DocStringLines { get; private set; }
        public string Name { get; private set; }
        public string JavaPackage { get; private set; }
        public string JavaName { get; private set; }
        public ISet<string> JavaParents { get; private set; }
        public string ServiceName { get; private set; }
        public string ServiceNamespace { get; private set; }

        public IList<MethodContext> Methods { get; private set; }

        public ServiceContext(string[] docStringLines, string name, string javaPackage, string javaName,
                              ISet<string> javaParents, string serviceName, string serviceNamespace)
        {
            DocStringLines = docStringLines;
            Name = name;
            JavaPackage = javaPackage;
            JavaName = javaName;
            JavaParents = javaParents;
            ServiceName = serviceName;
            ServiceNamespace = serviceNamespace;
            Methods = new List<MethodContext>();
        }

        public void AddMethod(MethodContext method)
        {
            this.Methods.Add(method);
        }

    }
}
