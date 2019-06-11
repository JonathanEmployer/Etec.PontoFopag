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

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RetornaBilhetesOriginaisController : ApiController
    {
        // GET: RetornaBilhetesOriginais
        /// <summary>
        /// RetornaBilhetesOriginais
        /// </summary>
        /// <param name="BilhetesPainel">Json com os dados de bilhetes originais</param>
        /// <param name="IdFuncionario">Id do funcionário</param>
        /// <returns> Retorna json de com o resultado do armazenamento dos bilhetes originais.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage RetornaBilhetesOriginais(BilhetesManutencao BilhetesPainel, int IdFuncionario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Desconsidera pre assinaladas, sempre serão geradas automaticamente pelo rotina de calcula
                    BilhetesPainel.ListaBilhetes = BilhetesPainel.ListaBilhetes.Where(w => w.Relogio != "PA").ToList();
                    string connectionStr = MetodosAuxiliares.Conexao();
                    BLL.BilhetesImp BllBilhete = new BLL.BilhetesImp(connectionStr);
                    BLL.Funcionario BllFuncionario = new BLL.Funcionario(connectionStr);
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(connectionStr);
                    Modelo.BilhetesImp BilheteAnt = new Modelo.BilhetesImp();
                    Modelo.Funcionario Func = new Modelo.Funcionario();
                    Func = BllFuncionario.LoadObject(IdFuncionario);
                    BLL.Marcacao BllMarcacao = new BLL.Marcacao(connectionStr);
                    Modelo.Marcacao marcacao = new Modelo.Marcacao();
                    List<Modelo.BilhetesImp> bilsPonto = new List<Modelo.BilhetesImp>();
                    DateTime data = BllMarcacao.LoadObject(BilhetesPainel.IdMarcacao).Data;
                    bilsPonto = bllBilhetesImp.LoadManutencaoBilhetes(Func.Dscodigo, data, true);

                    List<Modelo.BilhetesImp> BilhetesOriginais = new List<Modelo.BilhetesImp>();
                    

                    foreach (var bilPonto in bilsPonto.Where(x => !BilhetesPainel.ListaBilhetes.Select(s => s.Id).Contains(x.Id)))
                    {
                        if (bilPonto.Relogio == "MA")
                        {
                            bilPonto.Acao = Acao.Excluir;
                        }
                        else
                        {
                            bilPonto.Acao = Acao.Alterar;
                        }
                        BilhetesOriginais.Add(bilPonto);
                    }



                    if (bilsPonto != null && bilsPonto.Count() > 0)
                    {
                        foreach (var bilPainel in BilhetesPainel.ListaBilhetes)
                        {
                            Modelo.BilhetesImp bilPonto = new Modelo.BilhetesImp();
                            bilPonto = bilsPonto.Where(x => x.Id == bilPainel.Id).FirstOrDefault();

                            if (bilPonto != null && bilPonto.Id != 0)
                            {
                                bilPonto.Acao = Acao.Alterar;
                                bilPonto.Id = bilPainel.Id;

                                if (bilPonto.Relogio == "MA")
                                {
                                    bilPonto.Mar_data = bilPainel.DataMarcacao;
                                    bilPonto.Mar_hora = bilPainel.Hora;
                                }
                                bilPonto.Posicao = bilPainel.Posicao;
                                bilPonto.Ent_sai = bilPainel.Entrada_Saida;
                                bilPonto.Idjustificativa = bilPainel.IdJustificativa.GetValueOrDefault();
                                if (bilPainel.Ocorrencia == null)
                                {
                                    bilPonto.Ocorrencia = new char();
                                }
                                else
                                {
                                    bilPonto.Ocorrencia = Convert.ToChar(bilPainel.Ocorrencia);
                                }                               
                                bilPonto.Motivo = bilPainel.Motivo;
                                bilPonto.DescJustificativa = bilPainel.Motivo;
                                BilhetesOriginais.Add(bilPonto);
                            }
                            else
                            {
                                if (bilPainel.IdJustificativa == null || bilPainel.IdJustificativa == 0)
                                {
                                    bilPainel.Erro = true;
                                    bilPainel.Descricaoerro = "Para incluir um bilhete manual, deve ser informada uma justificativa.";
                                }
                                else
                                {
                                    Modelo.BilhetesImp biloriginal = new Modelo.BilhetesImp();
                                    biloriginal.Acao = Acao.Incluir;
                                    biloriginal.Mar_data = bilPainel.DataMarcacao;
                                    biloriginal.Mar_hora = bilPainel.Hora;
                                    biloriginal.Data = bilPainel.DataBilhete;
                                    biloriginal.Hora = bilPainel.Hora;
                                    biloriginal.Relogio = "MA";
                                    biloriginal.DsCodigo = Func.Dscodigo;
                                    biloriginal.Posicao = bilPainel.Posicao;
                                    biloriginal.Ent_sai = bilPainel.Entrada_Saida;
                                    biloriginal.Idjustificativa = bilPainel.IdJustificativa.GetValueOrDefault();
                                    if (bilPainel.Ocorrencia == null)
                                    {
                                        biloriginal.Ocorrencia = new char();
                                    }
                                    else
                                    {
                                        biloriginal.Ocorrencia = Convert.ToChar(bilPainel.Ocorrencia);
                                    }
                                    biloriginal.Motivo = bilPainel.Motivo;
                                    biloriginal.Ordem = "010";
                                    biloriginal.Func = Func.Dscodigo.ToString();
                                    biloriginal.Importado = 1;
                                    biloriginal.Codigo = bllBilhetesImp.MaxCodigo();
                                    biloriginal.Mar_relogio = "MA";
                                    biloriginal.Incdata = DateTime.Now;
                                    biloriginal.Inchora = DateTime.Now;
                                    biloriginal.DescJustificativa = bilPainel.Motivo;
                                    BilhetesOriginais.Add(biloriginal);
                                }
                            }

                        }

                        if (BilhetesPainel.ListaBilhetes.Where(x => x.Erro == true).Count() > 0)
                        {
                            BilhetesPainel.Erro = true;
                            BilhetesPainel.ErroDetalhe = "Bilhetes com Erro! Verifique!";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, BilhetesPainel);
                        }
                        else
                        {
                            marcacao = BllMarcacao.GetPorFuncionario(Func.Id, data, data, false).FirstOrDefault();
                            marcacao.BilhetesMarcacao = BilhetesOriginais;

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
                                BilhetesPainel.Erro = true;
                                BilhetesPainel.ErroDetalhe = String.Join("; ", erros.Select(x => x.Value));
                                return Request.CreateResponse(HttpStatusCode.BadRequest, BilhetesPainel);
                            }
                            else
                            {
                                List<Models.Bilhetes> bilhetesretorno = new List<Bilhetes>();
                                foreach (var item in marcacao.BilhetesMarcacao)
                                {
                                    Models.Bilhetes bilheteretorno = new Models.Bilhetes();
                                    bilheteretorno.DataBilhete = item.Data;
                                    bilheteretorno.DataMarcacao = item.Mar_data.GetValueOrDefault();
                                    bilheteretorno.Entrada_Saida = item.Ent_sai;
                                    bilheteretorno.Hora = item.Hora;
                                    bilheteretorno.Id = item.Id;
                                    bilheteretorno.IdJustificativa = item.Idjustificativa;
                                    bilheteretorno.Motivo = item.Motivo;
                                    bilheteretorno.Ocorrencia = item.Ocorrencia.ToString();
                                    bilheteretorno.Posicao = item.Posicao;
                                    bilheteretorno.Relogio = item.Relogio;
                                    bilhetesretorno.Add(bilheteretorno);
                                }
                                return Request.CreateResponse(HttpStatusCode.OK, BilhetesPainel);
                            }
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, BilhetesPainel);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                BilhetesPainel.Erro = true;
                if (ex.Message.Contains("IX_bilhetesimp_unique"))
                {
                    BilhetesPainel.ErroDetalhe = "Existem bilhetes duplicados. Verifique";
                }
                else
                {
                    BilhetesPainel.ErroDetalhe = ex.Message;
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, BilhetesPainel);
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
                return Bil.Mar_hora;
            }
            else
            {
                return "--:--";
            }
        }

        private void SetaValorProgressBar(int valor) { }

        private void SetaMinMaxProgressBar(int min, int max) { }

        private void SetaMensagem(string mensagem) { }

        private void IncrementaProgressBar(int incremento) { }
    }
}