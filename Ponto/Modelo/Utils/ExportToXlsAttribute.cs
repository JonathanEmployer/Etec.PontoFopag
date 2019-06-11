using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modelo.Utils
{
    public class ExportToXlsAttribute : DescriptionAttribute
    {
        public ExportToXlsAttribute()
            : base("")
        {

        }


        public int Index { get; set; }
        public ExportToXlsAttribute(string description, int index)
            : base(description)
        {
            Index = index;
        }
    }

    public class ItemsForExport
    {
        public string PropertyName { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
    }
}
