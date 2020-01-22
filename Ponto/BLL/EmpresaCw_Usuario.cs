using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using DAL.SQL;
using Modelo.Proxy;

namespace BLL
{
    public class EmpresaCw_Usuario : IBLL<Modelo.EmpresaCw_Usuario>
    {
        DAL.IEmpresaCw_Usuario dalEmpresaUsuario;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public EmpresaCw_Usuario() : this(null)
        {

        }

        public EmpresaCw_Usuario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EmpresaCw_Usuario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            dalEmpresaUsuario = new DAL.SQL.EmpresaCW_Usuario(new DataBase(ConnectionString));

            UsuarioLogado = usuarioLogado;
            dalEmpresaUsuario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo(List<Modelo.EmpresaCw_Usuario> empresaUsuarios)
        {
            try
            {
                return empresaUsuarios.Max(u => u.Codigo) + 1;
            }
            catch
            {
                return 1;
            }
        }

        public int MaxCodigo()
        {
            try
            {
                return dalEmpresaUsuario.MaxCodigo();
            }
            catch
            {
                return 1;
            }
        }

        public DataTable GetAll()
        {
            return dalEmpresaUsuario.GetAll();
        }

        public DataTable GetPorEmpresa(int idEmpresa)
        {
            return dalEmpresaUsuario.GetPorEmpresa(idEmpresa);
        }

        public Modelo.EmpresaCw_Usuario LoadObject(int id)
        {
            return dalEmpresaUsuario.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.EmpresaCw_Usuario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (objeto.IdEmpresa == 0)
            {
                ret.Add("cbIdEmpresa", "Campo obrigatório.");
            }

            if (objeto.IdCw_Usuario == 0)
            {
                ret.Add("cbIdUsuario", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EmpresaCw_Usuario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEmpresaUsuario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalEmpresaUsuario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEmpresaUsuario.Excluir(objeto);
                        break;
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
            return dalEmpresaUsuario.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.EmpresaCw_Usuario> GetListaPorEmpresa(int idEmpresa)
        {
            return dalEmpresaUsuario.GetListaPorEmpresa(idEmpresa);
        }

        public pxyEmpresaCwUsuario GetListaUsuariosLiberadosBloquadosPorEmpresa(int idEmpresa)
        {
            return dalEmpresaUsuario.GetListaUsuariosLiberadosBloquadosPorEmpresa(idEmpresa);
        }

        public Modelo.Cw_Usuario GetUsuarioPorCodigo(int codigo)
        {
            return dalEmpresaUsuario.GetUsuarioPorCodigo(codigo);
        }

        public bool CWUtilizaControleContratos()
        {
            return dalEmpresaUsuario.CWUtilizaControleContratos();
        }
    }
}
