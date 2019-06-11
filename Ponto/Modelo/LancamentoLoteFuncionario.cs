
namespace Modelo
{
    public class LancamentoLoteFuncionario : Modelo.ModeloBase
    {
        public int IdLancamentoLote { get; set; }
        public int IdFuncionario { get; set; }
        public LancamentoLote LancamentoLote { get; set; }
        public Funcionario Funcionario { get; set; }
        public bool Efetivado { get; set; }
        public bool EfetivadoAnt { get; set; }
        public int UltimaAcao { get; set; }
        public string DescricaoErro { get; set; }
    }
}
