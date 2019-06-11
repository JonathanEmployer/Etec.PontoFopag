using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Ahgora
{
    public class Ah10 : Relogio
    {
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            throw new NotImplementedException();
        }

        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            throw new NotImplementedException();
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object> RecebeInformacoesRep(out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool RemoveFuncionariosRep(out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExportacaoHabilitada()
        {
            return false;
        }

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string caminho, out string erros, System.IO.DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }

        public override bool ExclusaoHabilitada()
        {
            return true;
        }

        public override bool TesteConexao()
        {
            throw new NotImplementedException();
        }

        public override int UltimoNSR()
        {
            throw new NotImplementedException();
        }
    }
}
