
namespace Modelo
{
    public class FeriadoFuncionario : Modelo.ModeloBase
    {
        public int IdFeriado { get; set; }
        public int IdFuncionario { get; set; }
        public Feriado Feriado { get; set; }
        public Funcionario Funcionario { get; set; }
    }
}
