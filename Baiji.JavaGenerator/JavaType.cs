using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.JavaGenerator
{
    public class JavaType
    {
        private readonly string tripNamespace;
        private readonly string name;
        private readonly string javaPackageName;
        public bool IsLeanStruct { get; set; }
        public bool IsLeanEnum { get; set; }

        public JavaType(string tripNamespace,
            string name, string javaPackageName)
        {
            this.tripNamespace = tripNamespace;
            this.name = name;
            this.javaPackageName = javaPackageName;
            IsLeanStruct = false;
            IsLeanEnum = false;
        }

        public string Package
        {
            get { return this.javaPackageName; }
        }

        public string SimpleName
        {
            get { return this.name; }
        }

        public string ClassName
        {
            get { return this.javaPackageName + "." + name; }
        }

        public string Key
        {
            get { return tripNamespace + "." + name; }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (JavaType)obj;
            if (javaPackageName == null)
            {
                if (other.javaPackageName != null)
                    return false;
            }
            else if (javaPackageName != other.javaPackageName)
            {
                return false;
            }

            if (name == null)
            {
                if (other.name != null)
                    return false;
            }
            else if (name != other.name)
            {
                return false;
            }

            if (tripNamespace == null)
            {
                if (other.tripNamespace != null)
                    return false;
            }
            else if (tripNamespace != other.tripNamespace)
            {
                return false;
            }

            if (IsLeanStruct != other.IsLeanStruct) return false;
            if (IsLeanEnum != other.IsLeanEnum) return false;

            return true;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((javaPackageName == null) ? 0 : javaPackageName.GetHashCode());
            result = prime * result + ((name == null) ? 0 : name.GetHashCode());
            result = prime * result + ((tripNamespace == null) ? 0 : tripNamespace.GetHashCode());
            return result;
        }

        public override string ToString()
        {
            return "[trip namespace = " + tripNamespace + ", name = " + name + ", java package = " + javaPackageName + "]";
        }
    }
}
