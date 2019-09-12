using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Modelo.Proxy;
using BLL.CalculoMarcacoes;
using DAL.SQL;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Concurrent;
using System.Collections;

namespace BLL
{
    public class CartaoPonto
    {
        protected DAL.ICartaoPonto dalCartaoPonto;
        private DAL.ICalculaMarcacao dalMarcacao;
        private DAL.IFuncionario dalFuncionario;
        private DAL.IFechamentoBH dalFechamentoBH;
        private DAL.IFechamentoBHD dalFechamentoBHD;
        private DAL.IBancoHoras dalBancoHoras;
        private DAL.IHorario dalHorario;
        private DAL.IHorarioDetalhe dalHorarioDetalhe;

        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public CartaoPonto()
            : this(null)
        {

        }

        public CartaoPonto(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public CartaoPonto(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalCartaoPonto = new DAL.SQL.CartaoPonto(db);
                    dalMarcacao = new DAL.SQL.CalculaMarcacao(db);
                    dalFuncionario = new DAL.SQL.Funcionario(db);
                    dalFechamentoBH = new DAL.SQL.FechamentoBH(db);
                    dalFechamentoBHD = new DAL.SQL.FechamentoBHD(db);
                    dalBancoHoras = new DAL.SQL.BancoHoras(db);
                    dalHorario = new DAL.SQL.Horario(db);
                    dalHorarioDetalhe = new DAL.SQL.HorarioDetalhe(db);
                    break;
                case 2:
                    dalCartaoPonto = new DAL.FB.CartaoPonto();
                    dalMarcacao = new DAL.FB.CalculaMarcacao();
                    dalFuncionario = DAL.FB.Funcionario.GetInstancia;
                    dalFechamentoBH = DAL.FB.FechamentoBH.GetInstancia;
                    dalFechamentoBHD = DAL.FB.FechamentoBHD.GetInstancia;
                    dalBancoHoras = DAL.FB.BancoHoras.GetInstancia;
                    dalHorario = DAL.FB.Horario.GetInstancia;
                    dalHorarioDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
                    break;
            }

            UsuarioLogado = usuarioLogado;
            dalCartaoPonto.UsuarioLogado = usuarioLogado;
            dalMarcacao.UsuarioLogado = usuarioLogado;
            dalFuncionario.UsuarioLogado = usuarioLogado;
            dalFechamentoBH.UsuarioLogado = usuarioLogado;
            dalFechamentoBHD.UsuarioLogado = usuarioLogado;
            dalBancoHoras.UsuarioLogado = usuarioLogado;
            dalHorario.UsuarioLogado = usuarioLogado;
            dalHorarioDetalhe.UsuarioLogado = usuarioLogado;
        }

        public DataTable GetCartaoPontoRel(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, int normalFlexivel, int idhorario, Modelo.ProgressBar pPBRecalculo, bool ordenaDeptoFuncionario, string filtro, bool quebraAuto)
        {
            try
            {
                BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, UsuarioLogado);
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
                BLL.Funcionario bllFunc = new BLL.Funcionario(ConnectionString, UsuarioLogado);
                BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(ConnectionString, UsuarioLogado);

                pPBRecalculo.setaMinMaxPB(0, 100);
                pPBRecalculo.setaValorPB(10);

                pPBRecalculo.setaMensagem("Verificando marcações de ponto");
                CorrigeMarcacoes(funcionarios, dataInicial, dataFinal, bllMarcacao, bllFunc, bllInclusaoBanco);
                pPBRecalculo.setaMensagem("Carregando dados");
                DataTable dt = dalCartaoPonto.GetCartaoPontoRel(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo, normalFlexivel, ordenaDeptoFuncionario, filtro);

                Modelo.Global.logs = new List<string>();

                DataTable dtBancoHoras = new DataTable();
                pPBRecalculo.setaValorPB(30);

                DataTable ret = new DataTable();

                CriaColunas(pPBRecalculo, dt, dtBancoHoras, ret);

                pPBRecalculo.setaValorPB(90);

                int idfuncionarioAnt = 0, idempresaAnt = 0, iddepartamentoAnt = 0, idfuncaoAnt = 0;
                int idfuncionario = 0, idempresa = 0, iddepartamento = 0, idfuncao = 0;
                DateTime data;
                List<Int32> idsFuncs = dt.AsEnumerable().Select(row => Convert.ToInt32(row.Field<Int32>("idfuncionario"))).Distinct().ToList();
                pPBRecalculo.setaMensagem("Carregando jornadas");
                List<Modelo.JornadaAlternativa> jornadasAlternativas = bllJornadaAlternativa.GetPeriodoFuncionarios(dataInicial, dataFinal, idsFuncs);
                pPBRecalculo.setaMensagem("Carregando banco de horas");
                List<Modelo.BancoHoras> bancosdehoras = dalBancoHoras.GetAllListFuncs(false, idsFuncs);
                pPBRecalculo.setaMensagem("Carregando fechamento de banco");
                List<Modelo.FechamentoBH> fechamentosbh = dalFechamentoBH.GetAllListFuncs(idsFuncs, false);
                List<Modelo.FechamentoBHD> fechamentosbhd = dalFechamentoBHD.GetAllList();

                int chdiurna = 0, chnoturna = 0;
                string entrada_1, entrada_2, entrada_3, entrada_4;
                string saida_1, saida_2, saida_3, saida_4;

                int horasTrabalhadasDiurna = 0;
                int horasTrabalhadasNoturna = 0;
                int horasFaltaDiurna = 0;
                int horasFaltaNoturna = 0;
                int count = 0;
                string creditodebitobh = "";
                string creditodebitobhanterior = "";
                string creditodebitobhatual = "";
                string saldobancohorasfechamento = "";
                bool existeBH = false;
                bool b24Horas = false;

                pPBRecalculo.setaValorPB(0);
                pPBRecalculo.setaMinMaxPB(0, dt.Rows.Count);

                Modelo.TotalHoras objTotalHoras = new Modelo.TotalHoras(dataInicial, dataFinal);
                PercentualHoraExtra[] HorariosPHExtra = new PercentualHoraExtra[10];
                int idhorarioAnt = 0;
                bool bInseriuDois = false;
                List<string> percExtras = null;
                string percSabado = "", percDomingo = "", percFolga = "", percFeriado = "";
                IList<Modelo.EmpresaLogo> EmpresasLogos = new List<Modelo.EmpresaLogo>();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        string logoEmpresa = "";
                        data = SetaVariaveis(ref idhorario, ref idfuncionario, ref idempresa, ref iddepartamento, ref idfuncao, row);
                        pPBRecalculo.setaMensagem("Data: " + data.ToShortDateString() + " - Funcionário: " + row["funcionario"].ToString());

                        Modelo.EmpresaLogo empLogo = EmpresasLogos.Where(w => w.IdEmpresa == idempresa).FirstOrDefault();
                        if (empLogo == null || empLogo.IdEmpresa == 0)
                        {
                            BLL.EmpresaLogo bllEmpLogo = new BLL.EmpresaLogo(ConnectionString, UsuarioLogado);
                            empLogo = bllEmpLogo.GetAllListPorEmpresa(idempresa).FirstOrDefault();
                            if (empLogo == null)
                            {
                                empLogo = new Modelo.EmpresaLogo();
                                empLogo.IdEmpresa = idempresa;
                                empLogo.Logo = "";
                            }
                            else
                            {
                                Image image = BLL.cwkFuncoes.Base64ToImage(empLogo.Logo);
                                using (var newImage = BLL.cwkFuncoes.RedimensionarImagem(image, 260, 147))
                                {
                                    empLogo.Logo = BLL.cwkFuncoes.ImageToBase64(newImage, ImageFormat.Jpeg);
                                }
                            }
                            EmpresasLogos.Add(empLogo);
                        }

                        logoEmpresa = empLogo.Logo;



                        if (idhorario != idhorarioAnt)
                        {
                            BLL.TotalizadorHorasFuncionario.AtribuaPercentuaisExtra(HorariosPHExtra, row);
                            idhorarioAnt = idhorario;
                        }

                        percExtras = new List<string>();
                        percSabado = ""; percDomingo = ""; percFolga = ""; percFeriado = "";
                        if (idfuncionario != idfuncionarioAnt)
                        {
                            horasTrabalhadasDiurna = 0;
                            horasTrabalhadasNoturna = 0;

                            AtualizaFuncAnterior(dataInicial, dataFinal, dtBancoHoras, ret, idfuncionarioAnt,
                                idempresaAnt, iddepartamentoAnt, idfuncaoAnt, jornadasAlternativas, bancosdehoras,
                                fechamentosbh, fechamentosbhd, ref creditodebitobh, ref creditodebitobhanterior,
                                ref creditodebitobhatual, ref saldobancohorasfechamento, ref existeBH, ref objTotalHoras,
                                HorariosPHExtra, bInseriuDois, percExtras, ref percSabado, ref percDomingo, ref percFolga, ref percFeriado);

                            idfuncionarioAnt = idfuncionario;
                            idempresaAnt = idempresa;
                            iddepartamentoAnt = iddepartamento;
                            idfuncaoAnt = idfuncao;

                            if ((dataFinal - dataInicial).TotalDays < 31 || quebraAuto == false)
                            { dtBancoHoras.Rows.Clear(); }
                            dtBancoHoras.Rows.Add(row.ItemArray);
                        }
                        else
                        {
                            dtBancoHoras.Rows.Add(row.ItemArray);
                        }

                        bool folga = Convert.ToBoolean(row["folga"]);

                        #region Verifica Jornada da Marcação

                        Modelo.JornadaAlternativa objJornadaAlternativa =
                            bllJornadaAlternativa.PossuiRegistro(jornadasAlternativas, data, idfuncionario, idfuncao, iddepartamento, idempresa);

                        if (objJornadaAlternativa != null)
                        {
                            chdiurna = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.TotalTrabalhadaDiurna);
                            chnoturna = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.TotalTrabalhadaNoturna);
                            entrada_1 = objJornadaAlternativa.Entrada_1;
                            entrada_2 = objJornadaAlternativa.Entrada_2;
                            entrada_3 = objJornadaAlternativa.Entrada_3;
                            entrada_4 = objJornadaAlternativa.Entrada_4;
                            saida_1 = objJornadaAlternativa.Saida_1;
                            saida_2 = objJornadaAlternativa.Saida_2;
                            saida_3 = objJornadaAlternativa.Saida_3;
                            saida_4 = objJornadaAlternativa.Saida_4;
                        }
                        else
                        {
                            if (Convert.ToInt32(row["tipohorario"]) == 1)
                            {
                                chdiurna = Modelo.cwkFuncoes.ConvertHorasMinuto(row["chdiurnanormal"].ToString());
                                chnoturna = Modelo.cwkFuncoes.ConvertHorasMinuto(row["chnoturnanormal"].ToString());
                                entrada_1 = row["entrada_1normal"].ToString();
                                entrada_2 = row["entrada_2normal"].ToString();
                                entrada_3 = row["entrada_3normal"].ToString();
                                entrada_4 = row["entrada_4normal"].ToString();
                                saida_1 = row["saida_1normal"].ToString();
                                saida_2 = row["saida_2normal"].ToString();
                                saida_3 = row["saida_3normal"].ToString();
                                saida_4 = row["saida_4normal"].ToString();
                            }
                            else
                            {
                                if (row["chdiurnaflexivel"] is DBNull)
                                {
                                    chdiurna = 0;
                                    chnoturna = 0;
                                    entrada_1 = "--:--";
                                    entrada_2 = "--:--";
                                    entrada_3 = "--:--";
                                    entrada_4 = "--:--";
                                    saida_1 = "--:--";
                                    saida_2 = "--:--";
                                    saida_3 = "--:--";
                                    saida_4 = "--:--";
                                }
                                else
                                {
                                    chdiurna = Modelo.cwkFuncoes.ConvertHorasMinuto(row["chdiurnaflexivel"].ToString());
                                    chnoturna = Modelo.cwkFuncoes.ConvertHorasMinuto(row["chnoturnaflexivel"].ToString());
                                    entrada_1 = row["entrada_1flexivel"].ToString();
                                    entrada_2 = row["entrada_2flexivel"].ToString();
                                    entrada_3 = row["entrada_3flexivel"].ToString();
                                    entrada_4 = row["entrada_4flexivel"].ToString();
                                    saida_1 = row["saida_1flexivel"].ToString();
                                    saida_2 = row["saida_2flexivel"].ToString();
                                    saida_3 = row["saida_3flexivel"].ToString();
                                    saida_4 = row["saida_4flexivel"].ToString();
                                }
                            }
                        }

                        #endregion

                        if (row["ocorrencia"].ToString() != "Não Admitido"
                            && row["legenda"].ToString() != "F" //Feriado                    
                            && !folga //Folga
                            )
                        {
                            horasTrabalhadasDiurna += chdiurna;
                            horasTrabalhadasNoturna += chnoturna;
                            horasFaltaDiurna += Modelo.cwkFuncoes.ConvertHorasMinuto(row["horasfaltas"].ToString());
                            horasFaltaNoturna += Modelo.cwkFuncoes.ConvertHorasMinuto(row["horasfaltanoturna"].ToString());
                        }

                        //Monta o vetor apenas com as batidas validas

                        string[] batidas = new string[16];

                        MontaBatidasValidas(row, batidas);

                        //Insere os registros no dataset
                        bInseriuDois = InsereRegistrosDataSet(ret, entrada_1, entrada_2,
                            entrada_3, entrada_4, saida_1, saida_2, saida_3, saida_4,
                            horasTrabalhadasDiurna, horasTrabalhadasNoturna, bInseriuDois,
                            row, folga, batidas, objTotalHoras, logoEmpresa);
                        count++;

                        pPBRecalculo.incrementaPB(1);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                if ((dataFinal - dataInicial).TotalDays > 31 && quebraAuto == true)
                {
                    BLL.Empresa bllEmpresa = new BLL.Empresa(ConnectionString, UsuarioLogado);
                    List<Modelo.Empresa> objEmpresas = bllEmpresa.GetAllList();

                    DataColumn colunaGrupoData = ret.Columns.Add("ColunaGrupoData", typeof(Int32));
                    colunaGrupoData.AllowDBNull = true;
                    colunaGrupoData.Unique = false;

                    DataColumn colunaGrupo = dtBancoHoras.Columns.Add("ColunaGrupoData", typeof(Int32));
                    colunaGrupoData.AllowDBNull = true;
                    colunaGrupoData.Unique = false;
                    DataTable dtAll = ret.Clone();
                    DataTable dtRet = ret.Clone();
                    DataTable dtBanco = dtBancoHoras.Clone();
                    foreach (int idFunc in idsFuncs)
                    {
                        int idEmp = dt.AsEnumerable().Where(r => r.Field<int>("idfuncionario") == idFunc).FirstOrDefault().Field<int>("idempresa");
                        Dictionary<DateTime, DateTime> periodos = QuebraPeridoPorPeriodoFechamento(dataInicial, dataFinal, objEmpresas, idFunc, idEmp);
                        Int32 grupo = 0;
                        foreach (KeyValuePair<DateTime, DateTime> periodo in periodos)
                        {
                            ret.AsEnumerable().Where(row => Convert.ToInt32(row.Field<string>("idfuncionario")) == idFunc && Convert.ToDateTime(row.Field<string>("DataMarcacao")) >= periodo.Key && Convert.ToDateTime(row.Field<string>("DataMarcacao")) <= periodo.Value).ToList<DataRow>()
                               .ForEach(r => { r["ColunaGrupoData"] = grupo; });

                            dtBancoHoras.AsEnumerable().Where(row => Convert.ToInt32(row.Field<string>("idfuncionario")) == idFunc && Convert.ToDateTime(row.Field<string>("data")) >= periodo.Key && Convert.ToDateTime(row.Field<string>("data")) <= periodo.Value).ToList<DataRow>()
                               .ForEach(r => { r["ColunaGrupoData"] = grupo; });

                            ret.DefaultView.RowFilter = "idfuncionario =" + idFunc;
                            dtRet = (ret.DefaultView).ToTable();
                            dtRet.DefaultView.RowFilter = "ColunaGrupoData =" + grupo;
                            dtRet = (dtRet.DefaultView).ToTable();

                            dtBancoHoras.DefaultView.RowFilter = "idfuncionario =" + idFunc;
                            dtBanco = (dtBancoHoras.DefaultView).ToTable();
                            dtBanco.DefaultView.RowFilter = "ColunaGrupoData =" + grupo;
                            dtBanco = (dtBanco.DefaultView).ToTable();

                            AtualizaFuncAnterior(periodo.Key, periodo.Value, dtBanco, dtRet, idfuncionario,
                            idempresa, iddepartamento, idfuncao, jornadasAlternativas, bancosdehoras,
                            fechamentosbh, fechamentosbhd, ref creditodebitobh, ref creditodebitobhanterior,
                            ref creditodebitobhatual, ref saldobancohorasfechamento, ref existeBH,
                            ref objTotalHoras, HorariosPHExtra, bInseriuDois, percExtras, ref percSabado,
                            ref percDomingo, ref percFolga, ref percFeriado);
                            dtAll.Merge(dtRet);
                            grupo++;
                        }
                    }
                    return dtAll;
                }
                else
                {
                    AtualizaFuncAnterior(dataInicial, dataFinal, dtBancoHoras, ret, idfuncionario,
                    idempresa, iddepartamento, idfuncao, jornadasAlternativas, bancosdehoras,
                    fechamentosbh, fechamentosbhd, ref creditodebitobh, ref creditodebitobhanterior,
                    ref creditodebitobhatual, ref saldobancohorasfechamento, ref existeBH,
                    ref objTotalHoras, HorariosPHExtra, bInseriuDois, percExtras, ref percSabado,
                    ref percDomingo, ref percFolga, ref percFeriado);
                }

                return ret;
            }
            catch (Exception z)
            {

                throw z;
            }
        }

        private Dictionary<DateTime, DateTime> QuebraPeridoPorPeriodoFechamento(DateTime dtIni, DateTime dtFin, IList<Modelo.Empresa> emps, int idFunc, int idEmp)
        {
            Dictionary<DateTime, DateTime> periodos = new Dictionary<DateTime, DateTime>();
            if ((dtFin - dtIni).TotalDays <= 31)
            {
                periodos.Add(dtIni, dtFin);
            }
            else
            {
                int inicioFechamento, fimfechamento;
                DateTime inicioPeriodo, fimPeriodo;
                GetInicioFimFechamentoPonto(emps, idFunc, out inicioFechamento, out fimfechamento, idEmp);
                inicioPeriodo = dtIni;
                while (true)
                {
                    if (fimfechamento == 0)
                    {
                        fimPeriodo = cwkFuncoes.LastDayDate(inicioPeriodo);
                    }
                    else
                    {
                        int ultimoDiaMes = DateTime.DaysInMonth(inicioPeriodo.Year, inicioPeriodo.Month);
                        if (ultimoDiaMes < fimfechamento)
                        {
                            fimPeriodo = Convert.ToDateTime(ultimoDiaMes + "/" + inicioPeriodo.Month + "/" + inicioPeriodo.Year);
                        }
                        else
                        {
                            fimPeriodo = Convert.ToDateTime(fimfechamento + "/" + inicioPeriodo.Month + "/" + inicioPeriodo.Year);
                        }
                    }
                    if (inicioPeriodo >= fimPeriodo)
                    {
                        fimPeriodo = fimPeriodo.AddMonths(1);
                    }
                    if (fimPeriodo >= dtFin)
                    {
                        periodos.Add(inicioPeriodo, dtFin);
                        break;
                    }
                    else
                    {
                        periodos.Add(inicioPeriodo, fimPeriodo);
                        inicioPeriodo = fimPeriodo.AddDays(1);
                    }
                }
            }
            return periodos;
        }

        public void GetInicioFimFechamentoPonto(IList<Modelo.Empresa> emps, int idFunc, out int inicioFechamento, out int fimfechamento, int idEmp)
        {
            inicioFechamento = 0;
            fimfechamento = 0;
            bool encontrou = false;

            if (UsuarioLogado.UtilizaControleContratos)
            {
                Modelo.Contrato contrato = new Modelo.Contrato();
                BLL.Contrato bllContrato = new BLL.Contrato(ConnectionString, UsuarioLogado);
                contrato = bllContrato.ContratosPorFuncionario(idFunc).FirstOrDefault();
                if (UsuarioLogado.UtilizaControleContratos && contrato != null && contrato.Id > 0)
                {
                    //Quando o dia fechamento inicial for igual a 1 e o fechamento final igual a 30
                    //irá setar como 0, pois o usuário quer que seja o período do mês inteiro
                    if (contrato.DiaFechamentoInicial == 1 && contrato.DiaFechamentoFinal == 30)
                    {
                        inicioFechamento = 0;
                        fimfechamento = 0;
                        encontrou = true;
                    }
                    else
                    {
                        inicioFechamento = contrato.DiaFechamentoInicial;
                        fimfechamento = contrato.DiaFechamentoFinal;
                        encontrou = true;
                    }
                }
            }

            if (!encontrou || (inicioFechamento == 0 && fimfechamento == 0))
            {
                Modelo.Empresa emp = emps.Where(x => x.Id == idEmp).FirstOrDefault();
                if (emp != null && emp.Id > 0)
                {
                    if (emp.DiaFechamentoInicial == 1 && emp.DiaFechamentoFinal == 30)
                    {
                        inicioFechamento = 0;
                        fimfechamento = 0;
                        encontrou = true;
                    }
                    else
                    {
                        inicioFechamento = emp.DiaFechamentoInicial;
                        fimfechamento = emp.DiaFechamentoFinal;
                        encontrou = true;
                    }
                }
            }

            if (!encontrou || (inicioFechamento == 0 && fimfechamento == 0))
            {
                Parametros bllParam = new Parametros(ConnectionString, UsuarioLogado);
                Modelo.Parametros param = bllParam.LoadPrimeiro();
                if (param != null && param.Id > 0)
                {
                    if (param.DiaFechamentoInicial == 1 && param.DiaFechamentoFinal == 30)
                    {
                        inicioFechamento = 0;
                        fimfechamento = 0;
                    }
                    else
                    {
                        inicioFechamento = param.DiaFechamentoInicial;
                        fimfechamento = param.DiaFechamentoFinal;
                    }

                }
            }
        }

        public void CorrigeMarcacoes(string funcionarios, DateTime dataInicial, DateTime dataFinal, Marcacao bllMarcacao, Funcionario bllFunc, BLL.InclusaoBanco bllInclusaoBanco)
        {
            if (!String.IsNullOrEmpty(funcionarios) && funcionarios != "()")
            {
                IList<Modelo.Funcionario> lFuncionarios = bllFunc.GetAllListByIds(funcionarios);

                var _list = lFuncionarios.Select(x => x.Id).ToList();
                TratarMarcacoes(dataInicial, dataFinal, _list, bllFunc);
            }
        }

        public void GetEntradasSaidasValidas(ref int[] entradas, ref int[] saidas, string[] batidas)
        {
            int[] marcacoes = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            for (int i = 0; i < batidas.Length; i++)
            {
                string b = batidas[i].Replace("*", "");
                if (b != "--:--" && !String.IsNullOrEmpty(b))
                {
                    marcacoes[i] = Modelo.cwkFuncoes.ConvertHorasMinuto(b);
                }
            }

            int s = 0, e = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((i % 2) != 0)
                {
                    saidas[s] = marcacoes[i];
                    s++;
                }
                else
                {
                    entradas[e] = marcacoes[i];
                    e++;
                }
            }
        }

        private bool InsereRegistrosDataSet(DataTable ret, string entrada_1,
            string entrada_2, string entrada_3, string entrada_4, string saida_1,
            string saida_2, string saida_3, string saida_4, int horasTrabalhadasDiurna,
            int horasTrabalhadasNoturna, bool bInseriuDois, DataRow row, bool folga, string[] batidas,
            Modelo.TotalHoras objTotalHoras, string logoEmpresa)
        {
            bool imprimeobs;
            string observacao = row["campoobservacao"].ToString();

            observacao = observacao.TrimEnd() + " " + row["observacao"].ToString();
            observacao = observacao.TrimEnd() + " " + row["observacaofunc"].ToString();

            int totaldiasdsr = objTotalHoras.totalDSRDias;
            string totalhorasdsr = Modelo.cwkFuncoes.ConvertMinutosHora2(3, objTotalHoras.totalDSRMinutos);
            int totaldiasabono = objTotalHoras.totalAbonoDias;
            string totalhorasabono = Modelo.cwkFuncoes.ConvertMinutosHora2(3, objTotalHoras.totalAbonoMinutos);

            imprimeobs = Convert.ToBoolean(row["imprimeobservacao"]);
            #region Insere os registros no dataset
            // Ao Adicionar colinas aqui adicionar no "object[] values2" logo abaixo
            int codDep = 0;
            if (!(row["codigoDepartamento"] is DBNull))
            {
                codDep = Convert.ToInt32(row["codigoDepartamento"]);
            }

            int[] Entradas = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] Saidas = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            GetEntradasSaidasValidas(ref Entradas, ref Saidas, batidas);

            bool jornada24 = JornadaUltrapassa24Horas(entrada_1, entrada_2, entrada_3, entrada_4, saida_1, saida_2, saida_3, saida_4);
            string TotalHorasTrabalhadas = "---:--";
            TotalHorasTrabalhadas = CalculaMarcacao.CalculaTotalHorasTrabalhadas(Entradas, Saidas, (DateTime)row["data"], jornada24);
            string TotalHorasIntervalo = "---:--";
            TotalHorasIntervalo = CalculaMarcacao.CalculaTotalIntervalo(Entradas, Saidas);

            bool bConsideraCafe = VerificarConsideraCafe(row);
            string TotalHorasAlmoco = "---:--";
            TotalHorasAlmoco = CalculaMarcacao.CalculaTotalHorasAlmoco(Entradas, Saidas, bConsideraCafe);

            String observacaoInconsistencia = PegaObservacaoInconsistencia(TotalHorasTrabalhadas, TotalHorasAlmoco, Convert.ToString(row["Interjornada"]), Convert.ToString(row["limitehorastrabalhadasdia"]), Convert.ToString(row["LimiteMinimoHorasAlmoco"]), Convert.ToString(row["LimiteInterjornada"]));

            if ((entrada_3 != "--:--" && !String.IsNullOrEmpty(entrada_3))
                || (batidas[8] != "--:--" && !String.IsNullOrEmpty(batidas[8])))
            {
                bInseriuDois = true;
            }
            else
            {
                bInseriuDois = false;
            }

            object[] values = new object[]
                {
                    row["id"],
                    row["idhorario"],
                    row["legenda"],
                    row["data"] = Convert.ToDateTime((row["data"]).ToString()).ToString("dd/MM/yy"),
                    Modelo.cwkFuncoes.DiaSemana(Convert.ToDateTime(row["data"]), Modelo.cwkFuncoes.TipoDiaSemana.Reduzido).Trim(new char[] {'.'}),
                    //Entradas
                    batidas[0],
                    batidas[2],
                    batidas[4],
                    batidas[6],
                    //Saidas
                    batidas[1],
                    batidas[3],
                    batidas[5],
                    batidas[7],
                    row["horastrabalhadas"].ToString() == "--:--" ? "" : row["horastrabalhadas"].ToString(),
                    row["horasextrasdiurna"].ToString() == "--:--" ? "" : row["horasextrasdiurna"].ToString(),
                    row["horasfaltas"].ToString() == "--:--" ? "" : row["horasfaltas"].ToString(),
                    row["entradaextra"].ToString() == "--:--" ? "" : row["entradaextra"].ToString(),
                    row["saidaextra"].ToString() == "--:--" ? "" : row["saidaextra"].ToString(),
                    row["horastrabalhadasnoturnas"].ToString() == "--:--" ? "" : row["horastrabalhadasnoturnas"].ToString(),
                    row["horasextranoturna"].ToString() == "--:--" ? "" : row["horasextranoturna"].ToString(),
                    row["horasfaltanoturna"].ToString() == "--:--" ? "" : row["horasfaltanoturna"].ToString(),
                    row["ocorrencia"],
                    row["dscodigo"],
                    row["funcionario"],
                    row["matricula"],
                    ((DateTime)row["dataadmissao"]).ToShortDateString(),
                    row["codigofolha"],
                    row["funcao"],
                    row["pis"],
                    row["departamento"],
                    row["empresa"],
                    row["cnpj_cpf"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["thoraextra"],
                    row["thorafalta"],
                    row["idfuncionario"],
                    row["bancohorascre"],
                    row["bancohorasdeb"],
                    imprimeobs ? observacao : "",
                    row["LimiteInterjornada"],
                    row["Interjornada"],
                    row["InItinereHrsDentroJornada"],
                    row["InItinerePercDentroJornada"],
                    row["InItinereHrsForaJornada"],
                    row["InItinerePercForaJornada"],
                    row["AdicionalNoturno"].ToString() == "--:--" ? "" : row["AdicionalNoturno"].ToString(),
                    row["PercAdicNoturno"],
                    row["horaExtraInterjornada"],
                    folga ? "" : entrada_1 == "--:--" ? "" : entrada_1,
                    folga ? "" : entrada_2 == "--:--" ? "" : entrada_2,
                    folga ? "" : saida_1 == "--:--" ? "" : saida_1,
                    folga ? "" : saida_2 == "--:--" ? "" : saida_2,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasDiurna),
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasNoturna),
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    0,
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
                    0,
                    totaldiasdsr,
                    totalhorasdsr,
                    totaldiasabono,
                    totalhorasabono,
                    0,
                    codDep,
                    row["limitehorastrabalhadasdia"],
                    TotalHorasTrabalhadas,
                    TotalHorasIntervalo,
                    row["TotalIntervaloPrev"],
                    row["LimiteMinimoHorasAlmoco"],
                    row["habilitaperiodo01"],
                    row["dias_cafe_1"],
                    row["dias_cafe_2"],
                    row["dias_cafe_3"],
                    row["dias_cafe_4"],
                    row["dias_cafe_5"],
                    row["dias_cafe_6"],
                    row["dias_cafe_7"],
                    row["naoconsiderarcafe"],
                    TotalHorasAlmoco,
                    observacaoInconsistencia,
                    0,
                    0,
                    logoEmpresa,
                    ((DateTime)row["data"])
                };
            ret.Rows.Add(values);

            if (bInseriuDois)
            {
                object[] values2 = new object[]
                    {
                    "",
                    "",
                    "",
                    row["data"] = Convert.ToDateTime((row["data"]).ToString()).ToString("dd/MM/yy"),
                    "",         
                    //Entradas           
                    batidas[8],
                    batidas[10],
                    batidas[12],
                    batidas[14],
                    //Saídas
                    batidas[9],
                    batidas[11],
                    batidas[13],
                    batidas[15],
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    row["dscodigo"],
                    row["funcionario"],
                    "",
                    "",
                    "",
                    "",
                    "",
                    row["departamento"],
                    row["empresa"],
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    row["idfuncionario"],
                    row["bancohorascre"],
                    row["bancohorasdeb"],
                    imprimeobs ? observacao : "",
                    "",
                    "",
                    "--:--",
                    0,
                    "--:--",
                    0,
                    row["AdicionalNoturno"].ToString() == "--:--" ? "" : row["AdicionalNoturno"].ToString(),
                    row["PercAdicNoturno"],
                    "",
                    folga ? "" : entrada_3 == "--:--" ? "" : entrada_3,
                    folga ? "" : entrada_4 == "--:--" ? "" : entrada_4,
                    folga ? "" : saida_3 == "--:--" ? "" : saida_3,
                    folga ? "" : saida_4 == "--:--" ? "" : saida_4,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasDiurna),
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasNoturna),
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    0,
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
                    0,
                    totaldiasdsr,
                    totalhorasdsr,
                    totaldiasabono,
                    totalhorasabono,
                    0,
                    codDep,
                    row["limitehorastrabalhadasdia"],
                    TotalHorasTrabalhadas,
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
                    0,
                    0,
                    "",
                    ((DateTime)row["data"])
                    };
                ret.Rows.Add(values2);
            }
            #endregion
            return bInseriuDois;
        }

        private string PegaObservacaoInconsistencia(string TotalHorasTrabalhadas, string TotalHorasAlmoco, string Interjornada, string LimiteTotalHorasTrabalhadas, string LimiteTotalHorasAlmoco, string LimiteInterjornada)
        {
            int TotalHorasTrabalhadasInt = Modelo.cwkFuncoes.ConvertHorasMinuto(TotalHorasTrabalhadas);
            int TotalHorasAlmocoInt = Modelo.cwkFuncoes.ConvertHorasMinuto(TotalHorasAlmoco);

            int LimiteTotalHorasTrabalhadasInt = Modelo.cwkFuncoes.ConvertHorasMinuto(LimiteTotalHorasTrabalhadas);
            int LimiteTotalHorasAlmocoInt = Modelo.cwkFuncoes.ConvertHorasMinuto(LimiteTotalHorasAlmoco);
            int InterjornadaInt = Modelo.cwkFuncoes.ConvertHorasMinuto(Interjornada);
            int LimiteInterjornadaInt = Modelo.cwkFuncoes.ConvertHorasMinuto(LimiteInterjornada);

            IList<String> lObservacao = new List<String>();
            string observacao;
            try
            {

                if (TotalHorasTrabalhadasInt > LimiteTotalHorasTrabalhadasInt)
                {
                    lObservacao.Add("LMT");
                }
                if (TotalHorasAlmocoInt < LimiteTotalHorasAlmocoInt)
                {
                    lObservacao.Add("INTRA");
                }
                if (InterjornadaInt > 0 && InterjornadaInt < LimiteInterjornadaInt)
                {
                    lObservacao.Add("INTER");
                }

                observacao = String.Join(", ", lObservacao).Trim();
            }
            catch (Exception)
            {
                observacao = String.Empty;
            }

            return observacao;
        }

        private bool VerificarConsideraCafe(DataRow row)
        {
            string dia = Convert.ToString(row["dia"]);
            bool bMarcacaoNaoConsidCafe;
            bMarcacaoNaoConsidCafe = Convert.ToInt32(row["naoconsiderarcafe"]) == 0 ? false : true;

            bool bHorarioConsideraCafe;
            switch (dia)
            {
                case "Seg.":
                    bHorarioConsideraCafe = Convert.ToInt32(row["dias_cafe_1"]) == 0 ? false : true;
                    break;
                case "Ter.":
                    bHorarioConsideraCafe = Convert.ToInt32(row["dias_cafe_2"]) == 0 ? false : true;
                    break;
                case "Qua.":
                    bHorarioConsideraCafe = Convert.ToInt32(row["dias_cafe_3"]) == 0 ? false : true;
                    break;
                case "Qui.":
                    bHorarioConsideraCafe = Convert.ToInt32(row["dias_cafe_4"]) == 0 ? false : true;
                    break;
                case "Sex.":
                    bHorarioConsideraCafe = Convert.ToInt32(row["dias_cafe_5"]) == 0 ? false : true;
                    break;
                case "Sáb.":
                    bHorarioConsideraCafe = Convert.ToInt32(row["dias_cafe_6"]) == 0 ? false : true;
                    break;
                default:
                    bHorarioConsideraCafe = Convert.ToInt32(row["dias_cafe_7"]) == 0 ? false : true;
                    break;
            }

            return bHorarioConsideraCafe && !bMarcacaoNaoConsidCafe;
        }

        public static bool JornadaUltrapassa24Horas(string entrada_1, string entrada_2, string entrada_3, string entrada_4, string saida_1, string saida_2, string saida_3, string saida_4)
        {
            int[] entradas = new int[] { -1, -1, -1, -1 };
            int[] saidas = new int[] { -1, -1, -1, -1 };

            entradas[0] = entrada_1.Contains("--:--") ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto(entrada_1);
            entradas[1] = entrada_2.Contains("--:--") ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto(entrada_1);
            entradas[2] = entrada_3.Contains("--:--") ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto(entrada_1);
            entradas[3] = entrada_4.Contains("--:--") ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto(entrada_1);
            saidas[0] = saida_1.Contains("--:--") ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto(saida_1);
            saidas[1] = saida_2.Contains("--:--") ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto(saida_2);
            saidas[2] = saida_3.Contains("--:--") ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto(saida_3);
            saidas[3] = saida_4.Contains("--:--") ? -1 : Modelo.cwkFuncoes.ConvertHorasMinuto(saida_4);
            return Modelo.cwkFuncoes.JornadaUltrapassaMeiaNoite(entradas, saidas);
        }

        public void MontaBatidasValidas(DataRow row, string[] batidas)
        {
            for (int i = 0; i < 16; i++)
            {
                batidas[i] = "";
            }
            string aux = "";
            int z = 0;
            for (int k = 1; k <= 8; k++)
            {
                if (z > 15)
                {
                    break;
                }
                aux = row["tratent_" + k.ToString()].ToString();
                if (row["entrada_" + k.ToString()].ToString() != "--:--" && aux != "D")
                {
                    batidas[z] = row["entrada_" + k.ToString()].ToString();
                    if (aux == "I" || aux == "P")
                        batidas[z] += "*";
                    z++;
                }

                if (z > 15)
                {
                    break;
                }
                aux = row["tratsai_" + k.ToString()].ToString();
                if (row["saida_" + k.ToString()].ToString() != "--:--" && aux != "D")
                {
                    batidas[z] = row["saida_" + k.ToString()].ToString();
                    if (aux == "I" || aux == "P")
                        batidas[z] += "*";
                    z++;
                }
            }
        }

        private bool ValidaMarcacao(int idhorario, int idfuncionario, DateTime data, DataRow row)
        {
            BLL.Empresa bllEmpresa = new BLL.Empresa(ConnectionString, UsuarioLogado);
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            objMarcacao.Data = data;
            objMarcacao.Idfuncionario = idfuncionario;
            objMarcacao.Idhorario = idhorario;
            objMarcacao.Entrada_1 = Convert.ToString(row["entrada_1"]);
            objMarcacao.Entrada_2 = Convert.ToString(row["entrada_2"]);
            objMarcacao.Entrada_3 = Convert.ToString(row["entrada_3"]);
            objMarcacao.Entrada_4 = Convert.ToString(row["entrada_4"]);
            objMarcacao.Entrada_5 = Convert.ToString(row["entrada_5"]);
            objMarcacao.Entrada_6 = Convert.ToString(row["entrada_6"]);
            objMarcacao.Entrada_7 = Convert.ToString(row["entrada_7"]);
            objMarcacao.Entrada_8 = Convert.ToString(row["entrada_8"]);
            objMarcacao.Saida_1 = Convert.ToString(row["saida_1"]);
            objMarcacao.Saida_2 = Convert.ToString(row["saida_2"]);
            objMarcacao.Saida_3 = Convert.ToString(row["saida_3"]);
            objMarcacao.Saida_4 = Convert.ToString(row["saida_4"]);
            objMarcacao.Saida_5 = Convert.ToString(row["saida_5"]);
            objMarcacao.Saida_6 = Convert.ToString(row["saida_6"]);
            objMarcacao.Saida_7 = Convert.ToString(row["saida_7"]);
            objMarcacao.Saida_8 = Convert.ToString(row["saida_8"]);
            objMarcacao.Horastrabalhadas = Convert.ToString(row["horastrabalhadas"]);
            objMarcacao.Horasextrasdiurna = Convert.ToString(row["horasextrasdiurna"]);
            objMarcacao.Horasfaltas = Convert.ToString(row["horasfaltas"]);
            objMarcacao.Entradaextra = Convert.ToString(row["entradaextra"]);
            objMarcacao.Saidaextra = Convert.ToString(row["saidaextra"]);
            objMarcacao.Horastrabalhadasnoturnas = Convert.ToString(row["horastrabalhadasnoturnas"]);
            objMarcacao.Horasextranoturna = Convert.ToString(row["horasextranoturna"]);
            objMarcacao.Horasfaltanoturna = Convert.ToString(row["horasfaltanoturna"]);
            objMarcacao.Bancohorascre = Convert.ToString(row["bancohorascre"]);
            objMarcacao.Bancohorasdeb = Convert.ToString(row["bancohorasdeb"]);
            objMarcacao.Chave = Convert.ToString(row["chave"]);

            //Verifica se a marcação não foi manipulada
            if (!objMarcacao.MarcacaoOK())
            {
                Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();
                objEmpresa.BDAlterado = true;
                objEmpresa.Chave = objEmpresa.HashMD5ComRelatoriosValidacaoNova();
                bllEmpresa.Salvar(Modelo.Acao.Alterar, objEmpresa);
                StringBuilder str = new StringBuilder("A marcação do funcionário ");
                str.Append(row["funcionario"]);
                str.Append(" no dia ");
                str.Append(objMarcacao.Data.ToShortDateString());
                str.Append(" foi alterada.");
                str.AppendLine(" Para voltar a utilizar o sistema entre em contato com a revenda.");
                throw new Exception(str.ToString());
            }
            return true;
        }

        private DateTime SetaVariaveis(ref int idhorario, ref int idfuncionario, ref int idempresa, ref int iddepartamento, ref int idfuncao, DataRow row)
        {
            DateTime data;
            idhorario = Convert.ToInt32(row["idhorario"].ToString());
            data = Convert.ToDateTime(row["data"].ToString());
            idfuncionario = Convert.ToInt32(row["idfuncionario"]);
            idfuncao = Convert.ToInt32(row["idfuncao"]);
            idempresa = Convert.ToInt32(row["idempresa"]);
            iddepartamento = Convert.ToInt32(row["iddepartamento"]);
            return data;
        }

        private void CriaColunas(Modelo.ProgressBar pPBRecalculo, DataTable dt, DataTable dtBancoHoras, DataTable ret)
        {
            #region Criaçao das colunas

            List<DataColumn> colunas = new List<DataColumn>();
            foreach (DataColumn c in dt.Columns)
            {


                if (Array.IndexOf(new string[] { "entrada_5", "entrada_6", "entrada_7", "entrada_8"
                    , "saida_5", "saida_6", "saida_7", "saida_8", "valordsr", "tipohorario"
                    , "idempresa", "iddepartamento", "idfuncao"
                    , "tratent_1", "tratent_2", "tratent_3", "tratent_4", "tratent_5", "tratent_6", "tratent_7", "tratent_8"
                    , "tratsai_1", "tratsai_2", "tratsai_3", "tratsai_4", "tratsai_5", "tratsai_6", "tratsai_7", "tratsai_8", "folga", "chave"
                    , "chdiurnanormal", "chnoturnanormal", "flagfolganormal", "entrada_1normal", "entrada_2normal", "entrada_3normal", "entrada_4normal"
                    , "saida_1normal", "saida_2normal", "saida_3normal", "saida_4normal"
                    , "chdiurnaflexivel", "chnoturnaflexivel", "flagfolgaflexivel", "entrada_1flexivel", "entrada_2flexivel", "entrada_3flexivel", "entrada_4flexivel"
                    , "saida_1flexivel", "saida_2flexivel", "saida_3flexivel", "saida_4flexivel"
                    , "percentualextra50", "percentualextra60", "percentualextra70", "percentualextra80", "percentualextra90", "percentualextra100", "percentualextrasab"
                    , "percentualextradom", "percentualextrafer", "percentualextrafol", "considerasabadosemana", "consideradomingosemana", "tipoacumulo"
                    , "quantidadeextra50", "quantidadeextra60", "quantidadeextra70", "quantidadeextra80", "quantidadeextra90", "quantidadeextra100", "quantidadeextrasab"
                    , "quantidadeextradom", "quantidadeextrafer", "quantidadeextrafol", "imprimeobservacao", "observacaofunc", "campoobservacao", "exphorasextranoturna"
                    , "cargamistaflexivel", "cargamistanormal", "percextraprimeiro1", "percextraprimeiro2", "percextraprimeiro3", "percextraprimeiro4", "percextraprimeiro5"
                    , "percextraprimeiro6", "percextraprimeiro7", "percextraprimeiro8", "percextraprimeiro9", "percextraprimeiro10", "tipoacumulo1", "tipoacumulo2", "tipoacumulo3"
                    , "tipoacumulo4", "tipoacumulo5", "tipoacumulo6", "tipoacumulo7", "tipoacumulo8", "tipoacumulo9", "tipoacumulo10", "codigoDepartamento"
                    , "LimiteHorasTrabalhadasDia", "TotalHorasTrabalhadas", "TotalIntervalo", "TotalIntervaloPrev"
                    , "LimiteMinimoHorasAlmoco", "habilitaperiodo01", "dias_cafe_1", "dias_cafe_2", "dias_cafe_3", "dias_cafe_4", "dias_cafe_5", "dias_cafe_6", "dias_cafe_7"
                    , "naoconsiderarcafe", "TotalHorasAlmoco", "ObservacaoInconsistencia", "LegendasConcatenadas"
                    , "percentualextraNoturna50", "quantidadeextraNoturna50", "percentualextraNoturna60", "quantidadeextraNoturna60", "percentualextraNoturna70", "quantidadeextraNoturna70"
                    , "percentualextraNoturna80", "quantidadeextraNoturna80", "percentualextraNoturna90", "quantidadeextraNoturna90", "percentualextraNoturna100", "quantidadeextraNoturna100"
                    , "percentualextraNoturnasab", "quantidadeextraNoturnasab", "percentualextraNoturnadom", "quantidadeextraNoturnadom", "percentualextraNoturnafer", "quantidadeextraNoturnafer"
                    , "percentualextraNoturnafol", "quantidadeextraNoturnafol", "percextraprimeiroNoturna1", "percextraprimeiroNoturna2", "percextraprimeiroNoturna3", "percextraprimeiroNoturna4"
                    , "percextraprimeiroNoturna5", "percextraprimeiroNoturna6", "percextraprimeiroNoturna7", "percextraprimeiroNoturna8", "percextraprimeiroNoturna9", "percextraprimeiroNoturna10"
                    , "SeparaExtraNoturnaPercentual", "TotalIntervaloPrevL","idferiado","FeriadoParcial","FeriadoParcialInicio","FeriadoParcialFim","inicioAdNoturno","fimAdNoturno"
                }, c.ColumnName) == -1)
                {
                    ret.Columns.Add(c.ColumnName);
                }

                dtBancoHoras.Columns.Add(c.ColumnName);
            }

            pPBRecalculo.setaValorPB(50);


            DataColumn[] colunasHora = new DataColumn[]
            {
                new DataColumn("hentrada_1"),
                new DataColumn("hentrada_2"),
                new DataColumn("hsaida_1"),
                new DataColumn("hsaida_2"),
                new DataColumn("totaltrabalhadadiurna"),
                new DataColumn("totaltrabalhadanoturna"),
                new DataColumn("totaldiurnageral"),
                new DataColumn("totalnoturnageral"),
                new DataColumn("diurnanoturnageral"),
                new DataColumn("extradgeral"),
                new DataColumn("extrangeral"),
                new DataColumn("totalextrageral"),
                new DataColumn("faltadgeral"),
                new DataColumn("faltangeral"),
                new DataColumn("totalfaltageral"),
                new DataColumn("diasfaltageral"),
                new DataColumn("dsrgeral"),
                new DataColumn("horaextra1"),
                new DataColumn("horaextra2"),
                new DataColumn("horaextra3"),
                new DataColumn("horaextra4"),
                new DataColumn("extrasabado"),
                new DataColumn("extradomingo"),
                new DataColumn("extrafolga"),
                new DataColumn("extraferiado"),
                new DataColumn("saldobancohoras"),
                new DataColumn("creditodebitobh"),
                new DataColumn("saldobancohorasanterior"),
                new DataColumn("creditodebitobhanterior"),
                new DataColumn("saldobancohorasatual"),
                new DataColumn("creditodebitobhatual"),
                new DataColumn("saldobancohorasfechamento"),
                new DataColumn("existebh"),
                new DataColumn("totalDSRDias"),
                new DataColumn("totalDSRHoras"),
                new DataColumn("totalAbonoDias"),
                new DataColumn("totalAbonoHoras"),
                new DataColumn("qtdDDSR"),
                new DataColumn("codigoDepartamento"),
                new DataColumn("LimiteHorasTrabalhadasDia"),
                new DataColumn("TotalHorasTrabalhadas"),
                new DataColumn("TotalIntervalo"),
                new DataColumn("TotalIntervaloPrev"),
                new DataColumn("LimiteMinimoHorasAlmoco"),
                new DataColumn("habilitaperiodo01"),
                new DataColumn("dias_cafe_1"),
                new DataColumn("dias_cafe_2"),
                new DataColumn("dias_cafe_3"),
                new DataColumn("dias_cafe_4"),
                new DataColumn("dias_cafe_5"),
                new DataColumn("dias_cafe_6"),
                new DataColumn("dias_cafe_7"),
                new DataColumn("naoconsiderarcafe"),
                new DataColumn("TotalHorasAlmoco"),
                new DataColumn("ObservacaoInconsistencia"),
                new DataColumn("TotalBancoHorasCre"),
                new DataColumn("TotalBancoHorasDeb"),
                new DataColumn("LogoEmpresa"),
                new DataColumn("DataMarcacao"),
                new DataColumn("totalAdicionalNoturno"),
                new DataColumn("qtdAdNot")
            };

            ret.Columns.AddRange(colunasHora);

            #endregion
        }

        private void AtualizaFuncAnterior(DateTime dataInicial, DateTime dataFinal,
            DataTable dtBancoHoras, DataTable ret, int idfuncionario, int idempresa,
            int iddepartamento, int idfuncao, List<Modelo.JornadaAlternativa> jornadasAlternativas,
            List<Modelo.BancoHoras> bancosdehoras, List<Modelo.FechamentoBH> fechamentosbh,
            List<Modelo.FechamentoBHD> fechamentosbhd, ref string creditodebitobh,
            ref string creditodebitobhanterior, ref string creditodebitobhatual,
            ref string saldobancohorasfechamento, ref bool existeBH, ref Modelo.TotalHoras objTotalHoras,
            PercentualHoraExtra[] HorariosPHExtra, bool bInseriuDois, List<string> percExtras,
            ref string percSabado, ref string percDomingo, ref string percFolga, ref string percFeriado)
        {
            #region Atualiza os totais do funcionário anterior
            objTotalHoras = new Modelo.TotalHoras(dataInicial, dataFinal);

            var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(idempresa, iddepartamento, idfuncionario,
                idfuncao, dataInicial, dataFinal, jornadasAlternativas, dtBancoHoras, null, null, ConnectionString, UsuarioLogado);
            totalizadorHoras.TotalizeHoras(objTotalHoras);

            var totalizadorBancoHoras = new BLL.CalculoMarcacoes.TotalizadorBancoHoras(idempresa, iddepartamento, idfuncionario, idfuncao, dataInicial, dataFinal, bancosdehoras, fechamentosbh, fechamentosbhd, dtBancoHoras, true, ConnectionString, UsuarioLogado);
            totalizadorBancoHoras.PreenchaBancoHoras(objTotalHoras);
            creditodebitobh = objTotalHoras.sinalSaldoBHPeriodo == '+' ? "Crédito" : (objTotalHoras.sinalSaldoBHPeriodo == '-' ? "Débito" : "");
            creditodebitobhanterior = objTotalHoras.sinalSaldoAnteriorBH == '+' ? "Crédito" : (objTotalHoras.sinalSaldoAnteriorBH == '-' ? "Débito" : "");
            creditodebitobhatual = objTotalHoras.sinalSaldoBHAtual == '+' ? "Crédito" : (objTotalHoras.sinalSaldoBHAtual == '-' ? "Débito" : "");
            saldobancohorasfechamento = "";
            existeBH = bancosdehoras.Exists
            (b =>
                (
                    (b.Tipo == 0 && b.Identificacao == idempresa)
                    || (b.Tipo == 1 && b.Identificacao == iddepartamento)
                    || (b.Tipo == 2 && b.Identificacao == idfuncionario)
                    || (b.Tipo == 3 && b.Identificacao == idfuncao)
                )
                &&
                (
                    (dataFinal >= b.DataInicial && dataFinal <= b.DataFinal)
                    || (dataInicial >= b.DataInicial && dataInicial <= b.DataFinal)
                    || (dataInicial <= b.DataInicial && dataFinal >= b.DataFinal)
                )
            );

            //Percentuais extra
            StringBuilder str = new StringBuilder();
            percExtras.Clear();
            foreach (var item in objTotalHoras.RateioHorasExtras.OrderBy(r => r.Key))
            {
                str.Append(String.Format("{0:000}", item.Key) + "%          ");
                str.Append(Modelo.cwkFuncoes.ConvertMinutosHora(3, item.Value.Diurno));
                str.Append("   " + Modelo.cwkFuncoes.ConvertMinutosHora(3, item.Value.Noturno));
                percExtras.Add(str.ToString());
                str.Remove(0, str.Length);
            }

            #region Saldo do Banco de Horas Fechamento

            if (fechamentosbhd.Exists(f => f.Identificacao == idfuncionario))
            {
                Modelo.FechamentoBHD objFechamentoBHD = fechamentosbhd.Where(f => f.Identificacao == idfuncionario).OrderBy(f => f.Id).Last();
                Modelo.FechamentoBH objFechamentoBH = fechamentosbh.Where(f => f.Id == objFechamentoBHD.Idfechamentobh).First();

                saldobancohorasfechamento = "Saldo Banco de Horas [" + objFechamentoBH.Data.Value.ToShortDateString() + "] ->>  " + objFechamentoBHD.Saldobh;
                saldobancohorasfechamento += objFechamentoBHD.Tiposaldo == 1 ? " Débito" : objFechamentoBHD.Tiposaldo == 0 ? " Crédito" : "";

            }

            #endregion

            if (ret.Rows.Count > 0)
            {
                AuxAtualizaFuncAnterior(ret.Rows.Count - 1, ret, creditodebitobh, creditodebitobhanterior,
                    creditodebitobhatual, saldobancohorasfechamento, existeBH, objTotalHoras, percExtras,
                    percSabado, percDomingo, percFolga, percFeriado, dataInicial, dataFinal);
                if (bInseriuDois)
                {
                    AuxAtualizaFuncAnterior(ret.Rows.Count - 2, ret, creditodebitobh, creditodebitobhanterior,
                        creditodebitobhatual, saldobancohorasfechamento, existeBH, objTotalHoras, percExtras,
                        percSabado, percDomingo, percFolga, percFeriado, dataInicial, dataFinal);
                }
            }

            #endregion
        }

        private void AuxAtualizaFuncAnterior(int indice, DataTable ret, string creditodebitobh,
            string creditodebitobhanterior, string creditodebitobhatual, string saldobancohorasfechamento,
            bool existeBH, Modelo.TotalHoras objTotalHoras, List<string> percExtras, string percSabado,
            string percDomingo, string percFolga, string percFeriado, DateTime dataInicial, DateTime dataFinal)
        {
            DataRow row = ret.Rows[indice];
            row["TotalBancoHorasCre"] = existeBH ? objTotalHoras.creditoBHPeriodo : "";
            row["TotalBancoHorasDeb"] = existeBH ? objTotalHoras.debitoBHPeriodo : "";
            row["totaldiurnageral"] = objTotalHoras.horasTrabDiurna;
            row["totalnoturnageral"] = objTotalHoras.horasTrabNoturna;
            row["diurnanoturnageral"] = Modelo.cwkFuncoes.ConvertMinutosHora(3, objTotalHoras.horasTrabDiurnaMin + objTotalHoras.horasTrabNoturnaMin);
            row["extradgeral"] = objTotalHoras.horasExtraDiurna;
            row["extrangeral"] = objTotalHoras.horasExtraNoturna;
            row["totalextrageral"] = Modelo.cwkFuncoes.ConvertMinutosHora(3, objTotalHoras.horasExtraDiurnaMin + objTotalHoras.horasExtraNoturnaMin);
            row["faltadgeral"] = objTotalHoras.horasFaltaDiurna;
            row["faltangeral"] = objTotalHoras.horasFaltaNoturna;
            row["totalfaltageral"] = Modelo.cwkFuncoes.ConvertMinutosHora(3, objTotalHoras.horasFaltaDiurnaMin + objTotalHoras.horasFaltaNoturnaMin);
            row["diasfaltageral"] = objTotalHoras.FaltasDias;
            row["dsrgeral"] = objTotalHoras.horasDDSR;
            row["qtdDDSR"] = objTotalHoras.qtdDDSR;
            row["horaextra1"] = (percExtras.Count > 0 ? percExtras[0] : "");
            row["horaextra2"] = (percExtras.Count > 1 ? percExtras[1] : "");
            row["horaextra3"] = (percExtras.Count > 2 ? percExtras[2] : "");
            row["horaextra4"] = (percExtras.Count > 3 ? percExtras[3] : "");
            row["extrasabado"] = (percExtras.Count > 4 ? percExtras[4] : "");
            row["extradomingo"] = (percExtras.Count > 5 ? percExtras[5] : "");
            row["extrafolga"] = (percExtras.Count > 6 ? percExtras[6] : "");
            row["extraferiado"] = (percExtras.Count > 7 ? percExtras[7] : "");
            row["saldobancohoras"] = existeBH ? objTotalHoras.saldoBHPeriodo : "";
            row["creditodebitobh"] = existeBH ? creditodebitobh : "";
            row["saldobancohorasanterior"] = existeBH ? objTotalHoras.saldoAnteriorBH : "";
            row["creditodebitobhanterior"] = existeBH ? creditodebitobhanterior : "";
            row["saldobancohorasatual"] = existeBH ? objTotalHoras.saldoBHAtual : "";
            row["creditodebitobhatual"] = existeBH ? creditodebitobhatual : "";
            row["saldobancohorasfechamento"] = saldobancohorasfechamento;
            row["existebh"] = existeBH ? 1 : 0;
            row["totalnoturnageral"] = objTotalHoras.horasTrabNoturna;
            row["totalAdicionalNoturno"] = objTotalHoras.horasAdNoturno;
            row["qtdAdNot"] = objTotalHoras.qtdAdNot;

            SetTotalAbonoDsr(ret.Rows[indice], dataInicial, dataFinal);
        }

        private void SetTotalAbonoDsr(DataRow dataRow, DateTime datai, DateTime dataf)
        {

            Modelo.Funcionario funcionario = dalFuncionario.LoadObject(dalFuncionario.GetIdDsCodigo(dataRow["dscodigo"].ToString()));
            Modelo.Proxy.pxyAbonoDsrFuncionario pxy = dalMarcacao.GetAbonoDsrFuncionario(funcionario.Id, datai, dataf);
            dataRow["totaldsrdias"] = pxy.QtdDiasDsr;
            dataRow["totaldsrhoras"] = Modelo.cwkFuncoes.ConvertMinutosHora2(3, pxy.QtdMinutosDsr);
            dataRow["totalabonodias"] = pxy.QtdDiasAbono;
            dataRow["totalabonohoras"] = Modelo.cwkFuncoes.ConvertMinutosHora2(3, pxy.QtdMinutosAbono);
        }

        public DataTable GetCartaoPontoDiaria(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, int tipo, System.Windows.Forms.ProgressBar pPBRecalculo)
        {
            BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, cwkControleUsuario.Facade.getUsuarioLogado);
            BLL.Empresa bllEmpresa = new BLL.Empresa(ConnectionString, cwkControleUsuario.Facade.getUsuarioLogado);
            System.Diagnostics.Stopwatch tempo = new System.Diagnostics.Stopwatch();
            tempo.Start();

            pPBRecalculo.Minimum = 0;
            pPBRecalculo.Maximum = 100;
            pPBRecalculo.Value = 0;

            DataTable dt = dalCartaoPonto.GetCartaoPontoDiaria(dataInicial, dataFinal, empresas, departamentos, tipo);

            pPBRecalculo.Value = 30;

            DataTable ret = new DataTable();

            #region Criaçao das colunas

            List<DataColumn> colunas = new List<DataColumn>();
            foreach (DataColumn c in dt.Columns)
            {

                if (Array.IndexOf(new string[] { "entrada_5", "entrada_6", "entrada_7", "entrada_8"
                    , "saida_5", "saida_6", "saida_7", "saida_8", "valordsr", "tipohorario"
                    , "idempresa", "iddepartamento", "idfuncionario", "idfuncao"
                    , "tratent_1", "tratent_2", "tratent_3", "tratent_4", "tratent_5", "tratent_6", "tratent_7", "tratent_8"
                    , "tratsai_1", "tratsai_2", "tratsai_3", "tratsai_4", "tratsai_5", "tratsai_6", "tratsai_7", "tratsai_8", "folga", "chave"
                }, c.ColumnName) == -1)
                {
                    ret.Columns.Add(c.ColumnName);
                }
            }

            pPBRecalculo.Value = 50;


            DataColumn[] colunasHora = new DataColumn[]
            {
                new DataColumn("hentrada_1"),
                new DataColumn("hentrada_2"),
                new DataColumn("hsaida_1"),
                new DataColumn("hsaida_2"),
                new DataColumn("totaltrabalhadadiurna"),
                new DataColumn("totaltrabalhadanoturna"),
                new DataColumn("totaldiurnageral"),
                new DataColumn("totalnoturnageral"),
                new DataColumn("diurnanoturnageral"),
                new DataColumn("extradgeral"),
                new DataColumn("extrangeral"),
                new DataColumn("totalextrageral"),
                new DataColumn("faltadgeral"),
                new DataColumn("faltangeral"),
                new DataColumn("totalfaltageral"),
                new DataColumn("diasfaltageral"),
                new DataColumn("dsrgeral"),
                new DataColumn("horaextra1"),
                new DataColumn("horaextra2"),
                new DataColumn("horaextra3"),
                new DataColumn("horaextra4"),
                new DataColumn("extrasabado"),
                new DataColumn("extradomingo"),
                new DataColumn("extrafolga"),
                new DataColumn("extraferiado"),
                new DataColumn("saldobancohoras"),
                new DataColumn("creditodebitobh"),
                new DataColumn("saldobancohorasanterior"),
                new DataColumn("creditodebitobhanterior"),
                new DataColumn("saldobancohorasatual"),
                new DataColumn("creditodebitobhatual"),
                new DataColumn("saldobancohorasfechamento"),
                new DataColumn("existebh"),
                new DataColumn("totalDSRDias"),
                new DataColumn("totalDSRHoras"),
                new DataColumn("totalAbonoDias"),
                new DataColumn("totalAbonoHoras"),
                new DataColumn("qtdDDSR"),
                new DataColumn("totalAdicionalNoturno"),
                new DataColumn("qtdAdNot")
            };

            ret.Columns.AddRange(colunasHora);

            #endregion

            pPBRecalculo.Value = 100;

            int idhorario = 0;
            int idfuncionarioAnt = 0;
            int idfuncionario = 0;
            DateTime data;

            Modelo.Horario objHorario = new Modelo.Horario();
            Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();

            List<Modelo.JornadaAlternativa> jornadasAlternativas = bllJornadaAlternativa.GetPeriodo(dataInicial, dataFinal);
            List<Modelo.BancoHoras> bancosdehoras = dalBancoHoras.GetAllList(false);
            List<Modelo.HorarioDetalhe> horariosdetalhe = null;
            List<Modelo.Horario> horarios = null;

            horariosdetalhe = dalHorarioDetalhe.GetAllList();
            horarios = dalHorario.GetAllList(false, true);

            int horasTrabalhadasDiurna = 0;
            int horasTrabalhadasNoturna = 0;
            int horasExtraDiurna = 0;
            int horasExtraNoturna = 0;
            int horasFaltaDiurna = 0;
            int horasFaltaNoturna = 0;
            int totalFaltas = 0;
            int count = 0;
            string creditodebitobh = "";
            string creditodebitobhanterior = "";
            string creditodebitobhatual = "";
            string saldobancohorasfechamento = "";
            string totalDSRHoras = "";
            string totalDSRDias = "";
            string totalAbonoHoras = "";
            string totalAbonoDias = "";
            bool existeBH = false;

            pPBRecalculo.Value = 0;
            pPBRecalculo.Maximum = dt.Rows.Count;

            //Modelo.TotalHoras objTotalHoras = null;
            foreach (DataRow row in dt.Rows)
            {
                idhorario = Convert.ToInt32(row["idhorario"].ToString());
                data = Convert.ToDateTime(row["data"].ToString());
                idfuncionario = Convert.ToInt32(row["idfuncionario"].ToString());

                if (idhorario != objHorario.Id)
                {
                    var hor = horarios.Where(h => h.Id == idhorario);
                    if (hor.Count() > 0)
                    {
                        objHorario = hor.First();
                    }
                    else
                    {
                        objHorario = dalHorario.LoadObject(idhorario);
                        horarios.Add(objHorario);
                        horariosdetalhe.AddRange(dalHorarioDetalhe.LoadPorHorario(objHorario.Id));
                    }
                }

                #region Verifica se a marcação não foi manipulada

                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                objMarcacao.Data = data;
                objMarcacao.Idfuncionario = idfuncionario;
                objMarcacao.Idhorario = idhorario;
                objMarcacao.Entrada_1 = Convert.ToString(row["entrada_1"]);
                objMarcacao.Entrada_2 = Convert.ToString(row["entrada_2"]);
                objMarcacao.Entrada_3 = Convert.ToString(row["entrada_3"]);
                objMarcacao.Entrada_4 = Convert.ToString(row["entrada_4"]);
                objMarcacao.Entrada_5 = Convert.ToString(row["entrada_5"]);
                objMarcacao.Entrada_6 = Convert.ToString(row["entrada_6"]);
                objMarcacao.Entrada_7 = Convert.ToString(row["entrada_7"]);
                objMarcacao.Entrada_8 = Convert.ToString(row["entrada_8"]);
                objMarcacao.Saida_1 = Convert.ToString(row["saida_1"]);
                objMarcacao.Saida_2 = Convert.ToString(row["saida_2"]);
                objMarcacao.Saida_3 = Convert.ToString(row["saida_3"]);
                objMarcacao.Saida_4 = Convert.ToString(row["saida_4"]);
                objMarcacao.Saida_5 = Convert.ToString(row["saida_5"]);
                objMarcacao.Saida_6 = Convert.ToString(row["saida_6"]);
                objMarcacao.Saida_7 = Convert.ToString(row["saida_7"]);
                objMarcacao.Saida_8 = Convert.ToString(row["saida_8"]);
                objMarcacao.Horastrabalhadas = Convert.ToString(row["horastrabalhadas"]);
                objMarcacao.Horasextrasdiurna = Convert.ToString(row["horasextrasdiurna"]);
                objMarcacao.Horasfaltas = Convert.ToString(row["horasfaltas"]);
                objMarcacao.Entradaextra = Convert.ToString(row["entradaextra"]);
                objMarcacao.Saidaextra = Convert.ToString(row["saidaextra"]);
                objMarcacao.Horastrabalhadasnoturnas = Convert.ToString(row["horastrabalhadasnoturnas"]);
                objMarcacao.Horasextranoturna = Convert.ToString(row["horasextranoturna"]);
                objMarcacao.Horasfaltanoturna = Convert.ToString(row["horasfaltanoturna"]);
                objMarcacao.Bancohorascre = Convert.ToString(row["bancohorascre"]);
                objMarcacao.Bancohorasdeb = Convert.ToString(row["bancohorasdeb"]);
                objMarcacao.Chave = Convert.ToString(row["chave"]);


                //Verifica se a marcação não foi manipulada
                if (!objMarcacao.MarcacaoOK())
                {
                    Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();
                    objEmpresa.BDAlterado = true;
                    objEmpresa.Chave = objEmpresa.HashMD5ComRelatoriosValidacaoNova();
                    bllEmpresa.Salvar(Modelo.Acao.Alterar, objEmpresa);
                    StringBuilder str = new StringBuilder("A marcação do funcionário ");
                    str.Append(row["funcionario"]);
                    str.Append(" no dia ");
                    str.Append(objMarcacao.Data.ToShortDateString());
                    str.Append(" foi alterada.");
                    str.AppendLine(" Para voltar a utilizar o sistema entre em contato com a revenda.");
                    throw new Exception(str.ToString());
                }

                #endregion

                List<string> percExtras = new List<string>();
                string percSabado = "", percDomingo = "", percFolga = "", percFeriado = "";

                if (idfuncionario != idfuncionarioAnt)
                {
                    horasTrabalhadasDiurna = 0;
                    horasTrabalhadasNoturna = 0;
                    totalFaltas = 0;
                    saldobancohorasfechamento = "";
                    totalAbonoHoras = "";
                    totalAbonoDias = "";
                    totalDSRHoras = "";
                    totalDSRDias = "";
                    existeBH = bancosdehoras.Exists
                    (b =>
                        (
                            (b.Tipo == 0 && b.Identificacao == Convert.ToInt32(row["idempresa"]))
                            || (b.Tipo == 1 && b.Identificacao == Convert.ToInt32(row["iddepartamento"]))
                            || (b.Tipo == 2 && b.Identificacao == idfuncionario)
                            || (b.Tipo == 3 && b.Identificacao == Convert.ToInt32(row["idfuncao"]))
                        )
                        &&
                        (
                            (dataFinal >= b.DataInicial && dataFinal <= b.DataFinal)
                            || (dataInicial >= b.DataInicial && dataInicial <= b.DataFinal)
                            || (dataInicial <= b.DataInicial && dataFinal >= b.DataFinal)
                        )
                    );

                    idfuncionarioAnt = idfuncionario;
                }

                bool folga = Convert.ToBoolean(row["folga"]);

                #region Verifica Jornada da Marcação

                Modelo.JornadaAlternativa objJornadaAlternativa = null;

                //Funcionario
                if (jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 2 && j.Identificacao == idfuncionario).Count() == 1)
                {
                    objJornadaAlternativa = jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 2 && j.Identificacao == idfuncionario).Single();
                }
                //Funcao
                else if (jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 3 && j.Identificacao == Convert.ToInt32(row["idfuncao"].ToString())).Count() == 1)
                {
                    objJornadaAlternativa = jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 3 && j.Identificacao == Convert.ToInt32(row["idfuncao"].ToString())).Single();
                }
                //Departamento
                else if (jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 1 && j.Identificacao == Convert.ToInt32(row["iddepartamento"].ToString())).Count() == 1)
                {
                    objJornadaAlternativa = jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 1 && j.Identificacao == Convert.ToInt32(row["iddepartamento"].ToString())).Single();
                }
                //Empresa
                else if (jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 0 && j.Identificacao == Convert.ToInt32(row["idempresa"].ToString())).Count() == 1)
                {
                    objJornadaAlternativa = jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 0 && j.Identificacao == Convert.ToInt32(row["idempresa"].ToString())).Single();
                }

                if (objJornadaAlternativa != null)
                {
                    objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    objHorarioDetalhe.Entrada_1 = objJornadaAlternativa.Entrada_1;
                    objHorarioDetalhe.Entrada_2 = objJornadaAlternativa.Entrada_2;
                    objHorarioDetalhe.Entrada_3 = objJornadaAlternativa.Entrada_3;
                    objHorarioDetalhe.Entrada_4 = objJornadaAlternativa.Entrada_4;
                    objHorarioDetalhe.Saida_1 = objJornadaAlternativa.Saida_1;
                    objHorarioDetalhe.Saida_2 = objJornadaAlternativa.Saida_2;
                    objHorarioDetalhe.Saida_3 = objJornadaAlternativa.Saida_3;
                    objHorarioDetalhe.Saida_4 = objJornadaAlternativa.Saida_4;
                    objHorarioDetalhe.Totaltrabalhadadiurna = objJornadaAlternativa.TotalTrabalhadaDiurna;
                    objHorarioDetalhe.Totaltrabalhadanoturna = objJornadaAlternativa.TotalTrabalhadaNoturna;
                }
                else
                {
                    if (Convert.ToInt32(row["tipohorario"]) == 1)
                    {
                        objHorarioDetalhe = horariosdetalhe.Where(hd => hd.Idhorario == objHorario.Id && hd.Dia == Modelo.cwkFuncoes.Dia(data)).First();
                        //objHorarioDetalhe = objHorario.HorariosDetalhe.Where(hd => hd.Dia == Modelo.cwkFuncoes.Dia(data)).First();
                    }
                    else
                    {
                        //if (objHorario.HorariosFlexiveis.Exists(hd => hd.Data == data))
                        if (horariosdetalhe.Exists(hd => hd.Idhorario == objHorario.Id && hd.Data == data))
                        {
                            objHorarioDetalhe = horariosdetalhe.Where(hd => hd.Idhorario == objHorario.Id && hd.Data == data).First();
                            //objHorarioDetalhe = objHorario.HorariosFlexiveis.Where(hd => hd.Data == data).First();
                        }
                        else
                        {
                            objHorarioDetalhe = new Modelo.HorarioDetalhe();
                            objHorarioDetalhe.Totaltrabalhadadiurna = "--:--";
                            objHorarioDetalhe.Totaltrabalhadanoturna = "--:--";
                            objHorarioDetalhe.Entrada_1 = "--:--";
                            objHorarioDetalhe.Entrada_2 = "--:--";
                            objHorarioDetalhe.Entrada_3 = "--:--";
                            objHorarioDetalhe.Entrada_4 = "--:--";
                            objHorarioDetalhe.Saida_1 = "--:--";
                            objHorarioDetalhe.Saida_2 = "--:--";
                            objHorarioDetalhe.Saida_3 = "--:--";
                            objHorarioDetalhe.Saida_4 = "--:--";
                        }
                    }
                }

                #endregion

                #region Monta so vetor apenas com as batidas validas

                string[] batidas = new string[16];
                MontaBatidasValidas(row, batidas);
                #endregion

                #region Insere os registros no dataset
                object[] values = new object[]
                {
                    row["id"],
                    row["idhorario"],
                    row["legenda"],
                    ((DateTime)row["data"]).ToShortDateString(),
                    Modelo.cwkFuncoes.DiaSemana(Convert.ToDateTime(row["data"]), Modelo.cwkFuncoes.TipoDiaSemana.Reduzido).Trim(new char[] {'.'}),
                    //Entradas
                    batidas[0],
                    batidas[2],
                    batidas[4],
                    batidas[6],
                    //Saidas
                    batidas[1],
                    batidas[3],
                    batidas[5],
                    batidas[7],
                    row["horastrabalhadas"].ToString() == "--:--" ? "" : row["horastrabalhadas"].ToString(),
                    row["horasextrasdiurna"].ToString() == "--:--" ? "" : row["horasextrasdiurna"].ToString(),
                    row["horasfaltas"].ToString() == "--:--" ? "" : row["horasfaltas"].ToString(),
                    row["entradaextra"].ToString() == "--:--" ? "" : row["entradaextra"].ToString(),
                    row["saidaextra"].ToString() == "--:--" ? "" : row["saidaextra"].ToString(),
                    row["horastrabalhadasnoturnas"].ToString() == "--:--" ? "" : row["horastrabalhadasnoturnas"].ToString(),
                    row["horasextranoturna"].ToString() == "--:--" ? "" : row["horasextranoturna"].ToString(),
                    row["horasfaltanoturna"].ToString() == "--:--" ? "" : row["horasfaltanoturna"].ToString(),
                    row["ocorrencia"],
                    row["dscodigo"],
                    row["funcionario"],
                    row["matricula"],
                    ((DateTime)row["dataadmissao"]).ToShortDateString(),
                    row["codigofolha"],
                    row["funcao"],
                    row["departamento"],
                    row["empresa"],
                    row["cnpj_cpf"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["thoraextra"],
                    row["thorafalta"],
                    "",//existeBH ? objTotalHoras.creditoBHPeriodo : "",
                    "",//existeBH ? objTotalHoras.debitoBHPeriodo : "",
                    folga ? "" : objHorarioDetalhe.Entrada_1 == "--:--" ? "" : objHorarioDetalhe.Entrada_1,
                    folga ? "" : objHorarioDetalhe.Entrada_2 == "--:--" ? "" : objHorarioDetalhe.Entrada_2,
                    folga ? "" : objHorarioDetalhe.Saida_1 == "--:--" ? "" : objHorarioDetalhe.Saida_1,
                    folga ? "" : objHorarioDetalhe.Saida_2 == "--:--" ? "" : objHorarioDetalhe.Saida_2,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasDiurna),
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasNoturna),
                    "",//objTotalHoras.horasTrabDiurna,
                    "",//objTotalHoras.horasTrabNoturna,                   
                    "",//Modelo.cwkFuncoes.ConvertMinutosHora(3,Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.horasTrabDiurna)+ Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.horasTrabNoturna)),
                    "",//objTotalHoras.horasExtraDiurna,
                    "",//objTotalHoras.horasExtraNoturna,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasExtraDiurna + horasExtraNoturna),
                    "",//objTotalHoras.horasFaltaDiurna,
                    "",//objTotalHoras.horasFaltaNoturna,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasFaltaDiurna + horasFaltaNoturna),
                    totalFaltas,
                    "",//objTotalHoras.horasDSR,
                    (percExtras.Count > 0 ? percExtras[0] : ""),
                    (percExtras.Count > 1 ? percExtras[1] : ""),
                    (percExtras.Count > 2 ? percExtras[2] : ""),
                    (percExtras.Count > 3 ? percExtras[3] : ""),
                    percSabado,
                    percDomingo,
                    percFolga,
                    percFeriado,
                    "",//existeBH ? objTotalHoras.saldoBHPeriodo : "",
                    existeBH ? creditodebitobh : "",
                    "",//existeBH ? objTotalHoras.saldoAnteriorBH : "",
                    existeBH ? creditodebitobhanterior : "",
                    "",//existeBH ? objTotalHoras.saldoBHAtual : "",
                    existeBH ? creditodebitobhatual : "",
                    saldobancohorasfechamento,
                    existeBH ? 1 : 0,
                    totalDSRDias,
                    totalDSRHoras,
                    totalAbonoDias,
                    totalAbonoHoras
                };
                ret.Rows.Add(values);

                if ((objHorarioDetalhe.Entrada_3 != "--:--" && !String.IsNullOrEmpty(objHorarioDetalhe.Entrada_3))
                    || (batidas[8] != "--:--" && !String.IsNullOrEmpty(batidas[8])))
                {
                    object[] values2 = new object[]
                    {
                    "",
                    "",
                    "",
                    "",
                    "",         
                    //Entradas           
                    batidas[8],
                    batidas[10],
                    batidas[12],
                    batidas[14],
                    //Saídas
                    batidas[9],
                    batidas[11],
                    batidas[13],
                    batidas[15],
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
                    row["departamento"],
                    row["empresa"],
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",//existeBH ? objTotalHoras.creditoBHPeriodo : "",
                    "",//existeBH ? objTotalHoras.debitoBHPeriodo : "",
                    folga ? "" : objHorarioDetalhe.Entrada_3 == "--:--" ? "" : objHorarioDetalhe.Entrada_3,
                    folga ? "" : objHorarioDetalhe.Entrada_4 == "--:--" ? "" : objHorarioDetalhe.Entrada_4,
                    folga ? "" : objHorarioDetalhe.Saida_3 == "--:--" ? "" : objHorarioDetalhe.Saida_3,
                    folga ? "" : objHorarioDetalhe.Saida_4 == "--:--" ? "" : objHorarioDetalhe.Saida_4,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasDiurna),
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasNoturna),
                    "",//objTotalHoras.horasTrabDiurna,
                    "",//objTotalHoras.horasTrabNoturna,                   
                    "",//Modelo.cwkFuncoes.ConvertMinutosHora(3,Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.horasTrabDiurna)+ Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.horasTrabNoturna)),
                    "",//objTotalHoras.horasExtraDiurna,
                    "",//objTotalHoras.horasExtraNoturna,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasExtraDiurna + horasExtraNoturna),
                    "",//objTotalHoras.horasFaltaDiurna,
                    "",//objTotalHoras.horasFaltaNoturna,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasFaltaDiurna + horasFaltaNoturna),
                    totalFaltas,
                    "",//objTotalHoras.horasDSR,
                    (percExtras.Count > 0 ? percExtras[0] : ""),
                    (percExtras.Count > 1 ? percExtras[1] : ""),
                    (percExtras.Count > 2 ? percExtras[2] : ""),
                    (percExtras.Count > 3 ? percExtras[3] : ""),
                    percSabado,
                    percDomingo,
                    percFolga,
                    percFeriado,
                    "",//existeBH ? "Saldo do Mês: " + objTotalHoras.saldoBHPeriodo : "",
                    existeBH ? creditodebitobh : "",
                    "",//existeBH ? "Saldo Anterior: " + objTotalHoras.saldoAnteriorBH : "",
                    existeBH ? creditodebitobhanterior : "",
                    "",//existeBH ? "Saldo Atual: " + objTotalHoras.saldoBHAtual : "",
                    existeBH ? creditodebitobhatual : "",
                    saldobancohorasfechamento,
                    existeBH ? 1 : 0,
                    totalDSRDias,
                    totalDSRHoras,
                    totalAbonoDias,
                    totalAbonoHoras
                    };
                    ret.Rows.Add(values2);
                }
                #endregion

                count++;

                pPBRecalculo.Value++;
            }

            tempo.Stop();
            return ret;
        }

        public bool TratarMarcacoes(DateTime pDataInicial, DateTime pDataFinal, List<int> pListIdFunc, BLL.Funcionario pBllFuncionario)
        {
            #region instancias bll
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
            BLL.MudancaHorario bllMudancaoHorario = new BLL.MudancaHorario(ConnectionString, UsuarioLogado);
            BLL.FechamentoBHD bllFechamentoBHD = new BLL.FechamentoBHD(ConnectionString, UsuarioLogado);
            BLL.Parametros bllParametro = new BLL.Parametros(ConnectionString, UsuarioLogado);
            BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(ConnectionString, UsuarioLogado);
            BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, UsuarioLogado);
            BLL.Feriado bllFeriado = new BLL.Feriado(ConnectionString, UsuarioLogado);
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(ConnectionString, UsuarioLogado);
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(ConnectionString, UsuarioLogado);
            BLL.Horario bLLHorario = new BLL.Horario(ConnectionString, UsuarioLogado);
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            BLL.Contrato bllContrato = new BLL.Contrato(ConnectionString, UsuarioLogado);
            #endregion

            bool retorno = false;
            TimeSpan ts = pDataFinal.AddDays(1) - pDataInicial;
            Dictionary<int, int> _qtdMarcacoesFuncionarios = bllMarcacao.QuantidadeMarcacoesPorLista(pListIdFunc, pDataInicial, pDataFinal);

            if (_qtdMarcacoesFuncionarios.Any(x => x.Value != ts.TotalDays))
            {
                var idsFuncs = _qtdMarcacoesFuncionarios.Where(x => x.Value != ts.TotalDays).Select(x => x.Key).ToList<int>();
                var _listFuncionario = pBllFuncionario.GetAllListByIds(string.Join(",", idsFuncs.ToArray()));
                _listFuncionario = _listFuncionario.Where(x => x.bFuncionarioativo == true).ToList();
                idsFuncs = _listFuncionario.Select(s => s.Id).ToList();

                if (_listFuncionario.Count() > 0)
                {
                    var _hTlistaIdsHorario = bllMudancaoHorario.GetHashTableByList(pDataInicial, pDataFinal, _listFuncionario);

                    List<Modelo.Parametros> _parametros = bllParametro.GetAllList();
                    List<Modelo.InclusaoBanco> _inclusaoBancoLista = bllInclusaoBanco.GetAllListByFuncionarios(idsFuncs);
                    List<Modelo.JornadaAlternativa> _jornadasAlternativas = bllJornadaAlternativa.GetPeriodoFuncionarios(pDataInicial, pDataFinal, idsFuncs);
                    List<Modelo.FechamentoBHD> _fechamentoBHDLista = bllFechamentoBHD.getPorListaFuncionario(idsFuncs);
                    List<Modelo.Feriado> _feriadoLista = bllFeriado.GetFeriadosFuncionarioPeriodo(idsFuncs, pDataInicial, pDataFinal);
                    List<Modelo.BancoHoras> _bancoHorasLista = bllBancoHoras.GetAllListFuncs(false, idsFuncs);
                    List<Modelo.Afastamento> _afastamentosLista = bllAfastamento.GetAfastamentoFuncionarioPeriodo(idsFuncs, pDataInicial, pDataFinal);
                    List<Modelo.MudancaHorario> _mudancaHorarioList = bllMudancaoHorario.GetPeriodo(pDataInicial, pDataFinal, idsFuncs);
                    List<Modelo.Horario> _horarios = bLLHorario.GetParaIncluirMarcacao(_hTlistaIdsHorario, true);
                    List<Modelo.Marcacao> _marcacoesPeriodo = bllMarcacao.GetPorFuncionarios(idsFuncs, pDataInicial, pDataFinal, true);
                    List<pxyFechamentoPontoFuncionario> _fechamentos = bllFechamentoPontoFuncionario.ListaFechamentoPontoFuncionario(2, idsFuncs, pDataInicial);
                    List<Modelo.Contrato> _contratosLista = new List<Modelo.Contrato>();

                    if (_afastamentosLista.Any(x => x.IdContrato != null))
                    {
                        _afastamentosLista.ForEach(x =>
                        {
                            var obj = bllContrato.ContratosPorFuncionario(x.IdFuncionario);
                            _contratosLista.Union(obj);
                        });
                    }

                    var erros = bllMarcacao.AtualizaData(_parametros, _horarios, pDataInicial, pDataFinal, _listFuncionario, _inclusaoBancoLista, _jornadasAlternativas, _fechamentoBHDLista, _feriadoLista, _bancoHorasLista, _afastamentosLista, _contratosLista, _mudancaHorarioList, _marcacoesPeriodo, _fechamentos);

                    retorno = (erros.Count() == 0);
                }
                else
                    retorno = true;//para funcionários inativos retornar true
            }

            return retorno;
        }

        public bool TratarMarcacoesPorDepartamento(DateTime pDataInicial, DateTime pDataFinal, string departamentos, BLL.Funcionario pBllFuncionario)
        {
            List<int> _intListDptos, _intListIdFunc;
            _intListDptos = departamentos.Trim(new char[] { '(', ')' }).Split(',').Select(int.Parse).ToList();
            _intListIdFunc = pBllFuncionario.GetIdsFuncsPorIdsEmpOuDepOuContra(_intListDptos, null, null, true, true, true);

            return TratarMarcacoes(pDataInicial, pDataFinal, _intListIdFunc, pBllFuncionario);
        }

        public bool TratarMarcacoesPorEmpresa(DateTime pDataInicial, DateTime pDataFinal, string empresas, Funcionario pBllFuncionario)
        {
            List<int> _intListEmpresas, _intListIdFunc;
            _intListEmpresas = empresas.Trim(new char[] { '(', ')' }).Split(',').Select(int.Parse).ToList();
            _intListIdFunc = pBllFuncionario.GetIdsFuncsPorIdsEmpOuDepOuContra(null, null, _intListEmpresas, false, true, true);

            return TratarMarcacoes(pDataInicial, pDataFinal, _intListIdFunc, pBllFuncionario);
        }

        public DataTable GetCartaoPontoDiariaWeb(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, int tipo, Modelo.ProgressBar pPBRecalculo)
        {
            BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, UsuarioLogado);
            BLL.Empresa bllEmpresa = new BLL.Empresa(ConnectionString, UsuarioLogado);
            BLL.Funcionario bllFunc = new BLL.Funcionario(ConnectionString, UsuarioLogado);

            System.Diagnostics.Stopwatch tempo = new System.Diagnostics.Stopwatch();
            tempo.Start();

            pPBRecalculo.setaMinMaxPB(0, 100);
            pPBRecalculo.incrementaPBCMensagem(1, "Validando Marcações...");

            bool _tratarMarcacoes;

            if (!string.IsNullOrEmpty(departamentos.Replace("(", "").Replace(")", "")))
                _tratarMarcacoes = TratarMarcacoesPorDepartamento(dataInicial, dataFinal, departamentos, bllFunc);
            else if (!string.IsNullOrEmpty(empresas.Replace("(", "").Replace(")", "")))
                _tratarMarcacoes = TratarMarcacoesPorEmpresa(dataInicial, dataFinal, empresas, bllFunc);
            pPBRecalculo.incrementaPBCMensagem(1, "Carregando Marcações...");

            DataTable dt = dalCartaoPonto.GetCartaoPontoDiaria(dataInicial, dataFinal, empresas, departamentos, tipo);
            int maxProgres = (dt.Rows.Count * 2) + 1;
            pPBRecalculo.setaMinMaxPB(0, maxProgres);
            if (dt.Rows.Count > 0)
            {
                pPBRecalculo.incrementaPBCMensagem(dt.Rows.Count / 3, "Gerando layout dos dados...");
            }

            DataTable ret = new DataTable();

            #region Criaçao das colunas

            List<DataColumn> colunas = new List<DataColumn>();
            foreach (DataColumn c in dt.Columns)
            {

                if (Array.IndexOf(new string[] { "entrada_5", "entrada_6", "entrada_7", "entrada_8"
                    , "saida_5", "saida_6", "saida_7", "saida_8", "valordsr", "tipohorario"
                    , "idempresa", "iddepartamento", "idfuncionario", "idfuncao"
                    , "tratent_1", "tratent_2", "tratent_3", "tratent_4", "tratent_5", "tratent_6", "tratent_7", "tratent_8"
                    , "tratsai_1", "tratsai_2", "tratsai_3", "tratsai_4", "tratsai_5", "tratsai_6", "tratsai_7", "tratsai_8", "folga", "chave"
                }, c.ColumnName) == -1)
                {
                    ret.Columns.Add(c.ColumnName);
                }
            }

            DataColumn[] colunasHora = new DataColumn[]
            {
                new DataColumn("hentrada_1"),
                new DataColumn("hentrada_2"),
                new DataColumn("hsaida_1"),
                new DataColumn("hsaida_2"),
                new DataColumn("totaltrabalhadadiurna"),
                new DataColumn("totaltrabalhadanoturna"),
                new DataColumn("totaldiurnageral"),
                new DataColumn("totalnoturnageral"),
                new DataColumn("diurnanoturnageral"),
                new DataColumn("extradgeral"),
                new DataColumn("extrangeral"),
                new DataColumn("totalextrageral"),
                new DataColumn("faltadgeral"),
                new DataColumn("faltangeral"),
                new DataColumn("totalfaltageral"),
                new DataColumn("diasfaltageral"),
                new DataColumn("dsrgeral"),
                new DataColumn("horaextra1"),
                new DataColumn("horaextra2"),
                new DataColumn("horaextra3"),
                new DataColumn("horaextra4"),
                new DataColumn("extrasabado"),
                new DataColumn("extradomingo"),
                new DataColumn("extrafolga"),
                new DataColumn("extraferiado"),
                new DataColumn("saldobancohoras"),
                new DataColumn("creditodebitobh"),
                new DataColumn("saldobancohorasanterior"),
                new DataColumn("creditodebitobhanterior"),
                new DataColumn("saldobancohorasatual"),
                new DataColumn("creditodebitobhatual"),
                new DataColumn("saldobancohorasfechamento"),
                new DataColumn("existebh"),
                new DataColumn("totalDSRDias"),
                new DataColumn("totalDSRHoras"),
                new DataColumn("totalAbonoDias"),
                new DataColumn("totalAbonoHoras"),
                new DataColumn("qtdDDSR"),
                new DataColumn("dataCompleta")
            };

            ret.Columns.AddRange(colunasHora);

            #endregion

            if (dt.Rows.Count > 0)
            {
                pPBRecalculo.setaValorPB(0);
                pPBRecalculo.incrementaPBCMensagem(dt.Rows.Count / 2, "Carregando dados auxiliares...");
            }

            int idhorario = 0;
            int idfuncionarioAnt = 0;
            int idfuncionario = 0;
            DateTime data;

            Modelo.Horario objHorario = new Modelo.Horario();
            Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();

            List<int> idsFuncs = dt.AsEnumerable().Select(s => s.Field<int>("idfuncionario")).Distinct().ToList();
            List<Modelo.JornadaAlternativa> jornadasAlternativas = bllJornadaAlternativa.GetPeriodoFuncionarios(dataInicial, dataFinal, idsFuncs);

            List<Modelo.BancoHoras> bancosdehoras = dalBancoHoras.GetAllListFuncs(false, idsFuncs);
            Hashtable ids = new Hashtable();
            foreach (var columnIdHorario in dt.AsEnumerable()
                    .Select(s => new {
                        id = s.Field<int>("idhorario"),
                    })
                    .Distinct().ToList())
            {
                ids.Add(Convert.ToInt32(columnIdHorario.id), Convert.ToInt32(columnIdHorario.id));
            }
            List<Modelo.Horario> horarios = dalHorario.GetParaIncluirMarcacao(ids, true);
            List<Modelo.HorarioDetalhe> horariosdetalhe = horarios.SelectMany(s => s.HorariosDetalhe).Distinct().ToList();
            if (dt.Rows.Count > 0)
            {
                pPBRecalculo.setaValorPB(0);
                pPBRecalculo.incrementaPBCMensagem(dt.Rows.Count, "Gerando Dados...");
            }

            int horasTrabalhadasDiurna = 0;
            int horasTrabalhadasNoturna = 0;
            int horasExtraDiurna = 0;
            int horasExtraNoturna = 0;
            int horasFaltaDiurna = 0;
            int horasFaltaNoturna = 0;
            int totalFaltas = 0;
            int count = 0;
            string creditodebitobh = "";
            string creditodebitobhanterior = "";
            string creditodebitobhatual = "";
            string saldobancohorasfechamento = "";
            string totalDSRHoras = "";
            string totalDSRDias = "";
            string totalAbonoHoras = "";
            string totalAbonoDias = "";
            bool existeBH = false;


            //Modelo.TotalHoras objTotalHoras = null;
            foreach (DataRow row in dt.Rows)
            {
                idhorario = Convert.ToInt32(row["idhorario"].ToString());
                data = Convert.ToDateTime(row["data"].ToString());
                idfuncionario = Convert.ToInt32(row["idfuncionario"].ToString());

                if (idhorario != objHorario.Id)
                {
                    var hor = horarios.Where(h => h.Id == idhorario);
                    if (hor.Count() > 0)
                    {
                        objHorario = hor.First();
                    }
                    else
                    {
                        objHorario = dalHorario.LoadObject(idhorario);
                        horarios.Add(objHorario);
                        horariosdetalhe.AddRange(dalHorarioDetalhe.LoadPorHorario(objHorario.Id));
                    }
                }

                List<string> percExtras = new List<string>();
                string percSabado = "", percDomingo = "", percFolga = "", percFeriado = "";

                if (idfuncionario != idfuncionarioAnt)
                {
                    horasTrabalhadasDiurna = 0;
                    horasTrabalhadasNoturna = 0;
                    totalFaltas = 0;
                    saldobancohorasfechamento = "";
                    totalAbonoHoras = "";
                    totalAbonoDias = "";
                    totalDSRHoras = "";
                    totalDSRDias = "";
                    existeBH = bancosdehoras.Exists
                    (b =>
                        (
                            (b.Tipo == 0 && b.Identificacao == Convert.ToInt32(row["idempresa"]))
                            || (b.Tipo == 1 && b.Identificacao == Convert.ToInt32(row["iddepartamento"]))
                            || (b.Tipo == 2 && b.Identificacao == idfuncionario)
                            || (b.Tipo == 3 && b.Identificacao == Convert.ToInt32(row["idfuncao"]))
                        )
                        &&
                        (
                            (dataFinal >= b.DataInicial && dataFinal <= b.DataFinal)
                            || (dataInicial >= b.DataInicial && dataInicial <= b.DataFinal)
                            || (dataInicial <= b.DataInicial && dataFinal >= b.DataFinal)
                        )
                    );

                    idfuncionarioAnt = idfuncionario;
                }

                bool folga = Convert.ToBoolean(row["folga"]);

                #region Verifica Jornada da Marcação

                Modelo.JornadaAlternativa objJornadaAlternativa = null;

                //Funcionario
                if (jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 2 && j.Identificacao == idfuncionario).Count() == 1)
                {
                    objJornadaAlternativa = jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 2 && j.Identificacao == idfuncionario).Single();
                }
                //Funcao
                else if (jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 3 && j.Identificacao == Convert.ToInt32(row["idfuncao"].ToString())).Count() == 1)
                {
                    objJornadaAlternativa = jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 3 && j.Identificacao == Convert.ToInt32(row["idfuncao"].ToString())).Single();
                }
                //Departamento
                else if (jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 1 && j.Identificacao == Convert.ToInt32(row["iddepartamento"].ToString())).Count() == 1)
                {
                    objJornadaAlternativa = jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 1 && j.Identificacao == Convert.ToInt32(row["iddepartamento"].ToString())).Single();
                }
                //Empresa
                else if (jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 0 && j.Identificacao == Convert.ToInt32(row["idempresa"].ToString())).Count() == 1)
                {
                    objJornadaAlternativa = jornadasAlternativas.Where(j => data >= j.DataInicial && data <= j.DataFinal && tipo == 0 && j.Identificacao == Convert.ToInt32(row["idempresa"].ToString())).Single();
                }

                if (objJornadaAlternativa != null)
                {
                    objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    objHorarioDetalhe.Entrada_1 = objJornadaAlternativa.Entrada_1;
                    objHorarioDetalhe.Entrada_2 = objJornadaAlternativa.Entrada_2;
                    objHorarioDetalhe.Entrada_3 = objJornadaAlternativa.Entrada_3;
                    objHorarioDetalhe.Entrada_4 = objJornadaAlternativa.Entrada_4;
                    objHorarioDetalhe.Saida_1 = objJornadaAlternativa.Saida_1;
                    objHorarioDetalhe.Saida_2 = objJornadaAlternativa.Saida_2;
                    objHorarioDetalhe.Saida_3 = objJornadaAlternativa.Saida_3;
                    objHorarioDetalhe.Saida_4 = objJornadaAlternativa.Saida_4;
                    objHorarioDetalhe.Totaltrabalhadadiurna = objJornadaAlternativa.TotalTrabalhadaDiurna;
                    objHorarioDetalhe.Totaltrabalhadanoturna = objJornadaAlternativa.TotalTrabalhadaNoturna;
                }
                else
                {
                    if (Convert.ToInt32(row["tipohorario"]) == 1)
                    {
                        objHorarioDetalhe = horariosdetalhe.Where(hd => hd.Idhorario == objHorario.Id && hd.Dia == Modelo.cwkFuncoes.Dia(data)).First();
                        //objHorarioDetalhe = objHorario.HorariosDetalhe.Where(hd => hd.Dia == Modelo.cwkFuncoes.Dia(data)).First();
                    }
                    else
                    {
                        //if (objHorario.HorariosFlexiveis.Exists(hd => hd.Data == data))
                        if (horariosdetalhe.Exists(hd => hd.Idhorario == objHorario.Id && hd.Data == data))
                        {
                            objHorarioDetalhe = horariosdetalhe.Where(hd => hd.Idhorario == objHorario.Id && hd.Data == data).First();
                            //objHorarioDetalhe = objHorario.HorariosFlexiveis.Where(hd => hd.Data == data).First();
                        }
                        else
                        {
                            objHorarioDetalhe = new Modelo.HorarioDetalhe();
                            objHorarioDetalhe.Totaltrabalhadadiurna = "--:--";
                            objHorarioDetalhe.Totaltrabalhadanoturna = "--:--";
                            objHorarioDetalhe.Entrada_1 = "--:--";
                            objHorarioDetalhe.Entrada_2 = "--:--";
                            objHorarioDetalhe.Entrada_3 = "--:--";
                            objHorarioDetalhe.Entrada_4 = "--:--";
                            objHorarioDetalhe.Saida_1 = "--:--";
                            objHorarioDetalhe.Saida_2 = "--:--";
                            objHorarioDetalhe.Saida_3 = "--:--";
                            objHorarioDetalhe.Saida_4 = "--:--";
                        }
                    }
                }

                #endregion

                #region Monta so vetor apenas com as batidas validas

                string[] batidas = new string[16];
                MontaBatidasValidas(row, batidas);
                #endregion

                #region Insere os registros no dataset
                object[] values = new object[]
                {
                    row["id"],
                    row["idhorario"],
                    row["legenda"],
                    String.Format("{0:00}", ((DateTime)row["data"]).Day) + "/" + ((DateTime)row["data"]).Month.ToString(),
                    Modelo.cwkFuncoes.DiaSemana(Convert.ToDateTime(row["data"]), Modelo.cwkFuncoes.TipoDiaSemana.Reduzido).Trim(new char[] {'.'}),
                    //Entradas
                    batidas[0],
                    batidas[2],
                    batidas[4],
                    batidas[6],
                    //Saidas
                    batidas[1],
                    batidas[3],
                    batidas[5],
                    batidas[7],
                    row["horastrabalhadas"].ToString() == "--:--" ? "" : row["horastrabalhadas"].ToString(),
                    row["horasextrasdiurna"].ToString() == "--:--" ? "" : row["horasextrasdiurna"].ToString(),
                    row["horasfaltas"].ToString() == "--:--" ? "" : row["horasfaltas"].ToString(),
                    row["entradaextra"].ToString() == "--:--" ? "" : row["entradaextra"].ToString(),
                    row["saidaextra"].ToString() == "--:--" ? "" : row["saidaextra"].ToString(),
                    row["horastrabalhadasnoturnas"].ToString() == "--:--" ? "" : row["horastrabalhadasnoturnas"].ToString(),
                    row["horasextranoturna"].ToString() == "--:--" ? "" : row["horasextranoturna"].ToString(),
                    row["horasfaltanoturna"].ToString() == "--:--" ? "" : row["horasfaltanoturna"].ToString(),
                    row["ocorrencia"],
                    row["dscodigo"],
                    row["funcionario"],
                    row["matricula"],
                    ((DateTime)row["dataadmissao"]).ToShortDateString(),
                    row["codigofolha"],
                    row["funcao"],
                    row["departamento"],
                    row["empresa"],
                    row["cnpj_cpf"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["thoraextra"],
                    row["thorafalta"],
                    "",//existeBH ? objTotalHoras.creditoBHPeriodo : "",
                    "",//existeBH ? objTotalHoras.debitoBHPeriodo : "",
                    folga ? "" : objHorarioDetalhe.Entrada_1 == "--:--" ? "" : objHorarioDetalhe.Entrada_1,
                    folga ? "" : objHorarioDetalhe.Entrada_2 == "--:--" ? "" : objHorarioDetalhe.Entrada_2,
                    folga ? "" : objHorarioDetalhe.Saida_1 == "--:--" ? "" : objHorarioDetalhe.Saida_1,
                    folga ? "" : objHorarioDetalhe.Saida_2 == "--:--" ? "" : objHorarioDetalhe.Saida_2,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasDiurna),
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasNoturna),
                    "",//objTotalHoras.horasTrabDiurna,
                    "",//objTotalHoras.horasTrabNoturna,                   
                    "",//Modelo.cwkFuncoes.ConvertMinutosHora(3,Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.horasTrabDiurna)+ Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.horasTrabNoturna)),
                    "",//objTotalHoras.horasExtraDiurna,
                    "",//objTotalHoras.horasExtraNoturna,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasExtraDiurna + horasExtraNoturna),
                    "",//objTotalHoras.horasFaltaDiurna,
                    "",//objTotalHoras.horasFaltaNoturna,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasFaltaDiurna + horasFaltaNoturna),
                    totalFaltas,
                    "",//objTotalHoras.horasDSR,
                    (percExtras.Count > 0 ? percExtras[0] : ""),
                    (percExtras.Count > 1 ? percExtras[1] : ""),
                    (percExtras.Count > 2 ? percExtras[2] : ""),
                    (percExtras.Count > 3 ? percExtras[3] : ""),
                    percSabado,
                    percDomingo,
                    percFolga,
                    percFeriado,
                    "",//existeBH ? objTotalHoras.saldoBHPeriodo : "",
                    existeBH ? creditodebitobh : "",
                    "",//existeBH ? objTotalHoras.saldoAnteriorBH : "",
                    existeBH ? creditodebitobhanterior : "",
                    "",//existeBH ? objTotalHoras.saldoBHAtual : "",
                    existeBH ? creditodebitobhatual : "",
                    saldobancohorasfechamento,
                    existeBH ? 1 : 0,
                    totalDSRDias,
                    totalDSRHoras,
                    totalAbonoDias,
                    totalAbonoHoras,
                    0,
                    row["data"]
                };
                ret.Rows.Add(values);

                if ((objHorarioDetalhe.Entrada_3 != "--:--" && !String.IsNullOrEmpty(objHorarioDetalhe.Entrada_3))
                    || (batidas[8] != "--:--" && !String.IsNullOrEmpty(batidas[8])))
                {
                    object[] values2 = new object[]
                    {
                    "",
                    "",
                    "",
                    "",
                    "",         
                    //Entradas           
                    batidas[8],
                    batidas[10],
                    batidas[12],
                    batidas[14],
                    //Saídas
                    batidas[9],
                    batidas[11],
                    batidas[13],
                    batidas[15],
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
                    row["departamento"],
                    row["empresa"],
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",//existeBH ? objTotalHoras.creditoBHPeriodo : "",
                    "",//existeBH ? objTotalHoras.debitoBHPeriodo : "",
                    folga ? "" : objHorarioDetalhe.Entrada_3 == "--:--" ? "" : objHorarioDetalhe.Entrada_3,
                    folga ? "" : objHorarioDetalhe.Entrada_4 == "--:--" ? "" : objHorarioDetalhe.Entrada_4,
                    folga ? "" : objHorarioDetalhe.Saida_3 == "--:--" ? "" : objHorarioDetalhe.Saida_3,
                    folga ? "" : objHorarioDetalhe.Saida_4 == "--:--" ? "" : objHorarioDetalhe.Saida_4,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasDiurna),
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasTrabalhadasNoturna),
                    "",//objTotalHoras.horasTrabDiurna,
                    "",//objTotalHoras.horasTrabNoturna,                   
                    "",//Modelo.cwkFuncoes.ConvertMinutosHora(3,Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.horasTrabDiurna)+ Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.horasTrabNoturna)),
                    "",//objTotalHoras.horasExtraDiurna,
                    "",//objTotalHoras.horasExtraNoturna,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasExtraDiurna + horasExtraNoturna),
                    "",//objTotalHoras.horasFaltaDiurna,
                    "",//objTotalHoras.horasFaltaNoturna,
                    Modelo.cwkFuncoes.ConvertMinutosHora(3,horasFaltaDiurna + horasFaltaNoturna),
                    totalFaltas,
                    "",//objTotalHoras.horasDSR,
                    (percExtras.Count > 0 ? percExtras[0] : ""),
                    (percExtras.Count > 1 ? percExtras[1] : ""),
                    (percExtras.Count > 2 ? percExtras[2] : ""),
                    (percExtras.Count > 3 ? percExtras[3] : ""),
                    percSabado,
                    percDomingo,
                    percFolga,
                    percFeriado,
                    "",//existeBH ? "Saldo do Mês: " + objTotalHoras.saldoBHPeriodo : "",
                    existeBH ? creditodebitobh : "",
                    "",//existeBH ? "Saldo Anterior: " + objTotalHoras.saldoAnteriorBH : "",
                    existeBH ? creditodebitobhanterior : "",
                    "",//existeBH ? "Saldo Atual: " + objTotalHoras.saldoBHAtual : "",
                    existeBH ? creditodebitobhatual : "",
                    saldobancohorasfechamento,
                    existeBH ? 1 : 0,
                    totalDSRDias,
                    totalDSRHoras,
                    totalAbonoDias,
                    totalAbonoHoras,
                    0,
                    row["data"]
                    };
                    ret.Rows.Add(values2);
                }
                #endregion

                count++;

                pPBRecalculo.incrementaPBCMensagem(1, "");
            }

            tempo.Stop();
            return ret;
        }
    }
}
