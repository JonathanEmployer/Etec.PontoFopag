using cwkWebAPIPontoWeb.Utils;
using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BLL;
using Newtonsoft.Json;
using System.Security.Claims;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// Controler para retornar uma lista de marcações contendo os totalizadores de horas
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BilhetesController : ApiController
    {
        /// <summary>
        /// Método responsável por retornar a lista de bilhets por dia
        /// </summary>
        /// <param name="CPF">Parâmetro informado para informar o CPF do funcionário para a consulta</param>
        /// <param name="Data">Parâmetro informado para determinar o dia da consulta</param>
        /// <param name="Matricula">Parâmetro informado informar a matrícula do funcionário para a consulta</param>  
        /// <returns>Lista de bilhetes por dia</returns>

        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage Bilhetes(string CPF, string Data, string Matricula)
        {
            RetornoErro retErro = new RetornoErro(); 
            string connectionStr = MetodosAuxiliares.Conexao();
            CPF = CPF.Replace("-", "").Replace(".", "");
            Int64 CPFint = Convert.ToInt64(CPF);

            DateTime data = Convert.ToDateTime(Data);
            BilhetesManutencao bilhetesmanutencao = new BilhetesManutencao();

            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();

                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(connectionStr);
                    Modelo.Funcionario func = bllFuncionario.GetFuncionarioPorCpfeMatricula(CPFint, Matricula);
                    if (func == null || func.Id == null)
                    {
                        retErro.erroGeral = "Funcionário não Encontrado - Combinação CPF e Matrícula não encontrada";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }
                    List<Models.Bilhetes> ListaBilhetes = new List<Models.Bilhetes>();
                    List<Modelo.BilhetesImp> ListaBilhetesImp = new List<Modelo.BilhetesImp>();
                    BLL.BilhetesImp bllbilhetes = new BLL.BilhetesImp(connectionStr);
                    ListaBilhetesImp = bllbilhetes.GetImportadosPeriodo(new List<int>() { func.Id }, data, data, true);

                    foreach (var item in ListaBilhetesImp)
                    {
                        Models.Bilhetes bil = new Models.Bilhetes();
                        bil.DataMarcacao = item.Mar_data.GetValueOrDefault();
                        bil.DataBilhete = item.Data;
                        bil.Entrada_Saida = item.Ent_sai;
                        bil.Hora = item.Hora;
                        bil.Id = item.Id;
                        bil.IdJustificativa = item.Idjustificativa;
                        bil.Motivo = item.Motivo;
                        bil.Ocorrencia = item.Ocorrencia.ToString();
                        bil.Posicao = item.Posicao;
                        bil.Relogio = item.Relogio;
                        ListaBilhetes.Add(bil);
                    }
                    
                    bilhetesmanutencao.ListaBilhetes = ListaBilhetes;

                    return Request.CreateResponse(HttpStatusCode.OK, bilhetesmanutencao);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    bilhetesmanutencao.Erro = true;
                    bilhetesmanutencao.ErroDetalhe = retErro.erroGeral + retErro.ErrosDetalhados;
                    return Request.CreateResponse(HttpStatusCode.NotFound, bilhetesmanutencao);
                }
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

        /// <summary>
        /// Incluir/Tratar bilhetes
        /// </summary>
        /// <param name="Bilhetes">Json com os dados de bilhetes</param>
        /// <param name="IdFuncionario">Id do Funcionário</param>
        /// <returns> Retorna json do bilhete quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(BilhetesManutencao Bilhetes, int IdFuncionario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Desconsidera pre assinaladas, sempre serão geradas automaticamente pelo rotina de calcula
                    Bilhetes.ListaBilhetes = Bilhetes.ListaBilhetes.Where(w => w.Relogio != "PA").ToList();
                string connectionStr = MetodosAuxiliares.Conexao();
                BLL.BilhetesImp BllBilhete = new BLL.BilhetesImp(connectionStr);
                BLL.Funcionario BllFuncionario = new BLL.Funcionario(connectionStr);
                Dictionary<string, string> erros = new Dictionary<string, string>();
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(connectionStr);
                Modelo.BilhetesImp BilheteAnt = new Modelo.BilhetesImp();
                Modelo.Funcionario Func = new Modelo.Funcionario();
                Func = BllFuncionario.LoadObject(IdFuncionario);
                if (Func == null || Func.Id == 0)
                {
                    throw new Exception("Funcionário com id = "+IdFuncionario+" não encontrado no Pontofopag.");
                } else if (!Func.bFuncionarioativo)
                {
                    throw new Exception("Funcionário "+Func.Nome+" não esta ativo no Pontofopag.");
                }
                else if (Func.Excluido == 1)
                {
                    throw new Exception("Funcionário " + Func.Nome + " excluído no Pontofopag.");
                }

                BLL.Marcacao BllMarcacao = new BLL.Marcacao(connectionStr);
                Modelo.Marcacao marcacao = new Modelo.Marcacao();
                if (Bilhetes.ListaBilhetes == null)
                {
                    Bilhetes.ListaBilhetes = new List<Bilhetes>();
                }
                
                for (int i = 0; i < 8; i++)
                {
                    if (Bilhetes.ListaBilhetes.Where(x => x.Entrada_Saida == "E" && x.Posicao == i).GroupBy(x => new { x.Entrada_Saida, x.Posicao, x.Hora, x.DataMarcacao }).Count() > 1)
                    {
                        Bilhetes.ListaBilhetes.Where(x => x.Entrada_Saida == "E" && x.Posicao == i).ToList().ForEach(x => { x.Erro = true; x.Descricaoerro = "Posição duplicada!"; });
                    }
                    if (Bilhetes.ListaBilhetes.Where(x => x.Entrada_Saida == "S" && x.Posicao == i).GroupBy(x => new { x.Entrada_Saida, x.Posicao, x.Hora, x.DataMarcacao }).Count() > 1)
                    {
                        Bilhetes.ListaBilhetes.Where(x => x.Entrada_Saida == "S" && x.Posicao == i).ToList().ForEach(x => { x.Erro = true; x.Descricaoerro = "Posição duplicada!"; });
                    }
                    i++;
                }

                if (Bilhetes.ListaBilhetes.Where(x => x.Erro == true).Count() > 0)
                {
                    Bilhetes.Erro = true;
                    Bilhetes.ErroDetalhe = "Bilhetes com Erro! Verifique!";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Bilhetes);
                }

                if (Bilhetes.IdMarcacao > 0)
                {
                    marcacao = BllMarcacao.LoadObject(Bilhetes.IdMarcacao);
                }
                else if ( Bilhetes.ListaBilhetes.Count() > 0)
                {
                    marcacao = BllMarcacao.GetPorFuncionario(Func.Id, Convert.ToDateTime(Bilhetes.ListaBilhetes.FirstOrDefault().DataMarcacao), Convert.ToDateTime(Bilhetes.ListaBilhetes.FirstOrDefault().DataMarcacao), true).FirstOrDefault();   
                } 

                if (marcacao == null || marcacao.Id == 0)
                {
                    throw new Exception("Nenhuma marcação encontrada para a data do bilhete ou com o id informado.");
                }

                if ((marcacao.Idfechamentobh != null && marcacao.Idfechamentobh > 0) || (marcacao.IdFechamentoPonto != null && marcacao.IdFechamentoPonto > 0))
                {
                    throw new Exception("O espelho de ponto já foi fechado nesse dia, não é permitido realizar alterações.");
                }

                marcacao.BilhetesMarcacao.Where(w => w.Relogio == "MA" && !Bilhetes.ListaBilhetes.Select(s => s.Id).Contains(w.Id)).ToList().ForEach(f => f.Acao = Acao.Excluir);

                foreach (var bilhetePainel in Bilhetes.ListaBilhetes)
                {
                    Modelo.BilhetesImp bilhetePontofopag = null;
                    if (marcacao.BilhetesMarcacao.Where(x => x.Hora == bilhetePainel.Hora && x.Data == bilhetePainel.DataBilhete && x.Id != bilhetePainel.Id && bilhetePainel.Id == 0).Count() > 0)
                    {
                        bilhetePainel.Erro = true;
                        bilhetePainel.Descricaoerro = "Já existe um bilhete no mesmo horário e dia.";
                    }
                    else
                    {
                        if (bilhetePainel.Id > 0)
                        {
                            bilhetePontofopag = marcacao.BilhetesMarcacao.Where(x => x.Id == bilhetePainel.Id).FirstOrDefault();
                        }
                        if (bilhetePontofopag != null && bilhetePainel.Excluir == true)
                        {
                            if (bilhetePontofopag.Relogio != "MA")
                            {
                                bilhetePainel.Erro = true;
                                bilhetePainel.Descricaoerro = "Não é possível excluir um bilhete originado de um relógio!";
                            }
                            else
                            {
                                bilhetePontofopag.Acao = Acao.Excluir;
                            }
                        }
                        else if (bilhetePontofopag != null)
                        {
                            if ((bilhetePainel.Hora != bilhetePontofopag.Hora && bilhetePontofopag.Relogio != "MA") || bilhetePainel.DataBilhete != bilhetePontofopag.Data || bilhetePainel.Relogio != bilhetePontofopag.Relogio)
                            {
                                bilhetePainel.Erro = true;
                                bilhetePainel.Descricaoerro = "Não é permitido alterar os campos Relógio, Data ou Hora do Bilhete original. Verifique!";
                            }
                            else
                            {
                                if (bilhetePontofopag.Relogio == "MA")
                                {
                                    bilhetePontofopag.Hora = bilhetePainel.Hora;
                                    bilhetePontofopag.Mar_hora = bilhetePainel.Hora;
                                }
                                bilhetePontofopag.Ent_sai = bilhetePainel.Entrada_Saida;
                                bilhetePontofopag.Posicao = bilhetePainel.Posicao;
                                bilhetePontofopag.Idjustificativa = bilhetePainel.IdJustificativa.GetValueOrDefault();
                                bilhetePontofopag.Ocorrencia = Convert.ToChar(bilhetePainel.Ocorrencia.Substring(0, 1));
                                bilhetePontofopag.Motivo = bilhetePainel.Motivo;
                                bilhetePontofopag.Acao = Acao.Alterar;
                            }
                        }
                        else if (bilhetePainel.Id != 0 && bilhetePontofopag == null)
                        {
                            bilhetePainel.Erro = true;
                            bilhetePainel.Descricaoerro = "Bilhete não encontrado!";
                        }
                        else
                        {
                            if (bilhetePainel.Relogio != "MA")
                            {
                                bilhetePainel.Erro = true;
                                bilhetePainel.Descricaoerro = "Não é possível incluir um bilhete com identificação diferente de manual (MA)!";
                            }
                            Modelo.BilhetesImp BilheteNovo = new Modelo.BilhetesImp();
                            BilheteNovo.Acao = Acao.Incluir;
                            BilheteNovo.Ordem = "010";
                            BilheteNovo.Data = Convert.ToDateTime(bilhetePainel.DataBilhete).Date;
                            BilheteNovo.Hora = bilhetePainel.Hora.ToString();
                            BilheteNovo.Func = Func.Dscodigo.ToString();
                            BilheteNovo.Posicao = bilhetePainel.Posicao;
                            BilheteNovo.Ent_sai = bilhetePainel.Entrada_Saida;
                            BilheteNovo.Relogio = "MA";
                            BilheteNovo.Importado = 0;
                            BilheteNovo.Codigo = bllBilhetesImp.MaxCodigo();
                            BilheteNovo.Mar_data = bilhetePainel.DataBilhete;
                            BilheteNovo.Mar_hora = bilhetePainel.Hora;
                            BilheteNovo.Mar_relogio = bilhetePainel.Relogio;
                            BilheteNovo.DsCodigo = Func.Dscodigo.ToString();
                            BilheteNovo.Incdata = DateTime.Now;
                            BilheteNovo.Inchora = DateTime.Now;
                            BilheteNovo.Codigo = bllBilhetesImp.MaxCodigo();
                            BilheteNovo.Ocorrencia = Convert.ToChar(bilhetePainel.Ocorrencia);
                            BilheteNovo.Idjustificativa = bilhetePainel.IdJustificativa.GetValueOrDefault();
                            BilheteNovo.Motivo = bilhetePainel.Motivo;
                            BilheteNovo.Importado = 1;
                            BilheteNovo.Chave = null;
                            BilheteNovo.DescJustificativa = bilhetePainel.Motivo;
                            BilheteNovo.IdFuncionario = Func.Id;
                            BilheteNovo.PIS = Func.Pis;
                            marcacao.BilhetesMarcacao.Add(BilheteNovo);
                        }
                    }                  
                }

                if (Bilhetes.ListaBilhetes.Where(x => x.Erro == true).Count() > 0)
                {
                        Bilhetes.Erro = true;
                        Bilhetes.ErroDetalhe = "Bilhetes com Erro! Verifique!";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, Bilhetes);
                }
                else
	            {
                        //Seta as entradas e saídas na marcação
                        marcacao.Entrada_1 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "E", 1);
                        marcacao.Entrada_2 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "E", 2);
                        marcacao.Entrada_3 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "E", 3);
                        marcacao.Entrada_4 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "E", 4);
                        marcacao.Entrada_5 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "E", 5);
                        marcacao.Entrada_6 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "E", 6);
                        marcacao.Entrada_7 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "E", 7);
                        marcacao.Entrada_8 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "E", 8);
                        marcacao.Saida_1 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "S", 1);
                        marcacao.Saida_2 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "S", 2);
                        marcacao.Saida_3 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "S", 3);
                        marcacao.Saida_4 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "S", 4);
                        marcacao.Saida_5 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "S", 5);
                        marcacao.Saida_6 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "S", 6);
                        marcacao.Saida_7 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "S", 7);
                        marcacao.Saida_8 = VerificaBilheteNulo(marcacao.BilhetesMarcacao, "S", 8);
                        //Seta Número do Relógio na Marcação
                        marcacao.Ent_num_relogio_1 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "E", 1);
                        marcacao.Ent_num_relogio_2 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "E", 2);
                        marcacao.Ent_num_relogio_3 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "E", 3);
                        marcacao.Ent_num_relogio_4 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "E", 4);
                        marcacao.Ent_num_relogio_5 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "E", 5);
                        marcacao.Ent_num_relogio_6 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "E", 6);
                        marcacao.Ent_num_relogio_7 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "E", 7);
                        marcacao.Ent_num_relogio_8 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "E", 8);
                        marcacao.Sai_num_relogio_1 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "S", 1);
                        marcacao.Sai_num_relogio_2 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "S", 2);
                        marcacao.Sai_num_relogio_3 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "S", 3);
                        marcacao.Sai_num_relogio_4 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "S", 4);
                        marcacao.Sai_num_relogio_5 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "S", 5);
                        marcacao.Sai_num_relogio_6 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "S", 6);
                        marcacao.Sai_num_relogio_7 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "S", 7);
                        marcacao.Sai_num_relogio_8 = VerificaNumRelogioMarcacao(marcacao.BilhetesMarcacao, "S", 8);

                        List<string> log = new List<string>();
                        Modelo.ProgressBar pb = new Modelo.ProgressBar();
                        pb.incrementaPB = this.IncrementaProgressBar;
                        pb.setaMensagem = this.SetaMensagem;
                        pb.setaMinMaxPB = this.SetaMinMaxProgressBar;
                        pb.setaValorPB = this.SetaValorProgressBar;

                        BllMarcacao.ObjProgressBar = pb;

                        erros = BllMarcacao.Salvar(Modelo.Acao.Alterar, marcacao);
                        if (erros.Count() > 0)
                        {
                            Bilhetes.Erro = true;
                            Bilhetes.ErroDetalhe = String.Join("; ", erros.Select(x => x.Value));
                            return Request.CreateResponse(HttpStatusCode.BadRequest, Bilhetes);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, Bilhetes);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, Bilhetes);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Bilhetes.Erro = true;
                if (ex.Message.Contains("IX_bilhetesimp_unique"))
                {
                    Bilhetes.ErroDetalhe = "Existem bilhetes duplicados. Verifique";
                }
                else
	            {
                    Bilhetes.ErroDetalhe = ex.Message;
	            }
                return Request.CreateResponse(HttpStatusCode.BadRequest, Bilhetes);
            }
           
        }

        private string VerificaNumRelogioMarcacao(List<Modelo.BilhetesImp> BilValidos, string EntSai, int Posicao)
        {
            Modelo.BilhetesImp Bil = new Modelo.BilhetesImp();
            Bil = BilValidos.Where(x => x.Ent_sai == EntSai && x.Posicao == Posicao && x.Acao != Acao.Excluir).FirstOrDefault();
            if (Bil != null)
            {
                return Bil.Relogio; 
            }
            else
            {
                return "";
            }
        }

        private static string VerificaBilheteNulo(List<Modelo.BilhetesImp> BilValidos, string EntSai, int Posicao)
        {
            Modelo.BilhetesImp Bil = BilValidos.Where(x => x.Ent_sai == EntSai && x.Posicao == Posicao && x.Acao != Acao.Excluir).FirstOrDefault();
            if (Bil != null)
            {
                return  Bil.Mar_hora;
            }
            else
            {
                return "--:--";
            }
        }


        private void SetaValorProgressBar(int valor){}

        private void SetaMinMaxProgressBar(int min, int max){}

        private void SetaMensagem(string mensagem){}

        private void IncrementaProgressBar(int incremento){}
    }
}