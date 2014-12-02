
using CTripOSS.Baiji.IDLParser.Model;
using System.Collections.Generic;
namespace CTripOSS.Baiji.Generator.Util
{
    public class ContextUtils
    {
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
    }
}
