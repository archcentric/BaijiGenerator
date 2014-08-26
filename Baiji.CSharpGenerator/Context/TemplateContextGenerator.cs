using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTripOSS.Baiji.CSharpGenerator;
using CTripOSS.Baiji.IDLParser.Model;

namespace CTripOSS.Baiji.CSharpGenerator.Context
{
    public class TemplateContextGenerator
    {
        private static readonly MethodContext DISPOSE_METHOD_CONTEXT = new MethodContext(null, null, true, "Dispose", "void", null, null);
        private readonly GeneratorConfig generatorConfig;
        private readonly TypeRegistry typeRegistry;
        private readonly TypeToCSharpConverter typeConverter;
        private readonly string defaultNamespace;

        public TemplateContextGenerator(GeneratorConfig generatorConfig,
            TypeRegistry typeRegistry,
            TypeToCSharpConverter typeConverter,
            string defaultNamespace)
        {
            this.generatorConfig = generatorConfig;
            this.typeRegistry = typeRegistry;
            this.defaultNamespace = defaultNamespace;
            this.typeConverter = typeConverter;
        }

        public ServiceContext ServiceFromTrip(Service service)
        {
            var name = MangleCSharpServiceName(service.Name);
            var csharpType = typeRegistry.FindType(defaultNamespace, name);

            var csharpParents = new HashSet<string>();
            string serviceName, serviceNamespace;
            GetServiceNameAndNamespace(service, out serviceName, out serviceNamespace);

            var serviceContext = new ServiceContext(service.DocStringLines, name, csharpType.TypeNamespace,
                                                    csharpType.TypeName, csharpParents, serviceName, serviceNamespace);
            if (generatorConfig.ContainsTweak(GeneratorTweak.ADD_DISPOSABLE_INTERFACE))
            {
                csharpParents.Add("IDisposable");
                //serviceContext.AddMethod(DISPOSE_METHOD_CONTEXT);
            }

            return serviceContext;
        }

        public ClientContext ClientFromTrip(Service service)
        {
            var name = MangleCSharpClientName(service.Name);
            var csharpType = typeRegistry.FindType(defaultNamespace, name);

            var csharpParents = new HashSet<string>();
            string serviceName, serviceNamespace;
            GetServiceNameAndNamespace(service, out serviceName, out serviceNamespace);

            var clientContext = new ClientContext(service.DocStringLines, name, csharpType.TypeNamespace,
                                                  csharpType.TypeName, csharpParents, serviceName, serviceNamespace);
            return clientContext;
        }

        private void GetServiceNameAndNamespace(Service service, out string serviceName, out string serviceNamespace)
        {
            serviceName = serviceNamespace = null;
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
        }

        public StructContext StructFromTrip(Struct _struct)
        {
            var name = MangleCSharpTypeName(_struct.Name);
            var csharpType = typeRegistry.FindType(defaultNamespace, name);

            return new StructContext(_struct.DocStringLines, name, csharpType.TypeNamespace, csharpType.TypeName,
                                     _struct.IsServiceResponse, 0);
        }

        public MethodContext MethodFromTrip(BaijiMethod method)
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

            return new MethodContext(method.DocStringLines, method.Name, false, MangleCSharpMethodName(method.Name),
                                     typeConverter.ConvertToString(method.ReturnType),
                                     MangleCSharpArgumentName(method.ArgumentName),
                                     typeConverter.ConvertToString(method.ArgumentType));
        }

        public FieldContext FieldFromTrip(BaijiField field)
        {
            var fc = new FieldContext(
                    field.DocStringLines,
                    field.Name,
                    (short)field.Identifier,
                    //typeConverter.ConvertToString(field.Type),
                    MangleCSharpFieldName(field.Name),
                    MangleCSharpTypeName(field.Name),
                    field.Required == Required.REQUIRED,
                    typeConverter.ConvertToGenType(field.Type)
                );

            return fc;

        }

        public EnumContext EnumFromTrip(IntegerEnum integerEnum)
        {
            var name = MangleCSharpTypeName(integerEnum.Name);
            var csharpType = typeRegistry.FindType(defaultNamespace, name);
            return new EnumContext(integerEnum.DocStringLines, csharpType.TypeNamespace, csharpType.TypeName);
        }

        public EnumFieldContext FieldFromTrip(IntegerEnumField field)
        {
            return new EnumFieldContext(field.DocStringLines, MangleCSharpConstantName(field.Name), field.Value);
        }

        public static string MangleCSharpConstantName(string src)
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

        public static string MangleCSharpArgumentName(string src)
        {
            return MangleCSharpName(src, false);
        }

        /// <summary>
        /// Turn an incoming snake case name into camel case for use in a csharp field name.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string MangleCSharpFieldName(string src)
        {
            return MangleCSharpName(src, false);
        }

        /// <summary>
        /// Turn an incoming snake case name into camel case for use in a csharp method name.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string MangleCSharpMethodName(string src)
        {
            return MangleCSharpName(src, true);
        }

        /// <summary>
        /// Turn an incoming snake case name into camel case for use in a csharp type name.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string MangleCSharpTypeName(string src)
        {
            return MangleCSharpName(src, true);
        }

        /// <summary>
        /// Turn an incoming snake case name into camel case for use in a csharp service name.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string MangleCSharpServiceName(string src)
        {
            return "I" + MangleCSharpName(src, true);
        }

        /// <summary>
        /// Turn an incoming snake case name into camel case for use in a csharp client name.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string MangleCSharpClientName(string src)
        {
            return MangleCSharpName(src, true) + "Client";
        }

        private static string MangleCSharpName(string src, bool capitalize)
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
    }
}
