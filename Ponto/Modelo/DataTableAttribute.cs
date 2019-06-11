using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class DataTableAttribute : DescriptionAttribute
    {
        public DataTableAttribute()
            : base("")
        {

        }

        public DataTableAttribute(string columnName)
            : base(columnName)
        {
        }
    }
}
