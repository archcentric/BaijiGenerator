using System.Collections.Generic;
using System.Text;

namespace CTripOSS.Baiji.CSharpGenerator.Context
{
    public class StructContext : CSharpContext
    {
        public List<FieldContext> Fields
        {
            get;
            private set;
        }

        public string CSharpNamespace
        {
            get;
            private set;
        }

        public string CSharpName
        {
            get;
            private set;
        }

        public string[] DocStringLines
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool IsServiceResponse
        {
            get;
            set;
        }

        private int level;

        public StructContext(string[] docStringLines, string name, string csharpNamespace, string csharpName,
            bool isServiceResponse, int level)
        {
            DocStringLines = docStringLines;
            Name = name;
            CSharpNamespace = csharpNamespace;
            CSharpName = csharpName;
            IsServiceResponse = isServiceResponse;
            Fields = new List<FieldContext>();
            this.level = level;
        }

        public void AddField(FieldContext field)
        {
            Fields.Add(field);
        }

        private void IndentUp()
        {
            level++;
        }

        private void IndentDown()
        {
            level--;
        }

        private void ScopeUp(StringBuilder sb)
        {
            sb.Indent(level).AppendLine("{");
            level++;
        }

        private void ScopeDown(StringBuilder sb)
        {
            level--;
            sb.Indent(level).AppendLine("}");
        }

        public string SchemaField
        {
            get
            {
                return BuildSchemaField();
            }
        }

        public string GetMethod
        {
            get
            {
                return BuildStructGetter();
            }
        }

        public string PutMethod
        {
            get
            {
                return BuildStructPutter();
            }
        }

        public string ToStringMethod
        {
            get
            {
                return BuildStructToString();
            }
        }

        public string EqualsMethod
        {
            get
            {
                return BuildStructEquals();
            }
        }

        public string HashcodeMethod
        {
            get
            {
                return BuildStructHashcode();
            }
        }

        public string BuildSchemaField()
        {
            var schemaText = "";
            return string.Format(
                    "public static readonly Baiji.Schema.Schema SCHEMA = Baiji.Schema.Schema.Parse(@\"{0}\");",
                    schemaText.Replace("\"", "\"\""));
        }

        private string BuildStructGetter()
        {
            var sb = new StringBuilder();
            sb.Indent(level).AppendLine("public virtual object Get(int fieldPos)");
            sb.Indent(level).AppendLine("{");
            IndentUp();

            sb.Indent(level).AppendLine("switch (fieldPos)");
            sb.Indent(level).AppendLine("{");
            IndentUp();
            for (int i = 0; i < Fields.Count; ++i)
            {
                sb.Indent(level)
                    .AppendFormat("case {0}: return this.{1};", i, Fields[i].CSharpPropertyName)
                    .AppendLine();
            }
            sb.Indent(level)
                .AppendLine("default: throw new BaijiRuntimeException(\"Bad index \" + fieldPos + \" in Get()\");");
            IndentDown();
            sb.Indent(level).AppendLine("}");

            IndentDown();
            sb.Indent(level).AppendLine("}");
            return sb.ToString();
        }

        private string BuildStructPutter()
        {
            var sb = new StringBuilder();
            sb.Indent(level).AppendLine("public virtual void Put(int fieldPos, object fieldValue)");
            sb.Indent(level).AppendLine("{");
            IndentUp();

            sb.Indent(level).AppendLine("switch (fieldPos)");
            sb.Indent(level).AppendLine("{");
            IndentUp();
            for (int i = 0; i < Fields.Count; ++i)
            {
                sb.Indent(level)
                    .AppendFormat("case {0}: return this.{1} = ({2})fieldValue; break;", i, Fields[i].CSharpPropertyName,
                        Fields[i].GenType.CSharpTypeName)
                    .AppendLine();
            }
            sb.Indent(level)
                .AppendLine("default: throw new BaijiRuntimeException(\"Bad index \" + fieldPos + \" in Put()\");");
            IndentDown();
            sb.Indent(level).AppendLine("}");

            IndentDown();
            sb.Indent(level).AppendLine("}");
            return sb.ToString();
        }

        private string BuildStructToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Indent(level).Append("public override string ToString() {").AppendLine();
            IndentUp();

            sb.Indent(level)
                .Append("StringBuilder __sb = new StringBuilder(\"")
                .Append(Name)
                .Append("(\");")
                .AppendLine();

            Fields.Sort();

            sb.Indent(level).Append("bool __first = true;").AppendLine();

            foreach (var field in Fields)
            {
                sb.Indent(level).Append("if (").Append(field.CSharpPropertyName).Append(" != null) {").AppendLine();
                IndentUp();
                sb.Indent(level).Append("if(!__first) { __sb.Append(\", \"); }").AppendLine();
                sb.Indent(level).Append("__first = false;").AppendLine();
                sb.Indent(level).Append("__sb.Append(\"").Append(field.CSharpPropertyName).Append(": \");").AppendLine();

                if (field.GenType.IsStruct)
                {
                    sb.Indent(level)
                        .Append("__sb.Append(")
                        .Append(field.CSharpPropertyName)
                        .Append("== null ? \"<null>\" : ")
                        .Append(field.CSharpPropertyName).Append(".ToString());").AppendLine();
                }
                else
                {
                    sb.Indent(level).Append("__sb.Append(").Append(field.CSharpPropertyName).Append(");").AppendLine();
                }

                IndentDown();
                sb.Indent(level).Append("}").AppendLine();
            }

            sb.Indent(level).Append("__sb.Append(\")\");").AppendLine();
            sb.Indent(level).Append("return __sb.ToString();").AppendLine();

            IndentDown();
            sb.Indent(level).Append("}").AppendLine().AppendLine();

            return sb.ToString();
        }

        private string BuildStructEquals()
        {
            StringBuilder sb = new StringBuilder();

            sb.Indent(level).Append("public override bool Equals(object that) {").AppendLine();
            IndentUp();

            sb.Indent(level).Append("var other = that as ").Append(CSharpName).Append(";").AppendLine();
            sb.Indent(level).Append("if (other == null) return false;").AppendLine();
            sb.Indent(level).Append("if (ReferenceEquals(this, other)) return true;").AppendLine();

            Fields.Sort();

            bool first = true;

            foreach (var field in Fields)
            {
                if (first)
                {
                    first = false;
                    sb.Indent(level).Append("return ");
                    IndentUp();
                }
                else
                {
                    sb.AppendLine();
                    sb.Indent(level).Append("&& ");
                }

                if (field.GenType.IsContainer)
                {
                    sb.Append("TCollections.Equals(");
                    sb.Append(field.CSharpPropertyName).Append(", other.").Append(field.CSharpPropertyName).Append(")");
                }
                else if (field.GenType.IsBinary)
                {
                    sb.Append("(")
                        .Append(field.CSharpPropertyName)
                        .Append(" == null ? other.")
                        .Append(field.CSharpPropertyName)
                        .
                        Append(" == null : ").Append(field.CSharpPropertyName).Append(".SequenceEqual(other.").
                        Append(field.CSharpPropertyName).Append("))");
                }
                else if (field.GenType.IsDouble)
                {
                    sb.Append("(")
                        .Append(field.CSharpPropertyName)
                        .Append(" != null && other.")
                        .Append(field.CSharpPropertyName)
                        .Append(" != null ? (System.Math.Abs(").Append(field.CSharpPropertyName)
                        .Append(".Value - other.").Append(field.CSharpPropertyName).Append(".Value) < 1E-15)")
                        .Append(" : System.Object.Equals(")
                        .Append(field.CSharpPropertyName)
                        .Append(", other.")
                        .Append(field.CSharpPropertyName)
                        .Append("))");
                }
                else
                {
                    sb.Append("System.Object.Equals(");
                    sb.Append(field.CSharpPropertyName).Append(", other.").Append(field.CSharpPropertyName).Append(")");
                }
            }
            if (first)
            {
                sb.Indent(level).Append("return true;").AppendLine();
            }
            else
            {
                sb.AppendLine(";");
                IndentDown();
            }

            IndentDown();
            sb.Indent(level).Append("}").AppendLine().AppendLine();

            return sb.ToString();
        }

        private string BuildStructHashcode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Indent(level).Append("public override int GetHashCode() {").AppendLine();
            IndentUp();

            sb.Indent(level).Append("int hashcode = 0;").AppendLine();
            sb.Indent(level).Append("unchecked {").AppendLine();
            IndentUp();

            Fields.Sort();

            int tempFieldIndex = 0;
            foreach (var field in Fields)
            {
                if (field.GenType.IsContainer)
                {
                    sb.Indent(level).Append("hashcode = (hashcode * 397) ^ ");
                    sb.Append("(").Append(field.CSharpPropertyName).Append(" == null ? 0 : ");
                    sb.Append("(TCollections.GetHashCode(").Append(field.CSharpPropertyName).Append("))");
                    sb.Append(");").AppendLine();
                }
                else if (field.GenType.IsBinary)
                {
                    sb.Indent(level).Append("if (").Append(field.CSharpPropertyName).AppendLine(" != null)");
                    ScopeUp(sb);
                    string elem = "_byte" + (tempFieldIndex++);
                    sb.Indent(level)
                        .Append("foreach (byte ")
                        .Append(elem)
                        .Append(" in ")
                        .Append(field.CSharpPropertyName)
                        .AppendLine(")");
                    IndentUp();
                    sb.Indent(level).Append("hashcode = (hashcode * 397) ^").Append(elem).AppendLine(";");
                    IndentDown();
                    ScopeDown(sb);
                }
                else
                {
                    sb.Indent(level).Append("hashcode = (hashcode * 397) ^ ");
                    sb.Append("(").Append(field.CSharpPropertyName).Append(" == null ? 0 : ");
                    sb.Append("(").Append(field.CSharpPropertyName).Append(".GetHashCode())");
                    sb.Append(");").AppendLine();
                }
            }

            IndentDown();
            sb.Indent(level).Append("}").AppendLine();
            sb.Indent(level).Append("return hashcode;").AppendLine();

            IndentDown();
            sb.Indent(level).Append("}").AppendLine().AppendLine();

            return sb.ToString();
        }
    }

    public static class StringBuilderExtension
    {
        public static StringBuilder Indent(this StringBuilder sb, int level)
        {
            for (int i = 0; i < level; i++)
            {
                sb.Append("    ");
            }
            return sb;
        }
    }
}