using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator.Context
{
    public interface JavaContext
    {
        string JavaPackage { get; }
        string[] DocStringLines { get; }
        string JavaName { get; }
    }
}
