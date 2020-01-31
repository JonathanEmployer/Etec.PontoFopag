using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;

namespace BLL
{
    public class LoteCalculo : IBLL<Modelo.LoteCalculo>
    {
        
        public DataTable GetAll()
        {
            throw new NotImplementedException();
        }

        public Modelo.LoteCalculo LoadObject(int id)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LoteCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> Salvar(Acao pAcao, Modelo.LoteCalculo objeto)
        {
            throw new NotImplementedException();
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            throw new NotImplementedException();
        }
    }
}
