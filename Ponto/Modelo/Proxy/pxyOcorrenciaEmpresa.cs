using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class pxyOcorrenciaEmpresa
    {
        public string idsSelecionados { get; set; }
        public int idEmpresa { get; set; }
        public IList<Modelo.OcorrenciaEmpresa> lstOcorrencia { get; set; }

        public List<OcorrenciaEmpresa> GerarOcorrencias()
        {
            List<OcorrenciaEmpresa> lstOcorrenciaEmpresa = new List<OcorrenciaEmpresa>();

            try
            {
                List<int> lstIDs = new List<int>();
                if (!String.IsNullOrEmpty(idsSelecionados))
                {
                    lstIDs = idsSelecionados.Split(',').Select(Int32.Parse).ToList();
                }
                
                foreach (var idOcorrencia in lstIDs)
                {
                    lstOcorrenciaEmpresa.Add(new OcorrenciaEmpresa(idOcorrencia, idEmpresa));
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return lstOcorrenciaEmpresa;
        }
    }
}
