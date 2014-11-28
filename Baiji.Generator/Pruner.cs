using CTripOSS.Baiji.IDLParser.Model;
using System.Collections.Generic;

namespace CTripOSS.Baiji.Generator
{
    public class Pruner
    {
        private readonly IDictionary<string, DocumentContext> _contexts;
        private readonly IDictionary<string, DocPruner> _pruners;
        private readonly IDictionary<string, ISet<string>> _includedModelCache;

        public Pruner(IDictionary<string, DocumentContext> contexts)
        {
            _contexts = contexts;
            _pruners = new Dictionary<string, DocPruner>();
            _includedModelCache = new Dictionary<string, ISet<string>>();
            foreach (DocumentContext dc in _contexts.Values)
            {
                _pruners.Add(dc.Namespace, new DocPruner(dc.Document));
            }
        }

        public void Prune(Service svc, IList<BaijiMethod> selectedMethod)
        {
            foreach (KeyValuePair<string, DocPruner> kv in _pruners)
            {
                ISet<string> includedModels;
                var mp = kv.Value;
                mp.Prune(svc, selectedMethod, out includedModels);
                if (_includedModelCache.ContainsKey(kv.Key))
                {
                    _includedModelCache[kv.Key].UnionWith(includedModels);
                }
                else
                {
                    _includedModelCache.Add(kv.Key, includedModels);
                }
            }
            foreach (KeyValuePair<string, ISet<string>> kv in _includedModelCache)
            {
                _pruners[kv.Key].PruneExcluded(kv.Value);
            }
        }
    }
}
