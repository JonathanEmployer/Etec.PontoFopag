using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using cwkPontoMT.Integracao;

namespace BLL
{
    public class ImportacaoBilhetes
    {
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public ImportacaoBilhetes() : this(null)
        {

        }

        public ImportacaoBilhetes(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ImportacaoBilhetes(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;
        }

        public List<string> ImportacaoBilheteWebApi(Modelo.ProgressBar pb, List<Modelo.TipoBilhetes> listaTipoBilhetes, string diretorio, DateTime? datai, DateTime? dataf, string numRelogio, string login, string conectionStr, out bool bErro, bool naoValidaRep)
        {
            List<string> log = new List<string>();
            bErro = false;
            try
            {
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(ConnectionString, true);
                BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(ConnectionString, true);

                string mensagem = String.Empty;
                log.Add("Importação de Bilhetes");
                log.Add("Data = " + DateTime.Now.ToShortDateString());
                log.Add("Hora = " + DateTime.Now.ToShortTimeString());
                string dsCodigosFunc = String.Empty;
                bllBilhetesImp.ObjProgressBar = pb;
                bool temBilhetes = bllBilhetesImp.ImportacaoBilhetesWebApi(listaTipoBilhetes, diretorio, 1, false, String.Empty, ref datai, ref dataf, log, false, true, numRelogio, login, conectionStr, out bErro, ref dsCodigosFunc, naoValidaRep);
                DateTime? dataInicial;
                DateTime? dataFinal;

                if (!temBilhetes)
                {
                    DateTime datainicial, datafinal;
                    datainicial = DateTime.Now.AddDays(-7);
                    datafinal = DateTime.Now.AddDays(1);
                    Modelo.cwkFuncoes.EstendePeriodo(ref datainicial, ref datafinal);
                    datai = datainicial;
                    dataf = datafinal;
                }

                DataTable listaFuncionarios = null;
                if (bllImportaBilhetes.ImportarBilhetesWebApi(dsCodigosFunc, false, datai, dataf, out dataInicial, out dataFinal, pb, log, login, ref listaFuncionarios))
                {
                    foreach (DataRow funcionario in listaFuncionarios.Rows)
                    {
                        BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(2, Convert.ToInt32(funcionario["id"]), dataInicial.Value, dataFinal.Value.AddDays(1), pb, false, ConnectionString, true, false);
                        bllCalculaMarcacao.CalculaMarcacoesWebApi(login);
                    }
                    listaFuncionarios.Dispose();
                }
                else
                {
                    if (!temBilhetes)
                    {
                        log.Add("O Arquivo não tem bilhetes para importar ou\n não está com layout correto.\n Verifique.");
                        log.Add("---------------------------------------------------------------------------------------");
                    }
                    else
                    {
                        log.Add("Arquivo de bilhetes importado com sucesso. Nenhuma marcação processada.");
                        log.Add("---------------------------------------------------------------------------------------");
                    }
                }

            }
            catch (Exception ex)
            {
                log.Add(ex.Message);
                bErro = true;
            }
            return log;
        }

        public List<string> ImportacaoBilheteWeb(Modelo.ProgressBar pb, List<Modelo.TipoBilhetes> listaTipoBilhetes, string diretorio, int bilhete, bool bIndividual,
                                                 string dsCodFuncionario, DateTime? datai, DateTime? dataf, Modelo.UsuarioPontoWeb usuarioLogado ,bool? bRazaoSocial)
        {
            List<string> log = new List<string>();
            try
            {
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(ConnectionString, usuarioLogado);
                string mensagem = String.Empty;
                log.Add("Importação de Bilhetes");
                log.Add("Data = " + DateTime.Now.ToShortDateString());
                log.Add("Hora = " + DateTime.Now.ToShortTimeString());

                bllBilhetesImp.ObjProgressBar = pb;
                bool temBilhetes = bllBilhetesImp.ImportacaoBilhetes(listaTipoBilhetes, diretorio, bilhete, bIndividual, dsCodFuncionario, ref datai, ref dataf, log, usuarioLogado, out _, bRazaoSocial);

                ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(ConnectionString, UsuarioLogado);
                DateTime? dataInicial;
                DateTime? dataFinal;

                if (!temBilhetes)
                {
                    DateTime datainicial, datafinal;
                    datainicial = DateTime.Now.AddMonths(-1);
                    datafinal = DateTime.Now.AddDays(+2);
                    datai = datainicial;
                    dataf = datafinal;
                }
                List<string> FuncsProcessados = new List<string>();

                if (bllImportaBilhetes.ImportarBilhetes(dsCodFuncionario, false, datai, dataf, out dataInicial, out dataFinal, pb, log, out FuncsProcessados , bRazaoSocial))
                {
                    foreach (string dscodigo in FuncsProcessados)
                    {
                        BLL.Funcionario bllfunc = new BLL.Funcionario(ConnectionString, usuarioLogado);
                        int func = bllfunc.GetIdDsCodigo(dscodigo);
                        BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(2, func, dataInicial.Value, dataFinal.Value.AddDays(1), pb, false, ConnectionString, UsuarioLogado, false);
                        bllCalculaMarcacao.CalculaMarcacoes();
                    }
                }
                else
                {
                    if (!temBilhetes)
                    {
                        log.Add("O Arquivo não tem bilhetes para importar ou\não está com layout correto.\nVerifique.");
                        log.Add("---------------------------------------------------------------------------------------");
                    }
                    else
                    {
                        log.Add("Arquivo de bilhetes importado com sucesso. Nenhuma marcação processada.");
                        log.Add("---------------------------------------------------------------------------------------");
                    }
                }

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                log.Add(ex.Message);
            }
            return log;
        }

        public bool ImportacaoBilhete(Modelo.ProgressBar pb, List<Modelo.TipoBilhetes> listaTipoBilhetes, string diretorio, int bilhete, bool bIndividual, string func,
                                      DateTime? datai, DateTime? dataf, out string mensagem ,bool ? bRazaoSocial)
        {
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(ConnectionString, UsuarioLogado);
            BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(ConnectionString, UsuarioLogado);
            Stopwatch tempo = new Stopwatch();
            tempo.Start();

            bool ret = false;
            List<string> log = new List<string>();
            log.Add("Importação de Bilhetes");
            log.Add("Data = " + DateTime.Now.ToShortDateString());
            log.Add("Hora = " + DateTime.Now.ToShortTimeString());

            bllBilhetesImp.ObjProgressBar = pb;
            bool temBilhetes = bllBilhetesImp.ImportacaoBilhetes(listaTipoBilhetes, diretorio, bilhete, bIndividual, func, ref datai, ref dataf, log, null, out _, bRazaoSocial);

            DateTime? dataInicial;
            DateTime? dataFinal;

            if (!temBilhetes)
            {
                DateTime datainicial, datafinal;
                datainicial = DateTime.Now.AddDays(-7);
                datafinal = DateTime.Now.AddDays(+2);
                datai = datainicial;
                dataf = datafinal;
            }

            if (bllImportaBilhetes.ImportarBilhetes(func, false, datai, dataf, out dataInicial, out dataFinal, pb, log , bRazaoSocial))
            {
                BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(null, 0, dataInicial.Value, dataFinal.Value.AddDays(1), pb, false, ConnectionString, UsuarioLogado, false);
                bllCalculaMarcacao.CalculaMarcacoes();
                mensagem = "Importação realizada com sucesso.";
            }
            else
            {
                if (!temBilhetes)
                {
                    mensagem = "O Arquivo não tem bilhetes para importar ou\nnão está com layout correto.\nVerifique.";
                    GravarLogImportacao(log);
                    return false;
                }
                else
                {
                    mensagem = "Arquivo de bilhetes importado com sucesso. Nenhuma marcação processada.";
                }
            }
            ret = true;

            log.Add("---------------------------------------------------------------------------------------");
            GravarLogImportacao(log);

            tempo.Stop();
            return ret;
        }

        private void GravarLogImportacao(List<string> log)
        {
            StreamWriter file = new StreamWriter(Modelo.cwkGlobal.DirApp + "\\logImportacao.txt", false);
            foreach (string l in log)
            {
                file.WriteLine(l);
            }
            file.Close();
        }


        public Modelo.REP GetRepHeaderAFD(string header, out List<string> erros, bool? razaoSocial , DateTime dataIni , DateTime dataFim, ref string aviso)
        {
            erros = new List<string>();
            RegistroAFD reg = cwkPontoMT.Integracao.Util.RetornaLinhaAFD(header);

            if (String.IsNullOrEmpty(reg.Campo08)) 
            {
                erros.Add("Período AFD não foi encontrado");
                return new Modelo.REP();
            }

            string strDataInicio = reg.Campo08.Substring(4, 4) + "-" + reg.Campo08.Substring(2, 2) + "-" + reg.Campo08.Substring(0, 2);
            string strDataFim = reg.Campo09.Substring(4, 4) + "-" + reg.Campo09.Substring(2, 2) + "-" + reg.Campo09.Substring(0, 2);
            DateTime dtIniAFD = DateTime.MinValue; 
            DateTime dtFimAFD = DateTime.MinValue;

            if (!(DateTime.TryParse(strDataInicio, out dtIniAFD)) || !(DateTime.TryParse(strDataFim, out dtFimAFD)))
            {
                erros.Add("Formato Data Inválido");
                return new Modelo.REP();
            }

            if (reg.Campo02 != "1")
            {
                erros.Add("Cabeçalho do AFD não foi encontrado");
                return new Modelo.REP();
            }

            BLL.REP bllRep = new BLL.REP(ConnectionString, UsuarioLogado);
            string numeroRelogio = bllRep.GetNumInner(reg.Campo07);
            if ((numeroRelogio == null || numeroRelogio == "" )&& razaoSocial == false)
            {
                erros.Add("O REP " + reg.Campo07 + " não está cadastrado no sistema!");
                return new Modelo.REP();
            }

            if (!bllRep.GetCPFCNPJ(reg.Campo04, reg.Campo03) && razaoSocial == false)
            {
                erros.Add("CPF/CNPJ : " + reg.Campo04 + " não cadastrado ");
                numeroRelogio = String.Empty;
                return new Modelo.REP();
            }

            Modelo.REP rep = bllRep.LoadObjectPorNumRelogio(numeroRelogio);
            Modelo.REP repPermissao = bllRep.LoadObjectByCodigo(rep.Codigo);
            if (repPermissao == null || repPermissao.Id == 0)
            {
                erros.Add("Usuário não tem permissão para importar afd para o rep " + rep.Codigo + " | " + rep.Local);
                new Modelo.REP();
            }

            if( dtIniAFD < dataIni || dtIniAFD > dataFim || dtFimAFD < dataFim  )
            {
                aviso = "Período de importação fora do intervalo selecionado. Período - [" + dtIniAFD.ToString("dd/MM/yyyy") + "] a [" + dtFimAFD.ToString("dd/MM/yyyy") + "]";
            }

            return repPermissao;
        }
    }
}