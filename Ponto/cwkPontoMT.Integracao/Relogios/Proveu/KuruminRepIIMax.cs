using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKREPII.Adapter;
using CKREPII.Modelo;
using System.IO;
using cwkPontoMT.Integracao.Entidades;

namespace cwkPontoMT.Integracao.Relogios.Proveu
{
    public class KuruminRepIIMax : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            return GetAFD(dataI, dataF);
        }
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            try
            {

                IList<string> strings = CKREPIIExternalAdapter.GetAfdList(dataI, dataF, IP, NumeroSerie, Senha);
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
                regs = regs.OrderBy(o => o.Nsr).ToList();
                return regs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            erros = "";
            try
            {
                CKREPIIExternalAdapter.SetDST(inicio, termino, IP, NumeroSerie, Senha);
                return true;
            }
            catch (Exception e)
            {
                erros = "Configuração de horário de verão não disponível para este modelo de REP";
                return false;
            }
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            erros = "";
            try
            {
                CKREPIIExternalAdapter.SetDateTime(horario, IP, NumeroSerie, Senha);
                return true;
            }
            catch (Exception e)
            {
                erros += e.Message;
                return false;
            }
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            try
            {
                bool sucessoEmpresa = false;
                erros = "";
                if (Empregador != null)
                {
                    try
                    {
                        CKREPIIExternalAdapter.SetEmployer(new Empregador()
                        {
                            Cei = GetStringSomenteAlfanumerico(Empregador.CEI),
                            CpfCnpj = GetStringSomenteAlfanumerico(Empregador.Documento),
                            LocalPrestacao = Local,
                            RazaoSocial = Empregador.RazaoSocial,
                            TipoIdentificador = Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ? "1" : "0"
                        }, IP, NumeroSerie, Senha);
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
                        CKREPIIExternalAdapter.SetEmployee(new Funcionario()
                        {
                            Cracha = removeLeadingZeros(item.DsCodigo),
                            IdBiometria = "1",
                            Nome = item.Nome,
                            UtilizaLeitorBio = biometrico,
                            Pis = GetStringSomenteAlfanumerico(item.Pis)
                        }, IP, NumeroSerie, Senha);
                    }
                    catch (Exception e)
                    {
                        errosEmpregados.Add(item.Pis, "Erro ao comunicar/enviar para o Rep\r\n" + e.Message);
                    }
                }

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
            throw new NotImplementedException();
        }

        public override bool ExclusaoHabilitada()
        {
            return true;
        }

        public override bool RemoveFuncionariosRep(out string erros)
        {
            erros = "";
            foreach (var item in Empregados)
            {
                try
                {
                    CKREPIIExternalAdapter.RemoveEmployee(GetStringSomenteAlfanumerico(item.Pis), IP, NumeroSerie, Senha);
                }
                catch (Exception e)
                {
                    erros += "Erro ao excluir o funcionario: " + item.Nome + e.Message;
                }
            }

            return String.IsNullOrEmpty(erros);
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            erros = String.Empty;
            try
            {
                DirectoryInfo d;
                if (!Directory.Exists(caminho))
                {
                    d = Directory.CreateDirectory(caminho);
                }
                else
                {
                    d = new DirectoryInfo(caminho);
                }

                if (Empregador != null)
                {
                    string expEmpregador = ExportaEmpregador(Empregador);
                    File.WriteAllText(Path.Combine(d.FullName, "Empresa.PRV"), expEmpregador);
                }

                if (Empregados != null)
                {
                    string expEmpregados = ExportaEmpregados(Empregados);
                    File.WriteAllText(Path.Combine(d.FullName, "Funcionarios.PRV"), expEmpregados);
                }
                return true;
            }
            catch (Exception e)
            {
                erros = e.Message;
            }
            return false;
        }

        private string ExportaEmpregados(List<Entidades.Empregado> empregados)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in empregados)
            {
                sb.AppendLine(PreencherString(item.DsCodigo, 16, "esquerda") + ";" + PreencherString(item.Pis, 12, "esquerda")
                    + ";" + PreencherString(item.Nome, 52, "direita") + ";" + (item.Biometria == true ? "SIM" : "NAO"));
            }
            return sb.ToString();
        }

        private string ExportaEmpregador(Entidades.Empresa empregador)
        {
            return (int)empregador.TipoDocumento + ";" + PreencherString(GetStringSomenteAlfanumerico(empregador.Documento), 14, "esquerda") + ";"
                + (String.IsNullOrEmpty(empregador.CEI) ? "000000000000" : PreencherString(GetStringSomenteAlfanumerico(empregador.CEI), 12, "esquerda")) 
                + ";" + PreencherString(empregador.RazaoSocial, 150, "direita") + ";" + PreencherString(empregador.Local, 100, "direita");
        }

        private string PreencherString(string valor, int tamanho, string lado)
        {
            if (valor.Length < tamanho)
            {
                if (lado.Equals("direita"))
                {
                    string espaco = new string(' ', tamanho - valor.Length);
                    valor += espaco;
                }
                else
                {
                    string zero = new string('0', tamanho - valor.Length);
                    valor = zero + valor; 
                }

                return valor;
            }
            else
                return valor.Substring(0, tamanho);
        }

        public override bool ExportacaoHabilitada()
        {
            return true;
        }

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

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string nomeRelogio, out string erros, DirectoryInfo pasta)
        {
            erros = String.Empty;
            Dictionary<string, string> retorno = new Dictionary<string, string>();
            try
            {
                if (Empregador != null)
                {
                    string nomeGuid = Guid.NewGuid() + ".PRV";
                    string expEmpregador = ExportaEmpregador(Empregador);
                    File.WriteAllText(Path.Combine(pasta.FullName, nomeGuid), expEmpregador);
                    retorno.Add(nomeGuid, "Empregador");
                }

                if (Empregados != null)
                {
                    string nomeGuid = Guid.NewGuid() + ".PRV";
                    string expEmpregados = ExportaEmpregados(Empregados);
                    File.WriteAllText(Path.Combine(pasta.FullName, nomeGuid), expEmpregados);
                    retorno.Add(nomeGuid, "Empregados");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return retorno;
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
