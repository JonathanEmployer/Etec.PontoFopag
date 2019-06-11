using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepTrilobit;
using System.IO;
using System.Data;
using cwkPontoMT.Integracao.Entidades;

namespace cwkPontoMT.Integracao.Relogios
{
    public class REPTrilobit : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            return GetAFD(dataI, dataF);
        }
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            List<RegistroAFD> registros = new List<RegistroAFD>();
            REP relogio = new REP();

            string dataInicial = String.Empty, dataFinal = String.Empty;

            dataInicial =
                   dataI.Year.ToString().PadLeft(4, '0') +
                   dataI.Month.ToString().PadLeft(2, '0') +
                   dataI.Day.ToString().PadLeft(2, '0');

            dataFinal =
                dataF.Year.ToString().PadLeft(4, '0') +
                dataF.Month.ToString().PadLeft(2, '0') +
                dataF.Day.ToString().PadLeft(2, '0');

            DataTable ret = new DataTable();
            int _porta, _senha;

            SetPortaESenha(out _porta, out _senha);
            if (relogio.LerAFD(IP, _porta, _senha, dataInicial, dataFinal, ref ret, 0))
            {
                string[] cabecalhoRodape = new string[] { "000000000", "999999999" };
                foreach (DataRow item in ret.Rows)
                {
                    if (cabecalhoRodape.Contains(item["campo01"].ToString()) || item["campo02"].ToString() == "2")
                    {
                        registros.Add(new RegistroAFD()
                        {
                            Campo01 = item["campo01"].ToString(),
                            Campo02 = item["campo02"].ToString(),
                            Campo03 = item["campo03"].ToString(),
                            Campo04 = item["campo04"].ToString(),
                            Campo05 = item["campo05"].ToString(),
                            Campo06 = item["campo06"].ToString(),
                            Campo07 = item["campo07"].ToString(),
                            Campo08 = item["campo08"].ToString(),
                            Campo09 = item["campo09"].ToString(),
                            Campo10 = item["campo10"].ToString(),
                            Campo11 = item["campo11"].ToString(),
                        });
                    }
                    else
                    {
                        registros.Add(new RegistroAFD()
                        {
                            Campo01 = item["campo01"].ToString(),
                            Campo02 = item["campo02"].ToString(),
                            Campo03 = String.Empty,
                            Campo04 = item["campo03"].ToString(),
                            Campo05 = item["campo04"].ToString(),
                            Campo06 = item["campo05"].ToString(),
                            Campo07 = item["campo06"].ToString(),
                            Campo08 = item["campo07"].ToString(),
                            Campo09 = item["campo08"].ToString(),
                            Campo10 = item["campo09"].ToString(),
                            Campo11 = item["campo10"].ToString(),
                        });
                    }
                }
                return registros;
            }
            throw new Exception("Ocorreu um erro de comunicação com o relógio." + Environment.NewLine + relogio.ErrorException.Message, relogio.ErrorException);
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            var relogio = new REP();
            var log = new StringBuilder();
            int _porta, _senha;

            SetPortaESenha(out _porta, out _senha);
            EnviarEmpregador(log, _porta, _senha, relogio);
            EnviarEmpregados(log, _porta, _senha, relogio, biometrico);

            if (log.Length > 0)
            {
                erros = log.ToString();
                return false;
            }
            erros = String.Empty;
            return true;
        }

        private void EnviarEmpregados(StringBuilder log, int _porta, int _senha, REP relogio, bool biometrico)
        {
            if (Empregados != null && Empregados.Count > 0)
            {
                foreach (Entidades.Empregado item in Empregados)
                {
                    if (item.DsCodigo.Length > 20)
                    {
                        log.AppendLine("Funcionário " + item.Nome + ": O código (" + item.DsCodigo +
                                       ") ultrapassa o limite de 20 caracteres.");
                        continue;
                    }

                    if (item.Nome.Length > 52)
                        item.Nome = item.Nome.Substring(0, 52);

                    if (!relogio.CadastrarEmpregado(IP, _porta, _senha, item.Pis, item.Nome, item.DsCodigo, biometrico))
                    {
                        log.AppendLine(relogio.ErrorException.Message);
                    }
                }
            }
        }

        private void EnviarEmpregador(StringBuilder log, int _porta, int _senha, REP relogio)
        {
            if (Empregador != null)
            {
                if (
                    !relogio.CadastrarEmpregador(IP, _porta, _senha, (REP.eTipoDocumento)Empregador.TipoDocumento,
                                                 Empregador.Documento, Empregador.CEI, Empregador.RazaoSocial, Empregador.Local))
                {
                    log.AppendLine(relogio.ErrorException.Message);
                }
            }
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            REP relogio = new REP();
            int _porta, _senha;

            SetPortaESenha(out _porta, out  _senha);

            string cheat = FormatarData(inicio);
            if (!relogio.EnviarConfiguracao(IP, _porta, _senha, REP.eParamSetConfig.InicioHorarioVerao, cheat))
            {
                erros = relogio.ErrorException.Message;
                return false;
            }

            cheat = FormatarData(termino);
            if (!relogio.EnviarConfiguracao(IP, _porta, _senha, REP.eParamSetConfig.FimHorarioVerao, cheat))
            {
                erros = relogio.ErrorException.Message;
                return false;
            }

            erros = String.Empty;
            return true;
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            REP relogio = new REP();
            int _porta, _senha;

            SetPortaESenha(out _porta, out  _senha);

            string cheat = FormatarData(horario);
            if (!relogio.EnviarConfiguracao(IP, _porta, _senha, REP.eParamSetConfig.AjusteRelogio, cheat))
            {
                erros = relogio.ErrorException.Message;
                return false;
            }

            erros = String.Empty;
            return true;
        }

        private void SetPortaESenha(out int _porta, out int _senha)
        {
            if (!Int32.TryParse(Porta, out _porta))
                _porta = 19001;

            Int32.TryParse(Senha, out _senha);
        }

        private string FormatarData(DateTime horario)
        {
            return String.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", horario.Year, horario.Month, horario.Day, horario.Hour, horario.Minute, horario.Second);
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
            throw new Exception("Função não disponível para este modelo de relógio.");
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new Exception("Função não disponível para este modelo de relógio.");
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
