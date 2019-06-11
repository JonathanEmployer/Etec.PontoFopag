using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoIntegracaoRep.Modelo
{
    /// <summary>
    /// Modelo que guarda as informações do/para o equipamento que esta fazendo requisições para o serviço
    /// </summary>
    public class Equipamento
    {
        public int Id { get; set; }
        public int IdRep { get; set; }
        public int Codigo { get; set; }
        public String NumSerie { get; set; }
        public String Local { get; set; }
        public String NumRelogio { get; set; }
        public Int16 Relogio { get; set; }
        public int IdCliente { get; set; }
        public int IdEmpresa { get; set; }
        public bool TemDataHoraExportar { get; set; }
        public bool TemHorarioVeraoExportar { get; set; }
        public bool TemEmpresaExportar { get; set; }
        public bool TemFuncionarioExportar { get; set; }
        public int UltimoNSR { get; set; }
        /// <summary>
        /// Data em que ocorreu a última importação de bilhetes no sistema.
        /// </summary>
        public DateTime? DataUltimaImportacao { get; set; }
        /// <summary>
        /// Data em que ocorreu o último envio de dados para o equipamento (Dados de Funcionário, Empresa, Data/Hora e configuração)
        /// </summary>
        public DateTime? DataUltimaExportacao { get; set; }
        /// <summary>
        /// Tempo que vai mandar o equipamento "Dormir" para a próxima requisição
        /// </summary>
        public int TempoDormir { get; set; }
        /// <summary>
        /// Guarda a quantidade de requisições que o equipamento já fez para a aplicação desde quando ele foi configurado.
        /// </summary>
        public int TotalDeRequisicoes { get; set; }
        /// <summary>
        /// Guarda a quantidade de requisições que o equipamento fez desde quando o programa foi executado.
        /// </summary>
        public int RequisicoesExecucaoAtual { get; set; }
        public DateTime? DataPrimeiraImportacao { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataUltimaRequisicao { get; set; }
        /// <summary>
        /// String com o nome da empresa ao qual o equipamento pertence.
        /// </summary>
        public string Empresa { get; set; }
        /// <summary>
        /// Conexão do banco ao qual esse equipamento pertence.
        /// </summary>
        public string Conexao { get; set; }
        /// <summary>
        /// Guarda dos dados da empresa do relógio (Dados são obtidos por uma Requisição ReqEmpresa para o relógio.)
        /// </summary>
        public ReqEmpresa ReqEmpresa { get; set; }
        /// <summary>
        /// Controla se existe um processamento ocorrendo para aquele equipamento, pois caso acontece um processamento demorado e o relógio fizer outra chamanda enquanto esse esquipamento estiver processando, descarto essa requisição.
        /// </summary>
        public bool Processando { get; set; }
        /// <summary>
        /// Controla a última requisição que foi salva no banco
        /// </summary>
        public int UltimaRequisicaoSalva { get; set; }
        /// <summary>
        /// Guarda os dados da ultima Requisição de NSR solicitada ao equipamento (Método "\"req\":\"AFD\"")
        /// </summary>
        public ReqNSR ReqNSR { get; set; }
        /// <summary>
        /// Data apartir da qual deve iniciar a importação do bilhete
        /// </summary>
        public DateTime? DataInicioImportacao { get; set; }
        /// <summary>
        /// Dados da Empresa Enviada para o Equipamento
        /// </summary>
        public CwkEmpresa cwkEmpresa { get; set; }
        /// <summary>
        /// Funcionários a serem enviados para o equipamento
        /// </summary>
        public List<Funcionario> FuncionariosEnviar { get; set; }
        /// <summary>
        /// Funcionário que esta sendo enviado para o equipamento
        /// </summary>
        public Funcionario FuncionarioEnviando { get; set; }
        /// <summary>
        /// Funcionários a serem excluídos do equipamento
        /// </summary>
        public ExcluirFuncionarios ExcluirFuncionarios { get; set; }
        /// <summary>
        /// Horário de Verão a ser enviado para o equipamento
        /// </summary>
        public CwkEnviaHorarioVerao HorarioVeraoEnviar { get; set; }
        /// <summary>
        /// Data e hora a ser enviado para o equipamento
        /// </summary>
        public CwkEnviaDataHora DataHoraEnviar { get; set; }

        /// <summary>
        /// Passa os dados de equipamento novo (Atualizado) para um equipamento que possui referencia com a lista de equipamento
        /// </summary>
        /// <param name="equipamentoRef">Equipamento referenciado a lista de equipamentos</param>
        /// <param name="equipamentoAtualizado">Equipamento com os dados atualizado</param>
        public static void CopyEquipamento(ref Equipamento equipamentoRef, Equipamento equipamentoAtualizado)
        {
            equipamentoRef.Ativo = equipamentoAtualizado.Ativo;
            equipamentoRef.Codigo = equipamentoAtualizado.Codigo;
            equipamentoRef.Conexao = equipamentoAtualizado.Conexao;
            equipamentoRef.DataPrimeiraImportacao = equipamentoAtualizado.DataPrimeiraImportacao;
            equipamentoRef.DataUltimaExportacao = equipamentoAtualizado.DataUltimaExportacao;
            equipamentoRef.DataUltimaImportacao = equipamentoAtualizado.DataUltimaImportacao;
            equipamentoRef.DataUltimaRequisicao = equipamentoAtualizado.DataUltimaRequisicao;
            equipamentoRef.Empresa = equipamentoAtualizado.Empresa;
            equipamentoRef.Id = equipamentoAtualizado.Id;
            equipamentoRef.IdCliente = equipamentoAtualizado.IdCliente;
            equipamentoRef.IdEmpresa = equipamentoAtualizado.IdEmpresa;
            equipamentoRef.IdRep = equipamentoAtualizado.IdRep;
            equipamentoRef.Local = equipamentoAtualizado.Local;
            equipamentoRef.NumRelogio = equipamentoAtualizado.NumRelogio;
            equipamentoRef.Relogio = equipamentoAtualizado.Relogio;
            equipamentoRef.TemDataHoraExportar = equipamentoAtualizado.TemDataHoraExportar;
            equipamentoRef.TemEmpresaExportar = equipamentoAtualizado.TemEmpresaExportar;
            equipamentoRef.TemFuncionarioExportar = equipamentoAtualizado.TemFuncionarioExportar;
            equipamentoRef.TemHorarioVeraoExportar = equipamentoAtualizado.TemHorarioVeraoExportar;
            equipamentoRef.TempoDormir = equipamentoAtualizado.TempoDormir;
            equipamentoRef.UltimoNSR = equipamentoAtualizado.UltimoNSR;
        }
    }
}
