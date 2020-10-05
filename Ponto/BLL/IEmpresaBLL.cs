using System.Collections.Generic;

namespace BLL
{
    public interface IEmpresaBLL : IBLL<Modelo.Empresa>
    {
        Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Empresa objeto, bool requisicaoAPI = false);
    }
}
