using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controlid;
using cwkPontoMT.Integracao.Auxiliares;
using System.IO;
using cwkPontoMT.Integracao.Entidades;

namespace cwkPontoMT.Integracao.Relogios.ZPM
{
    public class ZPM_R130 : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            try
            {
                REPZPM.IniciaDriver(NumeroSerie, IP, Convert.ToInt32(Porta));
                if (nsrFim == Int32.MaxValue)
                {
                    nsrFim = nsrInicio + 65535;
                }
                IList<string> strings = REPZPM.RecebeAFDPorIntervaloNsr(nsrInicio, nsrFim);
                List<RegistroAFD> regs = new List<RegistroAFD>();
                foreach (string item in strings)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        try
                        {
                            Util.IncluiRegistro(item, dataI, dataF, regs);
                        }
                        catch (Exception e)
                        {

                            throw e;
                        }
                    }
                }
                REPZPM.EncerraDriver();
                return regs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            try
            {
                REPZPM.IniciaDriver(NumeroSerie, IP, Convert.ToInt32(Porta));
                IList<string> strings = REPZPM.RecebeAFDPorPeriodo(dataI, dataF, false);
                List<RegistroAFD> regs = new List<RegistroAFD>();
                foreach (string item in strings)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        try
                        {
                            Util.IncluiRegistro(item, dataI, dataF, regs);
                        }
                        catch (Exception e)
                        {

                            throw e;
                        }
                    }
                }
                REPZPM.EncerraDriver();
                return regs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            try
            {
                erros = "";
                if (termino < inicio)
                {
                    throw new Exception("A data de término do horário de verão não pode ser menor que a data de início.");
                }
                REPZPM.IniciaDriver(NumeroSerie, IP, Convert.ToInt32(Porta));
                REPZPM.AtualizaHorarioVerao(inicio, termino);
                REPZPM.EncerraDriver();
                return true;
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            try
            {
                erros = "";
                REPZPM.IniciaDriver(NumeroSerie, IP, Convert.ToInt32(Porta));
                REPZPM.AtualizaHorarioRelogio(horario);
                REPZPM.EncerraDriver();
                return true;
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            try
            {
                bool sucessoEmpresa = false;
                erros = "";
                REPZPM.IniciaDriver(NumeroSerie, IP, Convert.ToInt32(Porta));
                if (Empregador != null)
                {
                    try
                    {
                        REPZPM.CadastraEmpregador(
                            Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ?
                                TipoPessoa.Juridica : TipoPessoa.Fisica,
                                GetStringSomenteAlfanumerico(Empregador.Documento), 
                                GetStringSomenteAlfanumerico(Empregador.CEI), 
                                Empregador.RazaoSocial, Empregador.Local);

                        sucessoEmpresa = true;
                    }
                    catch (Exception e)
                    {
                        erros += e.Message;
                        sucessoEmpresa = false;
                    }
                }
                
                Dictionary<string, string> errosEmpregados = new Dictionary<string, string>();
                foreach (var item in Empregados)
                {
                    try
                    {
                        REPZPM.EnviaFuncionario(Operacao.Incluir, GetStringSomenteAlfanumerico(item.Pis), removeLeadingZeros(item.DsCodigo), 
                            item.Nome, null, true, removeLeadingZeros(item.DsCodigo), "", "", "");
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            REPZPM.EnviaFuncionario(Operacao.Alterar, GetStringSomenteAlfanumerico(item.Pis), removeLeadingZeros(item.DsCodigo), 
                            item.Nome, null, true, removeLeadingZeros(item.DsCodigo), "", "", "");
                        }
                        catch (Exception ex)
                        {
                            errosEmpregados.Add(item.Pis, "Erro ao comunicar/enviar para o Rep\r\n" + ex.Message);
                        }
                    }
                }

                REPZPM.EncerraDriver();
                if (errosEmpregados.Count > 0)
                {
                    foreach (string key in errosEmpregados.Keys)
                    {
                        erros += ("Pis: " + key + " -- " + errosEmpregados[key]);
                    }
                }
                
                if (Empregador != null)
                {
                    return (sucessoEmpresa && String.IsNullOrEmpty(erros));
                }
                else
                {
                    return (String.IsNullOrEmpty(erros));
                }
                
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }

        public override Dictionary<string, object> RecebeInformacoesRep(out string erros)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            erros = "";
            try
            {

                string numSerie = "";
                uint tamBobina = 0;
                uint restanteBobina = 0;
                uint uptime = 0;
                uint cortes = 0;
                uint papelAcumulado = 0;
                uint nsrAtual = 0;

                if (true) //receber info do rep
                {
                    result.Add("Número de Série: ", numSerie);
                    result.Add("Tamanho da bobina: ", tamBobina);
                    result.Add("Tamanho restante da bobina: ", restanteBobina);
                    result.Add("Tempo de funcionamento do relógio: ", uptime);
                    result.Add("Quantidade de cortes (guilhotina): ", cortes);
                    result.Add("Metragem de papel impresso: ", papelAcumulado);
                    result.Add("NSR Atual: ", nsrAtual);
                }
                else
                {
                    //result.Add("Não foi possível ler informações do relógio.", 0);
                }
            }
            catch (Exception e)
            {
                result.Add("Não foi possível ler informações do relógio.", e.Message);
                erros += e.Message;
            }
            return result;
        }

        public override bool ExclusaoHabilitada()
        {
            return true;
        }

        public override bool RemoveFuncionariosRep(out string erros)
        {
            erros = "";
            string err = "";
            try
            {
                REPZPM.IniciaDriver(NumeroSerie, IP, Convert.ToInt32(Porta));
                
                foreach (var item in Empregados)
                {
                    try
                    {
                        REPZPM.EnviaFuncionario(Operacao.Excluir, GetStringSomenteAlfanumerico(item.Pis), removeLeadingZeros(item.DsCodigo),
                            item.Nome, null, true, item.DsCodigo, "", "", "");
                    }
                    catch (Exception e)
                    {
                        err += "Erro ao excluir o funcionario: " + item.Nome + e.Message;
                    }
                }
                REPZPM.EncerraDriver();
                return String.IsNullOrEmpty(err);
            }
            catch (Exception e)
            {
                erros += e.Message;
                return false;
            }
        }

        #region Métodos Auxiliares ZPM
        
        private string GetStringSomenteAlfanumerico(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            string r = new string(s.ToCharArray().Where((c => char.IsLetterOrDigit(c) ||
                                                              char.IsWhiteSpace(c)))
                                                              .ToArray());
            return r;
        }

        private string removeLeadingZeros(string s)
        {
            string res = s;
            while (res.StartsWith("0"))
            {
                res = res.Substring(1);
            }
            return res;
        }

        #endregion

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            erros = "";
            return true;
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
