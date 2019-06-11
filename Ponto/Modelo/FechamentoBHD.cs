using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class FechamentoBHD : Modelo.ModeloBase
    {
         /// <summary>
        /// ID do fechamentoBH
        /// </summary>
        public int Idfechamentobh { get; set; }

        [Display(Name = "Seq.")]
        public int Seq { get; set; }
        public int Identificacao { get; set; }

        [Display(Name = "Crédito")]
        public string Credito { get; set; }

        [Display(Name = "Débito")]
        public string Debito { get; set; }

        [Display(Name = "Horas Pagas")]
        public string Saldo { get; set; }

        [Display(Name = "Saldo BH")]
        public string Saldobh { get; set; }

        public string MotivoFechamento { get; set; }

        /// <summary>
        /// 0: credito / 1: debito
        /// </summary>
        [Display(Name = "Tipo")]
        public int Tiposaldo { get; set; }

        /// <summary>
        /// Data do fechamento do banco de horas - CRNC 16/01/2010
        /// </summary>
        public DateTime? DataFechamento { get; set; }

        [Display(Name = "Identificação")]
        public virtual string Nome { get; set; }

        [Display(Name = "Saldo")]
        public virtual string SaldoInicial
        {
            get 
            {
                int _credito = Modelo.cwkFuncoes.ConvertHorasMinuto(Credito);
                int _debito = Modelo.cwkFuncoes.ConvertHorasMinuto(Debito);

                //Tem que fazer a conta do saldo toda vez porque o saldo do objeto esta sincronizado com o campo da tela
                int _saldo = _credito - _debito;
                string _saldoS = Modelo.cwkFuncoes.ConvertMinutosHora(5, _saldo);
                return _saldoS;
            }
        }

        public virtual string Perc1
        {
            get
            {
                String perc1 = String.Empty;

                int totalMinPerc1 = 0;

                foreach (var item in fechamentosbhdHE)
                {
                    totalMinPerc1 += Modelo.cwkFuncoes.ConvertHorasMinuto(item.QuantHorasPerc1);
                }

                perc1 =  Modelo.cwkFuncoes.ConvertMinutosHora2(3,totalMinPerc1);

                return perc1;
            }
        }

        public virtual string Perc2
        {
            get
            {
                String perc2 = String.Empty;

                int totalMinPerc2 = 0;

                foreach (var item in fechamentosbhdHE)
                {
                    totalMinPerc2 += Modelo.cwkFuncoes.ConvertHorasMinuto(item.QuantHorasPerc2);
                    Console.WriteLine(totalMinPerc2);
                }

                perc2 = Modelo.cwkFuncoes.ConvertMinutosHora2(3,totalMinPerc2);

                return perc2;
            }
        }


        public virtual IList<Modelo.FechamentobhdHE> fechamentosbhdHE { get; set; }

        public virtual FechamentoBH fechamentoBH { get; set; }
    }
}
