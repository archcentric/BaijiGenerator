using System.Collections.Generic;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator.Context
{
    public class StructContext : JavaContext
    {
        public List<FieldContext> Fields
        {
            get;
            private set;
        }

        public string JavaPackage
        {
            get;
            private set;
        }

        public string[] DocStringLines
        {
            get;
            private set;
        }

        public string JavaName
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
            private set;
        }

        private int level = 0;

        public StructContext(string[] docStringLines, string name, string javaPackage, string javaName,
            bool isServiceResponse, int level)
        {
            DocStringLines = docStringLines;
            Name = name;
            JavaPackage = javaPackage;
            JavaName = javaName;
            Fields = new List<FieldContext>();
            IsServiceResponse = isServiceResponse;
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

        public string BuildSchemaField()
        {
            var schemaText = "";
            return string.Format("public static final Schema SCHEMA = Schema.parse(\"{0}\");",
                    schemaText.Replace("\"", "\\\""));
        }

        private string BuildStructGetter()
        {
            var sb = new StringBuilder();
            sb.Indent(level).AppendLine("// Used by DatumWriter. Applications should not call.");
            sb.Indent(level).AppendLine("public java.lang.Object get(int field) {");
            IndentUp();

            sb.Indent(level).AppendLine("switch (field)");
            sb.Indent(level).AppendLine("{");
            IndentUp();
            for (int i = 0; i < Fields.Count; ++i)
            {
                sb.Indent(level)
                    .AppendFormat("case {0}: return this.{1};", i, Fields[i].JavaName)
                    .AppendLine();
            }
            sb.Indent(level)
                .AppendLine(
                    "default: throw new BaijiRuntimeException(\"Bad index \" + field + \" in get()\");");
            IndentDown();
            sb.Indent(level).AppendLine("}");

            IndentDown();
            sb.Indent(level).AppendLine("}");
            return sb.ToString();
        }

        private string BuildStructPutter()
        {
            var sb = new StringBuilder();
            sb.Indent(level).AppendLine("// Used by DatumReader. Applications should not call.");
            sb.Indent(level).AppendLine("@SuppressWarnings(value=\"unchecked\")");
            sb.Indent(level).AppendLine("public void put(int field, java.lang.Object value) {");
            IndentUp();

            sb.Indent(level).AppendLine("switch (field) {");
            IndentUp();
            for (int i = 0; i < Fields.Count; ++i)
            {
                sb.Indent(level)
                    .AppendFormat("case {0}: this.{1} = ({2})value; break;", i, Fields[i].JavaName,
                        Fields[i].GenType.JavaTypeName)
                    .AppendLine();
            }
            sb.Indent(level)
                .AppendLine("default: throw new BaijiRuntimeException(\"Bad index \" + field + \" in put()\");");
            IndentDown();
            sb.Indent(level).AppendLine("}");

            IndentDown();
            sb.Indent(level).AppendLine("}");
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