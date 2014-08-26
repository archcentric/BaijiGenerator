using System.Collections.Generic;

namespace CTripOSS.Baiji.CSharpGenerator.Context
{
    public class ClientContext : CSharpContext
    {
        public string Name { get; private set; }
        public string CSharpNamespace { get; private set; }
        public string[] DocStringLines { get; private set; }
        public string CSharpName { get; private set; }
        public ISet<string> CSharpParents { get; private set; }
        public string ServiceName { get; private set; }
        public string ServiceNamespace { get; private set; }
        public IList<MethodContext> Methods { get; private set; }

        public ClientContext(string[] docStringLines, string name, string csharpNamespace, string csharpName,
                             ISet<string> csharpParents, string serviceName, string serviceNamespace)
        {
            DocStringLines = docStringLines;
            Name = name;
            CSharpNamespace = csharpNamespace;
            CSharpName = csharpName;
            CSharpParents = csharpParents;
            ServiceName = serviceName;
            ServiceNamespace = serviceNamespace;
            Methods = new List<MethodContext>();
        }

        public void AddMethod(MethodContext method)
        {
            Methods.Add(method);
        }
    }
}
