
using CTripOSS.Baiji.IDLParser.Model;
using System.Collections.Generic;
namespace CTripOSS.Baiji.Generator.Util
{
    public class ContextUtils
    {
        private static ISet<string> COMMON_CODE_NAMESPACES = new HashSet<string> { 
            "com.ctriposs.baiji.rpc.common.types", 
            "com.ctriposs.baiji.rpc.mobile.common.types", 
            "CTripOSS.Baiji.Rpc.Common.Types", 
            "CTripOSS.Baiji.Rpc.Mobile.Common.Types", 
            "BJ" };

        public static Service ExtractService(IList<DocumentContext> contexts)
        {
            foreach (var dc in contexts)
            {
                var defs = dc.Document.Definitions;
                foreach (Definition def in defs)
                {
                    if (def is Service)
                    {
                        return (Service)def;
                    }
                }
            }
            return null;
        }

        public static bool isCommon(string codeNamespace)
        {
            return COMMON_CODE_NAMESPACES.Contains(codeNamespace);
        }
    }
}
