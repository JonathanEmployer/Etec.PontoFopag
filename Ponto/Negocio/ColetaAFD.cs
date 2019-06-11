using cwkPontoMT.Integracao;
using cwkPontoMT.Integracao.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Negocio
{
    public class ColetaAFD : ComunicacaoRelogio
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ColetaAFD(ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config, DateTime dataHoraComando)
            : base(rep, config, dataHoraComando)
        {
            tituloLog = "Coleta AFD";
            SetRelogio();
            Empresa empresa = new Empresa();
            empresa.CEI = rep.CEI;
            empresa.Documento = rep.CpfCnpjEmpregador;
            empresa.Local = rep.EnderecoEmpregador;
            empresa.RazaoSocial = rep.NomeEmpregador;
            empresa.TipoDocumento = rep.CpfCnpjEmpregador.Length < 14 ? TipoDocumento.CPF : TipoDocumento.CNPJ;
            relogio.SetEmpresa(empresa);
        }

        protected override void SetDadosEnvio()
        {
            throw new NotImplementedException();
        }

        protected override void EfetuarEnvio()
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, string> EfetuarEnvio(ref string caminho, System.IO.DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }

        public List<RegistroAFD> ImportarAFDRep()
        {
            string comando = "Importação de Bilhetes";
            log.Info(rep.NumSerie + " Iniciando importação de bilhetes");
            try
            {
                List<RegistroAFD> registros = new List<RegistroAFD>();
                string complemento = "";
                if (rep.UltimoNSR == 0)
                {
                    complemento = "no período de " + rep.DataInicioImportacao.ToString("dd/MM/yyyy") + " a " + DateTime.Now.ToString("dd/MM/yyyy");
                    Log.EnviarLogApi(rep, comando, "Coletando Dados " + complemento, "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                    registros = relogio.GetAFD(rep.DataInicioImportacao, DateTime.Now);
                }
                else
                {
                    complemento = "a partir do NSR " + rep.UltimoNSR + " (Data Início = "+rep.DataInicioImportacao.ToString("dd/MM/yyyy HH:mm")+")";
                    Log.EnviarLogApi(rep, comando, "Coletando Dados " + complemento, "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                    registros = relogio.GetAFDNsr(rep.DataInicioImportacao, DateTime.Now, (int)rep.UltimoNSR + 1, int.MaxValue, false);
                }

                int qtdLidos = registros.Where(w => w.Campo01 != "000000000" && w.Campo01 != "999999999").Count();
                log.Info(rep.NumSerie + " Total de bilhetes coletados: " + qtdLidos);
                if (qtdLidos > 0)
                {
                    Log.EnviarLogApi(rep, comando, qtdLidos + " Registro(s) coletado(s) com sucesso, coleta baseada " + complemento, "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                }
                else
                {
                    log.Info(rep.NumSerie + " Não foram encontrados novos registros");
                    Log.EnviarLogApi(rep, comando, "Não foram encontrados novos registros, coleta baseada " + complemento, "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                }
                return registros;
            }
            catch (Exception ex)
            {
                log.Info(rep.NumSerie + " Erro ao coletar bilhete, erro: " + ex.Message);
                Log.EnviarLogApi(rep, comando, "Erro ao coletar bilhete, erro: " + ex.Message, "Detalhes = "+ex.Message, Modelo.Enumeradores.SituacaoLog.Informacao);
                return new List<RegistroAFD>();
            }
        }

        public cwkPontoMT.Integracao.ResultadoImportacao EnviarAfdServidor(List<RegistroAFD> registros)
        {
            string comando = "Enviar AFD";
            log.Info(rep.NumSerie + " Recebendo token API");
            ComunicacaoApi comApi = new ComunicacaoApi(config.TokenAccess);
            log.Info(rep.NumSerie + " Enviando linhas do AFD para o servidor");
            cwkPontoMT.Integracao.ResultadoImportacao result = comApi.EnviarLinhasAfdServidor(registros).Result;
            StringBuilder res = new StringBuilder();
            log.Info(rep.NumSerie + " AFD processado");
            res.Append(" Afd processado, resultado: ");
            res.Append(" Registros processados = "+result.Resumo.RegistroProcessado);
            res.Append("; não utilizados = "+result.Resumo.RegistroNaoUtilizadoPeloSistema);
            res.Append("; duplicados = "+result.Resumo.RegistroDuplicado);
            res.Append("; ponto/b.h. fechado = " + result.Resumo.PontoFechado);
            res.Append("; funcionários não encotrados = " + result.Resumo.FuncNaoEncontrado);
            res.Append("; funcionários demitidos = " + result.Resumo.FuncDemitido);
            res.Append("; funcionários excluídos = " + result.Resumo.FuncExcluido);
            res.Append("; funcionários inativos = " + result.Resumo.FuncInativo);
            res.Append("; funcionários não selecionados para importação = " + result.Resumo.FuncNaoSelecionadoParaImportacao);
            log.Info(rep.NumSerie + " Serializando dados para JSON");
            string jsonRetorno = JsonConvert.SerializeObject(result.RegistrosAFD);    
            Log.EnviarLogApi(rep, comando, res.ToString(), " Retorno AFD = "+jsonRetorno, Modelo.Enumeradores.SituacaoLog.Informacao);
            return result;
        }

        protected override void SetDadosReceber()
        {
            throw new NotImplementedException();
        }

        protected override void EfetuarRecebimento(ComunicacaoApi comApi)
        {
            throw new NotImplementedException();
        }
    }
}
