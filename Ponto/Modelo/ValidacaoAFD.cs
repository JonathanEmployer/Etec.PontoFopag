using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    class ValidacaoAFD : Modelo.ModeloBase
    {
        public string Arquivo { get; set; }

        public List<string> ListaPIS { get; set; }

        public int? IdRegistroPonto { get; set; }
                
        public bool RazaoCliente { get; set; }

        public int IdFuncionarioSelecionado { get; set; }

        public int TotalLinhas { get; set; }

        public int Lidos { get; set; }

        public int Processados { get; set; }

        public int NaoEncontrados { get; set; }

        public int Errados { get; set; }

        public int Repetidos { get; set; }

        public int SemPermissao { get; set; }

        public int PontoFechado { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }
        
        public List<RegistroFuncionario> ListaRegistroFuncionario { get; set; }
    }

    class RegistroFuncionario : Modelo.ModeloBase
    {
       public int PIS { get; set; }
       public DateTime? Mar_data { get; set; }
       public string Mar_hora { get; set; }
       public int Nsr { get; set; }
       public int IdFuncionario { get; set; }
    }

}
