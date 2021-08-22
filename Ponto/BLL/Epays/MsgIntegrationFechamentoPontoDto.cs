using BLL.Util;
using System.Collections.Generic;

namespace BLL.Epays
{
    public enum enuAcaoFechamentoPonto
    {
        None = 0,
        Incluir = 1,
        Alterar = 2,
        Excluir = 3
    }

    public class MsgIntegrationFechamentoPontoDto : MessageIntegrationDto
    {
        public MsgIntegrationFechamentoPontoDto(string connString)
            : base(connString)
        { }

        public int IdFechamento { get; set; }

        public IEnumerable<int> IdsFuncionario { get; set; }

        public enuAcaoFechamentoPonto Acao { get; set; }

    }
}
