using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelFuncionario : pxyRelPontoWeb
    {
        public List<SelectRel> Relatorios
        {
            get
            {
                List<SelectRel> result = new List<SelectRel>();
                result.Add(new SelectRel() { Id = 1, Descricao = "1 | Por Nome" });
                result.Add(new SelectRel() { Id = 2, Descricao = "2 | Por Código" });
                result.Add(new SelectRel() { Id = 3, Descricao = "3 | Por Departamento" });
                result.Add(new SelectRel() { Id = 4, Descricao = "4 | Por Empresa" });
                result.Add(new SelectRel() { Id = 5, Descricao = "5 | Por Horário" });
                result.Add(new SelectRel() { Id = 6, Descricao = "6 | Ativos/Inativos" });
                result.Add(new SelectRel() { Id = 7, Descricao = "7 | Admitidos" });
                result.Add(new SelectRel() { Id = 8, Descricao = "8 | Demitidos" });
                return result;
            }
        }
        
        public string Relatorio { get; set; }
        
        [Display(Name = "Tipo")]
        public int AtivoInativo { get; set; }

        [Display(Name = "Letras Iniciais")]
        public string LetrasIniciais { get; set; }

        [Display(Name = "Letras Finais")]
        public string LetrasFinais { get; set; }

        [Display(Name = "Código Inicial")]
        public int CodigoInicial { get; set; }

        [Display(Name = "Código Final")]
        public int CodigoFinal { get; set; }


        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        public DateTime InicioPeriodo { get; set; }
        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public DateTime FimPeriodo { get; set; }

        public List<pxyHorario> Horarios { get; set; }

        public string idSelecionados { get; set; }


        public static pxyRelFuncionario Produce(pxyRelPontoWeb p)
        {
            pxyRelFuncionario obj = new pxyRelFuncionario();
            obj.Empresas = p.Empresas;
            obj.Departamentos = p.Departamentos;
            obj.Funcionarios = p.Funcionarios;
            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;
            obj.CodigoFinal = 0;
            obj.CodigoInicial = 0;
            obj.AtivoInativo = 0;
            obj.LetrasIniciais = string.Empty;
            obj.LetrasFinais = string.Empty;
            return obj;
        }
    }

    public class SelectRel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}
