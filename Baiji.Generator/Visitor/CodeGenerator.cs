using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Antlr4.StringTemplate;
using CTripOSS.Baiji.Generator.Context;
using CTripOSS.Baiji.Generator.Util;
using CTripOSS.Baiji.Helper;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.IDLParser.Visitor;

namespace CTripOSS.Baiji.Generator.Visitor
{
    /// <summary>
    /// Generate  source by visiting the document model
    /// </summary>
    public abstract class CodeGenerator : IVisitor
    {
        protected readonly DirectoryInfo _outputFolder;
        protected readonly TemplateLoader _templateLoader;
        protected readonly TemplateContextGenerator _contextGenerator;
        protected readonly GeneratorConfig _config;

        protected CodeGenerator(TemplateLoader templateLoader, DocumentContext context,
            GeneratorConfig config, DirectoryInfo outputFolder)
        {
            _outputFolder = outputFolder;
            _templateLoader = templateLoader;
            _contextGenerator = context.TemplateContextGenerator;
            _config = config;
        }

        protected abstract string FileExtension
        {
            get;
        }

        protected abstract string GenClientTweak
        {
            get;
        }

        protected abstract IDictionary<string, bool> GetTweakMap();

        private void Render(CodeContext context, string templateName)
        {
            var template = _templateLoader.Load(templateName);
            Enforce.IsNotNull(template, string.Format("No template for '{0}' found!", templateName));
            template.Add("context", context);

            var tweakMap = GetTweakMap();
            template.Add("tweaks", tweakMap);

            var globalValues = new Dictionary<string, string>();
            var codeGenVersion = Assembly.GetExecutingAssembly().GetName().Version;
            globalValues.Add("CodeGenVersion", codeGenVersion.ToString());
            template.Add("global", globalValues);

            var packages = context.Namespace.Split('.');
            DirectoryInfo folder = _outputFolder;

            foreach (string pkg in packages)
            {
                folder = folder.CreateSubdirectory(pkg);
            }

            var filename = Path.Combine(folder.FullName, context.TypeName + FileExtension);
            using (var writer = new StreamWriter(filename, false /*, Encoding.UTF8*/))
            {
                template.Write(new AutoIndentWriter(writer));
                writer.Flush();
            }
        }

        public void Visit(BaseType type)
        {
            throw new NotSupportedException();
        }

        public void Visit(Document document)
        {
            foreach (var definition in document.Definitions)
            {
                if (definition is Struct)
                {
                    Visit((Struct)definition);
                }
                else if (definition is IntegerEnum)
                {
                    Visit((IntegerEnum)definition);
                }
                else if (definition is Service)
                {
                    Visit((Service)definition);
                }
            }
        }

        public void Visit(Header header)
        {
            throw new NotSupportedException();
        }

        public void Visit(IdentifierType identifierType)
        {
            throw new NotSupportedException();
        }

        public void Visit(IntegerEnum integerEnum)
        {
            var enumContext = _contextGenerator.EnumFromIdl(integerEnum);

            foreach (var field in integerEnum.Fields)
            {
                enumContext.AddField(_contextGenerator.FieldFromIdl(field));
            }

            Render(enumContext, "intEnum");
        }

        public void Visit(IntegerEnumField integerEnumField)
        {
            throw new NotSupportedException();
        }

        public void Visit(ListType listType)
        {
            throw new NotSupportedException();
        }

        public void Visit(MapType mapType)
        {
            throw new NotSupportedException();
        }

        public void Visit(Service service)
        {
            if (!string.IsNullOrEmpty(GenClientTweak) && _config.ContainsTweak(GenClientTweak))
            {
                var clientContext = _contextGenerator.ClientFromIdl(service);
                foreach (var method in service.Methods)
                {
                    var methodContext = _contextGenerator.MethodFromIdl(method);
                    clientContext.AddMethod(methodContext);
                }
                Render(clientContext, "client");
            }
            else
            {
                var serviceContext = _contextGenerator.ServiceFromIdl(service);
                foreach (var method in service.Methods)
                {
                    var methodContext = _contextGenerator.MethodFromIdl(method);
                    serviceContext.AddMethod(methodContext);
                }
                Render(serviceContext, "service");
            }
        }

        public void Visit(Struct @struct)
        {
            var structContext = _contextGenerator.StructFromIdl(@struct);

            FieldContext responseStatusField = null;
            foreach (var field in structContext.Fields)
            {
                if (field.IdlName == "responseStatus" || field.IdlName == "ResponseStatus")
                {
                    responseStatusField = field;
                }
            }
            if (@struct.IsServiceResponse)
            {
                if (responseStatusField == null)
                {
                    var message =
                        string.Format("{0} must have a responseStatus field in order to be used as a service response.",
                            @struct.Name);
                    throw new ArgumentException(message);
                }
                var fieldType = responseStatusField.GenType.TypeName;
                var lastTypeSegment = fieldType != null ? fieldType.Split('.').Last() : null;
                if (lastTypeSegment != "ResponseStatusType")
                {
                    var message = string.Format("The type of {0} field in {1} must be ResponseStatusType.",
                        responseStatusField.IdlName, @struct.Name);
                    throw new ArgumentException(message);
                }
            }

            Render(structContext, "struct");
        }

        public void Visit(BaijiField field)
        {
            throw new NotSupportedException();
        }

        public void Visit(BaijiMethod method)
        {
            throw new NotSupportedException();
        }
    }
}