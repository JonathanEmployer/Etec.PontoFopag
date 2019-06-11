using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class ChangeLog
    {
        public string ClassName { get; set; }
        public string PropertyName { get; set; }
        public int PrimaryKey { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
