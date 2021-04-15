using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BLL
{
    public class EnvioDadosRep
    {
        DAL.IEnvioDadosRep dalEnvioEmpresaFuncionariosRep;
        private readonly string ConnectionString;
        private readonly Modelo.Cw_Usuario UsuarioLogado;
        public EnvioDadosRep() : this(null)
        {

        }

        public EnvioDadosRep(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EnvioDadosRep(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            UsuarioLogado = usuarioLogado;
            dalEnvioEmpresaFuncionariosRep = new DAL.SQL.EnvioDadosRep(new DataBase(ConnectionString));
            dalEnvioEmpresaFuncionariosRep.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEnvioEmpresaFuncionariosRep.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEnvioEmpresaFuncionariosRep.GetAll();
        }

        public Modelo.EnvioDadosRep LoadObject(int id)
        {
            return dalEnvioEmpresaFuncionariosRep.LoadObject(id);
        }

        public Modelo.Empresa GetEmpresas(Modelo.REP rep)
        {
            return dalEnvioEmpresaFuncionariosRep.GetEmpresas(rep);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.EnvioDadosRep objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            objeto.Empresas = objeto.Empresas == null ? new List<Modelo.Empresa>() : objeto.Empresas;
            objeto.Funcionarios = objeto.Funcionarios == null ? new List<Modelo.Funcionario>() : objeto.Funcionarios;
            List<Modelo.Funcionario> funcionariosSelecionados = objeto.Funcionarios.Where(f => f.Selecionado == true).ToList();
            List<Modelo.Empresa> empresasSelecionadas = objeto.Empresas.Where(e => e.Selecionado == true).ToList();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Valor do Código tem que ser diferente de zero (0).");
            }

            if (objeto.bEnviarEmpresa &&
            (objeto.listEnvioDadosRepDet == null || objeto.listEnvioDadosRepDet.Where(x => x.idEmpresa != null).Count() == 0))
            {
                ret.Add("Grid Empresas", "Nenhum Empresa selecionada.");
            }

            if (objeto.bEnviarFunc &&
                (objeto.listEnvioDadosRepDet == null || objeto.listEnvioDadosRepDet.Where(x => x.idFuncionario != null).Count() == 0))
            {
                ret.Add("Grid Funcionários", "Nenhum Funcionário selecionado.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EnvioDadosRep objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEnvioEmpresaFuncionariosRep.Incluir(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEnvioEmpresaFuncionariosRep.Excluir(objeto);
                        break;
                    default:
                        break;
                }
            }
            return erros;
        }
        public void Salvar(Modelo.EnvioDadosRep objeto)
        {
            dalEnvioEmpresaFuncionariosRep.Incluir(objeto);
        }


        public Dictionary<string, string> GerarArquivoDeEnvioParaDownload(ref string caminho, DirectoryInfo pasta, Modelo.EnvioDadosRep env, Modelo.REP objRep, Modelo.ProgressBar objProgressBar, out string nomeArquivoSemExt, string connectionString)
        {
            BLL.Empresa bllEmpresa = new BLL.Empresa(ConnectionString, UsuarioLogado);
            Modelo.Empresa objEmpresa = env.Empresas.Where(e => e.Id == objRep.IdEmpresa).FirstOrDefault();
            List<Modelo.Funcionario> listaFuncionarios = new List<Modelo.Funcionario>();
            nomeArquivoSemExt = Guid.NewGuid().ToString();
            if (env.bEnviarFunc)
            {
                listaFuncionarios = env.Funcionarios.Where(s => s.Selecionado == true) == null ? new List<Modelo.Funcionario>()
                    : env.Funcionarios.Where(s => s.Selecionado == true).ToList();
            }

            BLL.IntegracaoRelogio.EnvioDadosREP envioDadosRep = new BLL.IntegracaoRelogio.EnvioDadosREP(objEmpresa, listaFuncionarios, objRep.Id, connectionString, UsuarioLogado);
            Dictionary<string, string> retorno = envioDadosRep.ExportarWeb(ref caminho, pasta);

            return retorno;
        }

        public List<Modelo.Proxy.PxyEnvioDadosRepGrid> GetGrid()
        {
            return dalEnvioEmpresaFuncionariosRep.GetGrid();
        }

        public Modelo.EnvioDadosRep GetAllById(int id)
        {
            return dalEnvioEmpresaFuncionariosRep.GetAllById(id);
        }

        public List<Modelo.Proxy.pxyFuncionarioRelatorio> GetPxyFuncRel(int id)
        {
            return dalEnvioEmpresaFuncionariosRep.GetPxyFuncRel(id);
        }

        public List<Modelo.Proxy.PxyGridLogComunicador> GetGridLogImportacaoWebAPI()
        {
            return dalEnvioEmpresaFuncionariosRep.GetGridLogImportacaoWebAPI();
        }

        public List<Modelo.Proxy.PxyGridLogComunicador> GetGridLogImportacaoWebAPIById(int id)
        {
            return dalEnvioEmpresaFuncionariosRep.GetGridLogImportacaoWebAPIById(id);
        }

        public int ExluirEnvioDadosRepEDetalhes(int idEnvioDadosRep)
        {
            return dalEnvioEmpresaFuncionariosRep.ExluirEnvioDadosRepEDetalhes(idEnvioDadosRep);
        }

        public List<PxyIdFuncionarioLocalRep> GetRelogioPorFunc(List<int> idsFuncs)
        {
            return dalEnvioEmpresaFuncionariosRep.GetRelogioPorFunc(idsFuncs);
        }
    }
}
