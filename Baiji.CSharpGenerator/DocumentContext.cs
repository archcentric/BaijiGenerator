using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using CTripOSS.Baiji.IDLParser;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.Helper;
using CTripOSS.Baiji.CSharpGenerator.Context;

namespace CTripOSS.Baiji.CSharpGenerator
{
    public class DocumentContext
    {
        private GeneratorConfig generatorConfig;
        private Uri tripUri;

        public Document Document { get; private set; }
        public string Namespace { get; private set; }
        public TypeRegistry TypeRegistry { get; private set; }
        public TypeToCSharpConverter TypeConverter { get; private set; }

        public DocumentContext(Uri tripUri,
            string _namespace, GeneratorConfig generatorConfig,
            TypeRegistry typeRegistry)
        {
            Document = IdlParser.BuildDocument(tripUri);
            this.tripUri = tripUri;
            Namespace = _namespace;
            this.generatorConfig = generatorConfig;
            TypeRegistry = typeRegistry;
            TypeConverter = new TypeToCSharpConverter(typeRegistry, _namespace, CSharpNamespace);
        }

        public TemplateContextGenerator TemplateContextGenerator
        {
            get
            {
                return new TemplateContextGenerator(generatorConfig, TypeRegistry, TypeConverter, Namespace);
            }
        }

        public string CSharpNamespace
        {
            get
            {
                string effectiveCSharpNamespace = "trip";
                if (generatorConfig.ContainsTweak(GeneratorTweak.USE_PLAIN_CSHARP_NAMESPACE))
                {
                    effectiveCSharpNamespace = "csharp";
                }

                // Override takes precedence
                string csharpNamespace = generatorConfig.OverrideNamespace;
                // Otherwise fallback on namespace specified in .trip file
                if (csharpNamespace == null && Document.Header != null && Document.Header.Namespaces.ContainsKey(effectiveCSharpNamespace))
                {
                    csharpNamespace = Document.Header.Namespaces[effectiveCSharpNamespace];
                }
                // Or the default if we don't have an override namespace or a namespace in the .trip file
                if (csharpNamespace == null)
                {
                    csharpNamespace = generatorConfig.DefaultNamespace;
                }

                // If none of the above options get us a namespace to use, fail
                Enforce.IsNotNull(csharpNamespace, string.Format("trip uri {0} does not include a '{1}' namespace!", tripUri, effectiveCSharpNamespace));

                return csharpNamespace;
            }
        }
    }
}
