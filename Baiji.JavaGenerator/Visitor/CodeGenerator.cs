using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Antlr4.StringTemplate;
using CTripOSS.Baiji.Helper;
using CTripOSS.Baiji.IDLParser.Visitor;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.JavaGenerator.Context;
using CTripOSS.Baiji.JavaGenerator.Util;

namespace CTripOSS.Baiji.JavaGenerator.Visitor
{
    /// <summary>
    /// Generate  source by visiting the trip document model
    /// </summary>
    public class CodeGenerator : IVisitor
    {
        private DirectoryInfo outputFolder;
        private TemplateLoader templateLoader;
        private TemplateContextGenerator contextGenerator;
        private GeneratorConfig config;

        public CodeGenerator(TemplateLoader templateLoader, DocumentContext context,
            GeneratorConfig config, DirectoryInfo outputFolder)
        {
            this.outputFolder = outputFolder;
            this.templateLoader = templateLoader;
            this.contextGenerator = context.TemplateContextGenerator;
            this.config = config;
        }

        private void Render(JavaContext context, string templateName)
        {
            var template = templateLoader.Load(templateName);
            Enforce.IsNotNull(template, string.Format("No template for '{0}' found!", templateName));
            template.Add("context", context);

            var tweakMap = new Dictionary<string, bool>();
            var tweakValues = Enum.GetValues(typeof(GeneratorTweak));
            foreach (var tweak in tweakValues)
            {
                tweakMap.Add(tweak.ToString(), config.ContainsTweak((GeneratorTweak)tweak));
            }
            template.Add("tweaks", tweakMap);

            var globalValues = new Dictionary<string, string>();
            var codeGenVersion = Assembly.GetExecutingAssembly().GetName().Version;
            globalValues.Add("CodeGenVersion", codeGenVersion.ToString());
            template.Add("global", globalValues);

            var packages = context.JavaPackage.Split('.');
            DirectoryInfo folder = outputFolder;

            foreach (string pkg in packages)
            {
                folder = folder.CreateSubdirectory(pkg);
            }

            var filename = Path.Combine(folder.FullName, context.JavaName + ".java");
            using (StreamWriter writer = new StreamWriter(filename, false/*, Encoding.UTF8*/))
            {
                template.Write(new AutoIndentWriter(writer));
                writer.Flush();
            }
        }

        public void Visit(BaseType type)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Visit(IdentifierType identifierType)
        {
            throw new NotImplementedException();
        }

        public void Visit(IntegerEnum integerEnum)
        {
            var enumContext = contextGenerator.EnumFromLean(integerEnum);

            foreach (var field in integerEnum.Fields)
            {
                enumContext.AddField(contextGenerator.FieldFromLean(field));
            }

            Render(enumContext, "intEnum");
        }

        public void Visit(IntegerEnumField integerEnumField)
        {
            throw new NotImplementedException();
        }

        public void Visit(ListType listType)
        {
            throw new NotImplementedException();
        }

        public void Visit(MapType mapType)
        {
            throw new NotImplementedException();
        }

        public void Visit(Service service)
        {
            var serviceContext = contextGenerator.ServiceFromLean(service);

            foreach (var method in service.Methods)
            {
                var methodContext = contextGenerator.MethodFromLean(method);
                serviceContext.AddMethod(methodContext);
            }

            Render(serviceContext, "service");
        }

        public void Visit(Struct _struct)
        {
            var structContext = contextGenerator.StructFromLean(_struct);

            BaijiField responseStatusField = null;
            foreach (var field in _struct.Fields)
            {
                if (field.Name == "responseStatus" || field.Name == "ResponseStatus")
                {
                    responseStatusField = field;
                }
                structContext.AddField(contextGenerator.FieldFromLean(field));
            }

            if (_struct.IsServiceResponse)
            {
                if (responseStatusField == null)
                {
                    var message = string.Format("{0} must have a responseStatus field in order to be used as a service response.",
                                                _struct.Name);
                    throw new ArgumentException(message);
                }
                var fieldType = responseStatusField.Type as IdentifierType;
                var lastTypeSegment = fieldType != null ? fieldType.Name.Split('.').Last() : null;
                if (lastTypeSegment != "ResponseStatusType")
                {
                    var message = string.Format("The type of {0} field in {1} must be ResponseStatusType.",
                                                responseStatusField.Name, _struct.Name);
                    throw new ArgumentException(message);
                }
            }

            Render(structContext, "struct");
        }

        public void Visit(BaijiField tripField)
        {
            throw new NotImplementedException();
        }

        public void Visit(BaijiMethod tripMethod)
        {
            throw new NotImplementedException();
        }
    }
}
