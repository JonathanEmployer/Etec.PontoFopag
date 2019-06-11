using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica
{
    public class ConexRep : Relogio
    {
        public ConexRep()
        {
        }

        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF, 0, 0, false);
        }

        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            List<RegistroAFD> regs = new List<RegistroAFD>();
            try
            {
                BLL.EnviaRep enviaRep = new BLL.EnviaRep(Conn);
                enviaRep.EnviarRep(ObterDadosRep());

                Util.CriarCabecalho(regs, dataI, dataF, Empregador, NumeroSerie);

                BLL.AFD bllAFD = new BLL.AFD(ObterDadosRep());
                List<Modelo.REPAFD001> afd = new List<Modelo.REPAFD001>();
                if (nsrInicio > 0 && nsrFim > 0)
                {
                    afd = bllAFD.GetAFDNSR(nsrInicio, nsrFim);
                    bllAFD.ValidaFaltaDeNSR(nsrInicio, nsrFim, ref afd);
                }
                else
                {
                    afd = bllAFD.GetAFDPeriodo(dataI, dataF, nsrInicio, nsrFim);
                    bllAFD.ValidaFaltaDeNSRPeriodo(dataI, dataF, nsrInicio, nsrFim, ref afd);
                }

                
                foreach (Modelo.REPAFD001 bil in afd.Where(w => w.TIPO_REG == "3"))
                {
                    RegistroAFD reg = new RegistroAFD();
                    reg.Campo01 = bil.NSR;
                    reg.Campo02 = "3";
                    reg.Campo04 = bil.DATAHORA.GetValueOrDefault().ToString("ddMMyyyy");
                    reg.Campo05 = bil.DATAHORA.GetValueOrDefault().ToString("HHmm");
                    reg.Campo06 = bil.INFORM.Substring(22, 12);
                    reg.Nsr = Convert.ToInt32(bil.NSR);
                    reg.DataHoraRegistro = bil.DATAHORA.GetValueOrDefault();
                    regs.Add(reg);
                }

                // Se tem apenas cabeçalho
                if (regs.Count() == 1)
                {
                    regs = new List<RegistroAFD>();
                }

                return regs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            BLL.EnviaDataHora bllEnviaDataHora = new BLL.EnviaDataHora(ObterDadosRep());
            erros = String.Empty;

            try
            {
                return bllEnviaDataHora.EnviarHorarioVerao(inicio, termino);
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
            
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            BLL.EnviaDataHora bllEnviaDataHora = new BLL.EnviaDataHora(ObterDadosRep());
            erros = String.Empty;

            try
            {
                return bllEnviaDataHora.EnviarDataHora();
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            BLL.EnviaEmpresa enviaEmpresa = new BLL.EnviaEmpresa(ObterDadosRep());
            BLL.EnviaFuncionario enviaFuncionario = new BLL.EnviaFuncionario(ObterDadosRep());
            erros = String.Empty;

            if (Empregador != null)
            {
                try
                {
                    int tipoDocumento = (int)Empregador.TipoDocumento;

                    #region Validação do tamanho dos campos
                    if (Empregador.Documento.Length > 14)
                    {
                        throw new Exception("Quantidade de digitos do CNPJ/CPF maior do que o permitido. Favor, verifique!");
                    }
                    if (Empregador.CEI == "")
                    {
                        Empregador.CEI = "000000000000";
                    }
                    if (Empregador.CEI.Length > 12)
                    {
                        throw new Exception("Quantidade de digitos do CEI maior do que o permitido. Favor, verifique!");
                    }
                    if (Empregador.RazaoSocial.Length > 150)
                    {
                        throw new Exception("Quantidade de digitos da Razão Social maior do que o permitido. Favor, verifique!");
                    }
                    if (Empregador.Local.Length > 100)
                    {
                        throw new Exception("Quantidade de digitos do Local Serviço maior do que o permitido. Favor, verifique!");
                    }

                    #endregion

                    #region Retira os carácteres especiais

                    Empregador.RazaoSocial = cwkPontoMT.Integracao.Util.RemoveAcentosECaracteresEspeciais(Empregador.RazaoSocial);
                    Empregador.Local = cwkPontoMT.Integracao.Util.RemoveAcentosECaracteresEspeciais(Empregador.Local);

                    #endregion

                    return enviaEmpresa.EnviarEmpresa(tipoDocumento.ToString(), Empregador.Documento, Empregador.CEI, Empregador.RazaoSocial.ToUpper(), Empregador.Local.ToUpper());
                }
                catch (Exception e)
                {
                    erros = e.Message;
                    return false;
                }    
            }
            else if (Empregados.Count > 0)
            {
                try
                {
                    #region Tratamento de erro - Local arquivo
                    if (LocalArquivo == null)
                    {
                        throw new Exception("O Local de arquivo para leitura dos dados dos funcionários não foi informado. Verifique os parâmetros!");
                    }
                    #endregion

                    string matricula = "";
                    string nomeArquivo = "I_MAT_" + NumeroRelogio;
                    //LocalArquivo = @"C:\Users\wagnercastilho\Documents";
                    Telematica.BLL.Util.LimpaArquivo(LocalArquivo, nomeArquivo);

                    foreach (var funcionario in Empregados.OrderBy(o => o.Matricula))
                    {
                        //Valida se matrícula do funcionário, possuí mais do que 12 dígitos.
                        if (funcionario.Matricula.Length > 12)
                        {
                            throw new Exception("O funcionário " + funcionario.Nome + " possuí a matrícula do tamanho maior do que o permitido (limite 12), favor verificar!");
                        }

                        matricula = Telematica.BLL.Util.TratarZeroAEsquerda(funcionario.Matricula, 12);
                        funcionario.Matricula = Telematica.BLL.Util.TratarZeroAEsquerda(funcionario.Matricula, 12);
                        funcionario.Pis = Telematica.BLL.Util.TratarZeroAEsquerda(funcionario.Pis, 12);
                        funcionario.Nome = cwkPontoMT.Integracao.Util.RemoveAcentosECaracteresEspeciais(funcionario.Nome);
                        Telematica.BLL.Util.EscreveLogCaminhoBase(nomeArquivo, matricula, LocalArquivo);
                        enviaFuncionario.EnviarFuncionario(funcionario.Matricula, funcionario.Pis, funcionario.Nome.ToUpper());
                    }
                    enviaFuncionario.ExecutaComandoFunc(nomeArquivo + ".txt");
                    return true;
                }
                catch (Exception e)
                {
                    erros = e.Message;
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public Modelo.RepTelematica ObterDadosRep()
        {
            Modelo.RepTelematica dadosRep = new Modelo.RepTelematica();

            dadosRep.END_IP = Telematica.BLL.Util.ConvertIP15Digitos(IP);
            dadosRep.DESC_END = cwkPontoMT.Integracao.Util.RemoveAcentosECaracteresEspeciais(Local).ToUpper();
            dadosRep.LACES = NumeroRelogio.PadLeft(3, '0');
            dadosRep.FUSO = Util.DiffFusoMinutos(timeZoneInfoRep).ToString("D4");
            dadosRep.BIO_TIPO = Convert.ToString(TipoBiometria);
            dadosRep.TIP_LEIT = "0";
            dadosRep.Conn = Conn;
            dadosRep.CaminhoArquivo = LocalArquivo;
            dadosRep.NumeroRelogio = NumeroRelogio;
            dadosRep.NumSerieRep = NumeroSerie;
            return dadosRep;
        }

        public override Dictionary<string, object> RecebeInformacoesRep(out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool RemoveFuncionariosRep(out string erros)
        {
            BLL.EnviaFuncionario enviaFunc = new BLL.EnviaFuncionario(ObterDadosRep());
            erros = String.Empty;
            Modelo.DAT07 dadosRep = ObterDadosRep();

            #region Tratamento de erro - Local arquivo
            if (LocalArquivo == null)
            {
                throw new Exception("O Local de arquivo para leitura dos dados dos funcionários não foi informado. Verifique os parâmetros!");
            }
            #endregion

            string matricula = "";
            string nomeArquivo = "D_MAT_" + NumeroRelogio;
            List<string> listMatriculas = new List<string>();

            try
            {
                foreach (var funcionario in Empregados.OrderBy(o => o.Matricula))
                {
                    matricula = Telematica.BLL.Util.TratarZeroAEsquerda(funcionario.Matricula, 12);
                    listMatriculas.Add(matricula);
                }

                Telematica.BLL.Util.EscreveArquivo(LocalArquivo, nomeArquivo, listMatriculas);

                bool retorno = enviaFunc.EnviarComandoDeleteFunc(nomeArquivo + ".txt");
                if (retorno != true)
                {
                    throw new Exception("Não foi possível executar o comando de exclusão do funcionário. Favor, verifique!");
                }

                //Conforme orientação do Luis André da Brasil Acesso(consultor que nos orientou na integração), não serão 
                //excluídos os registros dos funcionários da tabela REPEMPR002 (dados do funcionário).
                //foreach (var func in Empregados)
                //{
                //    matricula = Telematica.BLL.Util.TratarZeroAEsquerda(func.Matricula, 12);
                //    bool ret = enviaFunc.DeletarFuncionario(matricula);
                //    if (ret != true)
                //    {
                //        throw new Exception("Os funcionários não foram deletados. Favor, verifique!");
                //    }
                //}
                
                return true;
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
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
            try
            {
                Modelo.RepTelematica repTelematica = ObterDadosRep();
                BLL.StatusRep bllStatus = new BLL.StatusRep(repTelematica);
                return Convert.ToBoolean(bllStatus.VerificaStatusRep(repTelematica.END_IP));
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível receber o status do REP, mensagem: " + e.Message); 
            }
        }

        public override int UltimoNSR()
        {
            try
            {
                Modelo.RepTelematica repTelematica = ObterDadosRep();
                BLL.AFD bllAfd = new BLL.AFD(repTelematica);
                return bllAfd.UltimoNSR(repTelematica.NumSerieRep);
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível obter o último NSR, mensagem: " + e.Message);
            }
        }
    }
}
