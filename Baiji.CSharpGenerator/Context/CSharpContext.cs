using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.CSharpGenerator.Context
{
    public interface CSharpContext
    {
        string CSharpNamespace { get; }
        string[] DocStringLines { get; }
        string CSharpName { get; }
    }
}
