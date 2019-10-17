using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Modelo.Utils;

namespace Modelo
{
    public class Afastamento : Modelo.ModeloBase
    {
       
        public Afastamento()
        {
            Horai = "--:--";
            Horaf = "--:--";
        }

        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }

        public string Descricao { get; set; }
        /// <summary>
        /// Identifica��o da Ocorr�ncia
        /// </summary>
        public int IdOcorrencia { get; set; }

        public int CodigoOcorrencia { get; set; }

        [DisplayName("Ocorr�ncia")]
        [TableHTMLAttribute("Ocorr�ncia", 2, true, ItensSearch.select, OrderType.none)]
        public string ocorrencia { get; set; }
        /// <summary>
        /// Tipo do Afastamento: 0 = Funcion�rio, 1 = Departamento, 2 = Empresa, 3 = Contrato
        /// </summary>
        [DisplayName("Tipo")]
        public int Tipo { get; set; }
        /// <summary>
        /// Data Inicial do Afastamento
        /// </summary>
        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "Campo Data Inicial Obrigat�rio")]
        [MinDate("01/01/1760")]
        public DateTime? Datai { get; set; }

        [TableHTMLAttribute("Data Inicial", 3, true, ItensSearch.text, OrderType.none)]
        public string DataInicialStr
        {
            get
            {
                return Datai == null ? "" : Datai.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Data Final do Afastamento
        /// </summary>

        [DisplayName("Data Final")]
        public DateTime? Dataf { get; set; }
        [TableHTMLAttribute("Data Final", 4, true, ItensSearch.text, OrderType.none)]
        public string DataFinalStr
        {
            get
            {
                return Dataf == null ? "" : Dataf.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Identifica��o do Funcion�rio, caso Tipo = 0
        /// </summary>
        public int IdFuncionario { get; set; }
        /// <summary>
        /// Identifica��o da Empresa, caso Tipo = 2
        /// /// </summary>
        public int IdEmpresa { get; set; }
        /// <summary>
        /// Identifica��o do Departamento, caso Tipo = 1
        /// </summary>
        public int IdDepartamento { get; set; }

        /// <summary>
        /// Identifica��o do Contrato, caso Tipo = 3
        /// </summary>
        public int? IdContrato { get; set; }
        /// <summary>
        /// Abono Diurno
        /// </summary>
        
        [DisplayName("Abono Diurno")]
        [TableHTMLAttribute("Abono Diurno", 11, true, ItensSearch.text, OrderType.none)]
        public string Horai { get; set; }
        /// <summary>
        /// Abono Noturno
        /// </summary>
       [DisplayName("Noturno")]
       [TableHTMLAttribute("Abono Noturno", 12, true, ItensSearch.text, OrderType.none)]
        public string Horaf { get; set; }
        /// <summary>
        /// Calcular Abono Parcialmente
        /// </summary>

        public Int16 Parcial { get; set; }

        [DisplayName("Parcial")]
        public bool BParcial
        {
            get { return Parcial == 1 ? true : false; }
            set { Parcial = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Parcial", 9, true, ItensSearch.text, OrderType.none)]
        public string ParcialDesc
        {
            get
            {
                return BParcial == true ? "Sim" : "N�o";
            }
        }

        [DisplayName("Suspens�o")]
        public bool bSuspensao { get; set; }

        [TableHTMLAttribute("Suspens�o", 10, true, ItensSearch.text, OrderType.none)]
        public string SuspensaoDesc
        {
            get
            {
                return bSuspensao == true ? "Sim" : "N�o";
            }
        }

        /// <summary>
        /// Abono sem calculo
        /// </summary>


        public Int16 SemCalculo { get; set; }

        [DisplayName("Sem C�lculo")]
        public bool BSemCalculo
        {
            get { return SemCalculo == 1 ? true : false; }
            set { SemCalculo = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Sem C�lculo", 7, true, ItensSearch.text, OrderType.none)]
        public string SemCalculoDesc
        {
            get
            {
                return BSemCalculo == true ? "Sim" : "N�o";
            }
        }

        /// <summary>
        /// Abonado
        /// </summary>
        /// 

        public Int16 Abonado { get; set; }
        
        [DisplayName("Abonado")]
        public bool BAbonado
        {
            get { return Abonado == 1 ? true : false; }
            set { Abonado = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Abonado", 8, true, ItensSearch.text, OrderType.none)]
        public string AbonadoDesc
        {
            get
            {
                return BAbonado == true ? "Sim" : "N�o";
            }
        }

        /// <summary>
        /// Abonado
        /// </summary>
        /// 

        public Int16 contabilizarjornada { get; set; }

        [DisplayName("Contabilizar Jornada Trabalhada")]
        public bool Bcontabilizarjornada
        {
            get { return contabilizarjornada == 1 ? true : false; }
            set { contabilizarjornada = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Contabilizar Jornada Trabalhada", 15, true, ItensSearch.text, OrderType.none)]
        public string ContabilizarjornadaDesc
        {
            get
            {
                return Bcontabilizarjornada == true ? "Sim" : "N�o";
            }
        }
        /// <summary>
        /// Valor Anterior da vari�vel Tipo
        /// </summary>
        public int Tipo_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da vari�vel Datai
        /// </summary>
        public DateTime? Datai_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da vari�vel Dataf
        /// </summary>
        public DateTime? Dataf_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da vari�vel IdFuncionario
        /// </summary>
        public int IdFuncionario_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da vari�vel IdEmpresa
        /// </summary>
        public int IdEmpresa_Ant { get; set; }
        /// <summary>
        ///  Valor Anterior da vari�vel IdDepartamento
        /// </summary>
        public int IdDepartamento_Ant { get; set; }

        /// <summary>
        ///  Valor Anterior da vari�vel IdDepartamento
        /// </summary>
        public int? IdContrato_Ant { get; set; }

        private string _Nome;
        [Display(Name = "Tipo")]
        [TableHTMLAttribute("Nome", 6, true, ItensSearch.text, OrderType.asc)]
        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }

        private string _TipoAfastamentoStr;
        [TableHTMLAttribute("Tipo Afastamento", 5, true, ItensSearch.select, OrderType.none)]
        public string TipoAfastamentoStr
        {
            get { return _TipoAfastamentoStr; }
            set { _TipoAfastamentoStr = value; }
        }
        
        [Display(Name = "Empresa")]
        public virtual string NomeEmpresa { get; set; }

        [Display(Name = "Departamento")]
        public virtual string NomeDepartamento { get; set; }
      
        [Display(Name = "Funcion�rio")]
        public virtual string NomeFuncionario { get; set; }

        [Display(Name = "Contrato")]
        [RequiredIf("Tipo", 3, "Tipo", "Contrato")]
        public virtual string NomeContrato { get; set; }
        public virtual Funcionario objFuncionario { get; set; }
        public bool NaoRecalcular { get; set; }

        public string ocorrenciaAnt { get; set; }
        public string IdIntegracao { get; set; }
        public int? IdLancamentoLoteFuncionario { get; set; }
        [Display(Name = "Observa��o")]
        [TableHTMLAttribute("Observa��o", 14, true, ItensSearch.text, OrderType.none)]
        public string Observacao { get; set; }
        [TableHTMLAttribute("Tipo Abono", 13, true, ItensSearch.select, OrderType.none)]
        public string TipoAbonoDesc
        {
            get
            {
                string TipoAbono = "";
                if (BParcial)
                {
                    TipoAbono = "Parcial";
                }
                else if (BAbonado)
                {
                    TipoAbono = "Abonado";
                }
                else if (BSemCalculo)
                {
                    TipoAbono = "Sem C�lculo";
                }
                else if (bSuspensao)
                {
                    TipoAbono = "Suspens�o";
                }
                return TipoAbono;
            }
        }
        /// <summary>
        /// Utilizado na API Marcacoes
        /// </summary>
        public bool OcorrenciaTipoFerias { get; set; }
        [DisplayName("Sem Abono")]
        public bool SemAbono { get; set; }

        public void SetTipoAfastamentoPorDefaulOcorrencia(Modelo.Ocorrencia ocorrencia)
        {
            switch (ocorrencia.DefaultTipoAfastamento)
            {
                case 1:
                    this.BAbonado = true;
                    break;
                case 2:
                    this.BSemCalculo = true;
                    break;
                case 3:
                    this.bSuspensao = true;
                    break;
                case 4:
                    this.SemAbono = true;
                    break;
                default:
                    this.BAbonado = true;
                    break;
            }
        }
    }
}
