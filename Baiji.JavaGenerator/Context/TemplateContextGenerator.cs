using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTripOSS.Baiji.IDLParser.Model;

namespace CTripOSS.Baiji.JavaGenerator.Context
{
    public class TemplateContextGenerator
    {
        private readonly GeneratorConfig generatorConfig;
        private readonly TypeRegistry typeRegistry;
        private readonly TypeToJavaConverter typeConverter;
        private readonly string defaultNamespace;

        public TemplateContextGenerator(GeneratorConfig generatorConfig,
            TypeRegistry typeRegistry,
            TypeToJavaConverter typeConverter,
            string defaultNamespace)
        {
            this.generatorConfig = generatorConfig;
            this.typeRegistry = typeRegistry;
            this.defaultNamespace = defaultNamespace;
            this.typeConverter = typeConverter;
        }

        public ServiceContext ServiceFromLean(Service service)
        {
            var name = MangleJavaTypeName(service.Name);
            var javaType = typeRegistry.FindType(defaultNamespace, name);

            var javaParents = new HashSet<string>();

            string serviceName = null, serviceNamespace = null;
            if (service.Annotations != null && service.Annotations.Count != 0)
            {
                var serviceNameAnnotation = service.Annotations.FirstOrDefault(a => a.Name == "serviceName");
                serviceName = serviceNameAnnotation != null ? serviceNameAnnotation.Value : null;
                var serviceNamespaceAnnotation = service.Annotations.FirstOrDefault(a => a.Name == "serviceNamespace");
                serviceNamespace = serviceNamespaceAnnotation != null ? serviceNamespaceAnnotation.Value : null;
            }
            if (serviceName == null || serviceNamespace == null)
            {
                var message = string.Format("serviceName and serviceNamespace annotations are required for {0}.",
                                            service.Name);
                throw new ArgumentException(message);
            }
            var serviceContext = new ServiceContext(service.DocStringLines, name, javaType.Package, javaType.SimpleName,
                                                    javaParents, serviceName, serviceNamespace);

            //javaParents.Add("java.io.Serializable");

            return serviceContext;
        }

        public StructContext StructFromLean(Struct _struct)
        {
            var name = MangleJavaTypeName(_struct.Name);
            var javaType = typeRegistry.FindType(defaultNamespace, name);

            return new StructContext(_struct.DocStringLines, name, javaType.Package, javaType.SimpleName, _struct.IsServiceResponse, 0);
        }

        public MethodContext MethodFromLean(BaijiMethod method)
        {
            if (!typeConverter.IsStructIdentifier(method.ReturnType))
            {
                throw new ArgumentException(
                    string.Format("Return type of method must be a struct type, method {0}, return type {1}.", method.Name, method.ReturnType.Name));
            }
            if (!typeConverter.IsStructIdentifier(method.ArgumentType))
            {
                throw new ArgumentException(
                    string.Format("Argument type of method must be a struct type, method {0}, argument type {1}.", method.Name, method.ArgumentType.Name));
            }

            return new MethodContext(method.DocStringLines, method.Name, false, MangleJavaMethodName(method.Name),
                                     typeConverter.ConvertToString(method.ReturnType),
                                     MangleJavaArgumentName(method.ArgumentName),
                                     typeConverter.ConvertToString(method.ArgumentType));
        }

        public FieldContext FieldFromLean(BaijiField field)
        {
            var fc = new FieldContext(field.DocStringLines,
                                      field.Name,
                                      (short)field.Identifier,
                                      //typeConverter.ConvertToString(field.Type),
                                      MangleJavaMethodName(field.Name),
                                      SetterName(field),
                                      GetterName(field),
                                      field.Required == Required.REQUIRED,
                                      typeConverter.ConvertToGenType(field.Type)
                );

            return fc;

        }

        public EnumContext EnumFromLean(IntegerEnum integerEnum)
        {
            var name = MangleJavaTypeName(integerEnum.Name);
            var javaType = typeRegistry.FindType(defaultNamespace, name);
            return new EnumContext(integerEnum.DocStringLines, javaType.Package, javaType.SimpleName);
        }

        public EnumFieldContext FieldFromLean(IntegerEnumField field)
        {
            return new EnumFieldContext(field.DocStringLines, MangleJavaConstantName(field.Name), field.Value);
        }

        public static string MangleJavaConstantName(string src)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(src))
            {
                bool lowerCase = false;
                for (int i = 0; i < src.Length; i++)
                {
                    if (Char.IsUpper(src[i]))
                    {
                        if (lowerCase)
                        {
                            sb.Append('_');
                        }
                        sb.Append(Char.ToUpper(src[i]));
                        lowerCase = false;
                    }
                    else
                    {
                        sb.Append(Char.ToUpper(src[i]));
                        if (Char.IsLetter(src[i]))
                        {
                            lowerCase = true;
                        }
                    }
                }
            }
            return sb.ToString();
        }

        public static string MangleJavaArgumentName(string src)
        {
            return MangleJavaName(src, false);
        }

        /// <summary>
        /// Turn an incoming snake case name into camel case for use in a java method name.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string MangleJavaMethodName(string src)
        {
            return MangleJavaName(src, false);
        }

        /// <summary>
        /// Turn an incoming snake case name into camel case for use in a java type name.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string MangleJavaTypeName(string src)
        {
            return MangleJavaName(src, true);
        }

        private static string MangleJavaName(string src, bool capitalize)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                throw new ArgumentException("Input name must not be blank!");
            }

            var sb = new StringBuilder();
            if (src.Length > 0)
            {
                sb.Append(capitalize ? Char.ToUpper(src[0]) : Char.ToLower(src[0]));
                var forceUpCase = false;
                for (int i = 1; i < src.Length; i++)
                {
                    if (src[i] == '_')
                    {
                        forceUpCase = true;
                        continue;
                    }
                    sb.Append(forceUpCase ? Char.ToUpper(src[i]) : src[i]);
                    forceUpCase = false;
                }
            }
            return sb.ToString();
        }

        private string GetterName(BaijiField field)
        {
            string type = typeConverter.ConvertToString(field.Type);
            return ("Boolean".Equals(type) ? "is" : "get") + MangleJavaTypeName(field.Name);
        }

        private string SetterName(BaijiField field)
        {
            return "set" + MangleJavaTypeName(field.Name);
        }
    }
}
