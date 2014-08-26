using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.Helper;
using CTripOSS.Baiji.CSharpGenerator.Context;

namespace CTripOSS.Baiji.CSharpGenerator
{
    public class TypeToCSharpConverter
    {
        private readonly string _namespace;
        private readonly TypeRegistry typeRegistry;
        private readonly List<Converter> converters;
        private readonly string csharpNamespace;

        public TypeToCSharpConverter(TypeRegistry typeRegistry,
            string _namespace,
            string csharpNamespace)
        {
            Enforce.IsNotNull<TypeRegistry>(typeRegistry, "typeRegistry");
            Enforce.IsNotNull<string>(_namespace, "namespace");
            this.typeRegistry = typeRegistry;
            this._namespace = _namespace;
            this.csharpNamespace = csharpNamespace;
            converters = new List<Converter>();
            converters.Add(new BaseConverter());
            converters.Add(new IdentifierConverter(this));
            converters.Add(new ListConverter(this));
            converters.Add(new MapConverter(this));
        }

        public string ConvertToString(BaijiType tripType)
        {
            foreach (var converter in converters)
            {
                if (converter.Accept(tripType))
                {
                    return converter.ConvertToString(tripType);
                }
            }
            throw new ArgumentException(string.Format("Trip type {0} is unknown!", tripType));
        }

        public GenType ConvertToGenType(BaijiType tripType)
        {
            foreach (var converter in converters)
            {
                if (converter.Accept(tripType))
                {
                    return converter.ConvertToGenType(tripType);
                }
            }
            throw new ArgumentException(string.Format("Trip type {0} is unknown!", tripType));
        }

        private class BaseConverter : Converter
        {
            private static readonly Dictionary<BType, string> CSHARP_PRIMITIVES_MAP;
            private static readonly Dictionary<BType, GType> GTYPE_BASETYPE_MAP;

            static BaseConverter()
            {
                CSHARP_PRIMITIVES_MAP = new Dictionary<BType, string>();
                CSHARP_PRIMITIVES_MAP[BType.BOOL] = "bool?";
                CSHARP_PRIMITIVES_MAP[BType.BYTE] = "sbyte?";
                CSHARP_PRIMITIVES_MAP[BType.I32] = "int?";
                CSHARP_PRIMITIVES_MAP[BType.I64] = "long?";
                CSHARP_PRIMITIVES_MAP[BType.DOUBLE] = "double?";
                CSHARP_PRIMITIVES_MAP[BType.STRING] = "string";
                CSHARP_PRIMITIVES_MAP[BType.BINARY] = "byte[]";

                GTYPE_BASETYPE_MAP = new Dictionary<BType, GType>();
                GTYPE_BASETYPE_MAP[BType.BOOL] = GType.Bool;
                GTYPE_BASETYPE_MAP[BType.BYTE] = GType.Byte;
                GTYPE_BASETYPE_MAP[BType.I32] = GType.I32;
                GTYPE_BASETYPE_MAP[BType.I64] = GType.I64;
                GTYPE_BASETYPE_MAP[BType.DOUBLE] = GType.Double;
                GTYPE_BASETYPE_MAP[BType.STRING] = GType.String;
                GTYPE_BASETYPE_MAP[BType.BINARY] = GType.Binary;
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

        public bool IsEnumIdentifier(IdentifierType id)
        {
            var csharpType = this.FindCSharpTypeFromIdentifierType(id);
            return csharpType.IsTripEnum;
        }

        public bool IsStructIdentifier(IdentifierType id)
        {
            var csharpType = this.FindCSharpTypeFromIdentifierType(id);
            return csharpType.IsTripStruct;
        }

        private CSharpType FindCSharpTypeFromIdentifierType(IdentifierType id)
        {
            var name = id.Name;
            // the name is [<trip-namespace>.]<trip type>
            var names = new List<string>(name.Split('.'));

            if (names.Count == 0 || names.Count > 3)
            {
                throw new ArgumentException("Only unqualified and trip-namespace qualified names are allowed!");
            }
            string tripName = names[0];
            string tripNamespace = this._namespace;

            if (names.Count == 2)
            {
                tripName = names[1];
                tripNamespace = names[0];
            }

            var csharpTypeName = tripNamespace + "." + TemplateContextGenerator.MangleCSharpTypeName(tripName);
            var csharpType = this.typeRegistry.FindType(csharpTypeName);
            return csharpType;
        }

        private class IdentifierConverter : Converter
        {
            private TypeToCSharpConverter typeToCSharpConverter;

            public IdentifierConverter(TypeToCSharpConverter typeToCSharpConverter)
            {
                this.typeToCSharpConverter = typeToCSharpConverter;
            }

            public bool Accept(BaijiType type)
            {
                return type.GetType() == typeof(IdentifierType);
            }

            private CSharpType FindCSharpTypeFromIdentifierType(IdentifierType id)
            {
                var name = id.Name;
                // the name is [<trip-namespace>.]<trip type>
                var names = new List<string>(name.Split('.'));

                if (names.Count == 0 || names.Count > 3)
                {
                    throw new ArgumentException("Only unqualified and trip-namespace qualified names are allowed!");
                }
                string tripName = names[0];
                string tripNamespace = typeToCSharpConverter._namespace;

                if (names.Count == 2)
                {
                    tripName = names[1];
                    tripNamespace = names[0];
                }

                var csharpTypeName = tripNamespace + "." + TemplateContextGenerator.MangleCSharpTypeName(tripName);
                var csharpType = typeToCSharpConverter.typeRegistry.FindType(csharpTypeName);
                return csharpType;
            }

            public string ConvertToString(BaijiType type)
            {
                var csharpType = this.FindCSharpTypeFromIdentifierType((IdentifierType)type);
                var clzName = ShortenClassName(csharpType.TypeFullName);
                if (csharpType.IsTripEnum) clzName += "?";
                return clzName;
            }

            private string ShortenClassName(string className)
            {
                // If the class is in the package we are currently generating code for, generate
                // only the simple name, otherwise generate the fully qualified class name.
                if (className.StartsWith(typeToCSharpConverter.csharpNamespace) && className.LastIndexOf(".") == typeToCSharpConverter.csharpNamespace.Length)
                {
                    className = className.Substring(typeToCSharpConverter.csharpNamespace.Length + 1);
                }
                return className;
            }


            public GenType ConvertToGenType(BaijiType type)
            {
                var csharpTypeName = ConvertToString(type);
                var csharpType = this.FindCSharpTypeFromIdentifierType((IdentifierType)type);
                if (csharpType.IsTripEnum)
                {
                    return new GenType(GType.Enum, csharpTypeName);
                }
                else return new GenType(GType.Struct, csharpTypeName);
            }
        }

        private class ListConverter : Converter
        {
            private TypeToCSharpConverter typeToCSharpConverter;

            public ListConverter(TypeToCSharpConverter typeToCSharpConverter)
            {
                this.typeToCSharpConverter = typeToCSharpConverter;
            }

            public bool Accept(BaijiType type)
            {
                return type.GetType() == typeof(ListType);
            }

            public string ConvertToString(BaijiType type)
            {
                var listType = type as ListType;
                string actualType = typeToCSharpConverter.ConvertToString(listType.Type);
                return "List<" + actualType + ">";
            }

            public GenType ConvertToGenType(BaijiType type)
            {
                var csharpTypeName = ConvertToString(type);

                var listType = type as ListType;
                var genType = new GenType(GType.List, csharpTypeName);
                genType.ElementType = typeToCSharpConverter.ConvertToGenType(listType.Type);
                genType.ElementCSharpTypeName = typeToCSharpConverter.ConvertToString(listType.Type);
                return genType;
            }
        }

        private class MapConverter : Converter
        {
            private TypeToCSharpConverter typeToCSharpConverter;

            public MapConverter(TypeToCSharpConverter typeToCSharpConverter)
            {
                this.typeToCSharpConverter = typeToCSharpConverter;
            }

            public bool Accept(BaijiType type)
            {
                return type.GetType() == typeof(MapType);
            }

            public string ConvertToString(BaijiType type)
            {
                var mapType = type as MapType;

                string actualKeyType = typeToCSharpConverter.ConvertToString(mapType.KeyType);
                string actualValueType = typeToCSharpConverter.ConvertToString(mapType.ValueType);

                return string.Format("Dictionary<{0}, {1}>", actualKeyType, actualValueType);
            }

            public GenType ConvertToGenType(BaijiType type)
            {
                var csharpTypeName = ConvertToString(type);

                var mapType = type as MapType;
                var genType = new GenType(GType.Map, csharpTypeName);
                genType.KeyType = typeToCSharpConverter.ConvertToGenType(mapType.KeyType);
                genType.KeyCSharpTypeName = typeToCSharpConverter.ConvertToString(mapType.KeyType);
                genType.ValueType = typeToCSharpConverter.ConvertToGenType(mapType.ValueType);
                genType.ValueCSharpTypeName = typeToCSharpConverter.ConvertToString(mapType.ValueType);
                return genType;
            }
        }

        private interface Converter
        {
            /// <summary>
            /// Return true if the converter accepts the proposed type.
            /// </summary>
            /// <param name="type">trip type</param>
            /// <returns>ture if accept, false otherwise</returns>
            bool Accept(BaijiType type);

            /// <summary>
            /// Convert the trip type into a string suitable for a csharp type.
            /// </summary>
            /// <param name="type">trip type</param>
            /// <returns>string representation of csharp type</returns>
            string ConvertToString(BaijiType type);

            /// <summary>
            /// Convert the trip type into a code generation type model
            /// </summary>
            /// <param name="type">trip type</param>
            /// <returns>a code type model</returns>
            GenType ConvertToGenType(BaijiType type);
        }
    }
}
