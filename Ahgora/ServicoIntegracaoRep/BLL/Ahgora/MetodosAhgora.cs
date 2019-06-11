using ServicoIntegracaoRep.DAL;
using ServicoIntegracaoRep.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BLLPonto = BLL;

namespace ServicoIntegracaoRep.BLL.Ahgora
{
    public class MetodosAhgora
    {
        public MetodosAhgora()
        {
        }
        /// <summary>
        /// Método que trata a requisição do relógio ahgora e responde com a solitação desejada.
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da Requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        public void TrataRequisicaoRetorno(HttpListenerContext context, HttpListenerResponse response,ref EquipamentoRequisicao equipamentoRequisitando,ref Equipamento equipamento)
        {
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                //Se o equipamento existe ou se o relógio esta devolvendo uma solicitado de dados da empresa ( "\"req\":\"empresa\""), deixo a rotina prosseguir
                if ((equipamento.Id > 0 && !equipamento.Processando && equipamento.Ativo))
                {
                    if (equipamento.DataPrimeiraImportacao == null || equipamento.DataPrimeiraImportacao == DateTime.MinValue)
                    {
                        equipamento.DataPrimeiraImportacao = DateTime.Now;
                    }

                    string[] chaves = new string[] { "\"req\":\"ini\"", "\"req\":\"NSR\"", "\"req\":\"AFD\"", "\"req\":\"empresa\"", "\"req\":\"cfg_empresa\"", "\"req\":\"cfg_funcionarios\"", "\"req\":\"pede_dados_PIS\"", "\"req\":\"exclui_funcionarios\"", "\"req\":\"cfg_data\""};
                    string requisicao = equipamentoRequisitando.Requisicao;
                    string chaveEncontrada = chaves.FirstOrDefault<string>(s => requisicao.Contains(s));
                    switch (chaveEncontrada)
                    {
                        case "\"req\":\"ini\"": // Quando o equipamento faz uma nova
                            {
                                if (equipamento.ReqEmpresa == null || String.IsNullOrEmpty(equipamento.ReqEmpresa.Empresa.RazaoSocial))
                                {
                                    SolicitaEmpresa(equipamentoRequisitando, context, equipamento, response);
                                }
                                else
                                {
                                    SolicitaNSR(equipamentoRequisitando, context, equipamento, response);
                                }
                            }
                            break;
                        case "\"req\":\"NSR\"": // Quando o equipamento envia o número do último NSR
                            {
                                ReqNSR repNSR = jsonSerializer.Deserialize<ReqNSR>(equipamentoRequisitando.Requisicao);
                                if (repNSR.Resp.ToUpper() == "OK")
                                {
                                    equipamento.ReqNSR = repNSR;
                                    NovaSolicitacao(context, response, equipamentoRequisitando, ref equipamento);
                                }
                            }
                            break;
                        case "\"req\":\"AFD\"": // Quando o equipamento envia um AFD
                            {
                                equipamento.Processando = true;
                                RepAFD repAFD = jsonSerializer.Deserialize<RepAFD>(equipamentoRequisitando.Requisicao);
                                bool erroImp = false;
                                ImportarBilhetes impBil = new ImportarBilhetes();
                                if (equipamento.UltimoNSR == 0 && equipamento.DataInicioImportacao != null && equipamento.DataInicioImportacao != DateTime.MinValue)
                                {
                                    foreach (string linha in repAFD.registros)
                                    {
                                        DateTime dt = new DateTime();
                                        //Descarta o Header do arquivo se houver
                                        if (linha.Substring(9, 1) == "1")
                                        {
                                            continue;
                                        }
                                        string dataStr = linha.Substring(10, 8);
                                        string dataStrFormatada = string.Empty;
                                        for (int i = 0; i < 8; i++)
                                        {
                                            if ((i == 2) || (i == 4))
                                            {
                                                dataStrFormatada += "-" + dataStr[i];
                                            }
                                            else
                                            {
                                                dataStrFormatada += dataStr[i];
                                            }
                                        }

                                        DateTime.TryParse(dataStrFormatada, out dt);
                                        if (dt != new DateTime() && dt >= equipamento.DataInicioImportacao)
                                        {
                                            equipamento.UltimoNSR = Convert.ToInt32(linha.Substring(1, 8));
                                            break;
                                        }
                                    }
                                    if (equipamento.UltimoNSR > 0)
                                    {
                                        SolicitaAFD(equipamentoRequisitando, context, equipamento, response, equipamento.UltimoNSR, equipamento.ReqNSR.Nsr);
                                        equipamento.Processando = false;
                                        break;
                                    }
                                    else
                                    {
                                        SolicitaAFD(equipamentoRequisitando, context, equipamento, response, Convert.ToInt32(repAFD.nsr_fim), equipamento.ReqNSR.Nsr);
                                        equipamento.Processando = false;
                                        break;
                                    }
                                }
                                AdicionaHeader(equipamento, repAFD);
                                erroImp = impBil.ImportaBilhete(equipamento.NumSerie, BLLPonto.CriptoString.Decrypt(equipamento.Conexao), repAFD.registros);
                                equipamento.Processando = false;
                                if (!erroImp)
                                {
                                    equipamento.UltimoNSR = Convert.ToInt32(repAFD.nsr_fim);
                                    equipamento.DataUltimaImportacao = DateTime.Now;
                                    DadosCentralCliente.AtualizaDadosRepCentralCliente(ref equipamento);
                                    NovaSolicitacao(context, response, equipamentoRequisitando, ref equipamento);
                                }
                            }
                            break;
                        case "\"req\":\"empresa\"": // Quando o equipamento Retorna uma Empresa
                            {
                                ReqEmpresa reqEmpresa = jsonSerializer.Deserialize<ReqEmpresa>(equipamentoRequisitando.Requisicao);
                                equipamento.ReqEmpresa = reqEmpresa;
                                if (equipamento.TemEmpresaExportar)
                                {
                                    EnviarEmpresa(context, response, equipamentoRequisitando, equipamento);
                                }
                                else
                                if (equipamento.TemHorarioVeraoExportar)
                                {
                                    if (equipamento.HorarioVeraoEnviar == null)
                                    {
                                        BuscarHorarioVeraoEnviar(context, response, equipamentoRequisitando, equipamento);
                                    }
                                    else
                                    {
                                        EnviarHorarioVerao(context, response, equipamentoRequisitando, equipamento);
                                    }
                                }
                                else
                                {
                                    SolicitaNSR(equipamentoRequisitando, context, equipamento, response);
                                }
                            }
                            break;
                        case "\"req\":\"cfg_empresa\"": // Retorno de um comando de alterar dados da empresa
                            {
                                ReqResp respCfgEmpresa = jsonSerializer.Deserialize<ReqResp>(equipamentoRequisitando.Requisicao);
                                if (respCfgEmpresa.Resp.ToUpper() == "OK")
                                {
                                    if (equipamento.cwkEmpresa != null)
                                    {
                                        string IdsEnvioDadosRep = equipamento.cwkEmpresa.IdEnvioDadosRep.ToString();
                                        string IdsEnvioDadosRepDet = equipamento.cwkEmpresa.IdEnvioDadosRepDet.ToString();
                                        ExecutaComandosSql.DeletaEnvioDadosRep(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), IdsEnvioDadosRep,  IdsEnvioDadosRepDet);
                                        equipamento.cwkEmpresa = null;
                                    }

                                    if (equipamento.HorarioVeraoEnviar != null)
                                    {
                                        ExecutaComandosSql.DeletaEnvioDataHoraHorarioVerao(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), equipamento.HorarioVeraoEnviar.IdEnvioHorarioVerao.ToString());
                                        equipamento.HorarioVeraoEnviar = null;
                                    }
                                    SolicitaEmpresa(equipamentoRequisitando, context, equipamento, response);                                    
                                }
                            }
                            break;
                        case "\"req\":\"pede_dados_PIS\"":
                            {
                                CfgFuncionarios cfgFuncionarios = new CfgFuncionarios();
                                //Verifica se retornou dados de funcionário(Caso o funcionário já exista pega os dados que o ponto ainda não controla e envia novamente para não perder esses dados)
                                if (!equipamentoRequisitando.Requisicao.Contains("\"Funcionario\":[]"))
                                {
                                    PedeDadosPIS pedeDadosPIS = jsonSerializer.Deserialize<PedeDadosPIS>(equipamentoRequisitando.Requisicao);
                                    if ((pedeDadosPIS.Resp.ToUpper() == "OK") && (pedeDadosPIS.Funcionario != null && !String.IsNullOrEmpty(pedeDadosPIS.Funcionario.PIS)))
                                    {
                                        equipamento.FuncionarioEnviando.BioDados = pedeDadosPIS.Funcionario.BioDados;
                                        equipamento.FuncionarioEnviando.CodBarras = pedeDadosPIS.Funcionario.CodBarras;
                                        equipamento.FuncionarioEnviando.MiFareDado = pedeDadosPIS.Funcionario.MiFareDado;
                                    }
                                }
                                cfgFuncionarios.Funcionarios = new List<Funcionario>();
                                cfgFuncionarios.Funcionarios.Add(equipamento.FuncionarioEnviando);
                                string jsonResp = Newtonsoft.Json.JsonConvert.SerializeObject(cfgFuncionarios);
                                MontaResposta(equipamentoRequisitando, context, response, jsonResp);
                            }
                            break;
                        case "\"req\":\"cfg_funcionarios\"": // Retorno de um comando de Incluir/Alterar dados da empresa
                            {
                                RespCfgFuncionarios respCfgFuncionarios = jsonSerializer.Deserialize<RespCfgFuncionarios>(equipamentoRequisitando.Requisicao);
                                if (respCfgFuncionarios.Resp.ToUpper() == "OK")
                                {
                                    string IdsEnvioDadosRep = String.Join(",",equipamento.FuncionarioEnviando.IDEnvioDadosRep);
                                    string IdsEnvioDadosRepDet = String.Join(",", equipamento.FuncionarioEnviando.IDEnvioDadosRepDet);
                                    ExecutaComandosSql.DeletaEnvioDadosRep(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), IdsEnvioDadosRep, IdsEnvioDadosRepDet);
                                    equipamento = DeletarOuRequisitarFuncionario(context, response, equipamentoRequisitando, equipamento);
                                }
                                else
                                {
                                    MandaDormir(context, response, equipamentoRequisitando);
                                }
                            }
                            break;
                        case "\"req\":\"exclui_funcionarios\"": // Retorno de um comando de Incluir/Alterar dados da empresa
                            {
                                ReqResp respExcluiFuncionarios = jsonSerializer.Deserialize<ReqResp>(equipamentoRequisitando.Requisicao);
                                if (respExcluiFuncionarios.Resp.ToUpper() == "OK")
                                {
                                    string IdsEnvioDadosRep = String.Join(",", equipamento.ExcluirFuncionarios.FuncionariosExcluir.Select(s => s.IDEnvioDadosRep));
                                    string IdsEnvioDadosRepDet = String.Join(",", equipamento.ExcluirFuncionarios.FuncionariosExcluir.Select(s => s.IDEnvioDadosRepDet));
                                    ExecutaComandosSql.DeletaEnvioDadosRep(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), IdsEnvioDadosRep, IdsEnvioDadosRepDet);
                                    equipamento = DeletarOuRequisitarFuncionario(context, response, equipamentoRequisitando, equipamento);
                                }
                                else
                                {
                                    MandaDormir(context, response, equipamentoRequisitando);
                                }
                            }
                            break;
                        case "\"req\":\"cfg_data\"": // Retorno de um comando de Incluir/Alterar dados da empresa
                            {
                                ReqResp resp = jsonSerializer.Deserialize<ReqResp>(equipamentoRequisitando.Requisicao);
                                if (resp.Resp.ToUpper() == "OK")
                                {
                                    ExecutaComandosSql.DeletaEnvioDataHoraHorarioVerao(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), equipamento.DataHoraEnviar.IdEnvioHorarioVerao.ToString());
                                    equipamento.DataHoraEnviar = null;
                                    SolicitaNSR(equipamentoRequisitando, context, equipamento, response);
                                }
                                else
                                {
                                    MandaDormir(context, response, equipamentoRequisitando);
                                }
                            }
                            break;
                        default: //Quando equipamento envia uma requisição "desconhecida" (Não tratada ainda) o sistema manda o equipamento dormir por um tempo.
                            {
                                MandaDormir(context, response, equipamentoRequisitando);
                            }
                            break;
                    }
                    //A cada 10 requisições atualiza os dados na central do cliente.
                    if ((equipamento.TotalDeRequisicoes - equipamento.UltimaRequisicaoSalva) > 10)
                    {
                        DadosCentralCliente.AtualizaDadosRepCentralCliente(ref equipamento);
                    }
                }
                else // Se o Relógio ainda não estiver cadastrado "Trata Erros" e coloca o equipamento para dormir por um tempo.
                {
                    Funcoes.TrataErros(equipamentoRequisitando, equipamento);
                    // Antes de colocar o equipamento para "Dormir", peço a empresa do equipamento para saber qual a empresa que esta fazendo a solicitação.
                    if (equipamento.ReqEmpresa == null || String.IsNullOrEmpty(equipamento.ReqEmpresa.Empresa.RazaoSocial))
                    {
                        if (equipamentoRequisitando.Requisicao.Contains("\"req\":\"empresa\""))
                        {
                            ReqEmpresa reqEmpresa = jsonSerializer.Deserialize<ReqEmpresa>(equipamentoRequisitando.Requisicao);
                            equipamento.ReqEmpresa = reqEmpresa;
                            MandaDormir(context, response, equipamentoRequisitando);
                        }
                        else
                        {
                            SolicitaEmpresa(equipamentoRequisitando, context, equipamento, response);
                        }
                    }
                    else
                    {
                        MandaDormir(context, response, equipamentoRequisitando);
                    }
                }
            }
            catch (Exception ex)
            {
                equipamento.Processando = false;
                throw ex;
            }
        }
        /// <summary>
        /// Método que envia os dados da empresa para o equipamento
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        private void EnviarEmpresa(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando, Equipamento equipamento)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            CfgEmpresa configEmpresa = jsonSerializer.Deserialize<CfgEmpresa>(equipamentoRequisitando.Requisicao);

            CwkEmpresa cwkEmpresa = BuscaEmpresaEnviar(equipamento);

            if (cwkEmpresa != null)
            {
                equipamento.cwkEmpresa = cwkEmpresa;
                configEmpresa.Empresa.CEI = cwkEmpresa.CEI;
                configEmpresa.Empresa.CNPJouCPF = cwkEmpresa.CNPJouCPF;
                configEmpresa.Empresa.Identificador = cwkEmpresa.Identificador;
                configEmpresa.Empresa.Local = cwkEmpresa.Local;
                configEmpresa.Empresa.RazaoSocial = cwkEmpresa.RazaoSocial;
                configEmpresa.Empresa.SenhaMenu = cwkEmpresa.SenhaMenu;
                configEmpresa.Empresa.Tipo = cwkEmpresa.Tipo;

                string jsonResp = Newtonsoft.Json.JsonConvert.SerializeObject(configEmpresa);
                MontaResposta(equipamentoRequisitando, context, response, jsonResp);
            }
            else
            {
                equipamento.TemEmpresaExportar = false;
                equipamento.DataUltimaExportacao = DateTime.Now;
                equipamento.cwkEmpresa = null;
                DadosCentralCliente.AtualizaDadosRepCentralCliente(ref equipamento, false, false, true, false);
                NovaSolicitacao(context, response, equipamentoRequisitando,ref equipamento);
            }
        }
        /// <summary>
        /// Busca dados da empresa a ser enviada.
        /// </summary>
        /// <param name="equipamento"> Equipamento que vai receber a configuração</param>
        /// <returns>Retorna empresa a ser enviada</returns>
        private static CwkEmpresa BuscaEmpresaEnviar(Equipamento equipamento)
        {
            string sql = @"select top(1) case when e.cpf is null or e.cpf = '' then
				                                                '1'
			                                                else
				                                                '2' end Tipo,
		                                                case when e.cpf is null or e.cpf = '' then
				                                                e.cnpj
			                                                else
				                                                e.cpf end CNPJouCPF,
		                                                r.local,
		                                                e.nome RazaoSocial,
		                                                e.nome Identificador,
		                                                e.cei CEI,
		                                                edr.id IDEnvioDadosRep,
                                                        edrd.id IDEnvioDadosRepDet,
														r.numserie,
                                                        edr.BOperacao
                                              from rep r
                                             inner join EnvioDadosRep edr on r.id = edr.IDRep
                                             inner join EnvioDadosRepDet edrd on edr.ID = edrd.IDEnvioDadosRep
                                             inner join empresa e on e.id = edrd.IDEmpresa 
                                             where r.numserie = '" + equipamento.NumSerie + "' order by edr.id";
            var lCwkEmpresa = ExecutaComandosSql.LerDados<CwkEmpresa>(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), sql);
            CwkEmpresa cwkEmpresa = null;
            if (lCwkEmpresa != null)
            {
                cwkEmpresa = lCwkEmpresa.FirstOrDefault();
            }
            // Se empresa != null e BOperacao == true significa que é para excluir a empresa, mas o ahgora não da essa possibilidade, por tanto excluo essa solicitacao e continuo com o processo.
            if (cwkEmpresa != null && cwkEmpresa.BOperacao)
            {
                string IdsEnvioDadosRep = cwkEmpresa.IdEnvioDadosRep.ToString();
                string IdsEnvioDadosRepDet = cwkEmpresa.IdEnvioDadosRepDet.ToString();
                ExecutaComandosSql.DeletaEnvioDadosRep(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), IdsEnvioDadosRep, IdsEnvioDadosRepDet);
                cwkEmpresa = BuscaEmpresaEnviar(equipamento);
            }
            return cwkEmpresa;
        }
        /// <summary>
        /// Método que envia os dados dos funcionários para o equipamento
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        private void BuscarFuncionariosEnviar(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando, Equipamento equipamento)
        {
            string sql = @"select f.dscodigo ID,
							f.pis PIS,
							f.nome Nome,
							f.senha Passwd,
							x.IDEnvioDadosRep,
							edrd.id IDEnvioDadosRepDet,
							x.numserie,
							x.BOperacao
					   from (
							 select top(1)
									edr.id IDEnvioDadosRep,
									r.numserie,
									edr.bOperacao BOperacao
								from rep r
							inner join EnvioDadosRep edr on r.id = edr.IDRep
                            inner join EnvioDadosRepDet edrd on edr.id = edrd.IDEnvioDadosRep and edrd.IDFuncionario is not null
							where r.numserie = '" + equipamento.NumSerie + @"' 
							order by edr.id
							) x
							inner join EnvioDadosRepDet edrd on x.IDEnvioDadosRep = edrd.IDEnvioDadosRep
							inner join funcionario f on f.id = edrd.idfuncionario";

            List<Funcionario> funcionarios = ExecutaComandosSql.LerDados<Funcionario>(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), sql);
            if (funcionarios == null || funcionarios.Count() == 0)
            {
                equipamento.FuncionariosEnviar = null;
                equipamento.FuncionarioEnviando = null;
                equipamento.ExcluirFuncionarios = null;

                equipamento.TemFuncionarioExportar = false;
                equipamento.DataUltimaExportacao = DateTime.Now;
                DadosCentralCliente.AtualizaDadosRepCentralCliente(ref equipamento, false, false, false, true);
                NovaSolicitacao(context, response, equipamentoRequisitando, ref equipamento);
            }
            else
            {
                equipamento.FuncionariosEnviar = funcionarios;
                equipamento = DeletarOuRequisitarFuncionario(context, response, equipamentoRequisitando, equipamento);
            }            
        }
        /// <summary>
        /// Método responsavel por separar os dados funcionários que serão deletados e ou Alterados/Incluido e gerar a requisição
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        /// <returns>Retorna o equipamento com os funcionarios separados o que é para excluir e o que é para Alterar/Incluir</returns>
        private Equipamento DeletarOuRequisitarFuncionario(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando, Equipamento equipamento)
        {
            if (equipamento.FuncionariosEnviar != null && equipamento.FuncionariosEnviar.Count() > 0)
            {
                string jsonResp = "";
                if (equipamento.FuncionariosEnviar.Where(x => x.BOperacao == true).Count() > 0)
                {
                    equipamento.ExcluirFuncionarios = new ExcluirFuncionarios();
                    equipamento.ExcluirFuncionarios.FuncionariosExcluir = new List<Funcionario>();
                    equipamento.ExcluirFuncionarios.FuncionariosExcluir = equipamento.FuncionariosEnviar.Where(x => x.BOperacao == true).ToList();
                    equipamento.FuncionariosEnviar.RemoveAll(x => x.BOperacao == true);
                    jsonResp = Newtonsoft.Json.JsonConvert.SerializeObject(equipamento.ExcluirFuncionarios);
                }
                else
                {
                    Funcionario func = equipamento.FuncionariosEnviar.FirstOrDefault();
                    equipamento.FuncionariosEnviar.Remove(func);
                    func.Passwd = BLLPonto.ClSeguranca.Descriptografar(func.Passwd);
                    equipamento.FuncionarioEnviando = func;
                    PededadosPIS pede_dados_PIS = new PededadosPIS();
                    pede_dados_PIS.PIS = equipamento.FuncionarioEnviando.PIS;
                    jsonResp = Newtonsoft.Json.JsonConvert.SerializeObject(pede_dados_PIS);
                }
                MontaResposta(equipamentoRequisitando, context, response, jsonResp);
            }
            else
            {
                BuscarFuncionariosEnviar(context, response, equipamentoRequisitando, equipamento);
            }
            return equipamento;
        }
        /// <summary>
        /// Método que busca os dados de horário de verão na central do cliente a ser enviado para o equipamento
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        private void BuscarHorarioVeraoEnviar(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando, Equipamento equipamento)
        {
            string sql = @"select	top(1) 
		                            ecdh.dtInicioHorarioVerao,
		                            ecdh.dtFimHorarioVerao,
		                            r.numserie,
		                            ecdh.id IdEnvioHorarioVerao
                            from rep r
                            inner join envioconfiguracoesdatahora ecdh on r.id = ecdh.idRelogio
                            where ecdh.bEnviaHorarioVerao = 1
                              and r.numserie = '" + equipamento.NumSerie + "' order by ecdh.id";
            CwkEnviaHorarioVerao cwkEnviaHorarioVerao = null;
            var result = ExecutaComandosSql.LerDados<CwkEnviaHorarioVerao>(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), sql);
            if (result != null)
            {
                cwkEnviaHorarioVerao = result.FirstOrDefault();
            }
            if (cwkEnviaHorarioVerao == null || String.IsNullOrEmpty(cwkEnviaHorarioVerao.Numserie))
            {
                equipamento.HorarioVeraoEnviar = null;
                equipamento.TemHorarioVeraoExportar = false;
                equipamento.DataUltimaExportacao = DateTime.Now;
                DadosCentralCliente.AtualizaDadosRepCentralCliente(ref equipamento, false, true, false, false);
                NovaSolicitacao(context, response, equipamentoRequisitando, ref equipamento);
            }
            else
            {
                equipamento.HorarioVeraoEnviar = cwkEnviaHorarioVerao;
                SolicitaEmpresa(equipamentoRequisitando, context, equipamento, response);
            }
        }
        /// <summary>
        /// Método que envia os dados de horario de verão para o equipamento
        /// Dados de Horário de verão fica "na empresa" no Ahgora, por isso busca os dados da empresa e depois so altera os dados do horário de verão (isso é feito para não perder as outras configurações que estava no equipamento)
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        private void EnviarHorarioVerao(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando, Equipamento equipamento)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            CfgEmpresa configEmpresa = jsonSerializer.Deserialize<CfgEmpresa>(equipamentoRequisitando.Requisicao);

            if (equipamento.HorarioVeraoEnviar != null)
            {
                configEmpresa.Empresa.HorarioVerao = "SIM";
                configEmpresa.Empresa.HorarioVeraoAutomatico = "SIM";
                configEmpresa.Empresa.DataInicioHV = equipamento.HorarioVeraoEnviar.DtInicioHorarioVerao.ToString("dd/MM/yyyy");
                configEmpresa.Empresa.DataFimHV = equipamento.HorarioVeraoEnviar.DtFimHorarioVerao.ToString("dd/MM/yyyy");

                string jsonResp = Newtonsoft.Json.JsonConvert.SerializeObject(configEmpresa);
                MontaResposta(equipamentoRequisitando, context, response, jsonResp);
            }
            else
            {
                NovaSolicitacao(context, response, equipamentoRequisitando, ref equipamento);
            }
        }
        /// <summary>
        /// Método que busca o pedido para mandar data e hora na central do cliente para o equipamento
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        private void BuscarDataHoraEnviar(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando, Equipamento equipamento)
        {
            string sql = @"select	top(1) 
		                            r.numserie,
		                            ecdh.id IdEnvioHorarioVerao
                            from rep r
                            inner join envioconfiguracoesdatahora ecdh on r.id = ecdh.idRelogio
                            where ecdh.bEnviaDataHoraServidor = 1
                              and r.numserie = '" + equipamento.NumSerie + "' order by ecdh.id";
            CwkEnviaDataHora cwkEnviaDataHora = null;
            var result = ExecutaComandosSql.LerDados<CwkEnviaDataHora>(BLLPonto.CriptoString.Decrypt(equipamento.Conexao), sql);
            if (result != null)
            {
                cwkEnviaDataHora = result.FirstOrDefault();
            }
            if (cwkEnviaDataHora == null || String.IsNullOrEmpty(cwkEnviaDataHora.Numserie))
            {
                equipamento.DataHoraEnviar = null;
                equipamento.TemDataHoraExportar = false;
                equipamento.DataUltimaExportacao = DateTime.Now;
                DadosCentralCliente.AtualizaDadosRepCentralCliente(ref equipamento, true, false, false, false);
                NovaSolicitacao(context, response, equipamentoRequisitando, ref equipamento);
            }
            else
            {
                equipamento.DataHoraEnviar = cwkEnviaDataHora;
                EnviarDataHora(context, response, equipamentoRequisitando, equipamento);
            }
        }
        /// <summary>
        /// Método que envia Data Hora para o equipamento
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        private void EnviarDataHora(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando, Equipamento equipamento)
        {
            if (equipamento.DataHoraEnviar != null)
            {
                CfgData cfgData = new CfgData();
                TimeZoneInfo timeZoneInfo = null;
                if (string.IsNullOrEmpty(equipamento.DataHoraEnviar.IdTimeZoneInfo))
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                }
                else
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(equipamento.DataHoraEnviar.IdTimeZoneInfo);
                }

                AguardaHoraSemSegundos(timeZoneInfo);
                DateTime dataUniversal = DateTime.Now.ToUniversalTime();
                dataUniversal = DateTime.Now.ToUniversalTime();
                cfgData.data = TimeZoneInfo.ConvertTimeFromUtc(dataUniversal, timeZoneInfo).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); ;
                string jsonResp = Newtonsoft.Json.JsonConvert.SerializeObject(cfgData);
                MontaResposta(equipamentoRequisitando, context, response, jsonResp);
            }
            else
            {
                NovaSolicitacao(context, response, equipamentoRequisitando,ref equipamento);
            }
        }

        private static void AguardaHoraSemSegundos(TimeZoneInfo timeZoneInfo)
        {
            DateTime dataUniversal = DateTime.Now.ToUniversalTime();
            DateTime dataAtual = TimeZoneInfo.ConvertTimeFromUtc(dataUniversal, timeZoneInfo);
            int segundos = dataAtual.Second;
            int esperaParaMinutoCheio = 60 - segundos;
            System.Threading.Thread.Sleep(esperaParaMinutoCheio * 1000);
        }
        /// <summary>
        /// Método que recebe o contexto da requisição, pega a requisição e retorna o Json da Requisição
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <returns>Retorna Json da Requisição</returns>
        public string TrataRequisicao(HttpListenerContext context)
        {
            String requisicao = new StreamReader(context.Request.InputStream, Encoding.UTF8).ReadToEnd();
            String[] possiveisRequisicoes = new String[] { "{\"req\":", "{\"resp\":", "{\"nsr_ini\"", "{\"lista\":", "{\"Funcionario\":", "{\"REP\":}", "{\"" };
            foreach (string v in possiveisRequisicoes)
            {
                if (requisicao.Contains(v))
                {
                    int ini = requisicao.IndexOf(v);
                    int fin = requisicao.LastIndexOf("\"}");
                    int tamanho = (fin - ini);
                    if (tamanho > 0)
                    {
                        requisicao = requisicao.Substring(ini, (tamanho + 2));
                    }
                    else
                    {
                        requisicao = "";
                    }
                    break;
                }
            }
            return requisicao;
        }
        /// <summary>
        /// Método para mandar uma responta de "Dormir" para o equipamento.
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        public void MandaDormir(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("cmd", "dorme");
            int tempo = equipamentoRequisitando.TempoDormir;
            if (tempo == 0)
            {
                tempo = 60;
            }
            dic.Add("tempo", tempo.ToString());
            var responseString = new JavaScriptSerializer().Serialize(dic);

            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            equipamentoRequisitando.Retorno = responseString;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            context.Response.Close();
        }
        /// <summary>
        /// Método para montar a resposta com a solicitação dos dados da empresa
        /// </summary>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        public void SolicitaEmpresa(EquipamentoRequisicao equipamentoRequisitando, HttpListenerContext context, Equipamento equipamento, HttpListenerResponse response)
        {
            Dictionary<string, string> resp = new Dictionary<string, string>();
            resp.Add("cmd", "empresa");
            MontaResposta(equipamentoRequisitando, context, response, resp);
        }
        /// <summary>
        ///  Método para montar a resposta com a solicitação do número do último NSR do equipamento
        /// </summary>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        public void SolicitaNSR(EquipamentoRequisicao equipamentoRequisitando, HttpListenerContext context, Equipamento equipamento, HttpListenerResponse response)
        {
            Dictionary<string, string> resp = new Dictionary<string, string>();
            resp.Add("cmd", "NSR");
            MontaResposta(equipamentoRequisitando, context, response, resp);
        }
        /// <summary>
        /// Método que verifica se o último NSR importado é menor que o ultimo NSR do Equipamento, se for pede a faixa do NSR se não verifica dados de exportação. 
        /// Caso não tenha NSR para importar ou dados para exportar (Empresa, Funcionário, Data e Horário de verão) manda o equipamento dormir.
        /// </summary>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        private void NovaSolicitacao(HttpListenerContext context, HttpListenerResponse response, EquipamentoRequisicao equipamentoRequisitando,ref Equipamento equipamento)
        {
                if (equipamento.ReqNSR == null)
                {
                    SolicitaNSR(equipamentoRequisitando, context, equipamento, response);
                }
                else
                if (((DateTime.Now - equipamento.DataUltimaImportacao.GetValueOrDefault()).TotalMinutes >= 1) ||
                     ((DateTime.Now - equipamento.DataUltimaExportacao.GetValueOrDefault()).TotalMinutes >= 1))
                {
                    //Atualiza os dados basicos na central do cliente
                    DadosCentralCliente.AtualizaDadosRepCentralCliente(ref equipamento);
                    //Busca os dados atualizados
                    DadosCentralCliente dbCentralCliente = new DadosCentralCliente();
                    Equipamento equipamentoAtualizado = dbCentralCliente.BuscaEquipamentoCentralCliente(equipamento.NumSerie);
                    Equipamento.CopyEquipamento(ref equipamento, equipamentoAtualizado);
                    
                    if (equipamento.TemEmpresaExportar)
                    {
                        SolicitaEmpresa(equipamentoRequisitando, context, equipamento, response);
                    }
                    else
                    if (equipamento.TemHorarioVeraoExportar)
                    {
                        BuscarHorarioVeraoEnviar(context, response, equipamentoRequisitando, equipamento);
                    }
                    if (equipamento.TemDataHoraExportar)
                    {
                        BuscarDataHoraEnviar(context, response, equipamentoRequisitando, equipamento);
                    }
                    else
                    if (equipamento.TemFuncionarioExportar)
                    {
                        BuscarFuncionariosEnviar(context, response, equipamentoRequisitando, equipamento);
                    }
                    else
                    if (equipamento.ReqNSR.Nsr > equipamento.UltimoNSR)
                    {
                        SolicitaAFD(equipamentoRequisitando, context, equipamento, response, equipamento.UltimoNSR, equipamento.ReqNSR.Nsr);
                    }
                    else
                    {
                        MandaDormir(context, response, equipamentoRequisitando);
                    }
                }
                else
                {
                    if (equipamento.ReqNSR.Nsr > equipamento.UltimoNSR)
                    {
                        SolicitaAFD(equipamentoRequisitando, context, equipamento, response, equipamento.UltimoNSR, equipamento.ReqNSR.Nsr);
                    } else
                    {
                        MandaDormir(context, response, equipamentoRequisitando);
                    }
                }
        }
        /// <summary>
        ///  Método para montar a resposta com a solicitação do AFD do equipamento (Retorna no máximo 1000 NSR por vez)
        /// </summary>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="equipamento">Objeto com os dados do equipamento que esta fazendo a requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="ini">NSR Inicial da Solicitação</param>
        /// <param name="fin">NSR Final da Solicitação</param>
        public void SolicitaAFD(EquipamentoRequisicao equipamentoRequisitando, HttpListenerContext context, Equipamento equipamento, HttpListenerResponse response, int ini, int fin)
        {   
            Dictionary<string, string> resp = new Dictionary<string, string>();
            resp.Add("cmd", "AFD");
            resp.Add("nsr_ini", ini.ToString());
            resp.Add("nsr_fim", fin.ToString());
            MontaResposta(equipamentoRequisitando, context, response, resp);
        }
        /// <summary>
        /// Método Responsável por formatar a resposta para o equipamento.
        /// </summary>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="resp">Dictionary que será convertido em Json</param>
        private void MontaResposta(EquipamentoRequisicao equipamentoRequisitando, HttpListenerContext context, HttpListenerResponse response, Dictionary<string, string> resp)
        {
            string responseString = "";
            responseString = new JavaScriptSerializer().Serialize(resp);
            equipamentoRequisitando.Retorno = responseString;
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            context.Response.Close();
        }
        /// <summary>
        /// Método Responsável por formatar a resposta para o equipamento.
        /// </summary>
        /// <param name="equipamentoRequisitando">Objeto com os dados da requisição do equipamento</param>
        /// <param name="context">Contexto da requisição</param>
        /// <param name="response">Resposta da Requisição</param>
        /// <param name="resp">Json com a solicitação para o rep</param>
        private void MontaResposta(EquipamentoRequisicao equipamentoRequisitando, HttpListenerContext context, HttpListenerResponse response, string resp)
        {
            equipamentoRequisitando.Retorno = resp;
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(resp);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            context.Response.Close();
        }
        /// <summary>
        /// Método que adiciona a linha de Header no AFD retornado pelo Ahgora.
        /// </summary>
        /// <param name="equipamento">Objeto com os dados do equipamento para poder pegar os dados e montar o Header</param>
        /// <param name="repAFD">Lista com os registros do afd onde será incluído o Header</param>
        public void AdicionaHeader(Equipamento equipamento, RepAFD repAFD)
        {
            string header = "0000000001";
            header += equipamento.ReqEmpresa.Empresa.Tipo;
            header += equipamento.ReqEmpresa.Empresa.CNPJouCPF.Replace("-", "").Replace(".", "").Replace("/", "").PadLeft(14, '0');
            header += equipamento.ReqEmpresa.Empresa.CEI.Replace("-", "").Replace(".", "").Replace("/", "").PadLeft(12, '0');
            header += equipamento.ReqEmpresa.Empresa.RazaoSocial.PadRight(150, ' ');
            header += equipamento.NumSerie.PadRight(17, '0');
            header += equipamento.NumSerie.PadRight(17, '0');
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("HHmm");
            repAFD.registros.Insert(0, header);
        }
    }
}
