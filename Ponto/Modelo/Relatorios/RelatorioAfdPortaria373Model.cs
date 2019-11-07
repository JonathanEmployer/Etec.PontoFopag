using System.Collections.Generic;

namespace Modelo.Relatorios
{
    public class RelatorioAfdPortaria373Model : RelatorioBaseModel, IRelatorioModel
    {
        public Dictionary<int, string> lIdEmpAndNumRep { 
            get 
            {
                Dictionary<int, string> retorno = new Dictionary<int, string>();
                if (this.IdSelecionados != null)
                {
                    foreach (string sel in this.IdSelecionados.Split(','))
                    {
                        string[] parts = sel.Split('|');
                        int.TryParse(parts[0], out int idEmpresa);
                        retorno.Add(idEmpresa, parts[1]);
                    } 
                }
                return retorno;
            } 
        }
    }
}
