using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using CTripOSS.Baiji.CSharpGenerator.Util;
using CTripOSS.Baiji.CSharpGenerator.Visitor;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.Helper;

namespace CTripOSS.Baiji.CSharpGenerator
{
    /// <summary>
    /// Parses a Trip IDL file and writes out csharp classes.
    /// </summary>
    public class Generator
    {
        private static ILog LOG = LogManager.GetLogger(typeof(Generator));

        private static Dictionary<string, List<string>> TEMPLATES = new Dictionary<string, List<string>>
        {
            {"csharp-regular", new List<string> { "common_st", "regular_st" }},
            {"csharp-immutable", new List<string> { "common_st", "immutable_st" }},
            {"csharp-ctor", new List<string> { "common_st", "ctor_st" }},
        };

        private DirectoryInfo outputFolder;
        private GeneratorConfig generatorConfig;
        private TemplateLoader templateLoader;
        private ISet<Uri> parsedDocuments = new HashSet<Uri>();
        private Stack<Uri> parentDocuments = new Stack<Uri>();

        public Generator(GeneratorConfig generatorConfig)
        {
            if (!TEMPLATES.ContainsKey(generatorConfig.CodeFlavor))
            {
                throw new ArgumentException(string.Format("Templating type {0} is unknown!", generatorConfig.CodeFlavor));
            }
            this.generatorConfig = generatorConfig;

            this.outputFolder = generatorConfig.OutputFolder;
            if (outputFolder != null && !outputFolder.Exists)
            {
                outputFolder.Create();
            }

            LOG.Debug(string.Format("Writing source files into {0} using {1} ...", outputFolder, generatorConfig.CodeFlavor));

            this.templateLoader = new TemplateLoader(TEMPLATES[generatorConfig.CodeFlavor]);
        }

        public void Parse(IList<Uri> inputs)
        {
            if (inputs == null || inputs.Count == 0)
            {
                throw new ArgumentException("No input files");
            }

            LOG.Info(string.Format("Parsing Trip IDL from {0}...", inputs));

            Dictionary<string, DocumentContext> contexts = new Dictionary<string, DocumentContext>();
            foreach (var inputUri in inputs)
            {
                parsedDocuments.Clear();
                Uri input = null;
                if (!inputUri.IsAbsoluteUri)
                {
                    Uri.TryCreate(generatorConfig.InputBase, inputUri, out input);
                }
                else
                {
                    input = inputUri;
                }

                parseDocument(input, contexts, new TypeRegistry());
            }

            markServiceResponseTypes(contexts);

            LOG.Info("IDL parsing complete, writing csharp code...");

            foreach (var context in contexts.Values)
            {
                generateFiles(context);
            }

            LOG.Info("CSharp code generation complete.");
        }

        private void parseDocument(Uri tripUri,
                                   Dictionary<string, DocumentContext> contexts,
                                   TypeRegistry typeRegistry)
        {
            if (tripUri == null || !tripUri.IsAbsoluteUri)
            {
                throw new ArgumentException("Only absolute URIs can be parsed!");
            }
            if (parentDocuments.Contains(tripUri))
            {
                throw new ArgumentException(string.Format("Input {0} recursively includes itself ({1})", tripUri, string.Join(" -> ", parentDocuments) + " -> " + tripUri));
            }

            if (parsedDocuments.Contains(tripUri))
            {
                LOG.Debug(string.Format("Skipping already parsed file {0}...", tripUri));
                return;
            }

            LOG.Debug(string.Format("Parsing {0}...", tripUri));

            var tripNamespace = extractTripNamespace(tripUri);
            if (string.IsNullOrWhiteSpace(tripNamespace))
            {
                throw new ArgumentException(string.Format("Trip URI {0} can not be translated to a namespace", tripUri));
            }

            var context = new DocumentContext(tripUri, tripNamespace, generatorConfig, typeRegistry);

            var document = context.Document;
            var header = document.Header;

            var csharpNamespace = context.CSharpNamespace;

            // Make a note that this document is a parent of all the documents included, directly or recursively
            parentDocuments.Push(tripUri);

            try
            {
                if (header != null)
                {
                    foreach (var include in header.Includes)
                    {
                        Uri includeUri = null;
                        Uri.TryCreate(generatorConfig.InputBase, include, out includeUri);
                        parseDocument(includeUri,
                            // If the includes should also generate code, pass the list of
                            // contexts down to the include parser, otherwise pass a null in
                                      generatorConfig.GenerateIncludedCode ? contexts : null,
                                      typeRegistry);
                    }
                }
            }
            finally
            {
                // Done parsing this document's includes, remove it from the parent chain
                parentDocuments.Pop();
            }

            // Make a note that we've already passed this document
            parsedDocuments.Add(tripUri);

            new TypeVisitor(csharpNamespace, context).Visit(document);

            if (contexts != null)
            {
                if (contexts.ContainsKey(context.Namespace))
                {
                    LOG.Info(string.Format("Trip Namespace {0} included multiple times!", context.Namespace));
                }
                contexts[context.Namespace] = context;
            }
        }

        private void markServiceResponseTypes(IDictionary<string, DocumentContext> contexts)
        {
            foreach (var context in contexts.Values)
            {
                if (context.Document.Definitions == null)
                {
                    continue;
                }
                foreach (var service in context.Document.Definitions.OfType<Service>())
                {
                    foreach (var method in service.Methods)
                    {
                        var returnTypeName = method.ReturnType.Name;
                        var nameSegments = returnTypeName.Split('.');
                        if (nameSegments.Length != 2)
                        {
                            // Ignore locally referenced structs. They have been marked when parsing the document.
                            continue;
                        }
                        string contextName = nameSegments[0], structName = nameSegments[1];
                        DocumentContext referredContext;
                        contexts.TryGetValue(contextName, out referredContext);
                        if (referredContext == null || referredContext.Document.Definitions == null)
                        {
                            continue;
                        }
                        var referredStruct = referredContext.Document.Definitions.OfType<Struct>()
                                                            .FirstOrDefault(s => s.Name == structName);
                        if (referredStruct != null)
                        {
                            referredStruct.IsServiceResponse = true;
                        }
                    }
                }
            }
        }

        private string extractTripNamespace(Uri tripUri)
        {
            var path = tripUri.AbsolutePath;
            var fileName = path.Split('/').Last();
            Enforce.IsNotNull(fileName, string.Format("No trip namespace found in {0}", tripUri));

            var name = fileName.Split('.').First();
            Enforce.IsNotNull(name, string.Format("No trip namespace found in {0}", tripUri));
            return name;
        }

        private void generateFiles(DocumentContext context)
        {
            LOG.Debug(string.Format("Generating code for {0}...", context.Namespace));

            Enforce.IsNotNull(outputFolder, "The output folder was not set!");
            // TODO, check folder valid

            var codeGenerator = new CodeGenerator(templateLoader, context, generatorConfig, outputFolder);
            codeGenerator.Visit(context.Document);
        }
    }
}
