using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class LocalizacaoRegistroPonto : IBLL<Modelo.LocalizacaoRegistroPonto>
    {
        DAL.ILocalizacaoRegistroPonto dalLocalizacaoRegistroPonto;
        private string ConnectionString;

        public LocalizacaoRegistroPonto() : this(null)
        {
            
        }

        public LocalizacaoRegistroPonto(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LocalizacaoRegistroPonto(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalLocalizacaoRegistroPonto = new DAL.SQL.LocalizacaoRegistroPonto(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalLocalizacaoRegistroPonto = new DAL.SQL.LocalizacaoRegistroPonto(new DataBase(ConnectionString));
                    break;
            }
            dalLocalizacaoRegistroPonto.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLocalizacaoRegistroPonto.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLocalizacaoRegistroPonto.GetAll();
        }

        public Modelo.LocalizacaoRegistroPonto LoadObject(int id)
        {
            return dalLocalizacaoRegistroPonto.LoadObject(id);
        }

        public List<Modelo.LocalizacaoRegistroPonto> GetAllList()
        {
            return dalLocalizacaoRegistroPonto.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LocalizacaoRegistroPonto objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LocalizacaoRegistroPonto objeto)
        {
            if (Modelo.Acao.Incluir == pAcao && objeto.Codigo == 0)
            {
                objeto.Codigo = MaxCodigo();
            }
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLocalizacaoRegistroPonto.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLocalizacaoRegistroPonto.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLocalizacaoRegistroPonto.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public void AtualizarRegistros(List<Modelo.LocalizacaoRegistroPonto> regs)
        {
            if (regs.Count > 0)
            {
                dalLocalizacaoRegistroPonto.AtualizarRegistros(regs); 
            }
        }

        public void InserirRegistros(List<Modelo.LocalizacaoRegistroPonto> regs)
        {
            if (regs.Count > 0)
            {
                dalLocalizacaoRegistroPonto.InserirRegistros(regs);
            }
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalLocalizacaoRegistroPonto.getId(pValor, pCampo, pValor2);
        }

        public Modelo.LocalizacaoRegistroPonto GetPorBilhete(int id)
        {
            return dalLocalizacaoRegistroPonto.GetPorBilhete(id);
        }

        public List<Modelo.Proxy.Relatorios.PxyRelLocalizacaoRegistroPonto> RelLocalizacaoRegistroPonto(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal)
        {
            return dalLocalizacaoRegistroPonto.RelLocalizacaoRegistroPonto(idsFuncionarios, datainicial, datafinal);
        }
    }
}
