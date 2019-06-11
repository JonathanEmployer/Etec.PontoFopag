using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modelo;
using Modelo.Proxy.Relatorios;
using System.Collections;

namespace BLL
{
    public class Inconsistencia : IBLL<Modelo.Inconsistencia>
    {
        DAL.IInconsistencia dalInconsistencia;
        
        private Modelo.ProgressBar objProgressBar;
        private string ConnectionString;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        #region Métodos básicos

        #region construtores
        public Inconsistencia()
            : this(null)
        {

        }

        public Inconsistencia(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Inconsistencia(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalInconsistencia = new DAL.SQL.Inconsistencia(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalInconsistencia = new DAL.SQL.Inconsistencia(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalInconsistencia = DAL.FB.Inconsistencia.GetInstancia;
                    break;
            }
            dalInconsistencia.UsuarioLogado = usuarioLogado;
        }

        #endregion

        public int MaxCodigo()
        {
            return dalInconsistencia.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalInconsistencia.GetAll();
        }

        public DataTable GetAllComOpcaoTodos()
        {
            DataTable dt = dalInconsistencia.GetAll();
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { 0, "Todas as Inconsistencias", 0, false };
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        public Modelo.Inconsistencia LoadObject(int id)
        {
            return dalInconsistencia.LoadObject(id);
        }

        public Hashtable GetHashIdDescricao()
        {
            return dalInconsistencia.GetHashIdDescricao();
        }

        public List<Modelo.Inconsistencia> GetAllList()
        {
            return dalInconsistencia.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Inconsistencia objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Inconsistencia objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalInconsistencia.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalInconsistencia.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalInconsistencia.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalInconsistencia.getId(pValor, pCampo, pValor2);
        }

        public Modelo.Inconsistencia LoadObjectByCodigo(int pCodigo)
        {
            return dalInconsistencia.LoadObjectByCodigo(pCodigo);
        }

        #endregion

        /// <summary>
        /// Retorna os dados para a geração do Relatório de Inconsistências
        /// </summary>
        /// <param name="cpfs">Lista com cpfs dos funcionários</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <returns>Retorna lista para geração de relatório</returns>
        public List<Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias> RelatorioInconsistencias(List<int> idsFuncs, string datainicial, string datafinal, List<bool> paramInconsistencia)
        {
            return dalInconsistencia.RelatorioInconsistencias(idsFuncs, datainicial, datafinal, paramInconsistencia);
        }
    }
}
