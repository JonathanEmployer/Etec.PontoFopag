using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class AFD : Modelo.ModeloBase
    {
        [Display(Name = "Registro")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [StringLength(400, ErrorMessage = "Número máximo de caracteres: {1}")]
        [DataTableAttribute()]
        public String LinhaAFD { get; set; }

        [Display(Name = "Orgão Fiscalizador")]
        [Required(ErrorMessage="Campo Obrigatório")]

        [DataTableAttribute()]
        public string OrgaoFiscalizador
        {
            get { return EnumOrgaoFiscalizador.ToString(); }
            set { EnumOrgaoFiscalizador = (EnumOrgaoFiscalizador)Enum.Parse(typeof(EnumOrgaoFiscalizador), value, true); }
        }

        public EnumOrgaoFiscalizador EnumOrgaoFiscalizador { get; set; }

        [Display(Name = "Situação")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [DataTableAttribute()]
        public EnumSituacaoAFD Situacao { get; set; }

        [Display(Name = "Observação")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [StringLength(300, ErrorMessage = "Número máximo de caracteres: {1}")]
        [DataTableAttribute()]
        public String Observacao { get; set; }

        [DataTableAttribute()]
        public Guid Identificador { get; set; }

        [DataTableAttribute()]
        public Guid Lote { get; set; }

        [DataTableAttribute()]
        public string Campo01 { get; set; }

        [DataTableAttribute()]
        public string Campo02 { get; set; }

        [DataTableAttribute()]
        public string Campo03 { get; set; }

        [DataTableAttribute()]
        public string Campo04 { get; set; }

        [DataTableAttribute()]
        public string Campo05 { get; set; }

        [DataTableAttribute()]
        public string Campo06 { get; set; }

        [DataTableAttribute()]
        public string Campo07 { get; set; }

        [DataTableAttribute()]
        public string Campo08 { get; set; }

        [DataTableAttribute()]
        public string Campo09 { get; set; }

        [DataTableAttribute()]
        public string Campo10 { get; set; }

        [DataTableAttribute()]
        public string Campo11 { get; set; }

        [DataTableAttribute()]
        public string Campo12 { get; set; }

        [DataTableAttribute()]
        public int Nsr { get; set; }

        [DataTableAttribute()]
        public String IpDnsRep { get; set; }

        [DataTableAttribute()]
        public String Relogio { get; set; }

        public int IdFuncionario { get; set; }

        [DataTableAttribute()]
        public DateTime? DataHora { get; set; }

        public string DescSituacaoRegistro()
        {
            switch (this.Situacao)
            {
                case EnumSituacaoAFD.NaoProcessado:
                    return "Registro não processado";
                case EnumSituacaoAFD.FuncNaoEncontrado:
                    return "Funcionário não encontrado";
                case EnumSituacaoAFD.FuncDemitido:
                    return "Funcionário demitido";
                case EnumSituacaoAFD.FuncInativo:
                    return "Funcionário inativo";
                case EnumSituacaoAFD.FuncExcluido:
                    return "Funcionário excluído";
                case EnumSituacaoAFD.UsuarioSemPermissao:
                    return "Usuário sem permissão";
                case EnumSituacaoAFD.RegistroProcessado:
                    return "Registro processado";
                case EnumSituacaoAFD.FuncNaoSelecionadoParaImportacao:
                    return "Funcionário não selecionado para importação";
                case EnumSituacaoAFD.RegistroNaoUtilizadoPeloSistema:
                    return "Registro não utilizado pelo sistema";
                case EnumSituacaoAFD.PontoFechado:
                    return "Ponto Fechado";
                case EnumSituacaoAFD.RegistroProcessadoAnteriormente:
                    return "Registro processado por outra importação";
                case EnumSituacaoAFD.RegistroAguardandoProcessamento:
                    return "Já existe esse registro na fila de importação adicionado por outro processo";
                case EnumSituacaoAFD.RepNaoCadastrado:
                    return "REP não está cadastrado no sistema";
                case EnumSituacaoAFD.EmpresaNaoEncontrada:
                    return "CNPJ do arquivo AFD não existe no cadastro de empresa";
                default:
                    return string.Empty;
            }
        }

        public void PreencheDataHora()
        {
            DateTime data = DateTime.MinValue;
            bool converteu = false;
            if (!String.IsNullOrEmpty(Campo03) && Campo03.Length == 8)// Portaria do AFD do MTE pula o campo 3 na doc, e a do Inmetro considera o campo 3 
            {
                string strData = Campo03.Substring(0, 2) + "/" + Campo03.Substring(2, 2) + "/" + Campo03.Substring(4, 4) + " " + Campo04.Substring(0, 2) + ":" + Campo04.Substring(2, 2);
                if (DateTime.TryParse(strData, out data))
                { converteu = true; }
            }

            if (!converteu && Campo04.Length == 8)// Portaria do AFD do MTE pula o campo 3 na doc, e a do Inmetro considera o campo 3 
            {
                string strData = Campo04.Substring(0, 2) + "/" + Campo04.Substring(2, 2) + "/" + Campo04.Substring(4, 4) + " " + Campo05.Substring(0, 2) + ":" + Campo05.Substring(2, 2);
                if (DateTime.TryParse(strData, out data))
                { converteu = true; }
            }
            DataHora = data;
        }
    }

    public enum EnumOrgaoFiscalizador
    {
         MTE,
         Inmetro,
         Outros
    }

    public enum EnumSituacaoAFD
    {
        NaoProcessado,
        FuncNaoEncontrado,
        FuncDemitido,
        FuncInativo,
        FuncExcluido,
        UsuarioSemPermissao,
        RegistroProcessado,
        FuncNaoSelecionadoParaImportacao,
        RegistroNaoUtilizadoPeloSistema,
        PontoFechado,
        RegistroProcessadoAnteriormente,
        RegistroAguardandoProcessamento,
        RepNaoCadastrado,
        EmpresaNaoEncontrada,
        ErroDesconhecido
    }
}
