using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.CSharpGenerator
{
    public enum GType : byte
    {
        Bool = 0,
        Byte = 1,
        Double = 2,
        I32 = 4,
        I64 = 5,
        String = 6,
        Binary = 7,
        Struct = 8,
        Enum = 9,
        Map = 10,
        List = 12
    }
}
