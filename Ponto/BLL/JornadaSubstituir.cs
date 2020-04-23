using DAL.SQL;
using iTextSharp.text.pdf.qrcode;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class JornadaSubstituir : IBLL<Modelo.JornadaSubstituir>
    {
        DAL.IJornadaSubstituir dalJornadaSubstituir;
        private string ConnectionString;

        public JornadaSubstituir() : this(null)
        {
            
        }

        public JornadaSubstituir(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public JornadaSubstituir(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalJornadaSubstituir = new DAL.SQL.JornadaSubstituir(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalJornadaSubstituir = new DAL.SQL.JornadaSubstituir(new DataBase(ConnectionString));
                    break;
            }
            dalJornadaSubstituir.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalJornadaSubstituir.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalJornadaSubstituir.GetAll();
        }

        public Modelo.JornadaSubstituir LoadObject(int id)
        {
            return dalJornadaSubstituir.LoadObject(id);
        }

        public List<Modelo.JornadaSubstituir> GetAllList()
        {
            return dalJornadaSubstituir.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.JornadaSubstituir objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            List<int> idsFuncs = objeto.JornadaSubstituirFuncionario.Where(w => w.Acao == Modelo.Acao.Incluir || w.Acao == Modelo.Acao.Excluir).Select(s => s.IdFuncionario).ToList();
            List<PxyJornadaSubstituirFuncionarioPeriodo> jornadasConflitantes = GetJornadasConflitantes(objeto.Id, objeto.DataInicio.GetValueOrDefault(), objeto.DataFim.GetValueOrDefault(), idsFuncs);
            if (jornadasConflitantes.Any())
            {
                string erroJornadasConflitantes = String.Join("; ", jornadasConflitantes.Select(s => $"Código: {s.JornadaSubstituirCodigo}; Data Início: { s.JornadaSubstituirDataInicio.ToShortDateString() }; Data Fim: { s.JornadaSubstituirDataFim.ToShortDateString() };  Funcionário:{s.FuncionarioCodigo} - {s.FuncionarioNome}"));
                ret.Add("JornadasConflitantes", erroJornadasConflitantes);
            }

            List<PxyFuncionarioFechamentosPontoEBH> fechamentos = GetFechamentosPontoEBH(objeto.DataInicio.GetValueOrDefault(), objeto.DataFim.GetValueOrDefault(), idsFuncs);
            if (fechamentos.Any())
            {
                string erroJornadasConflitantes = String.Join("; ", fechamentos.Select(s => $"Tipo Fechamento: {s.FechamentoTipoDesc}; Código: {s.FechamentoCodigo}; Data: { s.FechamentoData }; Funcionário:{s.FuncionarioCodigo} - {s.FuncionarioNome}"));
                ret.Add("Fechamentos", erroJornadasConflitantes);
            }

            return ret;
        }

        private List<PxyFuncionarioFechamentosPontoEBH> GetFechamentosPontoEBH(DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(ConnectionString, dalJornadaSubstituir.UsuarioLogado);
            List<Modelo.Proxy.PxyFuncionarioFechamentosPontoEBH> fechamentos = bllFuncionario.GetFuncionariosComUltimoFechamentosPontoEBH(true, idsFuncs, dataIni, dataFim);
            return fechamentos;
        }

        private List<PxyJornadaSubstituirFuncionarioPeriodo> GetJornadasConflitantes(int idJornada, DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo> jornadasConflitantes = GetPxyJornadaSubstituirFuncionarioPeriodo(dataIni, dataFim, idsFuncs);
            jornadasConflitantes = jornadasConflitantes == null ? new List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo>() : jornadasConflitantes;
            jornadasConflitantes = jornadasConflitantes.Where(w => w.JornadaSubstituirId != idJornada).ToList();
            return jornadasConflitantes;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.JornadaSubstituir objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalJornadaSubstituir.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalJornadaSubstituir.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalJornadaSubstituir.Excluir(objeto);
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
            return dalJornadaSubstituir.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo> GetPxyJornadaSubstituirFuncionarioPeriodo(DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            return dalJornadaSubstituir.GetPxyJornadaSubstituirFuncionarioPeriodo(dataIni, dataFim, idsFuncs);
        }
    }
}
