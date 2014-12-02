using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CTripOSS.Baiji.Generator.Util;
using CTripOSS.Baiji.Generator.Visitor;
using CTripOSS.Baiji.Helper;
using CTripOSS.Baiji.IDLParser.Model;
using CTripOSS.Baiji.IDLParser.Visitor;
using log4net;

namespace CTripOSS.Baiji.Generator
{
    /// <summary>
    /// Parses an IDL file and writes out code files.
    /// </summary>
    public abstract class Generator
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(Generator));

        protected readonly DirectoryInfo _outputFolder;
        protected readonly GeneratorConfig _generatorConfig;
        protected readonly TemplateLoader _templateLoader;
        protected readonly ISet<Uri> _parsedDocuments = new HashSet<Uri>();
        protected readonly Stack<Uri> _parentDocuments = new Stack<Uri>();

        protected Generator(GeneratorConfig generatorConfig, IDictionary<string, IList<string>> templates)
        {
            if (!templates.ContainsKey(generatorConfig.CodeFlavor))
            {
                throw new ArgumentException(string.Format("Templating type {0} is unknown!", generatorConfig.CodeFlavor));
            }
            _generatorConfig = generatorConfig;

            _outputFolder = generatorConfig.OutputFolder;
            if (_outputFolder != null && !_outputFolder.Exists)
            {
                _outputFolder.Create();
            }

            LOG.Debug(string.Format("Writing source files into {0} using {1} ...", _outputFolder,
                generatorConfig.CodeFlavor));

            _templateLoader = new TemplateLoader(templates[generatorConfig.CodeFlavor]);
        }

        public IDictionary<string, DocumentContext> GetContexts(IList<Uri> inputs)
        {
            if (inputs == null || inputs.Count == 0)
            {
                throw new ArgumentException("No input files");
            }

            LOG.Info(string.Format("Parsing IDL from {0}...", inputs));

            var contexts = new Dictionary<string, DocumentContext>();
            foreach (var inputUri in inputs)
            {
                _parsedDocuments.Clear();
                Uri input;
                if (!inputUri.IsAbsoluteUri)
                {
                    Uri.TryCreate(_generatorConfig.InputBase, inputUri, out input);
                }
                else
                {
                    input = inputUri;
                }

                ParseDocument(input, contexts, new TypeRegistry());
            }
            return contexts;
        }

        public void Parse(IDictionary<string, DocumentContext> contexts)
        {
            MarkServiceResponseTypes(contexts);

            LOG.Info("IDL parsing complete, writing code files...");

            foreach (var context in contexts.Values)
            {
                GenerateFiles(context);
            }

            LOG.Info("Code generation complete.");
        }

        public void Parse(IList<Uri> inputs)
        {
            var contexts = GetContexts(inputs);
            var service = ContextUtils.ExtractService(contexts.Values.ToList());

            IList<BaijiMethod> selectedMethod = new List<BaijiMethod>();
            selectedMethod.Add(service.Methods[0]);
            selectedMethod.Add(service.Methods[1]);
            Pruner pruner = new Pruner(contexts);
            pruner.Prune(selectedMethod);

            MarkServiceResponseTypes(contexts);

            LOG.Info("IDL parsing complete, writing code files...");

            foreach (var context in contexts.Values)
            {
                GenerateFiles(context);
            }

            LOG.Info("Code generation complete.");
        }

        public void Parse(IDictionary<string, DocumentContext> contexts, IList<BaijiMethod> selectedMethod)
        {
            Pruner pruner = new Pruner(contexts);
            pruner.Prune(selectedMethod);
            Parse(contexts);
        }

        private void ParseDocument(Uri uri,
            IDictionary<string, DocumentContext> contexts,
            TypeRegistry typeRegistry)
        {
            if (uri == null || !uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Only absolute URIs can be parsed!");
            }
            if (_parentDocuments.Contains(uri))
            {
                throw new ArgumentException(string.Format("Input {0} recursively includes itself ({1})", uri,
                    string.Join(" -> ", _parentDocuments) + " -> " + uri));
            }

            if (_parsedDocuments.Contains(uri))
            {
                LOG.Debug(string.Format("Skipping already parsed file {0}...", uri));
                return;
            }

            LOG.Debug(string.Format("Parsing {0}...", uri));

            var idlNamespace = ExtractNamespace(uri);
            if (string.IsNullOrWhiteSpace(idlNamespace))
            {
                throw new ArgumentException(string.Format("URI {0} can not be translated to a namespace", uri));
            }

            var context = CreateDocumentContext(uri, idlNamespace, _generatorConfig, typeRegistry);

            var document = context.Document;
            var header = document.Header;

            var codeNamespace = context.CodeNamespace;

            // Make a note that this document is a parent of all the documents included, directly or recursively
            _parentDocuments.Push(uri);

            try
            {
                if (header != null)
                {
                    foreach (var include in header.Includes)
                    {
                        Uri includeUri;
                        Uri.TryCreate(_generatorConfig.InputBase, include, out includeUri);
                        ParseDocument(includeUri,
                            // If the includes should also generate code, pass the list of
                            // contexts down to the include parser, otherwise pass a null in
                            _generatorConfig.GenerateIncludedCode ? contexts : null,
                            typeRegistry);
                    }
                }
            }
            finally
            {
                // Done parsing this document's includes, remove it from the parent chain
                _parentDocuments.Pop();
            }

            // Make a note that we've already passed this document
            _parsedDocuments.Add(uri);

            CreateTypeVisitor(codeNamespace, context).Visit(document);

            if (contexts != null)
            {
                if (contexts.ContainsKey(context.Namespace))
                {
                    LOG.Info(string.Format("Namespace {0} included multiple times!", context.Namespace));
                }
                contexts[context.Namespace] = context;
            }
        }

        private void MarkServiceResponseTypes(IDictionary<string, DocumentContext> contexts)
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

        private string ExtractNamespace(Uri uri)
        {
            var path = uri.AbsolutePath;
            var name = Path.GetFileNameWithoutExtension(path);
            Enforce.IsNotNull(name, string.Format("No namespace found in {0}", uri));
            return name.Replace(".", "_");
        }

        private void GenerateFiles(DocumentContext context)
        {
            LOG.Debug(string.Format("Generating code for {0}...", context.Namespace));

            Enforce.IsNotNull(_outputFolder, "The output folder was not set!");
            // TODO, check folder valid

            var codeGenerator = CreateCodeGenerator(context);
            codeGenerator.Visit(context.Document);
        }

        protected abstract IVisitor CreateCodeGenerator(DocumentContext context);

        protected abstract DocumentContext CreateDocumentContext(Uri uri, string idlNamespace, GeneratorConfig config,
            TypeRegistry typeRegistry);

        public virtual TypeVisitor CreateTypeVisitor(string codeNamespace, DocumentContext documentContext)
        {
            return new TypeVisitor(codeNamespace, documentContext);
        }
    }
}