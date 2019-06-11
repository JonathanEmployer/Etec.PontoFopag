using cwkPontoMT.Integracao.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.RWTech
{
    public class Pointline_BIOPROX_C : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            return GetAFD(dataI, dataF);
        }
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
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

        public override bool ExclusaoHabilitada()
        {
            return false;
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

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string nomeRelogio, out string erros, DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }

        public override bool TesteConexao()
        {
            throw new NotImplementedException();
        }

        public override int UltimoNSR()
        {
            throw new NotImplementedException();
        }

        public override List<Biometria> GetBiometria(out string erros)
        {
            throw new NotImplementedException();
        }
    }
}
