
using CTripOSS.Baiji.IDLParser.Model;
using System;
using System.Collections.Generic;
namespace CTripOSS.Baiji.Generator
{
    internal class DocPruner
    {
        private readonly IDictionary<string, int> _visited;
        private readonly IDictionary<string, Definition> _modelCache;
        private readonly IList<Definition> _retainedModels;
        private readonly ISet<string> _includedModels;
        private readonly Queue<string> _visitingQueue;
        private readonly Document _doc;

        internal DocPruner(Document doc)
        {
            _visited = new Dictionary<string, int>();
            _modelCache = new Dictionary<string, Definition>();
            _retainedModels = new List<Definition>();
            _includedModels = new HashSet<string>();
            _visitingQueue = new Queue<string>();
            _doc = doc;
            Cache();
        }

        private void Cache()
        {
            foreach (Definition def in _doc.Definitions)
            {
                _modelCache.Add(def.Name, def);
                _visited.Add(def.Name, 0);
            }
        }

        public void Prune(IList<BaijiMethod> selectedMethod, out ISet<string> includedModels)
        {
            foreach (var m in selectedMethod)
            {
                PrepareByMethod(m);
            }
            PruneModels();
            _doc.Definitions = _retainedModels;
            includedModels = _includedModels;
        }

        public void PruneExcluded(ISet<string> visitingList)
        {
            foreach (string v in visitingList)
            {
                TryEnqueue(v);
            }
            PruneModels();
            _doc.Definitions = _retainedModels;
        }

        protected void PrepareByMethod(BaijiMethod method)
        {
            TryEnqueue(method.ArgumentType.Name);
            TryEnqueue(method.ReturnType.Name);
        }

        protected void PruneModels()
        {
            while (_visitingQueue.Count > 0)
            {
                string modelName = _visitingQueue.Dequeue();
                if (!_modelCache.ContainsKey(modelName))
                    continue;
                var def = _modelCache[modelName];
                if (def == null)
                    Console.WriteLine(modelName);
                if (def.GetType() == typeof(Struct))
                {
                    PruneModel(def as Struct);
                    continue;
                }
                if (def.GetType() == typeof(IntegerEnum))
                {
                    PruneModel(def as IntegerEnum);
                    continue;
                }
            }

        }

        protected void PruneModel(Struct structModel)
        {
            _retainedModels.Add(structModel);
            foreach (BaijiField field in structModel.Fields)
            {
                string fieldTypeName;
                FindDefNameByType(field.Type, out fieldTypeName);
                if (fieldTypeName != null)
                {
                    TryEnqueue(fieldTypeName);
                }
            }
        }

        protected void PruneModel(IntegerEnum enumModel)
        {
            _retainedModels.Add(enumModel);
        }

        private void FindDefNameByType(BaijiType type, out string structName)
        {
            structName = null;
            if (type.GetType() == typeof(BaseType))
                return;
            if (type.GetType() == typeof(ListType))
            {
                FindDefNameByType(((ListType)type).Type, out structName);
                return;
            }
            if (type.GetType() == typeof(MapType))
            {
                FindDefNameByType(((MapType)type).ValueType, out structName);
                return;
            }
            if (type.GetType() == typeof(IdentifierType))
            {
                structName = ((IdentifierType)type).Name;
                return;
            }
        }

        private void TryEnqueue(string modelName)
        {
            if (_visited.ContainsKey(modelName) && _visited[modelName] == 0)
            {
                _visitingQueue.Enqueue(modelName);
                _visited[modelName] = 1;
            }
            else
            {
                _includedModels.Add(modelName);
            }
        }
    }
}