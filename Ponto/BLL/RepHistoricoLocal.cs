using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class RepHistoricoLocal : IBLL<Modelo.RepHistoricoLocal>
    {
        DAL.IRepHistoricoLocal dalRepHistoricoLocal;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public RepHistoricoLocal() : this(null)
        {
            
        }

        public RepHistoricoLocal(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public RepHistoricoLocal(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalRepHistoricoLocal = new DAL.SQL.RepHistoricoLocal(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalRepHistoricoLocal = null;
                    throw new Exception("Acesso a dados para o Firebird não foi implementado.");
            }
            UsuarioLogado = usuarioLogado;
            dalRepHistoricoLocal.UsuarioLogado = UsuarioLogado;
        }

        public int MaxCodigo(List<Modelo.RepHistoricoLocal> lista)
        {
            int ret = 0;
            foreach (Modelo.RepHistoricoLocal dja in lista)
            {
                if (dja.Codigo > ret)
                {
                    ret = dja.Codigo;
                }
            }

            return (ret + 1);
        }

        public DataTable GetAll()
        {
            return dalRepHistoricoLocal.GetAll();
        }

        public Modelo.RepHistoricoLocal LoadObject(int id)
        {
            return dalRepHistoricoLocal.LoadObject(id);
        }

        public List<Modelo.RepHistoricoLocal> LoadPorRep(int idRep)
        { 
            return dalRepHistoricoLocal.LoadPorRep(idRep);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.RepHistoricoLocal objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            if (objeto.Data == null)
            {
                ret.Add("Data", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Local))
            {
                ret.Add("Historico", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.RepHistoricoLocal objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalRepHistoricoLocal.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalRepHistoricoLocal.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalRepHistoricoLocal.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Dictionary<string, string> Salvar(Modelo.RepHistoricoLocal objeto, List<Modelo.RepHistoricoLocal> lista, Modelo.Acao pAcao)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                objeto.Acao = pAcao;
                if (pAcao == Modelo.Acao.Incluir)
                {                    
                    lista.Add(objeto);
                }
                else
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].Codigo == objeto.Codigo)
                        {
                            if (objeto.Id == 0 && pAcao == Modelo.Acao.Excluir)
                            {
                                lista.RemoveAt(i);
                            }
                            else
                            {
                                lista[i] = objeto;
                                if (objeto.Id == 0)
                                {
                                    lista[i].Acao = Modelo.Acao.Incluir;
                                }
                            }
                        }
                    }
                }
            }
            return erros;
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
            return dalRepHistoricoLocal.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Retorna o Último Histórico do Local do Rep Cadastrado de acordo com a data do histório (maior data)
        /// </summary>
        /// <param name="idRep">Id do rep que deseja o último histórico</param>
        /// <returns></returns>
        public Modelo.RepHistoricoLocal GetUltimoRepHistLocal(int idRep)
        {
            return dalRepHistoricoLocal.GetUltimoRepHistLocal(idRep);
        }

        /// <summary>
        /// Método que Retorna uma lista de Locais por Rep
        /// </summary>
        /// <returns>Retorna uma lista de Locais por Rep</returns>
        public List<pxyRepHistoricoLocalAgrupado> RepHistoricoLocalAgrupado()
        {
            return dalRepHistoricoLocal.RepHistoricoLocalAgrupado();
        }

        public List<Modelo.RepHistoricoLocal> GetAllGrid(int id)
        {
            return dalRepHistoricoLocal.GetAllGrid(id);
        }
    }
}
