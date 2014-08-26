using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using CTripOSS.Baiji.IDLParser.Visitor;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.Helper;
using CTripOSS.Baiji.JavaGenerator.Context;


namespace CTripOSS.Baiji.JavaGenerator.Visitor
{
    public class TypeVisitor : IVisitor
    {
        private static ILog LOG = LogManager.GetLogger(typeof(TypeVisitor));
        private string javaNamespace;
        private DocumentContext documentContext;

        public TypeVisitor(string javaNamespace, DocumentContext documentContext)
        {
            this.javaNamespace = javaNamespace;
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
                var javaType = new JavaType(documentContext.Namespace, TemplateContextGenerator.MangleJavaTypeName(definition.Name), javaNamespace);
                if (definition is Struct)
                {
                    javaType.IsLeanStruct = true;
                }
                else if (definition is IntegerEnum)
                {
                    javaType.IsLeanEnum = true;
                }

                LOG.Debug(string.Format("Registering type '{0}'", javaType));
                documentContext.TypeRegistry.Add(javaType);
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

        public void Visit(BaijiField field)
        {
            throw new NotImplementedException();
        }

        public void Visit(BaijiMethod method)
        {
            throw new NotImplementedException();
        }
    }
}