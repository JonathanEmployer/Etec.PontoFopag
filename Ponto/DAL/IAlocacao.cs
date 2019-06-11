using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{   
        public interface IAlocacao : DAL.IDAL
        {
            Modelo.Alocacao LoadObject(int id);
            bool BuscaAlocacao(string pNomeDescricao);
            int? getAlocacaoNome(string pNomeDescricao);
            int? GetIdPorCod(int Cod);
            List<Modelo.Alocacao> GetAllList();
            Modelo.Alocacao LoadObjectByCodigo(int idAlocacao);
        }
}


