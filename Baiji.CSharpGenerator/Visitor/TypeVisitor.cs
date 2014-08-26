using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using CTripOSS.Baiji.IDLParser.Visitor;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.Helper;
using CTripOSS.Baiji.CSharpGenerator.Context;


namespace CTripOSS.Baiji.CSharpGenerator.Visitor
{
    public class TypeVisitor : IVisitor
    {
        private static ILog LOG = LogManager.GetLogger(typeof(TypeVisitor));
        private string csharpNamespace;
        private DocumentContext documentContext;

        public TypeVisitor(string csharpNamespace, DocumentContext documentContext)
        {
            this.csharpNamespace = csharpNamespace;
            this.documentContext = documentContext;
        }

        public void Visit(BaseType type)
        {
            throw new NotImplementedException();
        }

        public void Visit(Document document)
        {
            foreach (var definition in document.Definitions)
            {
                if (definition is Service)
                {
                    var serviceType = new CSharpType(documentContext.Namespace,
                                                     TemplateContextGenerator.MangleCSharpServiceName(definition.Name),
                                                     csharpNamespace);
                    LOG.Debug(string.Format("Registering service type '{0}'", serviceType));
                    documentContext.TypeRegistry.Add(serviceType);

                    var clientType = new CSharpType(documentContext.Namespace,
                                                    TemplateContextGenerator.MangleCSharpClientName(definition.Name),
                                                    csharpNamespace);
                    LOG.Debug(string.Format("Registering client type '{0}'", clientType));
                    documentContext.TypeRegistry.Add(clientType);
                    return;
                }

                var csharpType = new CSharpType(documentContext.Namespace,
                                                TemplateContextGenerator.MangleCSharpTypeName(definition.Name),
                                                csharpNamespace);
                if (definition is Struct)
                {
                    csharpType.IsTripStruct = true;
                }
                else if (definition is IntegerEnum)
                {
                    csharpType.IsTripEnum = true;
                }

                LOG.Debug(string.Format("Registering type '{0}'", csharpType));
                documentContext.TypeRegistry.Add(csharpType);
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Visit(Struct _struct)
        {
            throw new NotImplementedException();
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
