using Modelo.Proxy;
using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioFuncionarioModel : RelatorioBaseModel, IRelatorioModel
    {
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
        [MinDate("01/01/1760")]
        public new DateTime InicioPeriodo { get; set; }

        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public new DateTime FimPeriodo { get; set; }

        public List<pxyHorario> Horarios { get; set; }

        public List<SelectRel> Relatorios {
            get {
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
    }
}
