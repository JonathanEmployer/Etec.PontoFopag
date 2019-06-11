using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyEmpresa : ModeloBase
    {
        public bool Selecionado { get; set; }
        public string Nome { get; set; }
        private Int16 _bPrincipal;

        public Int16 bPrincipal
        {
            get { return _bPrincipal; }
            set { _bPrincipal = value; }
        }

        public bool Principal { 
            get
            {
                return Convert.ToBoolean(bPrincipal);
            }
            set
            {
                bPrincipal = Convert.ToInt16(value);
            }
        }

        private string selecionadoStr;
        public string SelecionadoStr
        {
            get
            {
                return this.selecionadoStr;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    this.selecionadoStr = "N";
                }
                else
                {
                    this.selecionadoStr = value;
                }
            }
        }
    }
}
