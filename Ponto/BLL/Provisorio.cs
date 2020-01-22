using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class Provisorio : IBLL<Modelo.Provisorio>
    {
        DAL.IProvisorio dalProvisorio;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Provisorio() : this(null)
        {
            
        }

        public Provisorio(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Provisorio(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalProvisorio = new DAL.SQL.Provisorio(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalProvisorio.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalProvisorio.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalProvisorio.GetAll();
        }

        public bool VerificaBilhete(string pDSCodigo, DateTime pDatai, DateTime pDataf, out DateTime? ultimaData)
        {
            return dalProvisorio.VerificaBilhete(pDSCodigo, pDatai, pDataf, out ultimaData);
        }

        public List<Modelo.Provisorio> GetAllList()
        {
            return dalProvisorio.GetAllList();
        }

        public Modelo.Provisorio LoadObject(int id)
        {
            return dalProvisorio.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Provisorio objeto)
        {
            Funcionario bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
            Dictionary<string, string> ret = new Dictionary<string, string>();
            
            if (objeto.Dt_inicial == null)
            {
                ret.Add("txtDt_inicial", "Campo obrigatório.");
            }
            if (objeto.Dt_final == null)
            {
                ret.Add("txtDt_final", "Campo obrigatório.");
            }
            if (objeto.Dt_inicial != null && objeto.Dt_final != null)
            {
                if (objeto.Dt_final < objeto.Dt_inicial)
                {
                    ret.Add("txtDt_final", "A data final deve ser maior ou igual a data inicial.");
                }
            }
            if (objeto.Dsfuncionario == "" || objeto.Dsfuncionario == null)
            {
                ret.Add("cbIdDsfuncionario", "Campo obrigatório.");
            }
            if (objeto.Dsfuncionarionovo == "" || objeto.Dsfuncionarionovo == null)
            {
                ret.Add("txtDsfuncionarionovo", "Campo obrigatório.");
            }
            else
            {
                int aux = 0;
                if (Int32.TryParse(objeto.Dsfuncionarionovo, out aux))
                {
                    if (aux <= 0)
                    {
                        ret.Add("txtDsfuncionarionovo", "O código deve ser maior do que zero(0).");
                    }
                }
                else
                {
                    ret.Add("txtDsfuncionarionovo", "Somente números.");
                }
                string mensagem;
                if (bllFuncionario.DsCodigoUtilizado(objeto.Dsfuncionarionovo, out mensagem))
                {
                    ret.Add("txtDsfuncionarionovo", mensagem);
                }
                else if (objeto.Dt_inicial != null && objeto.Dt_final != null)
                {
                    if (this.ExisteProvisorio(objeto.Dsfuncionarionovo, objeto.Dt_inicial.Value, objeto.Dt_final.Value, objeto.Id))
                    {
                        ret.Add("txtDsfuncionarionovo", "O código " + objeto.Dsfuncionarionovo + " já está sendo utilizado dentro desse período.");
                    }
                }
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Provisorio objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalProvisorio.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalProvisorio.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalProvisorio.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public bool PossuiRegistro(DateTime pData, string pCodigo, out string pCodigoNovo)
        {
            bool ret = false;
            pCodigoNovo = "";

            foreach (Modelo.Provisorio p in dalProvisorio.getLista(pCodigo, pData))
            {
                pCodigoNovo = p.Dsfuncionarionovo;
                ret = true;
                break;
            }

            return ret;
        }

        public bool PossuiRegistro(List<Modelo.Provisorio> pProvisorioLista, DateTime pData, string pCodigo, out string pCodigoNovo)
        {
            pCodigoNovo = "";

            var prov = pProvisorioLista.Where(pv => pv.Dsfuncionario == pCodigo && pData >= pv.Dt_inicial && pData <= pv.Dt_final);

            if (prov.Count() == 0)
            {
                return false;
            }

            foreach (Modelo.Provisorio p in prov)
            {
                pCodigoNovo = p.Dsfuncionarionovo;
                return true;
            }

            return false;
        }

        public bool ExisteProvisorio(string pCodigo, DateTime pDataI, DateTime pDataF, int pIdProvisorio)
        {
            return dalProvisorio.ExisteProvisorio(pCodigo, pDataI, pDataF, pIdProvisorio);
        }

        public bool ExisteProvisorio(string pCodigo, DateTime pData)
        {
            return dalProvisorio.ExisteProvisorio(pCodigo, pData);
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
            return dalProvisorio.getId(pValor, pCampo, pValor2);
        }
    }
}
