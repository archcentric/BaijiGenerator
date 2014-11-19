using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTripOSS.Baiji.Generator.Visitor;
using CTripOSS.Baiji.IDLParser.Model;

namespace CTripOSS.Baiji.Generator.ObjectiveC.Visitor
{
    internal class OCTypeVisitor : TypeVisitor
    {
        public OCTypeVisitor(string codeNamespace, DocumentContext documentContext)
            : base(codeNamespace, documentContext)
        {
        }

        public override void Visit(IDLParser.Model.Document document)
        {
            foreach (var definition in document.Definitions)
            {
                var type = new CodeType(_documentContext.Namespace, definition.Name,
                    _documentContext.TypeMangler.MangleTypeName(_codeNamespace + definition.Name), _codeNamespace, definition);
                if (definition is Struct)
                {
                    type.IsStruct = true;
                }
                else if (definition is IntegerEnum)
                {
                    type.IsEnum = true;
                }
                LOG.Debug(string.Format("Registering type '{0}'", type));
                _documentContext.TypeRegistry.Add(type);
            }
        }
    }
}
