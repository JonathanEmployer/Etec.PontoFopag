using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class RegistraPontoBaseController : ExtendedApiController
    {
        protected HttpResponseMessage EfetuaRegistroPonto(Bilhete bil, Boolean bSalvaLocalizacao)
        {
            if (!ChecaFuncionarioInativo() &&
                !ChecaFuncionarioExcluido() &&
                !ChecaFuncionarioDemitido(bil))
            {
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                string relogio = "RE";
                BilhetesImp objBilhete = new BilhetesImp();
                try
                {
                    objBilhete.Ordem = "000";
                    objBilhete.Data = Convert.ToDateTime(bil.DataHoraBatida).Date;
                    objBilhete.Hora = bil.DataHoraBatida.ToShortTimeString();
                    objBilhete.Func = func.Dscodigo.ToString();
                    objBilhete.Relogio = relogio;
                    objBilhete.Importado = 0;
                    objBilhete.Codigo = bllBilhetesImp.MaxCodigo();
                    objBilhete.Mar_data = objBilhete.Data;
                    objBilhete.Mar_hora = objBilhete.Hora;
                    objBilhete.Mar_relogio = objBilhete.Relogio;
                    objBilhete.DsCodigo = objBilhete.Func;
                    objBilhete.Incdata = DateTime.Now;
                    objBilhete.Inchora = DateTime.Now;
                    objBilhete.PIS = func.Pis;
                    objBilhete.IdFuncionario = func.Id;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllBilhetesImp.Salvar(Acao.Incluir, objBilhete);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        Erros.erroGeral = erro;
                        ModelState.AddModelError("CustomError", erro + "****** Bilhetes imp********");
                    }
                    else
                    {
                        if (bSalvaLocalizacao)
                        {
                            BLL.LocalizacaoRegistroPonto bllLrp = new BLL.LocalizacaoRegistroPonto(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                            bil.LocalizacaoRegistroPonto.IdBilhetesImp = objBilhete.Id;
                            bllLrp.Salvar(Modelo.Acao.Incluir, bil.LocalizacaoRegistroPonto);
                        }

                        RegistraPonto registroPonto = new RegistraPonto();
                        DateTime? dataInicial;
                        DateTime? dataFinal;
                        dataInicial = objBilhete.Data;
                        dataFinal = objBilhete.Data;
                        List<string> log = new List<string>();
                        Modelo.ProgressBar pb = new Modelo.ProgressBar();
                        pb.incrementaPB = this.IncrementaProgressBar;
                        pb.setaMensagem = this.SetaMensagem;
                        pb.setaMinMaxPB = this.SetaMinMaxProgressBar;
                        pb.setaValorPB = this.SetaValorProgressBar;

                        BLLAPI.Marcacao.ThreadRecalculaMarcacao(objBilhete, func.Id, dataInicial, dataFinal, log, pb, StrConexao);

                        #region Preenche Bilhete
                        registroPonto.Chave = objBilhete.Chave;
                        registroPonto.data = objBilhete.Data.ToString("dd/MM/yyyy");
                        registroPonto.hora = objBilhete.Hora;
                        registroPonto.ns = objBilhete.Id.ToString();
                        registroPonto.nome = func.Nome;
                        registroPonto.pis = func.Pis;

                        BLL.Empresa bllEmpresa = new BLL.Empresa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                        Modelo.Empresa emp = bllEmpresa.LoadObject(func.Idempresa);
                        registroPonto.empresa = emp.Nome;
                        registroPonto.cnpj = emp.Cnpj;
                        registroPonto.cei = emp.CEI;

                        #endregion

                        return Request.CreateResponse(HttpStatusCode.OK, registroPonto);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    string erro = "";
                    if (ex.Message.Contains("Não é possível definir a coluna") && ex.Message.Contains("O valor viola o limite de MaxLength desta coluna."))
                        erro = "Número do relógio incorreto. ";
                    else
                    {
                        if (ex.Message.Contains("IX_bilhetesimp_unique") || ex.Message.Contains("UK_bilhetesimp_8"))
                        {
                            erro = "Já existe uma marcação registrada nesse horário";
                        }
                        else
                        {
                            erro = ex.Message + "****** Detalhes: *******" + ex.StackTrace;
                        }
                    }
                    ModelState.AddModelError("CustomError", erro);
                }
                return TrataErroModelState();
            }
            else
            {
                StringBuilder erro = new StringBuilder();

                if (ChecaFuncionarioInativo())
                    erro.AppendLine("Funcionário inativo.");
                if (ChecaFuncionarioExcluido())
                    erro.AppendLine("Funcionário excluído.");
                if (ChecaFuncionarioDemitido(bil))
                    erro.AppendLine("Funcionário demitido.");

                ModelState.AddModelError("CustomError", erro.ToString());
                return TrataErroModelState();
            }
        }

        private bool ChecaFuncionarioDemitido(Bilhete bil)
        {
            bool bDemitido = false;

            try
            {
                if (func.Datademissao != null)
                {
                    DateTime dataDemissao = func.Datademissao.Value;
                    DateTime dataBatida = bil.DataHoraBatida;
                    bDemitido = (dataBatida.Subtract(dataDemissao).Days > 0);
                }
            }
            catch
            {
                bDemitido = false;
            }


            return bDemitido;
        }

        private bool ChecaFuncionarioExcluido()
        {
            return (func.Excluido == 1);
        }

        private bool ChecaFuncionarioInativo()
        {
            return (!func.bFuncionarioativo);
        }
    }
}
