using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.Helper;
using CTripOSS.Baiji.JavaGenerator.Context;

namespace CTripOSS.Baiji.JavaGenerator
{
    public class TypeToJavaConverter
    {
        private readonly string _namespace;
        private readonly TypeRegistry typeRegistry;
        private readonly List<Converter> converters;
        private readonly string javaPackage;

        public TypeToJavaConverter(TypeRegistry typeRegistry,
            string _namespace,
            string javaPackage)
        {
            Enforce.IsNotNull<TypeRegistry>(typeRegistry, "typeRegistry");
            Enforce.IsNotNull<string>(_namespace, "namespace");
            this.typeRegistry = typeRegistry;
            this._namespace = _namespace;
            this.javaPackage = javaPackage;
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
            throw new ArgumentException(string.Format("Lean type {0} is unknown!", tripType));
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
            throw new ArgumentException(string.Format("Lean type {0} is unknown!", tripType));
        }

        private class BaseConverter : Converter
        {
            private static readonly Dictionary<BType, string> JAVA_PRIMITIVES_MAP;
            private static readonly Dictionary<BType, GType> GTYPE_BASETYPE_MAP;

            static BaseConverter()
            {
                JAVA_PRIMITIVES_MAP = new Dictionary<BType, string>();
                JAVA_PRIMITIVES_MAP[BType.BOOL] = "Boolean";
                JAVA_PRIMITIVES_MAP[BType.BYTE] = "Byte";
                JAVA_PRIMITIVES_MAP[BType.I32] = "Integer";
                JAVA_PRIMITIVES_MAP[BType.I64] = "Long";
                JAVA_PRIMITIVES_MAP[BType.DOUBLE] = "Double";
                JAVA_PRIMITIVES_MAP[BType.STRING] = "String";
                JAVA_PRIMITIVES_MAP[BType.BINARY] = "byte[]";

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
                return JAVA_PRIMITIVES_MAP[baseType];
            }


            public GenType ConvertToGenType(BaijiType type)
            {
                var javaTypeName = ConvertToString(type);
                var baseType = ((BaseType)type).BType;
                var gType = GTYPE_BASETYPE_MAP[baseType];
                var genType = new GenType(gType, javaTypeName);
                return genType;
            }
        }

        public bool IsEnumIdentifier(IdentifierType id)
        {
            var javaType = this.FindJavaTypeFromIdentifierType(id);
            return javaType.IsLeanEnum;
        }

        public bool IsStructIdentifier(IdentifierType id)
        {
            var javaType = this.FindJavaTypeFromIdentifierType(id);
            return javaType.IsLeanStruct;
        }

        private JavaType FindJavaTypeFromIdentifierType(IdentifierType id)
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

            var javaTypeName = tripNamespace + "." + TemplateContextGenerator.MangleJavaTypeName(tripName);
            var javaType = this.typeRegistry.FindType(javaTypeName);
            return javaType;
        }

        private class IdentifierConverter : Converter
        {
            private TypeToJavaConverter typeToJavaConverter;

            public IdentifierConverter(TypeToJavaConverter typeToJavaConverter)
            {
                this.typeToJavaConverter = typeToJavaConverter;
            }

            public bool Accept(BaijiType type)
            {
                return type.GetType() == typeof(IdentifierType);
            }

            private JavaType FindJavaTypeFromIdentifierType(IdentifierType id)
            {
                var name = id.Name;
                // the name is [<trip-namespace>.]<trip type>
                var names = new List<string>(name.Split('.'));

                if (names.Count == 0 || names.Count > 3)
                {
                    throw new ArgumentException("Only unqualified and trip-namespace qualified names are allowed!");
                }
                string tripName = names[0];
                string tripNamespace = typeToJavaConverter._namespace;

                if (names.Count == 2)
                {
                    tripName = names[1];
                    tripNamespace = names[0];
                }

                var javaTypeName = tripNamespace + "." + TemplateContextGenerator.MangleJavaTypeName(tripName);
                var javaType = typeToJavaConverter.typeRegistry.FindType(javaTypeName);
                return javaType;
            }

            public string ConvertToString(BaijiType type)
            {
                var javaType = this.FindJavaTypeFromIdentifierType((IdentifierType)type);
                return ShortenClassName(javaType.ClassName);
            }

            private string ShortenClassName(string className)
            {
                // If the class is in the package we are currently generating code for, generate
                // only the simple name, otherwise generate the fully qualified class name.
                if (className.StartsWith(typeToJavaConverter.javaPackage) && className.LastIndexOf(".") == typeToJavaConverter.javaPackage.Length)
                {
                    className = className.Substring(typeToJavaConverter.javaPackage.Length + 1);
                }
                return className;
            }

            public GenType ConvertToGenType(BaijiType type)
            {
                var javaTypeName = ConvertToString(type);
                var javaType = this.FindJavaTypeFromIdentifierType((IdentifierType)type);
                if (javaType.IsLeanEnum)
                {
                    return new GenType(GType.Enum, javaTypeName);
                }
                else return new GenType(GType.Struct, javaTypeName);
            }
        }

        private class ListConverter : Converter
        {
            private TypeToJavaConverter typeToJavaConverter;

            public ListConverter(TypeToJavaConverter typeToJavaConverter)
            {
                this.typeToJavaConverter = typeToJavaConverter;
            }

            public bool Accept(BaijiType type)
            {
                return type.GetType() == typeof(ListType);
            }

            public string ConvertToString(BaijiType type)
            {
                var listType = type as ListType;
                string actualType = typeToJavaConverter.ConvertToString(listType.Type);
                return "List<" + actualType + ">";
            }

            public GenType ConvertToGenType(BaijiType type)
            {
                var javaTypeName = ConvertToString(type);

                var listType = type as ListType;
                var genType = new GenType(GType.List, javaTypeName);
                genType.ElementType = typeToJavaConverter.ConvertToGenType(listType.Type);
                genType.ElementJavaTypeName = typeToJavaConverter.ConvertToString(listType.Type);
                return genType;
            }
        }

        private class MapConverter : Converter
        {
            private TypeToJavaConverter typeToJavaConverter;

            public MapConverter(TypeToJavaConverter typeToJavaConverter)
            {
                this.typeToJavaConverter = typeToJavaConverter;
            }

            public bool Accept(BaijiType type)
            {
                return type.GetType() == typeof(MapType);
            }

            public string ConvertToString(BaijiType type)
            {
                var mapType = type as MapType;

                string actualKeyType = typeToJavaConverter.ConvertToString(mapType.KeyType);
                string actualValueType = typeToJavaConverter.ConvertToString(mapType.ValueType);

                return string.Format("Map<{0}, {1}>", actualKeyType, actualValueType);
            }

            public GenType ConvertToGenType(BaijiType type)
            {
                var javaTypeName = ConvertToString(type);

                var mapType = type as MapType;
                var genType = new GenType(GType.Map, javaTypeName);
                genType.KeyType = typeToJavaConverter.ConvertToGenType(mapType.KeyType);
                genType.KeyJavaTypeName = typeToJavaConverter.ConvertToString(mapType.KeyType);
                genType.ValueType = typeToJavaConverter.ConvertToGenType(mapType.ValueType);
                genType.ValueJavaTypeName = typeToJavaConverter.ConvertToString(mapType.ValueType);
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
            /// Convert the trip type into a string suitable for a java type.
            /// </summary>
            /// <param name="type">trip type</param>
            /// <returns>string representation of java type</returns>
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
