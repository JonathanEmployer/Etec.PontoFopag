using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Collections;
using DAL.SQL;
using System.Configuration;
using System.Web.Hosting;

namespace BLL
{
    public class ImportaBilhetes
    {
        private DAL.IBilheteSimp dal;
        private DAL.IMarcacao dalMar;
        private DAL.IImportaBilhetes dalImportacao;
        private DAL.IHorarioDetalhe dalHorarioDetalhe;
        private DAL.IJornadaAlternativa dalJornadaAlternativa;
        private DAL.IMudancaHorario dalMudancaHorario;
        private DAL.IMudCodigoFunc dalMudancaCodigo;
        private DAL.IBilheteSimp dalBilhetes;
        private DAL.IFuncionario dalFuncionario;

        private Hashtable jornadaAlternativaList = null;
        private Hashtable horariosOrdenaSaidaList = null;
        private List<Modelo.MudancaHorario> mudancaHorarioList = null;
        private Dictionary<int, Int16> tiposHoraExtraFalta;
        private Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
        private string ConnectionString;
        private int idhorario = 0, idfunc = 0;
        private short tipohoraextrafalta = 0;
        private string legenda = "";
        private Modelo.Cw_Usuario UsuarioLogado;

        int maxcodMarcacao = 0;

        #region Progress Bar

        private Modelo.ProgressBar objProgressBar;

        private Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        #endregion

        public ImportaBilhetes()
            : this(null)
        {

        }

        public ImportaBilhetes(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ImportaBilhetes(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    DataBase db = new DataBase(ConnectionString);
                    dalImportacao = new DAL.SQL.ImportaBilhetes(db);
                    dalFuncionario = new DAL.SQL.Funcionario(db);
                    dal = new DAL.SQL.BilhetesImp(db);
                    dalMar = new DAL.SQL.Marcacao(db);
                    dalHorarioDetalhe = new DAL.SQL.HorarioDetalhe(db);
                    dalJornadaAlternativa = new DAL.SQL.JornadaAlternativa(db);
                    dalMudancaCodigo = new DAL.SQL.MudCodigoFunc(db);
                    dalMudancaHorario = new DAL.SQL.MudancaHorario(db);
                    dalBilhetes = new DAL.SQL.BilhetesImp(db);
                    break;
                case 2:
                    dalImportacao = DAL.FB.ImportaBilhetes.GetInstancia;
                    dalFuncionario = DAL.FB.Funcionario.GetInstancia;
                    dal = DAL.FB.BilhetesImp.GetInstancia;
                    dalMar = DAL.FB.Marcacao.GetInstancia;
                    dalHorarioDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
                    dalJornadaAlternativa = DAL.FB.JornadaAlternativa.GetInstancia;
                    dalMudancaCodigo = DAL.FB.MudCodigoFunc.GetInstancia;
                    dalMudancaHorario = DAL.FB.MudancaHorario.GetInstancia;
                    dalBilhetes = DAL.FB.BilhetesImp.GetInstancia;
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalImportacao.UsuarioLogado = UsuarioLogado;
            dal.UsuarioLogado = UsuarioLogado;
            dalMar.UsuarioLogado = UsuarioLogado;
            dalHorarioDetalhe.UsuarioLogado = UsuarioLogado;
            dalJornadaAlternativa.UsuarioLogado = UsuarioLogado;
            dalMudancaCodigo.UsuarioLogado = UsuarioLogado;
            dalMudancaHorario.UsuarioLogado = UsuarioLogado;
            dalBilhetes.UsuarioLogado = UsuarioLogado;
            maxcodMarcacao = dalMar.MaxCodigo();
        }

        public ImportaBilhetes(string connString, bool webAPI)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            Modelo.Cw_Usuario user = new Modelo.Cw_Usuario() { Nome = "Pontofopag", Login = "Pontofopag" };

            if (webAPI)
            {
                DataBase db = new DataBase(ConnectionString);
                dalImportacao = new DAL.SQL.ImportaBilhetes(db);
                dalFuncionario = new DAL.SQL.Funcionario(db);
                dal = new DAL.SQL.BilhetesImp(db);
                dalMar = new DAL.SQL.Marcacao(db);
                dalHorarioDetalhe = new DAL.SQL.HorarioDetalhe(db);
                dalJornadaAlternativa = new DAL.SQL.JornadaAlternativa(db);
                dalMudancaCodigo = new DAL.SQL.MudCodigoFunc(db);
                dalMudancaHorario = new DAL.SQL.MudancaHorario(db);
                dalBilhetes = new DAL.SQL.BilhetesImp(db);

                dalImportacao.UsuarioLogado = user;
                dalFuncionario.UsuarioLogado = user;
                dal.UsuarioLogado = user;
                dalMar.UsuarioLogado = user;
                dalHorarioDetalhe.UsuarioLogado = user;
                dalJornadaAlternativa.UsuarioLogado = user;
                dalMudancaCodigo.UsuarioLogado = user;
                dalMudancaHorario.UsuarioLogado = user;
                dalBilhetes.UsuarioLogado = user;
            }

            maxcodMarcacao = dalMar.MaxCodigo();
        }

        public bool ImportarBilhetes(string pDsCodigo, bool pManutBilhete, DateTime? pDataImpI, DateTime? pDataImpF, out DateTime? pdatai, out DateTime? pdataf, Modelo.ProgressBar pProgressBar, List<string> pLog)
        {
            List<string> Funcsprocessar = new List<string>();
            return ImportarBilhetes(pDsCodigo, pManutBilhete, pDataImpI, pDataImpF, out pdatai, out pdataf, pProgressBar, pLog, out Funcsprocessar);
        }
        public bool ImportarBilhetes(string pDsCodigo, bool pManutBilhete, DateTime? pDataImpI, DateTime? pDataImpF, out DateTime? pdatai, out DateTime? pdataf, Modelo.ProgressBar pProgressBar, List<string> pLog, out List<string> FuncsProcessados)
        {
            jornadaAlternativaList = null;
            horariosOrdenaSaidaList = null;
            mudancaHorarioList = null;

            ObjProgressBar = pProgressBar;
            if (ObjProgressBar.setaMensagem == null && ObjProgressBar.incrementaPB == null)
            {
                ObjProgressBar = cwkFuncoes.ProgressVazia();
            }
            DataTable dtBilhete;
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            dtBilhete = dal.GetBilhetesImportar(pDsCodigo, pManutBilhete, pDataImpI, pDataImpF);
            if (dtBilhete.Rows.Count > 0)
            {
                dtBilhete = ReorganizaBilhetesPorRegistroDeFuncionario(false, pDataImpI, pDataImpF, dtBilhete, ref pDsCodigo);
                if (String.IsNullOrEmpty(pDsCodigo))
                {
                    pDsCodigo = String.Join(",", dtBilhete.AsEnumerable().Select(myRow => myRow.Field<string>("bildscodigo")).ToList().Distinct());
                }
                ObjProgressBar.setaMinMaxPB(0, dtBilhete.Rows.Count);
                ObjProgressBar.setaValorPB(0);
                ObjProgressBar.setaMensagem("Processando bilhetes...");

                #region Atualiza as datas inicial e final
                if ((dtBilhete == null || dtBilhete.Rows.Count == 0) && (pDataImpI == null || pDataImpF == new DateTime() || pDataImpF == null || pDataImpF == new DateTime()))
                {
                    DateTime? datai;
                    DateTime? dataf;
                    dal.GetDataBilhetesImportar(pDsCodigo, pManutBilhete, out datai, out dataf);
                    pdatai = datai;
                    pdataf = dataf;
                }
                else
                {
                    //Tenta pegar a menor e a maior data dos bilhetes (Na menor tira um dia e na maior adiciona um dia, pois o bilhete pode ser alocado no dia anterior ou posterior), caso não consiga considera a data de início do parametro
                    try
                    {
                        pdatai = Convert.ToDateTime(dtBilhete.Compute("MIN(data)", null)).AddDays(-1);
                        pdataf = Convert.ToDateTime(dtBilhete.Compute("MAX(data)", null)).AddDays(+1);
                        pDataImpI = pdatai;
                        pDataImpF = pdataf;
                    }
                    catch (Exception)
                    {
                        pdatai = pDataImpI;
                        pdataf = pDataImpF;
                    }
                }
                #endregion

                #region Declaração das Variáveis
                //Cria a lista dos bilhetes para importar            
                DataTable dtMarcacao = new DataTable();
                DataRow dr;
                Hashtable htFuncionario = new Hashtable();
                int auxidfunc = 0;
                int aux = 0;

                int entradaAnt = 0;
                int saidaAnt = 0;
                bool VerificaDiaAnt = false;

                int entrada = 0;
                int saida = 0;
                int limiteHoraMax = 0;
                int limiteMin = 0;
                int limiteMax = 0;

                int ordenabilhetesaida = 0;
                int horaBilhete = 0;
                int dia = 0;

                int ordembilhete = 0;
                int qtdMarIncluida = 0;
                int idBilhete = 0, idBilheteAnt = 0;
                #endregion

                #region Definição do DataRow para Marcacao
                DataColumn[] dcMarcacao = new DataColumn[]
                        {
                            new DataColumn("marcacaoid"),
                            new DataColumn("data"),
                            new DataColumn("marcacao_ent1"),
                            new DataColumn("marcacao_ent2"),
                            new DataColumn("marcacao_ent3"),
                            new DataColumn("marcacao_ent4"),
                            new DataColumn("marcacao_ent5"),
                            new DataColumn("marcacao_ent6"),
                            new DataColumn("marcacao_ent7"),
                            new DataColumn("marcacao_ent8"),
                            new DataColumn("marcacao_sai1"),
                            new DataColumn("marcacao_sai2"),
                            new DataColumn("marcacao_sai3"),
                            new DataColumn("marcacao_sai4"),
                            new DataColumn("marcacao_sai5"),
                            new DataColumn("marcacao_sai6"),
                            new DataColumn("marcacao_sai7"),
                            new DataColumn("marcacao_sai8"),
                            new DataColumn("ent_num_relogio_1"),
                            new DataColumn("ent_num_relogio_2"),
                            new DataColumn("ent_num_relogio_3"),
                            new DataColumn("ent_num_relogio_4"),
                            new DataColumn("ent_num_relogio_5"),
                            new DataColumn("ent_num_relogio_6"),
                            new DataColumn("ent_num_relogio_7"),
                            new DataColumn("ent_num_relogio_8"),
                            new DataColumn("sai_num_relogio_1"),
                            new DataColumn("sai_num_relogio_2"),
                            new DataColumn("sai_num_relogio_3"),
                            new DataColumn("sai_num_relogio_4"),
                            new DataColumn("sai_num_relogio_5"),
                            new DataColumn("sai_num_relogio_6"),
                            new DataColumn("sai_num_relogio_7"),
                            new DataColumn("sai_num_relogio_8"),
                            new DataColumn("marcacaohorario"),
                            new DataColumn("parametro_inicioadnoturno"),
                            new DataColumn("parametro_fimadnoturno"),
                            new DataColumn("horario_ordem_ent"),
                            new DataColumn("horario_limitemin"),
                            new DataColumn("horario_limitemax"),
                            new DataColumn("horario_ent1"),
                            new DataColumn("horario_ent2"),
                            new DataColumn("horario_ent3"),
                            new DataColumn("horario_ent4"),
                            new DataColumn("horario_sai1"),
                            new DataColumn("horario_sai2"),
                            new DataColumn("horario_sai3"),
                            new DataColumn("horario_sai4"),
                            new DataColumn("horario_ordenabilhetesaida"),
                            new DataColumn("jornadaid"),
                            new DataColumn("funcionarioid"),
                            new DataColumn("funcionariodscodigo"),
                            new DataColumn("acao"),
                            new DataColumn("codigo"),
                            new DataColumn("tipohoraextrafalta"),
                            new DataColumn("IdDocumentoWorkflow"),
                            new DataColumn("DocumentoWorkflowAberto"),
                            new DataColumn("LegendasConcatenadas")
                        };
                #endregion
                dtMarcacao.Columns.AddRange(dcMarcacao);

                List<string> lista = new List<string>();
                Hashtable htmarcacao = null;

                DataRow drMar;
                pLog.Add("---------------------------------------------------------------------------------------");
                #region Processa os registros para importação
                Modelo.BilhetesImp objBilhete;
                List<Modelo.BilhetesImp> bilhetesPersistir = new List<Modelo.BilhetesImp>();
                List<int> idsFuncionarios = dtBilhete.AsEnumerable().Select(r => r.Field<int>("funcionarioid")).ToList();
                for (int i = 0; i < dtBilhete.Rows.Count; i++)
                {
                    idBilhete = Convert.ToInt32(dtBilhete.Rows[i]["bimp_id"]);
                    if (idBilheteAnt == idBilhete)
                    {
                        continue;
                    }
                    idBilheteAnt = idBilhete;

                    //Cria uma hastable com os funcionarios
                    aux = dtBilhete.Rows[i]["funcionarioid"] is DBNull ? 0 : Convert.ToInt32(dtBilhete.Rows[i]["funcionarioid"]);
                    if (aux == 0)
                    {
                        continue;
                    }
                    //Carrega as listas apenas quando for utilizar pela primeira vez
                    else
                    {
                        if (htmarcacao == null)
                        {
                            //Pega a Hastable da tabela de marcacao
                            htmarcacao = dalImportacao.GetBilhetesImportar(pDsCodigo, ((DateTime)pDataImpI).AddDays(-1), ((DateTime)pDataImpF).AddDays(1));
                        }
                        if (jornadaAlternativaList == null)
                        {
                            //Pega todas as jornadas alternativas em um determinado periodo
                            jornadaAlternativaList = dalJornadaAlternativa.GetHashIdObjeto(pDataImpI.Value.AddDays(-1), pDataImpF.Value.AddDays(1), 2, idsFuncionarios);
                        }
                        if (mudancaHorarioList == null)
                        {
                            mudancaHorarioList = dalMudancaHorario.GetPeriodo(pdatai.Value.AddDays(-1), pdataf.Value.AddDays(1), idsFuncionarios.Distinct().ToList());

                            if (mudancaHorarioList.Count > 0)
                            {
                                List<int> ids = new List<int>();
                                foreach (Modelo.MudancaHorario mud in mudancaHorarioList)
                                {
                                    if (!ids.Contains(mud.Idhorario))
                                        ids.Add(mud.Idhorario);

                                    if (!ids.Contains(mud.Idhorario_ant))
                                        ids.Add(mud.Idhorario_ant);
                                }
                                tiposHoraExtraFalta = dalImportacao.GetTipoHExtraFaltaHorarios(ids);
                            }
                        }
                    }

                    if (auxidfunc != aux)
                    {
                        auxidfunc = Convert.ToInt32(dtBilhete.Rows[i]["funcionarioid"]);

                        if (!htFuncionario.ContainsKey(auxidfunc))
                            htFuncionario.Add(auxidfunc, dtBilhete.Rows[i]);

                        ObjProgressBar.setaMensagem("Funcionário(a): " + dtBilhete.Rows[i]["funcionarionome"].ToString());
                    }

                    horaBilhete = Convert.ToInt32(dtBilhete.Rows[i]["hora"]);
                    VerificaBilheteHashTableMarcacao(dtBilhete.Rows[i], dtMarcacao, htmarcacao, (DateTime)dtBilhete.Rows[i]["mar_data"]);

                    if (Convert.ToInt32(dtBilhete.Rows[i]["importado"]) == 0)
                    {
                        dia = ValidaDataBilhete(dtBilhete, ref entradaAnt, ref saidaAnt, ref VerificaDiaAnt, ref entrada, ref saida, limiteHoraMax, ref limiteMin, ref limiteMax, ref ordenabilhetesaida, ref horaBilhete, htmarcacao, i, out drMar, dtMarcacao, qtdMarIncluida);

                        VerificaBilheteHashTableMarcacao(dtBilhete.Rows[i], dtMarcacao, htmarcacao, ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia));
                    }
                    else
                    {
                        dia = 0;
                    }

                    //Carrega a Marcação na Data Correta
                    drMar = (DataRow)htmarcacao[String.Format("{0:dd/MM/yyyy}", ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia)) + dtBilhete.Rows[i]["funcionarioid"].ToString()];
                    ordembilhete = drMar["horario_ordem_ent"] is DBNull ? 0 : Convert.ToInt32(drMar["horario_ordem_ent"]);

                    //Encaixa Bilhete na Marcação
                    if (ordembilhete == 0 || new string[] { "000", String.Empty }.Contains((string)dtBilhete.Rows[i]["ordem"]))
                    {
                        #region Atribui Entrada Saida
                        for (int m = 1; m < 9; m++)
                        {
                            var horaEntrada = Convert.ToInt32(drMar["marcacao_ent" + m.ToString()]);
                            var horaSaida = Convert.ToInt32(drMar["marcacao_sai" + m.ToString()]);
                            // Caso não exista a marcação insere, se existir apenas atualiza
                            if ((horaEntrada == -1) || horaEntrada == horaBilhete)
                            {
                                AtribuiMarcacaoEnt(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                break;
                            }
                            else if ((horaSaida == -1) || horaSaida == horaBilhete)
                            {
                                AtribuiMarcacaiSai(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (((string)dtBilhete.Rows[i]["ordem"] == "010") || ((string)dtBilhete.Rows[i]["ordem"] == "110"))
                        {
                            #region Atribui Entradas
                            for (int m = 1; m < 9; m++)
                            {
                                var horaEntrada = Convert.ToInt32(drMar["marcacao_ent" + m.ToString()]);
                                // Caso não exista a marcação insere, se existir apenas atualiza
                                if ((horaEntrada == -1) || horaEntrada == horaBilhete)
                                {
                                    AtribuiMarcacaoEnt(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                    break;
                                }
                            }
                            #endregion
                        }
                        else if (((string)dtBilhete.Rows[i]["ordem"] == "011") || ((string)dtBilhete.Rows[i]["ordem"] == "111"))
                        {
                            #region Atribui Saidas
                            for (int m = 1; m < 9; m++)
                            {
                                var horaSaida = Convert.ToInt32(drMar["marcacao_sai" + m.ToString()]);
                                // Caso não exista a marcação insere, se existir apenas atualiza
                                if ((horaSaida == -1) || horaSaida == horaBilhete)
                                {
                                    AtribuiMarcacaiSai(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                    break;
                                }
                            }
                            #endregion
                        }
                    }

                    if (Convert.ToInt32(drMar["marcacaoid"]) != 0)
                        drMar["acao"] = 2; //Alterar

                    //Atualiza o valor das marcações após o encaixe
                    htmarcacao[String.Format("{0:dd/MM/yyyy}", ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia)) + dtBilhete.Rows[i]["funcionarioid"].ToString()] = drMar;

                    dtBilhete.Rows[i]["mar_data"] = Convert.ToDateTime(drMar["data"]);
                    dtBilhete.Rows[i]["mar_hora"] = Modelo.cwkFuncoes.ConvertMinutosBatida(horaBilhete);
                    dtBilhete.Rows[i]["mar_relogio"] = dtBilhete.Rows[i]["relogio"];
                    dtBilhete.Rows[i]["importado"] = 1;
                    dtBilhete.Rows[i]["bildscodigo"] = drMar["funcionariodscodigo"];
                    dtBilhete.Rows[i]["acao"] = 2; //Alterar

                    //Salva a alteração do bilhete na lista
                    objBilhete = this.PreencheBilheteComDataRow(dtBilhete.Rows[i]);
                    bilhetesPersistir.Add(objBilhete);

                    ObjProgressBar.incrementaPB(1);
                }
                dtBilhete.Dispose();
                #endregion

                if (htmarcacao == null)
                {
                    //Pega a Hastable da tabela de marcacao
                    htmarcacao = dalImportacao.GetBilhetesImportar(pDsCodigo, ((DateTime)pDataImpI).AddDays(-1), pDataImpF);
                }
                if (mudancaHorarioList == null)
                {
                    mudancaHorarioList = dalMudancaHorario.GetPeriodo(pdatai.Value.AddDays(-1), pdataf.Value, idsFuncionarios.Distinct().ToList());
                    if (mudancaHorarioList.Count > 0)
                    {
                        List<int> ids = new List<int>();
                        foreach (Modelo.MudancaHorario mud in mudancaHorarioList)
                        {
                            if (!ids.Contains(mud.Idhorario))
                                ids.Add(mud.Idhorario);

                            if (!ids.Contains(mud.Idhorario_ant))
                                ids.Add(mud.Idhorario_ant);
                        }
                        tiposHoraExtraFalta = dalImportacao.GetTipoHExtraFaltaHorarios(ids);
                    }
                }

                DataTable listaFuncionarios = dalImportacao.GetFuncionariosImportacao(pDsCodigo);

                #region Verifica se no periodo estendido o funcionario possui marcação
                ObjProgressBar.setaMinMaxPB(0, listaFuncionarios.Rows.Count);
                ObjProgressBar.setaValorPB(0);
                ObjProgressBar.setaMensagem("Verificando Marcações...");
                foreach (DataRow funcionario in listaFuncionarios.Rows)
                {
                    DateTime dt = pdatai.Value;
                    while (dt <= pdataf)
                    {
                        if (!htmarcacao.ContainsKey(String.Format("{0:dd/MM/yyyy}", dt) + funcionario["id"].ToString()))
                        {
                            IncluiMarcacaoLimpa(dtMarcacao, htmarcacao, dt, funcionario);
                        }
                        dt = dt.AddDays(1);
                    }
                    ObjProgressBar.incrementaPB(1);
                }
                listaFuncionarios.Dispose();
                #endregion

                #region Processa o Hastable para gerar as marcações
                ObjProgressBar.setaMinMaxPB(0, htmarcacao.Count);
                ObjProgressBar.setaValorPB(0);
                ObjProgressBar.setaMensagem("Preparando atualização das Marcações...");
                DateTime auxData;
                int count = 0;
                List<Modelo.Marcacao> marcacoesPersistir = new List<Modelo.Marcacao>();
                BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
                foreach (DictionaryEntry de in htmarcacao)
                {
                    dr = (DataRow)de.Value;

                    if (Convert.ToInt32(dr["acao"]) == 0)
                        continue;

                    objMarcacao = this.PreencheMarcacaoComDataRow(dr);
                    marcacoesPersistir.Add(objMarcacao);
                    count++;

                    ObjProgressBar.incrementaPB(1);
                }
                if (marcacoesPersistir.Count > 0)
                {
                    auxData = marcacoesPersistir.Min(m => m.Data);
                    if (auxData < pdatai)
                        pdatai = auxData;
                    auxData = marcacoesPersistir.Max(m => m.Data);
                    if (auxData > pdataf)
                        pdataf = auxData;
                }

                htmarcacao.Clear();
                pLog.Add("Marcações Processadas = " + count);
                #endregion

                ObjProgressBar.setaMinMaxPB(0, 1);
                ObjProgressBar.setaValorPB(1);
                ObjProgressBar.setaMensagem("Salvando Marcações...");
                foreach (var marc in marcacoesPersistir)
                {
                    marc.BilhetesMarcacao.AddRange(bilhetesPersistir.Where(w => w.Mar_data == marc.Data && w.DsCodigo == marc.Dscodigo).ToList());
                    bllMarcacao.OrdenaMarcacao(marc, true);
                }

                dalImportacao.PersisteDados(marcacoesPersistir, bilhetesPersistir);
                FuncsProcessados = bilhetesPersistir.Select(x => x.DsCodigo).Distinct().ToList();
                return true;
            }
            //Não importou nenhum bilhete
            else
            {
                pLog.Add("Marcações Processadas = 0");
                pdatai = null;
                pdataf = null;
                FuncsProcessados = new List<string>();
                return false;
            }
        }

        public bool ImportarBilhetesNovo(List<int> idsBilhetes, out DateTime pDataImpI, out DateTime pDataImpF)
        {
            jornadaAlternativaList = null;
            horariosOrdenaSaidaList = null;
            mudancaHorarioList = null;

            DataTable dtBilhete;
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();

            dtBilhete = dal.GetBilhetesImportarByIDs(idsBilhetes);

            //Lista dos bilhetes antes de alterar os dados, é utilizada para remover do salvar os registros que não sofreram alterações
            List<Modelo.BilhetesImp> bilhetesOriginais = new List<Modelo.BilhetesImp>();
            DataTable dtBilheteAnt = dtBilhete.Copy();

            if (dtBilhete.Rows.Count > 0)
            {
                DateTime dataMinBilhete = dtBilhete.AsEnumerable().Min(x => Convert.ToDateTime(x.Field<DateTime>("data")));
                DateTime dataMinMarcacao = dtBilhete.AsEnumerable().Min(x => Convert.ToDateTime(x.Field<DateTime>("mar_data")));
                pDataImpI = dataMinBilhete > dataMinMarcacao ? dataMinMarcacao : dataMinBilhete;
                DateTime dataMaxBilhete = dtBilhete.AsEnumerable().Max(x => Convert.ToDateTime(x.Field<DateTime>("data")));
                DateTime dataMaxMarcacao = dtBilhete.AsEnumerable().Max(x => Convert.ToDateTime(x.Field<DateTime>("mar_data")));
                pDataImpF = dataMaxBilhete > dataMaxMarcacao ? dataMaxBilhete : dataMaxMarcacao;
                string pDsCodigo = "";
                dtBilhete = ReorganizaBilhetesPorRegistroDeFuncionario(false, pDataImpI, pDataImpF, dtBilhete, ref pDsCodigo);

                if (String.IsNullOrEmpty(pDsCodigo))
                {
                    pDsCodigo = String.Join(",", dtBilhete.AsEnumerable().Select(myRow => myRow.Field<string>("bildscodigo")).ToList().Distinct());
                }

                #region Declaração das Variáveis
                //Cria a lista dos bilhetes para importar            
                DataTable dtMarcacao = new DataTable();
                DataRow dr;
                Hashtable htFuncionario = new Hashtable();
                int auxidfunc = 0;
                int aux = 0;

                int entradaAnt = 0;
                int saidaAnt = 0;
                bool VerificaDiaAnt = false;

                int entrada = 0;
                int saida = 0;
                int limiteHoraMax = 0;
                int limiteMin = 0;
                int limiteMax = 0;

                int ordenabilhetesaida = 0;
                int horaBilhete = 0;
                int dia = 0;

                int ordembilhete = 0;
                int qtdMarIncluida = 0;
                int idBilhete = 0, idBilheteAnt = 0;
                #endregion

                #region Definição do DataRow para Marcacao
                DataColumn[] dcMarcacao = new DataColumn[]
                        {
                            new DataColumn("marcacaoid"),
                            new DataColumn("data"),
                            new DataColumn("marcacao_ent1"),
                            new DataColumn("marcacao_ent2"),
                            new DataColumn("marcacao_ent3"),
                            new DataColumn("marcacao_ent4"),
                            new DataColumn("marcacao_ent5"),
                            new DataColumn("marcacao_ent6"),
                            new DataColumn("marcacao_ent7"),
                            new DataColumn("marcacao_ent8"),
                            new DataColumn("marcacao_sai1"),
                            new DataColumn("marcacao_sai2"),
                            new DataColumn("marcacao_sai3"),
                            new DataColumn("marcacao_sai4"),
                            new DataColumn("marcacao_sai5"),
                            new DataColumn("marcacao_sai6"),
                            new DataColumn("marcacao_sai7"),
                            new DataColumn("marcacao_sai8"),
                            new DataColumn("ent_num_relogio_1"),
                            new DataColumn("ent_num_relogio_2"),
                            new DataColumn("ent_num_relogio_3"),
                            new DataColumn("ent_num_relogio_4"),
                            new DataColumn("ent_num_relogio_5"),
                            new DataColumn("ent_num_relogio_6"),
                            new DataColumn("ent_num_relogio_7"),
                            new DataColumn("ent_num_relogio_8"),
                            new DataColumn("sai_num_relogio_1"),
                            new DataColumn("sai_num_relogio_2"),
                            new DataColumn("sai_num_relogio_3"),
                            new DataColumn("sai_num_relogio_4"),
                            new DataColumn("sai_num_relogio_5"),
                            new DataColumn("sai_num_relogio_6"),
                            new DataColumn("sai_num_relogio_7"),
                            new DataColumn("sai_num_relogio_8"),
                            new DataColumn("marcacaohorario"),
                            new DataColumn("parametro_inicioadnoturno"),
                            new DataColumn("parametro_fimadnoturno"),
                            new DataColumn("horario_ordem_ent"),
                            new DataColumn("horario_limitemin"),
                            new DataColumn("horario_limitemax"),
                            new DataColumn("horario_ent1"),
                            new DataColumn("horario_ent2"),
                            new DataColumn("horario_ent3"),
                            new DataColumn("horario_ent4"),
                            new DataColumn("horario_sai1"),
                            new DataColumn("horario_sai2"),
                            new DataColumn("horario_sai3"),
                            new DataColumn("horario_sai4"),
                            new DataColumn("horario_ordenabilhetesaida"),
                            new DataColumn("jornadaid"),
                            new DataColumn("funcionarioid"),
                            new DataColumn("funcionariodscodigo"),
                            new DataColumn("acao"),
                            new DataColumn("codigo"),
                            new DataColumn("tipohoraextrafalta"),
                            new DataColumn("IdDocumentoWorkflow"),
                            new DataColumn("DocumentoWorkflowAberto"),
                            new DataColumn("LegendasConcatenadas")
                        };
                #endregion
                dtMarcacao.Columns.AddRange(dcMarcacao);

                List<string> lista = new List<string>();
                Hashtable htmarcacao = null;

                DataRow drMar;
                #region Processa os registros para importação
                Modelo.BilhetesImp objBilhete;
                List<Modelo.BilhetesImp> bilhetesPersistir = new List<Modelo.BilhetesImp>();
                List<int> idsFuncionarios = dtBilhete.AsEnumerable().Select(r => r.Field<int>("funcionarioid")).Distinct().ToList();
                for (int i = 0; i < dtBilhete.Rows.Count; i++)
                {
                    idBilhete = Convert.ToInt32(dtBilhete.Rows[i]["bimp_id"]);
                    if (idBilheteAnt == idBilhete)
                    {
                        continue;
                    }
                    idBilheteAnt = idBilhete;

                    //Cria uma hastable com os funcionarios
                    aux = dtBilhete.Rows[i]["funcionarioid"] is DBNull ? 0 : Convert.ToInt32(dtBilhete.Rows[i]["funcionarioid"]);
                    if (aux == 0)
                    {
                        continue;
                    }
                    //Carrega as listas apenas quando for utilizar pela primeira vez
                    else
                    {
                        if (htmarcacao == null)
                        {
                            //Pega a Hastable da tabela de marcacao, na hora de carregar as marcações pega sempre um dia antes de um dia depois dos bilhetes, pois podem existir bilhetes que serão alocados no dia anterior, ou no dia posterior
                            htmarcacao = dalImportacao.GetBilhetesImportar(pDsCodigo, ((DateTime)pDataImpI).AddDays(-1), pDataImpF.AddDays(1));
                        }
                        if (jornadaAlternativaList == null)
                        {
                            //Pega todas as jornadas alternativas em um determinado periodo, as datas são alteradas para mais um dia e menos um dia, pois de acordo com a ordenação dos bilhetes, ele pode abrir menos um dia ou mais um dia da marcação
                            jornadaAlternativaList = dalJornadaAlternativa.GetHashIdObjeto(pDataImpI.AddDays(-1), pDataImpF.AddDays(1), 2, idsFuncionarios);
                        }
                        if (mudancaHorarioList == null)
                        {
                            mudancaHorarioList = dalMudancaHorario.GetPeriodo(pDataImpI.AddDays(-1), pDataImpI.AddDays(1), idsFuncionarios.Distinct().ToList());

                            if (mudancaHorarioList.Count > 0)
                            {
                                List<int> ids = new List<int>();
                                foreach (Modelo.MudancaHorario mud in mudancaHorarioList)
                                {
                                    if (!ids.Contains(mud.Idhorario))
                                        ids.Add(mud.Idhorario);

                                    if (!ids.Contains(mud.Idhorario_ant))
                                        ids.Add(mud.Idhorario_ant);
                                }
                                tiposHoraExtraFalta = dalImportacao.GetTipoHExtraFaltaHorarios(ids);
                            }
                        }
                    }

                    if (auxidfunc != aux)
                    {
                        auxidfunc = Convert.ToInt32(dtBilhete.Rows[i]["funcionarioid"]);

                        if (!htFuncionario.ContainsKey(auxidfunc))
                            htFuncionario.Add(auxidfunc, dtBilhete.Rows[i]);
                    }

                    horaBilhete = Convert.ToInt32(dtBilhete.Rows[i]["hora"]);
                    VerificaBilheteHashTableMarcacao(dtBilhete.Rows[i], dtMarcacao, htmarcacao, (DateTime)dtBilhete.Rows[i]["mar_data"]);

                    if (Convert.ToInt32(dtBilhete.Rows[i]["importado"]) == 0 || Convert.ToInt32(dtBilhete.Rows[i]["importado"]) == 2)
                    {
                        dia = ValidaDataBilhete(dtBilhete, ref entradaAnt, ref saidaAnt, ref VerificaDiaAnt, ref entrada, ref saida, limiteHoraMax, ref limiteMin, ref limiteMax, ref ordenabilhetesaida, ref horaBilhete, htmarcacao, i, out drMar, dtMarcacao, qtdMarIncluida);

                        VerificaBilheteHashTableMarcacao(dtBilhete.Rows[i], dtMarcacao, htmarcacao, ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia));
                    }
                    else
                    {
                        dia = 0;
                    }

                    //Carrega a Marcação na Data Correta
                    drMar = (DataRow)htmarcacao[String.Format("{0:dd/MM/yyyy}", ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia)) + dtBilhete.Rows[i]["funcionarioid"].ToString()];
                    ordembilhete = drMar["horario_ordem_ent"] is DBNull ? 0 : Convert.ToInt32(drMar["horario_ordem_ent"]);

                    //Encaixa Bilhete na Marcação
                    if (ordembilhete == 0 || new string[] { "000", String.Empty }.Contains((string)dtBilhete.Rows[i]["ordem"]))
                    {
                        #region Atribui Entrada Saida
                        for (int m = 1; m < 9; m++)
                        {
                            var horaEntrada = Convert.ToInt32(drMar["marcacao_ent" + m.ToString()]);
                            var horaSaida = Convert.ToInt32(drMar["marcacao_sai" + m.ToString()]);
                            // Caso não exista a marcação insere, se existir apenas atualiza
                            if ((horaEntrada == -1) || horaEntrada == horaBilhete)
                            {
                                AtribuiMarcacaoEnt(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                break;
                            }
                            else if ((horaSaida == -1) || horaSaida == horaBilhete)
                            {
                                AtribuiMarcacaiSai(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (((string)dtBilhete.Rows[i]["ordem"] == "010") || ((string)dtBilhete.Rows[i]["ordem"] == "110"))
                        {
                            #region Atribui Entradas
                            for (int m = 1; m < 9; m++)
                            {
                                var horaEntrada = Convert.ToInt32(drMar["marcacao_ent" + m.ToString()]);
                                // Caso não exista a marcação insere, se existir apenas atualiza
                                if ((horaEntrada == -1) || horaEntrada == horaBilhete)
                                {
                                    AtribuiMarcacaoEnt(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                    break;
                                }
                            }
                            #endregion
                        }
                        else if (((string)dtBilhete.Rows[i]["ordem"] == "011") || ((string)dtBilhete.Rows[i]["ordem"] == "111"))
                        {
                            #region Atribui Saidas
                            for (int m = 1; m < 9; m++)
                            {
                                var horaSaida = Convert.ToInt32(drMar["marcacao_sai" + m.ToString()]);
                                // Caso não exista a marcação insere, se existir apenas atualiza
                                if ((horaSaida == -1) || horaSaida == horaBilhete)
                                {
                                    AtribuiMarcacaiSai(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                    break;
                                }
                            }
                            #endregion
                        }
                    }

                    if (Convert.ToInt32(drMar["marcacaoid"]) != 0)
                        drMar["acao"] = 2; //Alterar

                    //Atualiza o valor das marcações após o encaixe
                    htmarcacao[String.Format("{0:dd/MM/yyyy}", ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia)) + dtBilhete.Rows[i]["funcionarioid"].ToString()] = drMar;

                    dtBilhete.Rows[i]["mar_data"] = Convert.ToDateTime(drMar["data"]);
                    dtBilhete.Rows[i]["mar_hora"] = Modelo.cwkFuncoes.ConvertMinutosBatida(horaBilhete);
                    dtBilhete.Rows[i]["mar_relogio"] = dtBilhete.Rows[i]["relogio"];
                    dtBilhete.Rows[i]["importado"] = 1;
                    dtBilhete.Rows[i]["bildscodigo"] = drMar["funcionariodscodigo"];
                    dtBilhete.Rows[i]["acao"] = 2; //Alterar

                    //Salva a alteração do bilhete na lista
                    objBilhete = this.PreencheBilheteComDataRow(dtBilhete.Rows[i]);
                    DataRow drAnt = dtBilheteAnt.AsEnumerable().Where(row => row.Field<int>("bimp_id") == Convert.ToInt32(dtBilhete.Rows[i]["bimp_id"])).FirstOrDefault();
                    if (drAnt != null)
                    {
                        Modelo.BilhetesImp objBilheteAntAlteracao = this.PreencheBilheteComDataRow(drAnt);
                        bilhetesOriginais.Add(objBilheteAntAlteracao);
                    }
                    bilhetesPersistir.Add(objBilhete);
                }
                dtBilhete.Dispose();
                #endregion

                if (htmarcacao == null)
                {
                    //Pega a Hastable da tabela de marcacao
                    htmarcacao = dalImportacao.GetBilhetesImportar(pDsCodigo, ((DateTime)pDataImpI).AddDays(-1), pDataImpF);
                }
                if (mudancaHorarioList == null)
                {
                    mudancaHorarioList = dalMudancaHorario.GetPeriodo(pDataImpI.AddDays(-1), pDataImpF, idsFuncionarios);
                    if (mudancaHorarioList.Count > 0)
                    {
                        List<int> ids = new List<int>();
                        foreach (Modelo.MudancaHorario mud in mudancaHorarioList)
                        {
                            if (!ids.Contains(mud.Idhorario))
                                ids.Add(mud.Idhorario);

                            if (!ids.Contains(mud.Idhorario_ant))
                                ids.Add(mud.Idhorario_ant);
                        }
                        tiposHoraExtraFalta = dalImportacao.GetTipoHExtraFaltaHorarios(ids);
                    }
                }

                DataTable listaFuncionarios = dalImportacao.GetFuncionariosImportacao(pDsCodigo);

                #region Verifica se no periodo estendido o funcionario possui marcação  
                foreach (DataRow funcionario in listaFuncionarios.Rows)
                {
                    DateTime dt = pDataImpI;
                    while (dt <= pDataImpF)
                    {
                        if (!htmarcacao.ContainsKey(String.Format("{0:dd/MM/yyyy}", dt) + funcionario["id"].ToString()))
                        {
                            IncluiMarcacaoLimpa(dtMarcacao, htmarcacao, dt, funcionario);
                        }
                        dt = dt.AddDays(1);
                    }
                }
                listaFuncionarios.Dispose();
                #endregion

                #region Processa o Hastable para gerar as marcações
                DateTime auxData;
                int count = 0;
                List<Modelo.Marcacao> marcacoesPersistir = new List<Modelo.Marcacao>();
                BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
                foreach (DictionaryEntry de in htmarcacao)
                {
                    dr = (DataRow)de.Value;

                    if (Convert.ToInt32(dr["acao"]) == 0)
                        continue;

                    objMarcacao = this.PreencheMarcacaoComDataRow(dr);
                    marcacoesPersistir.Add(objMarcacao);
                    count++;

                }
                if (marcacoesPersistir.Count > 0)
                {
                    auxData = marcacoesPersistir.Min(m => m.Data);
                    if (auxData < pDataImpI)
                        pDataImpI = auxData;
                    auxData = marcacoesPersistir.Max(m => m.Data);
                    if (auxData > pDataImpF)
                        pDataImpF = auxData;
                }

                htmarcacao.Clear();
                #endregion

                foreach (var marc in marcacoesPersistir)
                {
                    marc.BilhetesMarcacao.AddRange(bilhetesPersistir.Where(w => w.Mar_data == marc.Data && w.DsCodigo == marc.Dscodigo).ToList());
                    bllMarcacao.OrdenaMarcacao(marc, true);

                    #region Remove os bilhetes que não sofreram alterações para não ficar gravando dados desnecessários no banco de dados
                    List<Modelo.BilhetesImp> bilhetesSemAlteracao = new List<Modelo.BilhetesImp>();
                    foreach (Modelo.BilhetesImp bilhete in marc.BilhetesMarcacao)
                    {
                        if (bilhete.BilheteIsEqual(bilhetesOriginais.Where(w => w.Id == bilhete.Id).FirstOrDefault()))
                        {
                            bilhetesSemAlteracao.Add(bilhete);
                        }
                    }
                    if (bilhetesSemAlteracao.Count > 0)
                    {
                        marc.BilhetesMarcacao.RemoveAll(r => bilhetesSemAlteracao.Select(s => s.Id).Contains(r.Id));
                    }
                    #endregion
                }

                var groupFunc = marcacoesPersistir.GroupBy(g => g.Idfuncionario);

                List<Modelo.BilhetesImp> bilhetesSalvar = new List<Modelo.BilhetesImp>();
                List<Modelo.Marcacao> marcacoesSalvar = new List<Modelo.Marcacao>();
                int cont = 0;
                foreach (var group in groupFunc)
                {
                    cont++;
                    marcacoesSalvar.AddRange(group);
                    foreach (var marcs in group)
                    {
                        bilhetesSalvar.AddRange(marcs.BilhetesMarcacao);
                    }
                    if (cont == groupFunc.Count() || (marcacoesSalvar.Count() + bilhetesSalvar.Count()) > 500)
                    {
                        dalImportacao.PersisteDados(marcacoesSalvar, bilhetesSalvar);
                        bilhetesSalvar = new List<Modelo.BilhetesImp>();
                        marcacoesSalvar = new List<Modelo.Marcacao>();
                    }
                }
                return true;
            }
            //Não importou nenhum bilhete
            else
            {
                pDataImpI = DateTime.Now;
                pDataImpF = DateTime.Now;
                return false;
            }
        }

        /// <summary>
        /// Processo que redistribui as batidas de acordo com os horários do registro de emprego
        /// </summary>
        /// <param name="pManutBilhete">Apenas passar true quando quiser que realmente refaça todo o dia, normalmente o processo so roda quando tem bilhetes importados = 0</param>
        /// <param name="pDataImpI">Data Inicio</param>
        /// <param name="pDataImpF">Data Fim</param>
        /// <param name="dtBilhete">Datatable com o bilhetes a serem considerados</param>
        /// <param name="pDsCodigo">Retorna os dscodigos a serem processados caso passado algum em específico</param>
        /// <returns>Retorna Datatable com os bilhetes reorganizados por registro a ser processado</returns>
        private DataTable ReorganizaBilhetesPorRegistroDeFuncionario(bool pManutBilhete, DateTime? pDataImpI, DateTime? pDataImpF, DataTable dtBilhete, ref string pDsCodigo)
        {
            try
            {
                List<string> PISs = dtBilhete.AsEnumerable()
                                   .Select(r => r.Field<string>("PIS")).Distinct()
                                   .ToList();

                if (pDataImpI == null)
                {
                    string di = dtBilhete.Compute("MIN(data)", null).ToString();
                    DateTime? dtBilMin = String.IsNullOrEmpty(di) ? (DateTime?)(null) : DateTime.Parse(di);
                    pDataImpI = dtBilMin;
                }

                if (pDataImpF == null)
                {
                    string df = dtBilhete.Compute("MAX(data)", null).ToString();
                    DateTime? dtBilMax = String.IsNullOrEmpty(df) ? (DateTime?)(null) : DateTime.Parse(df);
                    pDataImpF = dtBilMax;
                }
                
                List<Modelo.Proxy.PxyBilhetesFuncsDoisRegistros> FuncsRedistribuirBilhetesRegistro = dal.FuncsDoisRegistrosRegistribuirBilhetes(pManutBilhete, PISs, pDataImpI.GetValueOrDefault(), pDataImpF.GetValueOrDefault());
                Dictionary<int, DataTable> batidasFunc = new Dictionary<int, DataTable>();
                bool reordenouRegistros = false;
                foreach (var grupo in FuncsRedistribuirBilhetesRegistro.GroupBy(s => new { s.PIS, s.Mar_data }))
                {
                    if (grupo.Select(s => s).Count() >= 2)
                    {
                        List<Modelo.BilhetesImp> bilheteReorganizar = dal.GetBilhetesFuncPis(new List<string>() { grupo.Key.PIS }, grupo.Key.Mar_data, grupo.Key.Mar_data);
                        Modelo.Proxy.PxyBilhetesFuncsDoisRegistros ultimoRegistroFuncionario = grupo.Select(s => s).OrderBy(o => o.UltimaSaidaDt).LastOrDefault();
                        int qtdAlocada = 0;
                        foreach (Modelo.Proxy.PxyBilhetesFuncsDoisRegistros dtFuncDia in grupo.Select(s => s).OrderBy(o => o.UltimaSaidaDt))
                        {

                            int ordem = 0;
                            int posicao = 1;
                            if (bilheteReorganizar.Count > 0)
                            {
                                for (int i = qtdAlocada; i < bilheteReorganizar.Count; i++)
                                {
                                    ordem++;
                                    DateTime? marData = bilheteReorganizar[i].Mar_data;
                                    string marHora = bilheteReorganizar[i].Mar_hora;
                                    var time = TimeSpan.Parse(marHora);
                                    DateTime marHoraData = marData.GetValueOrDefault().Add(time);

                                    Modelo.BilhetesImp bilProx = new Modelo.BilhetesImp();

                                    if (i + 1 < bilheteReorganizar.Count)
                                    {
                                        bilProx = bilheteReorganizar[i + 1];
                                    }

                                    //Aloca marcação para esse registro Se:
                                    // -For o ultimo funcionario a alocar, aloca tudo para o mesmo
                                    if (dtFuncDia.IdFuncionario == ultimoRegistroFuncionario.IdFuncionario || //ou
                                                                                                              //-A batida é menor que o horário de saida do registro e não tem mais bilhetes ou a data do próximo bilhete é menor que a entrada o próximo registro 
                                       (marHoraData <= dtFuncDia.UltimaSaidaDt && (bilProx.Id == 0 || qtdAlocada == 0 || qtdAlocada % 2 != 0 || bilProx.Mar_data.GetValueOrDefault().Add(TimeSpan.Parse(bilProx.Mar_hora)) < ultimoRegistroFuncionario.PrimeiraEntradaDt)) || //ou
                                                                                                                                                                                                                                                                              //-A batida é maior que o horário de saida do registro mas o registro ainda esta impar e tem mais bilhetes e quantidade restante for suficiente para o segundo turno ou tem virado de dia na jornada
                                       (marHoraData >= dtFuncDia.UltimaSaidaDt && (qtdAlocada % 2 != 0 && (bilProx.Id == 0 || ((bilheteReorganizar.Count - (qtdAlocada + 1)) % 2 == 0 && (bilheteReorganizar.Count - (qtdAlocada + 1)) >= ultimoRegistroFuncionario.QtdBatidaJornada) || dtFuncDia.PrimeiraEntradaDt.Date != dtFuncDia.UltimaSaidaDt.Date || ultimoRegistroFuncionario.PrimeiraEntradaDt.Date != ultimoRegistroFuncionario.UltimaSaidaDt.Date)))
                                       )
                                    {
                                        bilheteReorganizar[i].Func = dtFuncDia.DsCodigo;
                                        bilheteReorganizar[i].DsCodigo = dtFuncDia.DsCodigo;
                                        bilheteReorganizar[i].IdFuncionario = dtFuncDia.IdFuncionario;
                                        bilheteReorganizar[i].PIS = dtFuncDia.PIS;
                                        if (ordem % 2 != 0)
                                        {
                                            bilheteReorganizar[i].Ent_sai = "E";
                                            bilheteReorganizar[i].Posicao = posicao;
                                        }
                                        else
                                        {
                                            bilheteReorganizar[i].Ent_sai = "S";
                                            bilheteReorganizar[i].Posicao = posicao;
                                            posicao++;
                                        }
                                        qtdAlocada++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            if (!String.IsNullOrEmpty(pDsCodigo))
                            {
                                List<String> dsCods = pDsCodigo.Split(',').ToList();
                                if ((!dsCods.Any(s => s == dtFuncDia.DsCodigo)))
                                {
                                    dsCods.Add(dtFuncDia.DsCodigo);
                                }
                                pDsCodigo = String.Join(",", dsCods);
                            }

                            //Prepara a marcação para receber as alterações no bilhete
                            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
                            List<Modelo.Marcacao> listaMarcacoes = bllMarcacao.GetPorFuncionario(dtFuncDia.IdFuncionario, dtFuncDia.Mar_data, dtFuncDia.Mar_data, true);

                            foreach (Modelo.Marcacao marc in listaMarcacoes)
                            {
                                marc.Ent_num_relogio_1 = "";
                                marc.Ent_num_relogio_2 = "";
                                marc.Ent_num_relogio_3 = "";
                                marc.Ent_num_relogio_4 = "";
                                marc.Ent_num_relogio_5 = "";
                                marc.Ent_num_relogio_6 = "";
                                marc.Ent_num_relogio_7 = "";
                                marc.Ent_num_relogio_8 = "";
                                marc.Sai_num_relogio_1 = "";
                                marc.Sai_num_relogio_2 = "";
                                marc.Sai_num_relogio_3 = "";
                                marc.Sai_num_relogio_4 = "";
                                marc.Sai_num_relogio_5 = "";
                                marc.Sai_num_relogio_6 = "";
                                marc.Sai_num_relogio_7 = "";
                                marc.Sai_num_relogio_8 = "";
                                marc.Entrada_1 = "--:--";
                                marc.Entrada_2 = "--:--";
                                marc.Entrada_3 = "--:--";
                                marc.Entrada_4 = "--:--";
                                marc.Entrada_5 = "--:--";
                                marc.Entrada_6 = "--:--";
                                marc.Entrada_7 = "--:--";
                                marc.Entrada_8 = "--:--";
                                marc.Saida_1 = "--:--";
                                marc.Saida_2 = "--:--";
                                marc.Saida_3 = "--:--";
                                marc.Saida_4 = "--:--";
                                marc.Saida_5 = "--:--";
                                marc.Saida_6 = "--:--";
                                marc.Saida_7 = "--:--";
                                marc.Saida_8 = "--:--";
                                marc.Acao = Modelo.Acao.Alterar;
                            }
                            bllMarcacao.Salvar(Modelo.Acao.Alterar, listaMarcacoes);
                        }
                        bilheteReorganizar.ForEach(s => s.Acao = Modelo.Acao.Alterar);
                        dalBilhetes.Alterar(bilheteReorganizar);
                        string filtro = " NOT ( PIS = '" + grupo.Key.PIS + "' and mar_data = '" + grupo.Key.Mar_data + "') ";
                        if (dtBilhete.Select(filtro).Count() == 0)
                        {
                            dtBilhete = new DataTable();

                        }
                        else
                        {
                            dtBilhete = dtBilhete.Select(filtro).CopyToDataTable();
                        }

                        DataTable dtBilheteAlterados = dalBilhetes.GetBilhetesPorPIS(new List<string>() { grupo.Key.PIS }, grupo.Key.Mar_data, grupo.Key.Mar_data);
                        dtBilhete.Merge(dtBilheteAlterados);
                        reordenouRegistros = true;
                    }
                }
                if (reordenouRegistros)
                {
                    DataView v = dtBilhete.DefaultView;
                    v.Sort = "func asc, data asc, hora asc";
                    dtBilhete = v.ToTable();
                }
                return dtBilhete;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao separar batidas por registros do funcionário, erro: " + e.Message);
            }
        }

        public bool ImportarBilhetesWebApi(string pDsCodigo, bool pManutBilhete, DateTime? pDataImpI, DateTime? pDataImpF, out DateTime? pdatai, out DateTime? pdataf, Modelo.ProgressBar pProgressBar, List<string> pLog, string login, ref DataTable listaFuncionarios)
        {
            jornadaAlternativaList = null;
            horariosOrdenaSaidaList = null;
            mudancaHorarioList = null;

            ObjProgressBar = pProgressBar;
            DataTable dtBilhete;
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            dtBilhete = dal.GetBilhetesImportar(pDsCodigo, pManutBilhete, pDataImpI, pDataImpF);
            dtBilhete = ReorganizaBilhetesPorRegistroDeFuncionario(false, pDataImpI, pDataImpF, dtBilhete, ref pDsCodigo);
            if (dtBilhete.Rows.Count > 0)
            {
                ObjProgressBar.setaMinMaxPB(0, dtBilhete.Rows.Count);
                ObjProgressBar.setaValorPB(0);
                ObjProgressBar.setaMensagem("Processando bilhetes...");

                #region Atualiza as datas inicial e final
                if (pDataImpI == null || pDataImpF == new DateTime() || pDataImpF == null || pDataImpF == new DateTime())
                {
                    DateTime? datai;
                    DateTime? dataf;
                    dal.GetDataBilhetesImportar(pDsCodigo, pManutBilhete, out datai, out dataf);
                    pdatai = datai;
                    pdataf = dataf;
                }
                else
                {
                    pdatai = pDataImpI;
                    pdataf = pDataImpF;
                }
                if (!pManutBilhete)
                {
                    //Estende o período para recalcular as marcacoes
                    DateTime dtInicial = pdatai.Value, dtFinal = pdataf.Value;
                    Modelo.cwkFuncoes.EstendePeriodo(ref dtInicial, ref dtFinal);
                    pdatai = dtInicial;
                    pdataf = dtFinal;
                    pDataImpI = pdatai;
                    pDataImpF = pdataf;
                }
                #endregion

                #region Declaração das Variáveis
                //Cria a lista dos bilhetes para importar            
                DataTable dtMarcacao = new DataTable();
                DataRow dr;
                Hashtable htFuncionario = new Hashtable();
                int auxidfunc = 0;
                int aux = 0;

                int entradaAnt = 0;
                int saidaAnt = 0;
                bool VerificaDiaAnt = false;

                int entrada = 0;
                int saida = 0;
                int limiteHoraMax = 0;
                int limiteMin = 0;
                int limiteMax = 0;

                int ordenabilhetesaida = 0;
                int horaBilhete = 0;
                int dia = 0;

                int ordembilhete = 0;
                int qtdMarIncluida = 0;
                int idBilhete = 0, idBilheteAnt = 0;
                #endregion

                #region Definição do DataRow para Marcacao
                DataColumn[] dcMarcacao = new DataColumn[]
                        {
                            new DataColumn("marcacaoid"),
                            new DataColumn("data"),
                            new DataColumn("marcacao_ent1"),
                            new DataColumn("marcacao_ent2"),
                            new DataColumn("marcacao_ent3"),
                            new DataColumn("marcacao_ent4"),
                            new DataColumn("marcacao_ent5"),
                            new DataColumn("marcacao_ent6"),
                            new DataColumn("marcacao_ent7"),
                            new DataColumn("marcacao_ent8"),
                            new DataColumn("marcacao_sai1"),
                            new DataColumn("marcacao_sai2"),
                            new DataColumn("marcacao_sai3"),
                            new DataColumn("marcacao_sai4"),
                            new DataColumn("marcacao_sai5"),
                            new DataColumn("marcacao_sai6"),
                            new DataColumn("marcacao_sai7"),
                            new DataColumn("marcacao_sai8"),
                            new DataColumn("ent_num_relogio_1"),
                            new DataColumn("ent_num_relogio_2"),
                            new DataColumn("ent_num_relogio_3"),
                            new DataColumn("ent_num_relogio_4"),
                            new DataColumn("ent_num_relogio_5"),
                            new DataColumn("ent_num_relogio_6"),
                            new DataColumn("ent_num_relogio_7"),
                            new DataColumn("ent_num_relogio_8"),
                            new DataColumn("sai_num_relogio_1"),
                            new DataColumn("sai_num_relogio_2"),
                            new DataColumn("sai_num_relogio_3"),
                            new DataColumn("sai_num_relogio_4"),
                            new DataColumn("sai_num_relogio_5"),
                            new DataColumn("sai_num_relogio_6"),
                            new DataColumn("sai_num_relogio_7"),
                            new DataColumn("sai_num_relogio_8"),
                            new DataColumn("marcacaohorario"),
                            new DataColumn("parametro_inicioadnoturno"),
                            new DataColumn("parametro_fimadnoturno"),
                            new DataColumn("horario_ordem_ent"),
                            new DataColumn("horario_limitemin"),
                            new DataColumn("horario_limitemax"),
                            new DataColumn("horario_ent1"),
                            new DataColumn("horario_ent2"),
                            new DataColumn("horario_ent3"),
                            new DataColumn("horario_ent4"),
                            new DataColumn("horario_sai1"),
                            new DataColumn("horario_sai2"),
                            new DataColumn("horario_sai3"),
                            new DataColumn("horario_sai4"),
                            new DataColumn("horario_ordenabilhetesaida"),
                            new DataColumn("jornadaid"),
                            new DataColumn("funcionarioid"),
                            new DataColumn("funcionariodscodigo"),
                            new DataColumn("acao"),
                            new DataColumn("codigo"),
                            new DataColumn("tipohoraextrafalta"),
                            new DataColumn("IdDocumentoWorkflow"),
                            new DataColumn("DocumentoWorkflowAberto"),
                            new DataColumn("LegendasConcatenadas")
                        };
                #endregion
                dtMarcacao.Columns.AddRange(dcMarcacao);

                List<string> lista = new List<string>();
                Hashtable htmarcacao = null;

                DataRow drMar;
                pLog.Add("---------------------------------------------------------------------------------------");
                #region Processa os registros para importação
                Modelo.BilhetesImp objBilhete;
                List<Modelo.BilhetesImp> bilhetesPersistir = new List<Modelo.BilhetesImp>();
                List<int> idsFuncionarios = dtBilhete.AsEnumerable().Select(r => r.Field<int>("funcionarioid")).ToList();
                for (int i = 0; i < dtBilhete.Rows.Count; i++)
                {
                    idBilhete = Convert.ToInt32(dtBilhete.Rows[i]["bimp_id"]);
                    if (idBilheteAnt == idBilhete)
                    {
                        continue;
                    }
                    idBilheteAnt = idBilhete;

                    //Cria uma hastable com os funcionarios
                    aux = dtBilhete.Rows[i]["funcionarioid"] is DBNull ? 0 : Convert.ToInt32(dtBilhete.Rows[i]["funcionarioid"]);
                    if (aux == 0)
                    {
                        continue;
                    }
                    //Carrega as listas apenas quando for utilizar pela primeira vez
                    else
                    {
                        if (htmarcacao == null)
                        {
                            //Pega a Hastable da tabela de marcacao
                            htmarcacao = dalImportacao.GetBilhetesImportar(pDsCodigo, ((DateTime)pDataImpI).AddDays(-1), ((DateTime)pDataImpF).AddDays(1));
                        }
                        if (jornadaAlternativaList == null)
                        {
                            //Pega todas as jornadas alternativas em um determinado periodo
                            jornadaAlternativaList = dalJornadaAlternativa.GetHashIdObjeto(pDataImpI.Value.AddDays(-1), pDataImpF.Value.AddDays(1), 2, idsFuncionarios);
                        }
                        if (mudancaHorarioList == null)
                        {
                            mudancaHorarioList = dalMudancaHorario.GetPeriodo(pdatai.Value.AddDays(-1), pdataf.Value.AddDays(1), idsFuncionarios);

                            if (mudancaHorarioList.Count > 0)
                            {
                                List<int> ids = new List<int>();
                                foreach (Modelo.MudancaHorario mud in mudancaHorarioList)
                                {
                                    if (!ids.Contains(mud.Idhorario))
                                        ids.Add(mud.Idhorario);

                                    if (!ids.Contains(mud.Idhorario_ant))
                                        ids.Add(mud.Idhorario_ant);
                                }
                                tiposHoraExtraFalta = dalImportacao.GetTipoHExtraFaltaHorarios(ids);
                            }
                        }
                    }

                    if (auxidfunc != aux)
                    {
                        auxidfunc = Convert.ToInt32(dtBilhete.Rows[i]["funcionarioid"]);

                        if (!htFuncionario.ContainsKey(auxidfunc))
                            htFuncionario.Add(auxidfunc, dtBilhete.Rows[i]);

                        ObjProgressBar.setaMensagem("Funcionário(a): " + dtBilhete.Rows[i]["funcionarionome"].ToString());
                    }

                    horaBilhete = Convert.ToInt32(dtBilhete.Rows[i]["hora"]);
                    VerificaBilheteHashTableMarcacao(dtBilhete.Rows[i], dtMarcacao, htmarcacao, (DateTime)dtBilhete.Rows[i]["mar_data"]);

                    if (Convert.ToInt32(dtBilhete.Rows[i]["importado"]) == 0)
                    {
                        dia = ValidaDataBilhete(dtBilhete, ref entradaAnt, ref saidaAnt, ref VerificaDiaAnt, ref entrada, ref saida, limiteHoraMax, ref limiteMin, ref limiteMax, ref ordenabilhetesaida, ref horaBilhete, htmarcacao, i, out drMar, dtMarcacao, qtdMarIncluida);

                        VerificaBilheteHashTableMarcacao(dtBilhete.Rows[i], dtMarcacao, htmarcacao, ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia));
                    }
                    else
                    {
                        dia = 0;
                    }

                    //Carrega a Marcação na Data Correta
                    drMar = (DataRow)htmarcacao[String.Format("{0:dd/MM/yyyy}", ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia)) + dtBilhete.Rows[i]["funcionarioid"].ToString()];
                    ordembilhete = drMar["horario_ordem_ent"] is DBNull ? 0 : Convert.ToInt32(drMar["horario_ordem_ent"]);

                    //Encaixa Bilhete na Marcação
                    if (ordembilhete == 0 || new string[] { "000", String.Empty }.Contains((string)dtBilhete.Rows[i]["ordem"]))
                    {
                        #region Atribui Entrada Saida
                        for (int m = 1; m < 9; m++)
                        {
                            var horaEntrada = Convert.ToInt32(drMar["marcacao_ent" + m.ToString()]);
                            var horaSaida = Convert.ToInt32(drMar["marcacao_sai" + m.ToString()]);
                            // Caso não exista a marcação insere, se existir apenas atualiza
                            if ((horaEntrada == -1) || horaEntrada == horaBilhete)
                            {
                                AtribuiMarcacaoEnt(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                break;
                            }
                            else if ((horaSaida == -1) || horaSaida == horaBilhete)
                            {
                                AtribuiMarcacaiSai(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (((string)dtBilhete.Rows[i]["ordem"] == "010") || ((string)dtBilhete.Rows[i]["ordem"] == "110"))
                        {
                            #region Atribui Entradas
                            for (int m = 1; m < 9; m++)
                            {
                                var horaEntrada = Convert.ToInt32(drMar["marcacao_ent" + m.ToString()]);
                                // Caso não exista a marcação insere, se existir apenas atualiza
                                if ((horaEntrada == -1) || horaEntrada == horaBilhete)
                                {
                                    AtribuiMarcacaoEnt(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                    break;
                                }
                            }
                            #endregion
                        }
                        else if (((string)dtBilhete.Rows[i]["ordem"] == "011") || ((string)dtBilhete.Rows[i]["ordem"] == "111"))
                        {
                            #region Atribui Saidas
                            for (int m = 1; m < 9; m++)
                            {
                                var horaSaida = Convert.ToInt32(drMar["marcacao_sai" + m.ToString()]);
                                // Caso não exista a marcação insere, se existir apenas atualiza
                                if ((horaSaida == -1) || horaSaida == horaBilhete)
                                {
                                    AtribuiMarcacaiSai(dtBilhete, drMar, horaBilhete, (string)dtBilhete.Rows[i]["relogio"], m, i);
                                    break;
                                }
                            }
                            #endregion
                        }
                    }

                    if (Convert.ToInt32(drMar["marcacaoid"]) != 0)
                        drMar["acao"] = 2; //Alterar

                    //Atualiza o valor das marcações após o encaixe
                    htmarcacao[String.Format("{0:dd/MM/yyyy}", ((DateTime)dtBilhete.Rows[i]["mar_data"]).AddDays(dia)) + dtBilhete.Rows[i]["funcionarioid"].ToString()] = drMar;

                    dtBilhete.Rows[i]["mar_data"] = Convert.ToDateTime(drMar["data"]);
                    dtBilhete.Rows[i]["mar_hora"] = Modelo.cwkFuncoes.ConvertMinutosBatida(horaBilhete);
                    dtBilhete.Rows[i]["mar_relogio"] = dtBilhete.Rows[i]["relogio"];
                    dtBilhete.Rows[i]["importado"] = 1;
                    dtBilhete.Rows[i]["bildscodigo"] = drMar["funcionariodscodigo"];
                    dtBilhete.Rows[i]["acao"] = 2; //Alterar

                    //Salva a alteração do bilhete na lista
                    objBilhete = this.PreencheBilheteComDataRow(dtBilhete.Rows[i]);
                    bilhetesPersistir.Add(objBilhete);

                    ObjProgressBar.incrementaPB(1);
                }
                dtBilhete.Dispose();
                #endregion

                if (htmarcacao == null)
                {
                    //Pega a Hastable da tabela de marcacao
                    htmarcacao = dalImportacao.GetBilhetesImportar(pDsCodigo, ((DateTime)pDataImpI).AddDays(-1), pDataImpF);
                }
                if (mudancaHorarioList == null)
                {
                    mudancaHorarioList = dalMudancaHorario.GetPeriodo(pdatai.Value.AddDays(-1), pdataf.Value, idsFuncionarios);
                    if (mudancaHorarioList.Count > 0)
                    {
                        List<int> ids = new List<int>();
                        foreach (Modelo.MudancaHorario mud in mudancaHorarioList)
                        {
                            if (!ids.Contains(mud.Idhorario))
                                ids.Add(mud.Idhorario);

                            if (!ids.Contains(mud.Idhorario_ant))
                                ids.Add(mud.Idhorario_ant);
                        }
                        tiposHoraExtraFalta = dalImportacao.GetTipoHExtraFaltaHorarios(ids);
                    }
                }
                listaFuncionarios = dalImportacao.GetFuncionariosImportacaoWebApi(pDsCodigo);

                #region Verifica se no periodo estendido o funcionario possui marcação
                ObjProgressBar.setaMinMaxPB(0, listaFuncionarios.Rows.Count);
                ObjProgressBar.setaValorPB(0);
                ObjProgressBar.setaMensagem("Verificando Marcações...");
                foreach (DataRow funcionario in listaFuncionarios.Rows)
                {
                    DateTime dt = pdatai.Value;
                    while (dt <= pdataf)
                    {
                        if (!htmarcacao.ContainsKey(String.Format("{0:dd/MM/yyyy}", dt) + funcionario["id"].ToString()))
                        {
                            IncluiMarcacaoLimpa(dtMarcacao, htmarcacao, dt, funcionario);
                        }
                        dt = dt.AddDays(1);
                    }
                    ObjProgressBar.incrementaPB(1);
                }
                #endregion

                #region Processa o Hastable para gerar as marcações
                ObjProgressBar.setaMinMaxPB(0, htmarcacao.Count);
                ObjProgressBar.setaValorPB(0);
                ObjProgressBar.setaMensagem("Preparando atualização das Marcações...");
                DateTime auxData;
                int count = 0;
                List<Modelo.Marcacao> marcacoesPersistir = new List<Modelo.Marcacao>();
                BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
                foreach (DictionaryEntry de in htmarcacao)
                {
                    dr = (DataRow)de.Value;

                    if (Convert.ToInt32(dr["acao"]) == 0)
                        continue;

                    objMarcacao = this.PreencheMarcacaoComDataRowWebApi(dr, login);
                    marcacoesPersistir.Add(objMarcacao);
                    count++;

                    ObjProgressBar.incrementaPB(1);
                }
                if (marcacoesPersistir.Count > 0)
                {
                    auxData = marcacoesPersistir.Min(m => m.Data);
                    if (auxData < pdatai)
                        pdatai = auxData;
                    auxData = marcacoesPersistir.Max(m => m.Data);
                    if (auxData > pdataf)
                        pdataf = auxData;
                }

                htmarcacao.Clear();
                pLog.Add("Marcações Processadas = " + count);
                #endregion

                ObjProgressBar.setaMinMaxPB(0, 1);
                ObjProgressBar.setaValorPB(1);
                ObjProgressBar.setaMensagem("Salvando Marcações...");
                foreach (var marc in marcacoesPersistir)
                {
                    marc.BilhetesMarcacao.AddRange(bilhetesPersistir.Where(w => w.Mar_data == marc.Data && w.DsCodigo == marc.Dscodigo).ToList());
                    bllMarcacao.OrdenaMarcacao(marc, true);
                }
                dalImportacao.PersisteDadosWebApi(marcacoesPersistir, bilhetesPersistir, login);
                return true;
            }
            //Não importou nenhum bilhete
            else
            {
                pLog.Add("Marcações Processadas = 0");
                pdatai = null;
                pdataf = null;
                return false;
            }
        }

        public void PersisteDados(List<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes)
        {
            dalImportacao.PersisteDados(marcacoes, bilhetes);
        }

        private void VerificaBilheteHashTableMarcacao(DataRow dr, DataTable dtMarcacao, Hashtable htmarcacao, DateTime pData)
        {
            //Verifica se o bilhete tem na tabela htmarcacao            
            if (!htmarcacao.ContainsKey(String.Format("{0:dd/MM/yyyy}", pData) + dr["funcionarioid"].ToString()))
            {
                IncluiMarcacao(dr, dtMarcacao, htmarcacao, pData);
            }
        }

        public DataTable ReturnFuncsWithoutDsCod(DateTime? dataI, DateTime? dataF)
        {
            return dalImportacao.GetFuncsWithoutDscodigo(dataI, dataF);
        }

        private void IncluiMarcacao(DataRow dr, DataTable dtMarcacao, Hashtable htmarcacao, DateTime pData)
        {
            if (maxcodMarcacao == 0)
            {
                maxcodMarcacao = dalMar.MaxCodigo();
            }
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            bool ordenaBilheteSaida = Convert.ToBoolean(dr["horario_ordenabilhetesaida"]);
            object[] jornada = new object[8];
            legenda = "";
            idhorario = 0;
            tipohoraextrafalta = Convert.ToInt16(dr["tipohoraextrafalta"]);
            bllMarcacao.VerificaMudancaHorario(Convert.ToInt32(dr["funcionarioid"]), pData, mudancaHorarioList, ref legenda, ref idhorario);
            if (idhorario > 0)
            {
                tiposHoraExtraFalta.TryGetValue(idhorario, out tipohoraextrafalta);
            }

            if (ordenaBilheteSaida)
            {
                if (horariosOrdenaSaidaList == null)
                {
                    horariosOrdenaSaidaList = dalHorarioDetalhe.LoadHorariosOrdenaSaida();
                }
                int key;
                if (idhorario > 0 && horariosOrdenaSaidaList.ContainsKey(idhorario))
                    key = idhorario;
                else
                    key = (int)dr["idhorario"];
                Modelo.pxyHorarioDetalheImportacao horario = (Modelo.pxyHorarioDetalheImportacao)horariosOrdenaSaidaList[key];
                Modelo.HorarioDetalhe hd;
                IEnumerable<Modelo.HorarioDetalhe> auxHor;
                if (horario.tipoHorario == 1)
                {
                    auxHor = horario.horariosDetalhe.Where(h => h.Dia == Modelo.cwkFuncoes.Dia(pData));
                }
                else
                {
                    auxHor = horario.horariosDetalhe.Where(h => h.Data == pData);
                }
                if (auxHor.Count() > 0)
                    hd = auxHor.First();
                else
                    hd = new Modelo.HorarioDetalhe();

                jornada[0] = hd.EntradaMin_1;
                jornada[1] = hd.EntradaMin_2;
                jornada[2] = hd.EntradaMin_3;
                jornada[3] = hd.EntradaMin_4;
                jornada[4] = hd.SaidaMin_1;
                jornada[5] = hd.SaidaMin_2;
                jornada[6] = hd.SaidaMin_3;
                jornada[7] = hd.SaidaMin_3;
            }
            else
            {
                jornada[0] = dr["horario_ent1"];
                jornada[1] = dr["horario_ent2"];
                jornada[2] = dr["horario_ent3"];
                jornada[3] = dr["horario_ent4"];
                jornada[4] = dr["horario_sai1"];
                jornada[5] = dr["horario_sai2"];
                jornada[6] = dr["horario_sai3"];
                jornada[7] = dr["horario_sai4"];
            }

            object[] values = new object[]
                        {
                            0,
                            pData,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            idhorario > 0 ? idhorario : dr["idhorario"],
                            dr["parametro_inicioadnoturno"],
                            dr["parametro_fimadnoturno"],
                            dr["horario_ordem_ent"],
                            dr["horario_limitemin"],
                            dr["horario_limitemax"],
                            jornada[0],
                            jornada[1],
                            jornada[2],
                            jornada[3],
                            jornada[4],
                            jornada[5],
                            jornada[6],
                            jornada[7],
                            dr["horario_ordenabilhetesaida"],
                            dr["jornadaid"],
                            dr["funcionarioid"],
                            dr["func"],
                            1,
                            maxcodMarcacao,
                            tipohoraextrafalta
                        };

            dtMarcacao.Rows.Add(values);

            htmarcacao.Add(String.Format("{0:dd/MM/yyyy}", pData) + dr["funcionarioid"].ToString(), dtMarcacao.Rows[dtMarcacao.Rows.Count - 1]);
        }

        private void IncluiMarcacaoLimpa(DataTable dtMarcacao, Hashtable htmarcacao, DateTime pData, DataRow funcionario)
        {
            if (maxcodMarcacao == 0)
            {
                maxcodMarcacao = dalMar.MaxCodigo();
            }
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            legenda = "";
            idhorario = 0;
            idfunc = Convert.ToInt32(funcionario["id"]);
            tipohoraextrafalta = Convert.ToInt16(funcionario["tipohoraextrafalta"]);
            bllMarcacao.VerificaMudancaHorario(idfunc, pData, mudancaHorarioList, ref legenda, ref idhorario);
            if (idhorario > 0)
            {
                tiposHoraExtraFalta.TryGetValue(idhorario, out tipohoraextrafalta);
                //tipohoraextrafalta = tiposHoraExtraFalta.Where(t => t.Key == idhorario).First().Value;
            }
            object[] values = new object[]
                        {
                            0,
                            pData,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            "",
                            idhorario > 0 ? idhorario : Convert.ToInt32(funcionario["idhorario"]),
                            -1,
                            -1,
                            0,
                            0,
                            0,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            -1,
                            0,
                            0,
                            idfunc,
                            funcionario["dscodigo"].ToString(),
                            1,
                            maxcodMarcacao,
                            tipohoraextrafalta
                        };

            dtMarcacao.Rows.Add(values);

            htmarcacao.Add(String.Format("{0:dd/MM/yyyy}", pData) + idfunc.ToString(), dtMarcacao.Rows[dtMarcacao.Rows.Count - 1]);
        }

        private void AtribuiMarcacaoEnt(DataTable dtBilhete, DataRow drMar, int horaBilhete, string relogio, int i, int pIndexBilhete)
        {
            try
            {
                drMar["marcacao_ent" + i.ToString()] = horaBilhete;
                drMar["ent_num_relogio_" + i.ToString()] = relogio;
                dtBilhete.Rows[pIndexBilhete]["ent_sai"] = "E";
                dtBilhete.Rows[pIndexBilhete]["posicao"] = i;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void AtribuiMarcacaiSai(DataTable dtBilhete, DataRow drMar, int horaBilhete, string relogio, int i, int pIndexBilhete)
        {
            try
            {
                drMar["marcacao_sai" + i.ToString()] = horaBilhete;
                drMar["sai_num_relogio_" + i.ToString()] = relogio;
                dtBilhete.Rows[pIndexBilhete]["ent_sai"] = "S";
                dtBilhete.Rows[pIndexBilhete]["posicao"] = i;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Método que Valida se o bilhete que está sendo importado pertence ou não aquela data
        /// </summary>
        private int ValidaDataBilhete(DataTable dtBilhete, ref int entradaAnt, ref int saidaAnt, ref bool VerificaDiaAnt, ref int entrada, ref int saida, int limiteHoraMax, ref int limiteMin, ref int limiteMax, ref int ordenabilhetesaida, ref int horaBilhete, Hashtable htmarcacao, int i, out DataRow drMar, DataTable dtMarcacao, int qtdMarIncluida)
        {
            int idja = 0; //ID da Jornada Alternativa
            int limiteMinAnt = 0
            , limiteMaxAnt = 0
            , horaMaxDiaAnt = 0;
            bool possuihorarioDia = true, possuihorarioAnt = true, possuiHorarioPost = true;
            bool extrapolouAnterior = false, extrapolouPosterior = false;
            int horaMinDiaPosterior = 0;

            #region DiaAnterior
            //Verifica se existe a marcação do dia anterior
            if (!htmarcacao.ContainsKey(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtBilhete.Rows[i]["mar_data"]).AddDays(-1)) + dtBilhete.Rows[i]["funcionarioid"].ToString()))
            {
                IncluiMarcacao(dtBilhete.Rows[i], dtMarcacao, htmarcacao, Convert.ToDateTime(dtBilhete.Rows[i]["mar_data"]).AddDays(-1));
            }

            //Calcula Menor Entrada e Maior Saida do dia anterior
            DataRow drMarAnt = (DataRow)htmarcacao[String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtBilhete.Rows[i]["mar_data"]).AddDays(-1)) + dtBilhete.Rows[i]["funcionarioid"].ToString()];

            //Pega o ID da Jornada Alternativa do dia anterior
            idja = Convert.ToInt32(drMarAnt["jornadaid"]);

            //Verifica se tem jornada alternativa
            if (idja != 0)
            {
                entradaAnt = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Entrada_1);

                if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_4 != "--:--")
                    saidaAnt = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_4);
                else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_3 != "--:--")
                    saidaAnt = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_3);
                else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_2 != "--:--")
                    saidaAnt = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_2);
                else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_1 != "--:--")
                    saidaAnt = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_1);
                else
                {
                    saidaAnt = -1;
                    entradaAnt = -1;
                    possuihorarioAnt = false;
                }

                limiteMaxAnt = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).LimiteMax);
                limiteMinAnt = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).LimiteMin);
            }
            else
            {
                entradaAnt = Convert.ToInt32(drMarAnt["horario_ent1"]);

                //Encontra á última Saida
                if (Convert.ToInt32(drMarAnt["horario_sai4"]) >= 0)
                    saidaAnt = Convert.ToInt32(drMarAnt["horario_sai4"]);
                else if (Convert.ToInt32(drMarAnt["horario_sai3"]) >= 0)
                    saidaAnt = Convert.ToInt32(drMarAnt["horario_sai3"]);
                else if (Convert.ToInt32(drMarAnt["horario_sai2"]) >= 0)
                    saidaAnt = Convert.ToInt32(drMarAnt["horario_sai2"]);
                else if (Convert.ToInt32(drMarAnt["horario_sai1"]) >= 0)
                    saidaAnt = Convert.ToInt32(drMarAnt["horario_sai1"]);
                else
                {
                    saidaAnt = -1;
                    entradaAnt = -1;
                    possuihorarioAnt = false;
                }

                limiteMaxAnt = Convert.ToInt32(drMarAnt["horario_limitemax"]);
                limiteMinAnt = Convert.ToInt32(drMarAnt["horario_limitemin"]);
            }

            //Calcula a última hora valida para o dia anterior
            if (saidaAnt == -1)
                horaMaxDiaAnt = 0;
            else
            {
                if (saidaAnt < entradaAnt)
                {
                    horaMaxDiaAnt = saidaAnt + limiteMaxAnt;
                    extrapolouAnterior = true;
                }
                else if ((1440 - saidaAnt) < limiteMaxAnt)
                {
                    horaMaxDiaAnt = limiteMaxAnt - (1440 - saidaAnt);
                    extrapolouAnterior = true;
                }
                else
                    horaMaxDiaAnt = saidaAnt + limiteMaxAnt;
            }
            #endregion

            #region DiaAtual
            //Pega as informações da marcaçao
            drMar = (DataRow)htmarcacao[String.Format("{0:dd/MM/yyyy}", dtBilhete.Rows[i]["mar_data"]) + dtBilhete.Rows[i]["funcionarioid"].ToString()];
            ordenabilhetesaida = Convert.ToInt32(drMar["horario_ordenabilhetesaida"]);

            //Pega o ID da Jornada Alternativa do dia anterior
            idja = Convert.ToInt32(drMar["jornadaid"]);

            //Verifica se tem jornada alternativa
            if (idja != 0)
            {
                entrada = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Entrada_1);
                limiteMin = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).LimiteMin);
                limiteMax = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).LimiteMax);

                if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_4 != "--:--")
                    saida = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_4);
                else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_3 != "--:--")
                    saida = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_3);
                else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_2 != "--:--")
                    saida = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_2);
                else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_1 != "--:--")
                    saida = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_1);
                else
                {
                    entrada = -1;
                    saida = -1;
                    possuihorarioDia = false;
                }
            }
            else
            {
                entrada = Convert.ToInt32(drMar["horario_ent1"]);
                limiteMin = Convert.ToInt32(drMar["horario_limitemin"]);
                limiteMax = Convert.ToInt32(drMar["horario_limitemax"]);

                //Encontra á última Saida
                if (Convert.ToInt32(drMar["horario_sai4"]) > 0)
                    saida = Convert.ToInt32(drMar["horario_sai4"]);
                else if (Convert.ToInt32(drMar["horario_sai3"]) > 0)
                    saida = Convert.ToInt32(drMar["horario_sai3"]);
                else if (Convert.ToInt32(drMar["horario_sai2"]) > 0)
                    saida = Convert.ToInt32(drMar["horario_sai2"]);
                else if (Convert.ToInt32(drMar["horario_sai1"]) > 0)
                    saida = Convert.ToInt32(drMar["horario_sai1"]);
                else
                {
                    entrada = -1;
                    saida = -1;
                    possuihorarioDia = false;
                }
            }

            if (!possuihorarioDia && possuihorarioAnt)
            {
                entrada = entradaAnt;
                saida = saidaAnt;
                limiteMin = limiteMinAnt;
                limiteMax = limiteMaxAnt;
                possuihorarioDia = true;
            }

            int horaMinimaDia = 0;
            int horaMaximaDia = 0;
            if (possuihorarioDia)
            {
                if (entrada <= limiteMin)
                {
                    horaMinimaDia = (1440 - entrada) - (limiteMin - entrada);
                }
                else
                {
                    horaMinimaDia = entrada - limiteMin;
                }

                if ((1440 - saida) < limiteMax)
                {
                    horaMaximaDia = limiteMax - (1440 - saida);
                }
                else
                {
                    horaMaximaDia = saida + limiteMax;
                }
            }
            #endregion

            if (ordenabilhetesaida == 1)
            {
                #region DiaPosterior
                int entradaPost, saidaPost, limiteMinPost;

                //Verifica se existe a marcação do dia posterior
                if (!htmarcacao.ContainsKey(String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtBilhete.Rows[i]["mar_data"]).AddDays(1)) + dtBilhete.Rows[i]["funcionarioid"].ToString()))
                {
                    IncluiMarcacao(dtBilhete.Rows[i], dtMarcacao, htmarcacao, Convert.ToDateTime(dtBilhete.Rows[i]["mar_data"]).AddDays(1));
                }

                //Calcula Menor Entrada e Maior Saida do dia anterior
                DataRow drMarPost = (DataRow)htmarcacao[String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtBilhete.Rows[i]["mar_data"]).AddDays(1)) + dtBilhete.Rows[i]["funcionarioid"].ToString()];

                //Pega o ID da Jornada Alternativa do dia anterior
                idja = Convert.ToInt32(drMarPost["jornadaid"]);

                //Verifica se tem jornada alternativa
                if (idja != 0)
                {
                    entradaPost = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Entrada_1);

                    if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_4 != "--:--")
                        saidaPost = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_4);
                    else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_3 != "--:--")
                        saidaPost = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_3);
                    else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_2 != "--:--")
                        saidaPost = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_2);
                    else if (((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_1 != "--:--")
                        saidaPost = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).Saida_1);
                    else
                    {
                        saidaPost = -1;
                        entradaPost = -1;
                        possuiHorarioPost = false;
                    }

                    limiteMinPost = Modelo.cwkFuncoes.ConvertHorasMinuto(((Modelo.JornadaAlternativa)jornadaAlternativaList[idja]).LimiteMin);
                }
                else
                {
                    entradaPost = Convert.ToInt32(drMarPost["horario_ent1"]);

                    //Encontra a última Saida
                    if (Convert.ToInt32(drMarPost["horario_sai4"]) >= 0)
                        saidaPost = Convert.ToInt32(drMarPost["horario_sai4"]);
                    else if (Convert.ToInt32(drMarPost["horario_sai3"]) >= 0)
                        saidaPost = Convert.ToInt32(drMarPost["horario_sai3"]);
                    else if (Convert.ToInt32(drMarPost["horario_sai2"]) >= 0)
                        saidaPost = Convert.ToInt32(drMarPost["horario_sai2"]);
                    else if (Convert.ToInt32(drMarPost["horario_sai1"]) >= 0)
                        saidaPost = Convert.ToInt32(drMarPost["horario_sai1"]);
                    else
                    {
                        saidaPost = -1;
                        entradaPost = -1;
                        possuiHorarioPost = false;
                    }

                    limiteMinPost = Convert.ToInt32(drMarPost["horario_limitemin"]);
                }

                if (possuiHorarioPost)
                {
                    if ((entradaPost - limiteMinPost) < 0)
                    {
                        horaMinDiaPosterior = 1440 - (limiteMinPost - entradaPost);
                        extrapolouPosterior = true;
                    }
                }
                #endregion
            }

            //Chama a função que encontra o dia da marcação do Bilhete
            return BLL.CalculoHoras.RegraDiaAnterior(horaBilhete, horaMinimaDia, possuihorarioDia, horaMaxDiaAnt, extrapolouAnterior, horaMinDiaPosterior, extrapolouPosterior);
        }

        public Modelo.Marcacao PreencheMarcacaoComDataRow(DataRow dr)
        {
            if (maxcodMarcacao == 0)
            {
                maxcodMarcacao = dalMar.MaxCodigo();
            }
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();

            objMarcacao.Id = Convert.ToInt32(dr["marcacaoid"]);
            objMarcacao.Idfuncionario = Convert.ToInt32(dr["funcionarioid"]);
            objMarcacao.Dscodigo = dr["funcionariodscodigo"].ToString();
            objMarcacao.Data = Convert.ToDateTime(dr["data"]);
            objMarcacao.Dia = Modelo.cwkFuncoes.DiaSemana(objMarcacao.Data, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
            objMarcacao.Idhorario = Convert.ToInt32(dr["marcacaohorario"]);
            objMarcacao.Entrada_1 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent1"]));
            objMarcacao.Entrada_2 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent2"]));
            objMarcacao.Entrada_3 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent3"]));
            objMarcacao.Entrada_4 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent4"]));
            objMarcacao.Entrada_5 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent5"]));
            objMarcacao.Entrada_6 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent6"]));
            objMarcacao.Entrada_7 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent7"]));
            objMarcacao.Entrada_8 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent8"]));
            objMarcacao.Saida_1 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai1"]));
            objMarcacao.Saida_2 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai2"]));
            objMarcacao.Saida_3 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai3"]));
            objMarcacao.Saida_4 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai4"]));
            objMarcacao.Saida_5 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai5"]));
            objMarcacao.Saida_6 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai6"]));
            objMarcacao.Saida_7 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai7"]));
            objMarcacao.Saida_8 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai8"]));
            objMarcacao.Ent_num_relogio_1 = dr["ent_num_relogio_1"].ToString();
            objMarcacao.Ent_num_relogio_2 = dr["ent_num_relogio_2"].ToString();
            objMarcacao.Ent_num_relogio_3 = dr["ent_num_relogio_3"].ToString();
            objMarcacao.Ent_num_relogio_4 = dr["ent_num_relogio_4"].ToString();
            objMarcacao.Ent_num_relogio_5 = dr["ent_num_relogio_5"].ToString();
            objMarcacao.Ent_num_relogio_6 = dr["ent_num_relogio_6"].ToString();
            objMarcacao.Ent_num_relogio_7 = dr["ent_num_relogio_7"].ToString();
            objMarcacao.Ent_num_relogio_8 = dr["ent_num_relogio_8"].ToString();
            objMarcacao.Sai_num_relogio_1 = dr["sai_num_relogio_1"].ToString();
            objMarcacao.Sai_num_relogio_2 = dr["sai_num_relogio_2"].ToString();
            objMarcacao.Sai_num_relogio_3 = dr["sai_num_relogio_3"].ToString();
            objMarcacao.Sai_num_relogio_4 = dr["sai_num_relogio_4"].ToString();
            objMarcacao.Sai_num_relogio_5 = dr["sai_num_relogio_5"].ToString();
            objMarcacao.Sai_num_relogio_6 = dr["sai_num_relogio_6"].ToString();
            objMarcacao.Sai_num_relogio_7 = dr["sai_num_relogio_7"].ToString();
            objMarcacao.Sai_num_relogio_8 = dr["sai_num_relogio_8"].ToString();
            objMarcacao.Incdata = System.DateTime.Now;
            objMarcacao.Inchora = System.DateTime.Now;
            if (UsuarioLogado != null && !String.IsNullOrEmpty(UsuarioLogado.Login))
                objMarcacao.Incusuario = UsuarioLogado.Login;
            else
                objMarcacao.Incusuario = cwkControleUsuario.Facade.getUsuarioLogado.Login;
            objMarcacao.Codigo = maxcodMarcacao;
            objMarcacao.Horastrabalhadas = "--:--";
            objMarcacao.Horasextrasdiurna = "--:--";
            objMarcacao.Horasfaltas = "--:--";
            objMarcacao.Entradaextra = "--:--";
            objMarcacao.Saidaextra = "--:--";
            objMarcacao.Horastrabalhadasnoturnas = "--:--";
            objMarcacao.Horasextranoturna = "--:--";
            objMarcacao.Horasfaltanoturna = "--:--";
            objMarcacao.Bancohorascre = "---:--";
            objMarcacao.Bancohorasdeb = "---:--";
            objMarcacao.Acao = (Modelo.Acao)Convert.ToInt32(dr["acao"]);
            objMarcacao.Chave = objMarcacao.ToMD5();
            objMarcacao.ExpHorasextranoturna = "--:--";
            objMarcacao.TipoHoraExtraFalta = Convert.ToInt16(dr["tipohoraextrafalta"]);
            objMarcacao.Legenda = String.Empty;
            objMarcacao.IdDocumentoWorkflow = dr["IdDocumentoWorkflow"] is DBNull ? 0 : Convert.ToInt32(dr["IdDocumentoWorkflow"]);
            objMarcacao.DocumentoWorkflowAberto = dr["DocumentoWorkflowAberto"] is DBNull ? false : Convert.ToBoolean(dr["DocumentoWorkflowAberto"]);
            objMarcacao.LegendasConcatenadas = Convert.ToString(dr["LegendasConcatenadas"]);


            return objMarcacao;
        }

        public Modelo.Marcacao PreencheMarcacaoComDataRowWebApi(DataRow dr, string login)
        {
            if (maxcodMarcacao == 0)
            {
                maxcodMarcacao = dalMar.MaxCodigo();
            }
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();

            objMarcacao.Id = Convert.ToInt32(dr["marcacaoid"]);
            objMarcacao.Idfuncionario = Convert.ToInt32(dr["funcionarioid"]);
            objMarcacao.Dscodigo = dr["funcionariodscodigo"].ToString();
            objMarcacao.Data = Convert.ToDateTime(dr["data"]);
            objMarcacao.Dia = Modelo.cwkFuncoes.DiaSemana(objMarcacao.Data, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
            objMarcacao.Idhorario = Convert.ToInt32(dr["marcacaohorario"]);
            objMarcacao.Entrada_1 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent1"]));
            objMarcacao.Entrada_2 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent2"]));
            objMarcacao.Entrada_3 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent3"]));
            objMarcacao.Entrada_4 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent4"]));
            objMarcacao.Entrada_5 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent5"]));
            objMarcacao.Entrada_6 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent6"]));
            objMarcacao.Entrada_7 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent7"]));
            objMarcacao.Entrada_8 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_ent8"]));
            objMarcacao.Saida_1 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai1"]));
            objMarcacao.Saida_2 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai2"]));
            objMarcacao.Saida_3 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai3"]));
            objMarcacao.Saida_4 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai4"]));
            objMarcacao.Saida_5 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai5"]));
            objMarcacao.Saida_6 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai6"]));
            objMarcacao.Saida_7 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai7"]));
            objMarcacao.Saida_8 = Modelo.cwkFuncoes.ConvertMinutosBatida(Convert.ToInt32(dr["marcacao_sai8"]));
            objMarcacao.Ent_num_relogio_1 = dr["ent_num_relogio_1"].ToString();
            objMarcacao.Ent_num_relogio_2 = dr["ent_num_relogio_2"].ToString();
            objMarcacao.Ent_num_relogio_3 = dr["ent_num_relogio_3"].ToString();
            objMarcacao.Ent_num_relogio_4 = dr["ent_num_relogio_4"].ToString();
            objMarcacao.Ent_num_relogio_5 = dr["ent_num_relogio_5"].ToString();
            objMarcacao.Ent_num_relogio_6 = dr["ent_num_relogio_6"].ToString();
            objMarcacao.Ent_num_relogio_7 = dr["ent_num_relogio_7"].ToString();
            objMarcacao.Ent_num_relogio_8 = dr["ent_num_relogio_8"].ToString();
            objMarcacao.Sai_num_relogio_1 = dr["sai_num_relogio_1"].ToString();
            objMarcacao.Sai_num_relogio_2 = dr["sai_num_relogio_2"].ToString();
            objMarcacao.Sai_num_relogio_3 = dr["sai_num_relogio_3"].ToString();
            objMarcacao.Sai_num_relogio_4 = dr["sai_num_relogio_4"].ToString();
            objMarcacao.Sai_num_relogio_5 = dr["sai_num_relogio_5"].ToString();
            objMarcacao.Sai_num_relogio_6 = dr["sai_num_relogio_6"].ToString();
            objMarcacao.Sai_num_relogio_7 = dr["sai_num_relogio_7"].ToString();
            objMarcacao.Sai_num_relogio_8 = dr["sai_num_relogio_8"].ToString();
            objMarcacao.Incdata = System.DateTime.Now;
            objMarcacao.Inchora = System.DateTime.Now;
            objMarcacao.Incusuario = login;
            objMarcacao.Codigo = maxcodMarcacao;
            objMarcacao.Horastrabalhadas = "--:--";
            objMarcacao.Horasextrasdiurna = "--:--";
            objMarcacao.Horasfaltas = "--:--";
            objMarcacao.Entradaextra = "--:--";
            objMarcacao.Saidaextra = "--:--";
            objMarcacao.Horastrabalhadasnoturnas = "--:--";
            objMarcacao.Horasextranoturna = "--:--";
            objMarcacao.Horasfaltanoturna = "--:--";
            objMarcacao.Bancohorascre = "---:--";
            objMarcacao.Bancohorasdeb = "---:--";
            objMarcacao.Acao = (Modelo.Acao)Convert.ToInt32(dr["acao"]);
            objMarcacao.Chave = objMarcacao.ToMD5();
            objMarcacao.ExpHorasextranoturna = "--:--";
            objMarcacao.TipoHoraExtraFalta = Convert.ToInt16(dr["tipohoraextrafalta"]);
            objMarcacao.Legenda = String.Empty;
            objMarcacao.IdDocumentoWorkflow = dr["IdDocumentoWorkflow"] is DBNull ? 0 : Convert.ToInt32(dr["IdDocumentoWorkflow"]);
            objMarcacao.DocumentoWorkflowAberto = dr["DocumentoWorkflowAberto"] is DBNull ? false : Convert.ToBoolean(dr["DocumentoWorkflowAberto"]);
            objMarcacao.LegendasConcatenadas = Convert.ToString(dr["LegendasConcatenadas"]);
            return objMarcacao;
        }

        private Modelo.BilhetesImp PreencheBilheteComDataRow(DataRow dr)
        {
            Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();

            objBilhete.Id = Convert.ToInt32(dr["bimp_id"]);
            objBilhete.Ordem = dr["ordem"] is DBNull ? "" : (string)dr["ordem"];
            objBilhete.Data = Convert.ToDateTime(dr["data"]);
            objBilhete.Hora = Modelo.cwkFuncoes.ConvertMinutosBatida((int)dr["hora"]);
            objBilhete.Func = (string)dr["func"];
            objBilhete.Relogio = (string)dr["relogio"];
            objBilhete.Importado = Convert.ToInt16(dr["importado"]);
            objBilhete.Mar_data = Convert.ToDateTime(dr["mar_data"]);
            objBilhete.Mar_hora = (string)dr["mar_hora"];
            objBilhete.Mar_relogio = (string)dr["mar_relogio"];
            objBilhete.Posicao = Convert.ToInt32(dr["posicao"]);
            objBilhete.Ent_sai = dr["ent_sai"] is DBNull ? String.Empty : (string)dr["ent_sai"];
            objBilhete.DsCodigo = (string)dr["bildscodigo"];
            objBilhete.Ocorrencia = dr["ocorrencia"] is DBNull ? new char() : Convert.ToChar(dr["ocorrencia"]);
            objBilhete.Motivo = dr["motivo"] is DBNull ? String.Empty : Convert.ToString(dr["motivo"]);
            objBilhete.Idjustificativa = dr["idjustificativa"] is DBNull ? 0 : Convert.ToInt32(dr["idjustificativa"]);
            objBilhete.Nsr = dr["nsr"] is DBNull ? 0 : Convert.ToInt32(dr["nsr"]);
            objBilhete.IdFuncionario = dr["IdFuncionario"] is DBNull ? 0 : Convert.ToInt32(dr["IdFuncionario"]);
            objBilhete.PIS = dr["PIS"] is DBNull ? String.Empty : Convert.ToString(dr["PIS"]);
            if (!(dr["IdRegistroPonto"] is DBNull))
            {
                objBilhete.IdRegistroPonto = Convert.ToInt32(dr["IdRegistroPonto"]);
            }

            if (!(dr["IdLancamentoLoteFuncionario"] is DBNull))
            {
                objBilhete.IdLancamentoLoteFuncionario = Convert.ToInt32(dr["IdLancamentoLoteFuncionario"]);
            }
            objBilhete.Chave = objBilhete.ToMD5();

            return objBilhete;
        }

        public void ReprocessarFuncionarios(List<Modelo.Proxy.PxyReprocessarFuncionario> Funcionarios, List<string> pLog)
        {
            foreach (Modelo.Proxy.PxyReprocessarFuncionario funcReprocessar in Funcionarios)
            {
                DateTime? dataInicial;
                DateTime? dataFinal;
                if (ImportarBilhetes(funcReprocessar.DsCodigo, false, funcReprocessar.DataInicial, funcReprocessar.DataFinal, out dataInicial, out dataFinal, cwkFuncoes.ProgressVazia(), pLog))
                {
                    BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(2, funcReprocessar.IdFuncionario, dataInicial.Value, dataFinal.Value.AddDays(1), cwkFuncoes.ProgressVazia(), false, ConnectionString, UsuarioLogado, false);
                    bllCalculaMarcacao.CalculaMarcacoes();
                }
            }
        }

        public string PathAFD()
        {
            string caminhoAfd = ConfigurationManager.AppSettings["ArquivosPontofopag"];
            if (String.IsNullOrEmpty(caminhoAfd))
            {
                caminhoAfd = HostingEnvironment.MapPath("~/App_Data/Arquivos");
            }

            try
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);

                string database = builder.InitialCatalog;
                caminhoAfd = Path.Combine(caminhoAfd, database);
                caminhoAfd = Path.Combine(caminhoAfd, "AFD");
            }
            catch (Exception)
            {
            }

            if (!Directory.Exists(caminhoAfd))
            {
                Directory.CreateDirectory(caminhoAfd);
            }

            return caminhoAfd;
        }
    }
}