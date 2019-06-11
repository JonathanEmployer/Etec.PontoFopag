using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelAfastamento : pxyRelPontoWeb
    {
        [Display(Name = "Ocorrência")]
        public Ocorrencia OcorrenciaEscolhida { get; set; }
        public string Ocorrencia { get; set; }
        [Display(Name = "Tipo")]
        public int TipoRelatorio { get; set; }

        public string idSelecionados { get; set; }
        public static pxyRelAfastamento Produce(pxyRelPontoWeb p)
        {
            pxyRelAfastamento obj = new pxyRelAfastamento();

            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            obj.OcorrenciasAfastamento = p.OcorrenciasAfastamento;

            // Retirar o preenchimento dessas variaveis após converter os relatórios para o novo filtro
            obj.Empresas = p.Empresas;
            obj.Departamentos = p.Departamentos;
            obj.Funcionarios = p.Funcionarios;
            // Fim Retirar

            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;
            return obj;
        }
    }
}
