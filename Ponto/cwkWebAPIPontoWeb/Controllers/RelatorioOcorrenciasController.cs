using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class RelatorioOcorrenciasController : ApiController
    {

        [HttpPost]
        [Authorize]
        [TratamentoDeErro]
        public HttpResponseMessage RelatorioOcorrencias(RelatorioOcorrenciasParam parametros)
        {
            RetornoErro retErro = new RetornoErro();
            try
            {
                Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
                if (ModelState.IsValid)
                {
                    string connectionStr = MetodosAuxiliares.Conexao();
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(connectionStr);
                    ConcurrentBag<int> idsFuncionarios = new ConcurrentBag<int>();
                    List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
                    funcionarios = bllFuncionario.GetAllFuncsListPorCPF(parametros.CPFsMatriculas.Select(s => s.CPF).ToList());
                    IList<bool> pegaOcorrencias = new List<bool>();

                    if (funcionarios.Count() <= 0)
                    {
                        retErro.erroGeral = "Não existem funcionários para os números de CPF informados";
                        return TrataErroModelState(retErro);
                    }

                    Parallel.ForEach(parametros.CPFsMatriculas, (CpfMatricula) =>
                    {
                        if (!String.IsNullOrEmpty(CpfMatricula.CPF) && !String.IsNullOrEmpty(CpfMatricula.Matricula))
                        {
                            Modelo.Funcionario func = funcionarios.Where(w => Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(w.CPF)) == Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(CpfMatricula.CPF)) && w.Matricula == CpfMatricula.Matricula).FirstOrDefault();
                            if (func != null && func.Id > 0)
                            {
                                idsFuncionarios.Add(func.Id);
                            }
                        }
                    });

                    if (idsFuncionarios.Count() <= 0)
                    {
                        retErro.erroGeral = "Ocorreu um erro ao gerar o relatório. Verifique se o CPF e a Matrícula pertencem ao mesmo funcionário.";
                        return TrataErroModelState(retErro);
                    }

                    #region popula lista de parâmetros vindos dos checkboxes

                    pegaOcorrencias.Add(parametros.EntradaAtrasada);
                    pegaOcorrencias.Add(parametros.SaidaAntecipada);
                    pegaOcorrencias.Add(parametros.Falta);
                    pegaOcorrencias.Add(parametros.DebitoBH);
                    pegaOcorrencias.Add(parametros.Ocorrencia);
                    pegaOcorrencias.Add(parametros.MarcacoesIncorretas);
                    pegaOcorrencias.Add(parametros.HorasExtras);
                    pegaOcorrencias.Add(parametros.Atraso);
                    pegaOcorrencias.Add(parametros.Justificativa);
                    
                    #endregion

                    string sIdsOcorrencia = null;
                    if (parametros.IdsOcorrencias != null && parametros.IdsOcorrencias.Count > 0)
                        sIdsOcorrencia = string.Join(",", parametros.IdsOcorrencias.Select(c => c.ToString()).ToArray());

                    string sIdsJustificativa = null;
                    if (parametros.IdsJustificativas != null && parametros.IdsJustificativas.Count > 0)
                        sIdsJustificativa = string.Join(",", parametros.IdsJustificativas.Select(c => c.ToString()).ToArray());


                    BLL.RelatorioOcorrenciaPontoWeb bllRelatorioOcorrencia = 
                            new BLL.RelatorioOcorrenciaPontoWeb(DateTime.Parse(parametros.InicioPeriodo),
                                                                DateTime.Parse(parametros.FimPeriodo),
                                                                2, 
                                                                "(" + string.Join(", ", idsFuncionarios) + ")",
                                                                0, 
                                                                0,
                                                                false,
                                                                pegaOcorrencias,
                                                                sIdsOcorrencia,
                                                                sIdsJustificativa,
                                                                connectionStr, 
                                                                userPW);

                    DataTable dt = bllRelatorioOcorrencia.GeraRelatorio();
                    List<Modelo.Proxy.Relatorios.PxyRelatorioOcorrencias> listRel = new List<Modelo.Proxy.Relatorios.PxyRelatorioOcorrencias>();
                    BLL.Marcacao bllMarcacao = new BLL.Marcacao(connectionStr);

                    foreach (DataRow row in dt.Rows)
                    {
                        //Modelo.Marcacao marc = bllMarcacao.LoadObject(Convert.ToInt32(row["id"].ToString()));
                        //Modelo.Funcionario func = bllFuncionario.LoadObject(marc.Idfuncionario);
                        Modelo.Proxy.Relatorios.PxyRelatorioOcorrencias pxy = new Modelo.Proxy.Relatorios.PxyRelatorioOcorrencias();
                        pxy.CPF = row["cpf"].ToString();
                        pxy.Batidas = row["marcacoes"].ToString();
                        pxy.Data = DateTime.Parse(row["data"].ToString());
                        pxy.DescricaoOcorrencia = row["ocorrencia"].ToString();
                        pxy.Matricula = row["Matricula"].ToString();
                        pxy.Competencia = row["Competencia"].ToString();
                        pxy.IdDocumentoWorkflow = row["IdDocumentoWorkflow"].ToString();
                        if (pxy.DescricaoOcorrencia == "Débito Banco de Horas")
	                    {
		                  pxy.QuantidadeHoras = row["bancohorasdeb"].ToString();
	                    }
                        else
	                    {
                            string diurna = (row["horasextradiurna"].ToString() != "--:--" ? row["horasextradiurna"].ToString() : "");
                            string noturna = (row["horasextranoturna"].ToString() != "--:--" ? row["horasextranoturna"].ToString() : "");
                            pxy.QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora2(2, Modelo.cwkFuncoes.ConvertHorasMinuto(diurna) + Modelo.cwkFuncoes.ConvertHorasMinuto(noturna));
	                    }
                        pxy.DiaSemana = row["dia"].ToString();


                        listRel.Add(pxy);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, listRel);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                retErro.erroGeral = ex.Message;
            }

            return TrataErroModelState(retErro);
        }

        private HttpResponseMessage TrataErroModelState(RetornoErro retErro)
        {
            List<ErroDetalhe> lErroDet = new List<ErroDetalhe>();
            var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                                        .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                        );
            foreach (var item in errorList)
            {
                ErroDetalhe ed = new ErroDetalhe();
                ed.campo = item.Key;
                ed.erro = String.Join(", ", item.Value);
                lErroDet.Add(ed);
            }
            if (retErro.erroGeral == "")
            {
                retErro.erroGeral = "Um ou mais erros encontrados, verifique os detalhes!";
            }
            retErro.ErrosDetalhados = lErroDet;
            return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
        }
    }
}
