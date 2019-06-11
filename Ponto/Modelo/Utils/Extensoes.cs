using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Modelo.Utils
{
    public static class Extensoes
    {
        // Retorna ums string relativa a hora vazia considerando casas decimais
        public static String Hora2CasasEmpty(this string str)
        {
            return "--:--";
        }

        // Retorna ums string relativa a hora vazia considerando casas decimais
        public static String Hora3CasasEmpty(this string str)
        {
            return "---:--";
        }
    }
}
