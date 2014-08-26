using System;
using System.Collections.Generic;
using CTripOSS.Baiji.IDLParser.Model;

namespace CTripOSS.Baiji.Generator.CSharp
{
    internal class CSharpBaseConverter : TypeConverter.Converter
    {
        private static readonly IDictionary<BType, string> CSHARP_PRIMITIVES_MAP;
        private static readonly IDictionary<BType, GenType.Type> GTYPE_BASETYPE_MAP;

        static CSharpBaseConverter()
        {
            CSHARP_PRIMITIVES_MAP = new Dictionary<BType, string>();
            CSHARP_PRIMITIVES_MAP[BType.BOOL] = "bool?";
            CSHARP_PRIMITIVES_MAP[BType.I32] = "int?";
            CSHARP_PRIMITIVES_MAP[BType.I64] = "long?";
            CSHARP_PRIMITIVES_MAP[BType.DOUBLE] = "double?";
            CSHARP_PRIMITIVES_MAP[BType.STRING] = "string";
            CSHARP_PRIMITIVES_MAP[BType.BINARY] = "byte[]";

            GTYPE_BASETYPE_MAP = new Dictionary<BType, GenType.Type>();
            GTYPE_BASETYPE_MAP[BType.BOOL] = GenType.Type.Bool;
            GTYPE_BASETYPE_MAP[BType.I32] = GenType.Type.I32;
            GTYPE_BASETYPE_MAP[BType.I64] = GenType.Type.I64;
            GTYPE_BASETYPE_MAP[BType.DOUBLE] = GenType.Type.Double;
            GTYPE_BASETYPE_MAP[BType.STRING] = GenType.Type.String;
            GTYPE_BASETYPE_MAP[BType.BINARY] = GenType.Type.Binary;
        }

        public bool Accept(BaijiType type)
        {
            return type.GetType() == typeof(BaseType);
        }

        public string ConvertToString(BaijiType type)
        {
            var baseType = ((BaseType)type).BType;
            return CSHARP_PRIMITIVES_MAP[baseType];
        }

        public GenType ConvertToGenType(BaijiType type)
        {
            var csharpTypeName = ConvertToString(type);
            var baseType = ((BaseType)type).BType;
            var gType = GTYPE_BASETYPE_MAP[baseType];
            var genType = new GenType(gType, csharpTypeName);
            return genType;
        }
    }

    internal class CSharpIdentifierConverter : TypeConverter.Converter
    {
        private readonly TypeConverter _converter;

        public CSharpIdentifierConverter(TypeConverter converter)
        {
            _converter = converter;
        }

        public bool Accept(BaijiType type)
        {
            return type is IdentifierType;
        }

        private CodeType FindCodeTypeFromIdentifierType(IdentifierType id)
        {
            var name = id.Name;
            // the name is [<namespace>.]<type>
            var names = new List<string>(name.Split('.'));

            if (names.Count == 0 || names.Count > 3)
            {
                throw new ArgumentException("Only unqualified and namespace qualified names are allowed!", "id");
            }
            string idlName = names[0];
            string idlNamespace = _converter.IdlNamespace;

            if (names.Count == 2)
            {
                idlName = names[1];
                idlNamespace = names[0];
            }

            var typeName = idlNamespace + "." + _converter.TypeMangler.MangleTypeName(idlName);
            var type = _converter.TypeRegistry.FindType(typeName);
            return type;
        }

        public string ConvertToString(BaijiType type)
        {
            var codeType = FindCodeTypeFromIdentifierType((IdentifierType)type);
            var className = ShortenClassName(codeType.Name);
            if (codeType.IsEnum)
            {
                className += "?";
            }
            return className;
        }

        private string ShortenClassName(string className)
        {
            // If the class is in the package we are currently generating code for, generate
            // only the simple name, otherwise generate the fully qualified class name.
            if (className.StartsWith(_converter.CodeNamespace) &&
                className.LastIndexOf(".") == _converter.CodeNamespace.Length)
            {
                className = className.Substring(_converter.CodeNamespace.Length + 1);
            }
            return className;
        }

        public GenType ConvertToGenType(BaijiType type)
        {
            var csharpTypeName = ConvertToString(type);
            var csharpType = FindCodeTypeFromIdentifierType((IdentifierType)type);
            if (csharpType.IsEnum)
            {
                return new GenType(GenType.Type.Enum, csharpTypeName);
            }
            else
            {
                return new GenType(GenType.Type.Struct, csharpTypeName);
            }
        }
    }

    internal class CSharpListConverter : TypeConverter.Converter
    {
        private readonly TypeConverter _converter;

        public CSharpListConverter(TypeConverter converter)
        {
            _converter = converter;
        }

        public bool Accept(BaijiType type)
        {
            return type.GetType() == typeof(ListType);
        }

        public string ConvertToString(BaijiType type)
        {
            var listType = type as ListType;
            string actualType = _converter.ConvertToString(listType.Type);
            return "List<" + actualType + ">";
        }

        public GenType ConvertToGenType(BaijiType type)
        {
            var csharpTypeName = ConvertToString(type);

            var listType = type as ListType;
            var genType = new GenType(GenType.Type.List, csharpTypeName);
            genType.ElementType = _converter.ConvertToGenType(listType.Type);
            genType.ElementTypeName = _converter.ConvertToString(listType.Type);
            return genType;
        }
    }

    internal class CSharpMapConverter : TypeConverter.Converter
    {
        private readonly TypeConverter _converter;

        public CSharpMapConverter(TypeConverter converter)
        {
            _converter = converter;
        }

        public bool Accept(BaijiType type)
        {
            return type.GetType() == typeof(MapType);
        }

        public string ConvertToString(BaijiType type)
        {
            var mapType = type as MapType;

            string actualKeyType = _converter.ConvertToString(mapType.KeyType);
            string actualValueType = _converter.ConvertToString(mapType.ValueType);

            return string.Format("Dictionary<{0}, {1}>", actualKeyType, actualValueType);
        }

        public GenType ConvertToGenType(BaijiType type)
        {
            var csharpTypeName = ConvertToString(type);

            var mapType = type as MapType;
            var genType = new GenType(GenType.Type.Map, csharpTypeName);
            genType.KeyType = _converter.ConvertToGenType(mapType.KeyType);
            genType.KeyTypeName = _converter.ConvertToString(mapType.KeyType);
            genType.ValueType = _converter.ConvertToGenType(mapType.ValueType);
            genType.ValueTypeName = _converter.ConvertToString(mapType.ValueType);
            return genType;
        }
    }
}