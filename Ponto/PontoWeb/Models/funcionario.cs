//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PontoWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class funcionario
    {
        public funcionario()
        {
            this.afastamento = new HashSet<afastamento>();
            this.AlertasFuncionario = new HashSet<AlertasFuncionario>();
            this.funcionariohistorico = new HashSet<funcionariohistorico>();
            this.marcacao = new HashSet<marcacao>();
            this.marcacaoacesso = new HashSet<marcacaoacesso>();
            this.mudancahorario = new HashSet<mudancahorario>();
            this.mudcodigofunc = new HashSet<mudcodigofunc>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string dscodigo { get; set; }
        public string matricula { get; set; }
        public string nome { get; set; }
        public Nullable<short> codigofolha { get; set; }
        public Nullable<int> idempresa { get; set; }
        public Nullable<int> iddepartamento { get; set; }
        public Nullable<int> idfuncao { get; set; }
        public Nullable<int> idhorario { get; set; }
        public Nullable<int> tipohorario { get; set; }
        public string carteira { get; set; }
        public Nullable<System.DateTime> dataadmissao { get; set; }
        public Nullable<System.DateTime> datademissao { get; set; }
        public Nullable<decimal> salario { get; set; }
        public Nullable<int> funcionarioativo { get; set; }
        public Nullable<int> naoentrarbanco { get; set; }
        public Nullable<int> naoentrarcompensacao { get; set; }
        public Nullable<int> excluido { get; set; }
        public string campoobservacao { get; set; }
        public string foto { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public string pis { get; set; }
        public string senha { get; set; }
        public string toleranciaentrada { get; set; }
        public string toleranciasaida { get; set; }
        public Nullable<int> quantidadetickets { get; set; }
        public Nullable<int> tipotickets { get; set; }
        public string CPF { get; set; }
        public string Mob_Senha { get; set; }
        public Nullable<System.DateTime> DtNascimento { get; set; }
    
        public virtual ICollection<afastamento> afastamento { get; set; }
        public virtual ICollection<AlertasFuncionario> AlertasFuncionario { get; set; }
        public virtual departamento departamento { get; set; }
        public virtual empresa empresa { get; set; }
        public virtual funcao funcao { get; set; }
        public virtual horario horario { get; set; }
        public virtual ICollection<funcionariohistorico> funcionariohistorico { get; set; }
        public virtual ICollection<marcacao> marcacao { get; set; }
        public virtual ICollection<marcacaoacesso> marcacaoacesso { get; set; }
        public virtual ICollection<mudancahorario> mudancahorario { get; set; }
        public virtual ICollection<mudcodigofunc> mudcodigofunc { get; set; }
    }
}
