using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTripOSS.Baiji.IDLParser;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.Helper;
using System.Net;
using CTripOSS.Baiji.JavaGenerator.Context;

namespace CTripOSS.Baiji.JavaGenerator
{
    public class DocumentContext
    {
        private GeneratorConfig generatorConfig;
        private Uri tripUri;

        public Document Document { get; private set; }
        public string Namespace { get; private set; }
        public TypeRegistry TypeRegistry { get; private set; }
        public TypeToJavaConverter TypeConverter { get; private set; }

        public DocumentContext(Uri tripUri,
            string _namespace, GeneratorConfig generatorConfig,
            TypeRegistry typeRegistry)
        {
            Document = IdlParser.BuildDocument(tripUri);
            this.tripUri = tripUri;
            Namespace = _namespace;
            this.generatorConfig = generatorConfig;
            TypeRegistry = typeRegistry;
            TypeConverter = new TypeToJavaConverter(typeRegistry, _namespace, JavaPackage);
        }

        public TemplateContextGenerator TemplateContextGenerator
        {
            get
            {
                return new TemplateContextGenerator(generatorConfig, TypeRegistry, TypeConverter, Namespace);
            }
        }

        public string JavaPackage
        {
            get
            {
                string effectiveJavaNamespace = "trip";
                if (generatorConfig.ContainsTweak(GeneratorTweak.USE_PLAIN_JAVA_NAMESPACE))
                {
                    effectiveJavaNamespace = "java";
                }

                // Override takes precedence
                string javaPackage = generatorConfig.OverridePackage;
                // Otherwise fallback on package specified in .trip file
                if (javaPackage == null && Document.Header != null && Document.Header.Namespaces.ContainsKey(effectiveJavaNamespace))
                {
                    javaPackage = Document.Header.Namespaces[effectiveJavaNamespace];
                }
                // Or the default if we don't have an override package or a package in the .trip file
                if (javaPackage == null)
                {
                    javaPackage = generatorConfig.DefaultPackage;
                }

                // If none of the above options get us a package to use, fail
                Enforce.IsNotNull(javaPackage, string.Format("trip uri {0} does not include a '{1}' namespace!", tripUri, effectiveJavaNamespace));

                return javaPackage;
            }
        }
    }
}
