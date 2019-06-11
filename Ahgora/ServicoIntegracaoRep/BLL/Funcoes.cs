using ServicoIntegracaoRep.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BllPonto = BLL;

namespace ServicoIntegracaoRep.BLL
{
    public class Funcoes
    {
        /// <summary>
        /// Trata possíveis erros na requisição.
        /// </summary>
        /// <param name="equipamentoRequisitando">Dados da Requisição do equipamento</param>
        /// <param name="equipamento">Equipamento que esta realizando a requisição</param>
        public static void TrataErros(EquipamentoRequisicao equipamentoRequisitando, Equipamento equipamento)
        {
            // Se o equipamento não existir ou não estiver ativo adiciono o "Erro" e Coloco para dormir por 10 minutos
            if (equipamento == null || equipamento.Id == 0)
            {
                equipamentoRequisitando.Erro = "Equipamento não encontrado na base de dados da Central do Cliente da Employer";
                equipamentoRequisitando.TempoDormir = 60;
                if (equipamento.ReqEmpresa != null)
                {
                    Funcoes.EnviaErroEquipamentoEmail(equipamento, equipamentoRequisitando, equipamentoRequisitando.Erro, "");
                }
            }
            else if (!equipamento.Ativo)
            {
                equipamentoRequisitando.Erro = "Equipamento não esta ativo para importação";
                equipamentoRequisitando.TempoDormir = 60;
            }
            else if (equipamento.Processando)
	        {
                equipamentoRequisitando.Erro = "Requisição Descartada. Existe processamento de outra requisição no momento";
            }
        }

        /// <summary>
        /// Método que recebe a requisição recebida do equipamento e atualiza o mesmo na lista de requisições
        /// (Se a requisição já existe na lista, recupero, caso não exista, adiciona)
        /// </summary>
        /// <param name="equipRequisitando">Objeto com a requisição.</param>
        /// <param name="LEquipRequisicao">Lista com as requisições já adicionadas</param>
        public static void AtualizaListaDeRequisicoes(EquipamentoRequisicao equipRequisitando, ref List<EquipamentoRequisicao> LEquipRequisicao)
        {
            if (LEquipRequisicao.Where(e => e.RequisicoesExecucaoAtual == equipRequisitando.RequisicoesExecucaoAtual && e.NumSerie == equipRequisitando.NumSerie).Count() > 0)
            {
                EquipamentoRequisicao equiAtualizar = LEquipRequisicao.Where(e => e.RequisicoesExecucaoAtual == equipRequisitando.RequisicoesExecucaoAtual && e.NumSerie == equipRequisitando.NumSerie).First();
                equiAtualizar = equipRequisitando;
            }
            else
            {
                LEquipRequisicao.Add(equipRequisitando);
            }
        }

        /// <summary>
        /// Método para enviar erro por e-mail com os detalhes do equipamento que gerou o erro.
        /// </summary>
        /// <param name="equipamento">Equipamento que fez a chamada que gerou o erro.</param>
        /// <param name="erro">Mensagem de Erro</param>
        /// <param name="detalhesErro">Detalhes sobre o erro</param>
        public static void EnviaErroEquipamentoEmail(Equipamento equipamento, EquipamentoRequisicao equipRequisitando, string erro, string detalhesErro)
        {
            try
            {
                string conteudoEmail = @"<p><span><strong>Número de Série do Rep: </strong>{NumSerie} </span></p>
                                                <p><span><strong>Empresa: </strong>{Empresa}</span></p>
                                                <p><span><strong>Data da Ocorrência: </strong>{Data}</span></p>
                                                <p><span><strong>Descrição do Erro: </strong>{Message}</span></p>
                                                <p><span><strong>StackTrace: </strong>{StackTrace}</span></p>
                                                <p><span><strong>Detalhes da Empresa do Equipamento: </strong>{DetalhesEmpresa}</span></p>";


                                                
                String detEmpresa = "";
                if (equipamento.ReqEmpresa != null && equipamento.ReqEmpresa.Empresa != null)
                {
                    detEmpresa = "Nome: " + equipamento.ReqEmpresa.Empresa.RazaoSocial
                                    + " CNPJ/CPF: " + equipamento.ReqEmpresa.Empresa.CNPJouCPF
                                    + " Local: " + equipamento.ReqEmpresa.Empresa.Local;
                }

                conteudoEmail = conteudoEmail.Replace("{NumSerie}", equipamento.NumSerie)
                                .Replace("{Empresa}", equipamento.Empresa)
                                .Replace("{Data}", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"))
                                .Replace("{Message}", erro)
                                .Replace("{StackTrace}", detalhesErro)
                                .Replace("{DetalhesEmpresa}", detEmpresa);

                
                if (equipRequisitando != null)
                {
                    conteudoEmail += @"</br>
                                        </br>
                                        <p><span><strong>Json da Requisição: </strong>{Req}</span></p>
                                        <p><span><strong>Json da Resposta: </strong>{Rep}</span></p>";
                    conteudoEmail = conteudoEmail.Replace("{Req}", equipRequisitando.Requisicao)
                                    .Replace("{Rep}", equipRequisitando.Retorno);
                }
                ServEnvioEmailPonteAzul.EnviaEmailClient enviaEmail = new ServEnvioEmailPonteAzul.EnviaEmailClient();
                enviaEmail.EnviaEmail("no-reply@employer.com.br", "diegoherrera@employer.com.br", "Erro Serviço de Integração no Rep " + equipamento.NumSerie + " da empresa " + equipamento.Empresa + " as " + DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), conteudoEmail,
                    "", null, "", "", "", "", "", null);
            }
            catch (Exception)
            {
            }
        }

        public static IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
