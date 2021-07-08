namespace BLL.Util
{
    public class MsgIntegrationFechamentoPontoDto : MessageIntegrationDto
    {

        public MsgIntegrationFechamentoPontoDto(string connString)
            : base(connString)
        {

        }

        public string NomeArquivo { get; set; }

        public int Mes { get; set; }

        public int Ano { get; set; }

        public int IdFechamento { get; set; }

        public int IdFuncionario { get; set; }

        public int IdEmpresa { get; set; }

        public (int current, int total) Info { get; set; }

    }
}
