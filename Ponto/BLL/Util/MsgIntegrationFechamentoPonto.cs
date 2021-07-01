namespace BLL.Util
{
    public class MsgIntegrationFechamentoPonto : MessageIntegrationDto
    {

        public MsgIntegrationFechamentoPonto(string connString)
            : base(connString)
        {

        }

        public string NomeArquivo { get; set; }

        public int Mes { get; set; }

        public int Ano { get; set; }

        public int IdFechamento { get; set; }

        public int IdFuncionario { get; set; }

        public int IdEmpresa { get; set; }
    }
}
