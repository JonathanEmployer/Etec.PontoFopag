using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao
{
    public class ResultadoImportacao
    {
        public DateTime InicioProcessamento { get; set; }
        public DateTime FimProcessamento { get; set; }
        public IList<RegistroAFD> RegistrosAFD { get; set; }
        public Resumo Resumo { get; set; }
    }
    public class RegistroAFD
    {
        private string _LinhaAFD;

        public string LinhaAFD
        {
            get { return _LinhaAFD; }
            set { _LinhaAFD = value; }
        }
        public DateTime DataHoraRegistro { get; set; }
        public string Campo01 { get; set; }
        public string Campo02 { get; set; }
        public string Campo03 { get; set; }
        public string Campo04 { get; set; }
        public string Campo05 { get; set; }
        public string Campo06 { get; set; }
        public string Campo07 { get; set; }
        public string Campo08 { get; set; }
        public string Campo09 { get; set; }
        public string Campo10 { get; set; }
        public string Campo11 { get; set; }
        public string Campo12 { get; set; }
        public int Nsr { get; set; }
        public Guid Identificador { get; set; }
        public Guid Lote { get; set; }

        public override string ToString()
        {
            return LinhaAFD;
        }

        public SituacaoRegistro StatusColeta { get; set; }

        public string SituacaoRegistroStr { get { return StatusColeta.ToString(); } }

        public int IdFuncionario { get; set; }

        public void PreencheDataHoraRegistro()
        {
            DateTime data = DateTime.MinValue;
            bool converteu = false;
            if (Campo03 != null && Campo03.Length == 8)// Portaria do AFD do MTE pula o campo 3 na doc, e a do Inmetro considera o campo 3 
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

            DataHoraRegistro = data;
        }
    }

    public class Resumo
    {
        public int FuncNaoEncontrado { get; set; }
        public int FuncDemitido { get; set; }
        public int FuncInativo { get; set; }
        public int FuncExcluido { get; set; }
        public int UsuarioSemPermissao { get; set; }
        public int RegistroProcessado { get; set; }
        public int RegistroSalvo { get; set; }
        public int FuncNaoSelecionadoParaImportacao { get; set; }
        public int RegistroNaoUtilizadoPeloSistema { get; set; }
        public int RegistroDuplicado { get {
            int retorno = 0;
            retorno = RegistroProcessado - RegistroSalvo;
            return retorno;
        } }
        public int PontoFechado { get; set; }
    }

    public class ValidaColeta
    {
        /// <summary>
        /// Pis que esta no arquivo (registro AFD)
        /// </summary>
        public string PIS { get; set; }
        public string dsCodigoFuncionario { get; set; }
        public int IdFuncionario { get; set; }
        /// <summary>
        /// Pis que esta no cadastro do funcionário
        /// </summary>
        public string PISCadFuncionario { get; set; }
        public SituacaoRegistro? SituacaoRegistro { get; set; }
        public DateTime UltimoFechamentoPonto { get; set; }
        public DateTime UltimoFechamentoBancoHoras { get; set; }
    }

    public enum SituacaoRegistro
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
        EmpresaNaoEncontrada
        //[Display(Name = "Funcionário pronto para importação")]
        //FuncProntoImportacao
    };
}