using System;

namespace CTripOSS.Baiji.JavaGenerator
{
    public class GenType
    {
        public GType GType { get; private set; }

        public string JavaTypeName { get; private set; }

        // for map key
        public GenType KeyType { get; set; }

        public string KeyJavaTypeName { get; set; }

        // for map value
        public GenType ValueType { get; set; }

        public string ValueJavaTypeName { get; set; }

        // for element type of List or Set
        public GenType ElementType { get; set; }

        public string ElementJavaTypeName { get; set; }

        public GenType(GType gType, string javaTypeName)
        {
            GType = gType;
            JavaTypeName = javaTypeName;
        }

        public bool IsBool
        {
            get
            {
                return GType == GType.Bool;
            }
        }

        public bool IsByte
        {
            get
            {
                return GType == GType.Byte;
            }
        }

        public bool IsDouble
        {
            get
            {
                return GType == GType.Double;
            }
        }
        
        public bool IsI32
        {
            get
            {
                return GType == GType.I32;
            }
        }

        public bool IsI64
        {
            get
            {
                return GType == GType.I64;
            }
        }

        public bool IsBinary
        {
            get
            {
                return GType == GType.Binary;
            }
        }

        public bool IsString
        {
            get
            {
                return GType == GType.String;
            }
        }

        public bool IsEnum
        {
            get
            {
                return GType == GType.Enum;
            }
        }

        public bool IsStruct
        {
            get
            {
                return GType == GType.Struct;
            }
        }

        public bool IsList
        {
            get
            {
                return GType == GType.List;
            }
        }

        public bool IsMap
        {
            get
            {
                return GType == GType.Map;
            }
        }

        public bool IsBaseType
        {
            get
            {
                return
                    GType == GType.Bool ||
                    GType == GType.Byte ||
                    GType == GType.Double ||
                    GType == GType.I32 ||
                    GType == GType.I64 ||
                    GType == GType.Binary ||
                    GType == GType.String;
            }
        }

        public bool IsContainer
        {
            get
            {
                return 
                    GType == GType.List ||
                    GType == GType.Map;
            }
        }


        public string LTypeName
        {
            get
            {
                switch (GType)
                {
                    case GType.Bool:
                        return "TType.BOOL";
                    case GType.Byte:
                        return "TType.BYTE";
                    case GType.Double:
                        return "TType.DOUBLE";
                    case GType.I32:
                        return "TType.I32";
                    case GType.I64:
                        return "TType.I64";
                    case GType.String:
                        return "TType.STRING";
                    case GType.Binary:
                        return "TType.STRING";
                    case GType.Enum:
                        return "TType.I32";
                    case GType.Struct:
                        return "TType.STRUCT";
                    case GType.List:
                        return "TType.LIST";
                    case GType.Map:
                        return "TType.MAP";
                    default:
                        throw new ArgumentException(string.Format("Can't convert GType {0} to TType", GType));

                }

            }
        }
    }
}
