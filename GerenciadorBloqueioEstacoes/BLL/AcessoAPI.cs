using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AcessoAPI
    {

        public Modelo.Acesso.AcessoAPI CarregarDadosAcesso()
        {
            try
            {
                Modelo.Utils.Utils.EscreveLog("Log", "Carregando dados de acesso a API");
                DAL.AcessoAPI dal = new DAL.AcessoAPI();
                string config = ConfigurationManager.AppSettings["acessoAPI"];
                int idAcesso;
                if (!string.IsNullOrEmpty(config) && int.TryParse(config, out idAcesso))
                {
                    Modelo.Utils.Utils.EscreveLog("Log", "Carregando configuração por ID = " + idAcesso);
                    return dal.Carregar(idAcesso);
                }
                Modelo.Utils.Utils.EscreveLog("Log", "Carregando primera config");
                return dal.Carregar();
            }
            catch (Exception e)
            {
                Modelo.Utils.Utils.EscreveLog("Log", "Erro ao carregar configurações da API, erro = "+e.Message);
                throw e;
            }
        }

        public void AtualizarToken(Modelo.Acesso.AcessoAPI acesso)
        {
            DAL.AcessoAPI dal = new DAL.AcessoAPI();
            dal.AtualizarToken(acesso);
        }

    }
}
