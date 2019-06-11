using System;

namespace Modelo
{
    public class pxyHorarioDetalheFuncionario
    {
        public int idFuncionario { get; set; }

        public string dscodigo { get; set; }

        public string nome { get; set; }

        public string matricula { get; set; }

        public int CodigoHorario { get; set; }

        public string descricao { get; set; }

        public int id { get; set; }

        public int idhorario { get; set; }

        public int dia { get; set; }

        public DateTime? data { get; set; }

        public string entrada_1 { get; set; }

        public string entrada_2 { get; set; }

        public string entrada_3 { get; set; }

        public string entrada_4 { get; set; }

        public string saida_1 { get; set; }

        public string saida_2 { get; set; }

        public string saida_3 { get; set; }

        public string saida_4 { get; set; }

        public string primeiraEntrada { get; set; }

        public string ultimaSaida { get; set; }

        public TimeSpan primeiraEntradaTime
        {
            get
            {
                TimeSpan ts = DateTime.MinValue.TimeOfDay;
                ts = ts.Add(TimeSpan.Parse(primeiraEntrada));
                return ts;
            }
        }

        public TimeSpan ultimaSaidaTime
        {
            get
            {
                TimeSpan ts = DateTime.MinValue.TimeOfDay;
                ts = ts.Add(TimeSpan.Parse(ultimaSaida));
                if (TimeSpan.Parse(ultimaSaida) < TimeSpan.Parse(primeiraEntrada))
                {
                    ts = DateTime.MinValue.AddDays(1).TimeOfDay;
                    ts = ts.Add(TimeSpan.Parse(ultimaSaida));
                }
                return ts;
            }
        }
    }
}
