using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTripOSS.Baiji.CSharpGenerator
{
    public class CSharpType
    {
        private readonly string tripNamespace;
        private readonly string name;
        private readonly string csharpNamespace;
        public bool IsTripStruct { get; set; }
        public bool IsTripEnum { get; set; }

        public CSharpType(string tripNamespace,
            string name, string csharpNamespace)
        {
            this.tripNamespace = tripNamespace;
            this.name = name;
            this.csharpNamespace = csharpNamespace;
            IsTripStruct = false;
            IsTripEnum = false;
        }

        public string TypeNamespace
        {
            get { return this.csharpNamespace; }
        }

        public string TypeName
        {
            get { return this.name; }
        }

        public string TypeFullName
        {
            get { return this.csharpNamespace + "." + name; }
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

            var other = (CSharpType)obj;
            if (csharpNamespace == null)
            {
                if (other.csharpNamespace != null)
                    return false;
            }
            else if (csharpNamespace != other.csharpNamespace)
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

            if (IsTripStruct != other.IsTripStruct) return false;
            if (IsTripEnum != other.IsTripEnum) return false;

            return true;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((csharpNamespace == null) ? 0 : csharpNamespace.GetHashCode());
            result = prime * result + ((name == null) ? 0 : name.GetHashCode());
            result = prime * result + ((tripNamespace == null) ? 0 : tripNamespace.GetHashCode());
            return result;
        }

        public override string ToString()
        {
            return "[trip namespace = " + tripNamespace + ", name = " + name + ", csharp namespace = " + csharpNamespace + "]";
        }
    }
}
