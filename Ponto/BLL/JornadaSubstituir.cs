using DAL.SQL;
using iTextSharp.text.pdf.qrcode;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

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

            if (objeto.JornadaSubstituirFuncionario.Any())
            {
                var anterior = LoadObject(objeto.Id);
                bool novoRegistro = anterior == null || anterior.Id == 0;
                bool alterouJornada = anterior == null || anterior.Id == 0 ||
                                      anterior.IdJornadaDe != objeto.IdJornadaDe || anterior.IdJornadaPara != objeto.IdJornadaPara ||
                                      anterior.DataInicio != objeto.DataInicio || anterior.DataFim != objeto.DataFim;
                bool exluirRegistro = (objeto.JornadaSubstituirFuncionario.Count == objeto.JornadaSubstituirFuncionario.Where(w => w.Acao == Modelo.Acao.Excluir).ToList().Count);

                //Valida os funcionários que estão sendo adicionados, excluídos, ou se algum dado da jornada foi alterado valida todos
                List<int> idsFuncs = objeto.JornadaSubstituirFuncionario.Where(w => w.Acao == Modelo.Acao.Incluir || w.Acao == Modelo.Acao.Excluir || alterouJornada == true).Select(s => s.IdFuncionario).ToList();
                #region Valida Fechamento Banco e de Ponto
                DateTime? dataValidarFechamento = null;
                if (novoRegistro || exluirRegistro || (!alterouJornada && idsFuncs.Any()))
                {
                    dataValidarFechamento = objeto.DataInicio;
                }
                else if (alterouJornada)
                {
                    if (objeto.DataInicio != anterior.DataInicio || objeto.IdJornadaDe != anterior.IdJornadaDe || objeto.IdJornadaPara != anterior.IdJornadaPara)
                    {
                        dataValidarFechamento = objeto.DataInicio <= anterior.DataInicio ? objeto.DataInicio : anterior.DataInicio;
                    }
                    else
                    {
                        dataValidarFechamento = objeto.DataFim <= anterior.DataFim ? objeto.DataFim : anterior.DataFim;
                    }
                }
                if (dataValidarFechamento != null)
                {
                    List<PxyFuncionarioFechamentosPontoEBH> fechamentos = GetFechamentosPontoEBH(dataValidarFechamento.GetValueOrDefault(), idsFuncs);
                    GerarRetErroFechamento(ret, fechamentos.ToList());
                }
                #endregion

                if (!exluirRegistro)
                {
                    List<PxyJornadaSubstituirFuncionarioPeriodo> jornadasConflitantes = GetJornadasConflitantes(objeto.Id, objeto.IdJornadaDe, objeto.DataInicio.GetValueOrDefault(), objeto.DataFim.GetValueOrDefault(), idsFuncs);
                    GerarRetErroJornadaConflitante(ret, jornadasConflitantes);  
                }
            }

            return ret;
        }

        private static void GerarRetErroFechamento(Dictionary<string, string> ret, List<PxyFuncionarioFechamentosPontoEBH> fechamentos)
        {
            if (fechamentos.Any())
            {
                string erroJornadasConflitantes = String.Join(Environment.NewLine, fechamentos.Select(s => $"Tipo Fechamento: {s.FechamentoTipoDesc}; Código: {s.FechamentoCodigo}; Data: { s.FechamentoData }; Funcionário: {s.FuncionarioCodigo} - {s.FuncionarioNome}"));
                ret.Add("Fechamentos", erroJornadasConflitantes);
            }
        }

        private static void GerarRetErroJornadaConflitante(Dictionary<string, string> ret, List<PxyJornadaSubstituirFuncionarioPeriodo> jornadasConflitantes)
        {
            if (jornadasConflitantes.Any())
            {
                string erroJornadasConflitantes = String.Join(Environment.NewLine, jornadasConflitantes.Select(s => $"Código: {s.JornadaSubstituirCodigo}; Data Início: { s.JornadaSubstituirDataInicio.ToShortDateString() }; Data Fim: { s.JornadaSubstituirDataFim.ToShortDateString() };  Funcionário:{s.FuncionarioCodigo} - {s.FuncionarioNome}"));
                ret.Add("JornadasConflitantes", erroJornadasConflitantes);
            }
        }

        private List<PxyFuncionarioFechamentosPontoEBH> GetFechamentosPontoEBH(DateTime dataIni, List<int> idsFuncs)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(ConnectionString, dalJornadaSubstituir.UsuarioLogado);
            List<Modelo.Proxy.PxyFuncionarioFechamentosPontoEBH> fechamentos = bllFuncionario.GetFuncionariosComUltimoFechamentosPontoEBH(true, idsFuncs, dataIni);
            return fechamentos;
        }

        private List<PxyJornadaSubstituirFuncionarioPeriodo> GetJornadasConflitantes(int idJornada, int idJornadaDe, DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo> jornadasConflitantes = GetPxyJornadaSubstituirFuncionarioPeriodo(dataIni, dataFim, idsFuncs);
            jornadasConflitantes = jornadasConflitantes == null ? new List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo>() : jornadasConflitantes;
            jornadasConflitantes = jornadasConflitantes.Where(w => w.JornadaSubstituirId != idJornada && w.JornadaSubstituirIdJornadaDe == idJornadaDe).ToList();
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

        public List<PxyJornadaSubstituirCalculo> GetPxyJornadaSubstituirCalculo(DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            return dalJornadaSubstituir.GetPxyJornadaSubstituirCalculo(dataIni, dataFim, idsFuncs);
        }
    }
}
