
namespace Modelo
{
    public class FechamentoPontoFuncionario : Modelo.ModeloBase
    {
        public int IdFechamentoPonto { get; set; }
        public int IdFuncionario { get; set; }
        public FechamentoPonto FechamentoPonto { get; set; }
        public Funcionario Funcionario { get; set; }
    }
}
