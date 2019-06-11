using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsComparable : Attribute
    {
        private bool Comparavel;


        public IsComparable(bool Comparavel)
        {
            this.Comparavel = Comparavel;
        }

        public bool COMPARAVEL
        {
            get { return Comparavel; }
            set { Comparavel = value; }
        }
    }
}
