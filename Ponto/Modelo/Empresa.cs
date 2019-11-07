using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo
{
    public class Empresa : Modelo.ModeloBase
    {
        public Empresa()
        {
            this.Id = 0;
            this.Codigo = 0;
            this.Incdata = new DateTime();
            this.Inchora = new DateTime();
            this.Validade = DateTime.MaxValue;
            this.bloqueioEdicaoEmp = 0;
        }
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Indica se essa é a empresa principal
        /// </summary>
        [Display(Name = "Principal")]
        public bool bPrincipal { get; set; }
        /// <summary>
        /// Tipo da licença
        /// 0 = Demostração - somente algumas funcionalidades;
        /// 1 = Empresa - funcionarios ilimitados por empresa;
        /// 2 = Funcionarios - numero de funcionarios limitados pela variavel quantidade;
        /// </summary>
        [Display(Name = "Tipo Licença")]
        public short TipoLicenca { get; set; }
        /// <summary>
        /// Quantidade de funcionarios para aquela versão
        /// 0 - se tipo de licença for por empresa
        /// </summary>
        public int Quantidade { get; set; }
        /// <summary>
        /// Nome da Empresa
        /// </summary> 
        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }
        /// <summary>
        /// Endereço da Empresa
        /// </summary>
        [TableHTMLAttribute("Endereço", 4, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }
        /// <summary>
        /// Cidade da Empresa
        /// </summary>
        [TableHTMLAttribute("Cidade", 5, true, ItensSearch.text, OrderType.none)]
        public string Cidade { get; set; }
        /// <summary>
        /// Estado da Empresa
        /// </summary>
        [TableHTMLAttribute("Estado", 8, true, ItensSearch.select, OrderType.none)]
        public string Estado { get; set; }
        /// <summary>
        /// CEP da Empresa
        /// </summary>
        [TableHTMLAttribute("Cep", 6, true, ItensSearch.text, OrderType.none)]
        public string Cep { get; set; }
        /// <summary>
        /// CNPJ da Empresa
        /// </summary>
        public string Cnpj { get; set; }
        /// <summary>
        /// CPF da Empresa
        /// </summary>
        public string Cpf { get; set; }
        /// <summary>
        /// CEI da Empresa
        /// </summary>
        [TableHTMLAttribute("CEI", 7, true, ItensSearch.text, OrderType.none)]
        public string CEI { get; set; } //Cadastro especifico do INSS
        /// <summary>
        /// Código da Chave para verificar se o Software foi copiado ou não
        /// </summary>
        public string Chave { get; set; }
        /// <summary>
        /// Número de série do software
        /// </summary>
        [Display(Name = "Número Série")]
        public string Numeroserie { get; set; }
        /// <summary>
        /// Indica que o banco de dados foi alterado fora do software do Ponto.
        /// </summary>
        [Display(Name = "Alterado")]
        public bool BDAlterado { get; set; }

        public bool Bloqueiousuarios { get; set; }
        public bool Relatorioabsenteismo { get; set; }
        public bool ExportacaoHorasAbonadas { get; set; }
        public bool ModuloRefeitorio { get; set; }
        public DateTime? Validade { get; set; }
        public string UltimoAcesso { get; set; }

        /// DiaFechamento
        [TableHTMLAttribute("Fechamento Inicial", 9, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Dia inicial")]
        [Range(0, 31, ErrorMessage = "A data inicial deve possuir valores entre 0 e 31")]
        public int DiaFechamentoInicial { get; set; }
        [TableHTMLAttribute("Fechamento Final", 10, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Dia final")]
        [Range(0, 31, ErrorMessage = "O dia de fechamento deve possuir valores entre 0 e 31")]
        public int DiaFechamentoFinal { get; set; }
        public EmpresaLogo EmpresaLogo { get; set; }

        [TableHTMLAttribute("CNPJ/CPF", 3, true, ItensSearch.text, OrderType.none)]
        public string CnpjCpf
        {
            get
            {
                if (String.IsNullOrEmpty(Cnpj))
                {
                    return this.Cpf;
                }
                else
                {
                    return this.Cnpj;
                }
            }
        }

        /// <summary>
        /// Gera o Código da Chave
        /// </summary>
        /// <returns></returns>
        public string ToMD5()
        {
            int bdalterado = this.BDAlterado ? 1 : 0;
            return "cwork" + Nome + Endereco + bPrincipal + Cidade + "sistemas" + Estado + TipoLicenca + Cep + Cnpj + "ltda" + String.Format("{0:dd/MM/yyyy}", Incdata) + String.Format("{0:HH:mm:ss}", Inchora) + Quantidade + Numeroserie + bdalterado + "me"
                + (Bloqueiousuarios ? Convert.ToInt16(Bloqueiousuarios).ToString() : "")
                + (Relatorioabsenteismo ? Convert.ToInt16(Relatorioabsenteismo).ToString() : "")
                + (ExportacaoHorasAbonadas ? Convert.ToInt16(ExportacaoHorasAbonadas).ToString() : "")
                + (ModuloRefeitorio ? Convert.ToInt16(ModuloRefeitorio).ToString() : "");

        }

        private string ToMD5ComValidade()
        {
            string hash = ToMD5() + "validade" + (Validade.HasValue ? Validade.Value : DateTime.MaxValue);
            return hash;
        }

        public string HashMD5ComValidade()
        {
            return MD5HashGenerator.GenerateKey(this.ToMD5ComValidade());
        }

        public string ToMD5ComRelatoriosValidacaoNova()
        {
            string hash = ToMD5() + (relatorioInconsistencia ? Convert.ToInt16(relatorioInconsistencia).ToString() : "0")
                                  + (relatorioComparacaoBilhetes ? Convert.ToInt16(relatorioComparacaoBilhetes).ToString() : "0");
            return hash;
        }

        public string ToMD5ComRelatoriosValidacaoAntiga()
        {
            string hash = ToMD5() + (relatorioInconsistencia ? Convert.ToInt16(relatorioInconsistencia).ToString() : "0");
            return hash;
        }

        public string HashMD5ComRelatoriosValidacaoNova()
        {
            return MD5HashGenerator.GenerateKey(this.ToMD5ComRelatoriosValidacaoNova());
        }

        /// <summary>
        /// Verifica o Código da Chave
        /// </summary>
        /// <returns></returns>
        public bool EmpresaOK()
        {
            try
            {
                string auxRelatoriosValidacaoNova = HashMD5ComRelatoriosValidacaoNova();
                string auxRelatorioValidacaoAntiga = MD5HashGenerator.GenerateKey(this.ToMD5ComRelatoriosValidacaoAntiga());
                string auxComValidade = HashMD5ComValidade();
                string auxSemValidade = MD5HashGenerator.GenerateKey(this.ToMD5());

                if (auxRelatoriosValidacaoNova == this.Chave && !this.BDAlterado)
                {
                    return true;
                }
                if (auxRelatorioValidacaoAntiga == this.Chave && !this.BDAlterado)
                {
                    return true;
                }
                if (auxComValidade == this.Chave && !this.BDAlterado)
                {
                    return true;
                }
                if (auxSemValidade == this.Chave && !this.BDAlterado)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int IDRevenda { get; set; }

        public bool Selecionado { get; set; }

        public string nomeCodigo { get; set; }
        public bool UtilizaControleContratos { get; set; }

        public bool relatorioInconsistencia { get; set; }
        public bool utilizaregistradorfunc { get; set; }

        public bool relatorioComparacaoBilhetes { get; set; }
        public int? IdIntegracao { get; set; }


        [Display(Name = "Permite Class. H.Extras Painel")]
        public bool PermiteClassHorasExtrasPainel { get; set; }
        [TableHTMLAttribute("Permite Class. H.Extras Painel", 11, true, ItensSearch.text, OrderType.none)]
        public string PermiteClassHorasExtrasPainelDescricao
        {
            get
            {
                string permiteClassHorasExtrasPainelDescricao;
                switch (PermiteClassHorasExtrasPainel)
                {
                    case true: permiteClassHorasExtrasPainelDescricao = "Sim";
                        break;
                    default: permiteClassHorasExtrasPainelDescricao = "Não";
                        break;
                }
                return permiteClassHorasExtrasPainelDescricao;
            }

        }

        [Display(Name = "Bloquear Justificativas Fora do Período")]
        public bool BloqueiaJustificativaForaPeriodo { get; set; }
        [TableHTMLAttribute("Bloquear Just. Fora Período", 12, true, ItensSearch.select, OrderType.none)]
        public string BloqueiaJustificativaForaPeriodoDescricao
        {
            get
            {
                string bloqueiaJustificativaForaPeriodoDescricao;
                switch (BloqueiaJustificativaForaPeriodo)
                {
                    case true: bloqueiaJustificativaForaPeriodoDescricao = "Sim";
                        break;
                    default: bloqueiaJustificativaForaPeriodoDescricao = "Não";
                        break;
                }
                return bloqueiaJustificativaForaPeriodoDescricao;
            }

        }

        [Display(Name = "Limitar qtd. de abono a qtd. falta")]
        public bool LimitarQtdAbono { get; set; }
        [TableHTMLAttribute("Limitar qtd. abono", 16, true, ItensSearch.select, OrderType.none)]
        public string LimitarQtdAbonoDescricao
        {
            get
            {
                string limitarQtdAbonoDescricao;
                switch (LimitarQtdAbono)
                {
                    case true: limitarQtdAbonoDescricao = "Sim";
                        break;
                    default: limitarQtdAbonoDescricao = "Não";
                        break;
                }
                return limitarQtdAbonoDescricao;
            }
        }
        [TableHTMLAttribute("Justificativa Inicial", 13, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Início do Período de Justificativas")]
        [Range(0, 31, ErrorMessage = "A data inicial de justificativas deve possuir valores entre 0 e 31")]
        public int DtInicioJustificativa { get; set; }
        [TableHTMLAttribute("Justificativa Final", 14, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Fim do Período de Justificativas")]
        [Range(0, 31, ErrorMessage = "A data final de justificativas deve possuir valores entre 0 e 31")]
        public int DtFimJustificativa { get; set; }
        [Display(Name = "Horário padrão para integração de Funcionários")]
        public string NomeHorario { get; set; }
        public int? IdHorarioPadraoFunc { get; set; }
        [Display(Name = "Tipo Horário")]
        public int TipoHorarioPadraoFunc { get; set; }

        [Display(Name = "Permite Abono Parcial Painel")]
        public bool PermiteAbonoParcialPainel { get; set; }
        [TableHTMLAttribute("Permite Abono Parcial Painel", 15, true, ItensSearch.text, OrderType.none)]
        public string PermiteAbonoParcialPainelDescricao
        {
            get
            {
                string PermiteAbonoParcialPainelDescricao;
                switch (PermiteAbonoParcialPainel)
                {
                    case true: PermiteAbonoParcialPainelDescricao = "Sim";
                        break;
                    default: PermiteAbonoParcialPainelDescricao = "Não";
                        break;
                }
                return PermiteAbonoParcialPainelDescricao;
            }
        }

        [Display(Name = "Utiliza App Pontofopag")]
        public bool UtilizaAppPontofopag { get; set; }

        [Display(Name = "Reconhecimento Facial")]
        public bool UtilizaReconhecimentoFacilAppPontofopag { get; set; }
        [Display(Name = "Utiliza Web App Pontofopag")]
        public bool UtilizaWebAppPontofopag { get; set; }

        [Display(Name = "Reconhecimento Facial")]
        public bool UtilizaReconhecimentoFacilWebAppPontofopag { get; set; }
        public bool TermoAppAlterado { get; set; }
        public List<EmpresaTermoUso> TermosUso { get; set; }

        public int bloqueioEdicaoEmp { get; set; }
    }
}
