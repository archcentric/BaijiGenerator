using System;
using System.Collections.Generic;
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;
using CTripOSS.Baiji.JavaGenerator.Properties;
using log4net;

namespace CTripOSS.Baiji.JavaGenerator.Util
{
    public class TemplateLoader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TemplateLoader));

        private readonly ITemplateErrorListener ERROR_LISTENER = new LoaderErrorListener();

        private readonly List<string> templateNames;
        private volatile TemplateGroup tg = null;

        private readonly Dictionary<Type, IAttributeRenderer> attributeRenderers =
            new Dictionary<Type, IAttributeRenderer>();

        public TemplateLoader(List<string> templateNames)
        {
            this.templateNames = templateNames;
        }

        public TemplateLoader(List<string> templateNames, Dictionary<Type, IAttributeRenderer> attributeRenderers)
            : this(templateNames)
        {
            this.attributeRenderers = attributeRenderers;
        }

        public Template Load(string templateName)
        {
            var tg = GetTemplateGroup(templateNames);
            return tg.GetInstanceOf(templateName);
        }

        protected TemplateGroup GetTemplateGroup(List<string> templateNames)
        {
            if (tg == null)
            {
                // combile the header and all .st files and load everything into a TemplateGroup
                tg = new TemplateGroup();
                foreach (var templateName in templateNames)
                {
                    tg.ImportTemplates(GetTemplateGroupFromResource(templateName));
                }
                foreach (var type in attributeRenderers.Keys)
                {
                    var renderer = attributeRenderers[type];
                    tg.RegisterRenderer(type, renderer);
                }
            }

            return tg;
        }

        protected TemplateGroup GetTemplateGroupFromResource(string templateName)
        {
            var tg = new TemplateGroupString(templateName.Replace("_", "."),
                Resources.ResourceManager.GetString(templateName));
            tg.ErrorManager = new ErrorManager(ERROR_LISTENER);
            tg.Load();
            return tg;
        }

        private class LoaderErrorListener : ITemplateErrorListener
        {
            public void CompiletimeError(TemplateMessage msg)
            {
                Log.Error(msg.ToString());
            }

            public void IOError(TemplateMessage msg)
            {
                Log.Error(msg.ToString());
            }

            public void InternalError(TemplateMessage msg)
            {
                Log.Error(msg.ToString());
            }

            public void RuntimeError(TemplateMessage msg)
            {
                Log.Error(msg.ToString());
            }
        }
    }
}