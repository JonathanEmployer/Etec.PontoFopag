using BLL.Epays;
using BLL.Util;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Relatorios.V2
{
    public class RelatorioExportacaoFechamentoPontoBLL : RelatorioBaseBLL
    {
        private string _connString = "";
        private ExportacaoFechamentoPontoModel _parms;
        private ProgressBar _objProgressBar;
        private BLL.Parametros _bllParametro;
        private BLL.FechamentoPonto _bllFechamentoPonto;
        private BLL.Funcionario _bllFuncionario;
        private FechamentoPontoEpaysBLL _bllFechamentoEpays;
        private int _jobId;

        public RelatorioExportacaoFechamentoPontoBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar, int JobId)
            : base(relatorioFiltro, usuario, new ProgressBar())
        {
            _progressBar = getProgressVazio();
            _connString = _usuario.ConnectionString;

            _bllParametro = new BLL.Parametros(_connString, _usuario);
            _bllFechamentoPonto = new BLL.FechamentoPonto(_connString, _usuario);
            _bllFuncionario = new BLL.Funcionario(_connString, _usuario);
            _bllFechamentoEpays = new BLL.Epays.FechamentoPontoEpaysBLL(_connString);

            _jobId = JobId;
            _parms = (ExportacaoFechamentoPontoModel)relatorioFiltro;
            _objProgressBar = progressBar;
        }

        private ProgressBar getProgressVazio()
        {
            return new ProgressBar
            {
                incrementaPB = (incremento) => { },
                setaMensagem = (msg) => { },
                setaMinMaxPB = (min, max) => { },
                setaValorPB = (valor) => { },
                setaValorPBCMensagem = (val, msg) => { },
                incrementaPBCMensagem = (incremento, mensagem) => { }
            };
        }

        protected override string GetRelatorioPDF()
        {
            var IdTracking = Guid.NewGuid().ToString();
            var fechamento = _bllFechamentoPonto.LoadObject((int)_parms.IdFechamentoPonto);
            var idsFuncionario = GetIdsFuncionario(_parms.IdSelecionados);
            var dtPeriodosFechamento = _bllFuncionario.GetEmpresaPeriodoFechamentoPonto(idsFuncionario);
            var lstResult = new List<DocumentoHashDto>();
            var dicFuncsEmpParam = _parms.LstFuncs.ToDictionary(f => f.idFuncionario, f => f);

            _objProgressBar.setaMinMaxPB(0, dtPeriodosFechamento.Rows.Count);
            _objProgressBar.incrementaPBCMensagem(1, "Iniciado processo!");

            int count = 1;
            foreach (DataRow row in dtPeriodosFechamento.Rows)
            {
                var curRow = dtPeriodosFechamento.Rows.IndexOf(row) + 1;
                var curPeriodo = _bllFechamentoPonto.GetMesAnoFechamento(fechamento.Id, (int)row["idEmpresa"], (int)row["id"]);
                if (curPeriodo.Ano == null || curPeriodo.Mes == null)
                    continue;

                var periodoFechamento = _bllFechamentoPonto.GetPeriodoFechamento((int)curPeriodo.Mes, (int)curPeriodo.Ano,
                                                                                    Convert.ToInt32(row["DiaFechamentoInicial"]),
                                                                                        Convert.ToInt32(row["DiaFechamentoFinal"]));

                _parms.InicioPeriodo = periodoFechamento.dtInicio;
                _parms.FimPeriodo = periodoFechamento.dtFim;
                _parms.IdSelecionados = row["id"].ToString();
                _parms.TipoSelecao = 2;

                using (var Dt = GetDados((int)row["id"]))
                {
                    List<ReportParameter> parametros = GetParams();

                    string nomerel = String.Empty;
                    nomerel = GetReportName();

                    string ds = "dsCartaoPonto_DataTable1";

                    if (Dt.Rows.Count > 0)
                    {
                        var nomeArquivo = Path.Combine(IdTracking, Guid.NewGuid().ToString());
                        ParametrosReportView parametrosReport = new ParametrosReportView()
                        {
                            DataSourceName = ds,
                            DataTable = Dt,
                            NomeArquivo = nomeArquivo,
                            ReportRdlcName = nomerel,
                            ReportParameter = parametros,
                            TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
                        };

                        nomeArquivo = base.GerarArquivoReportView(parametrosReport);

                        lstResult.Add(new DocumentoHashDto
                        {
                            Tracking = IdTracking,
                            Cnpj = Dt.Rows[0]["cnpj_cpf"].ToString(),
                            NomeArquivo = nomeArquivo,
                            Mes = (int)curPeriodo.Mes,
                            Ano = (int)curPeriodo.Ano,
                            IdFechamento = fechamento.Id,
                            IdEmpresa = (int)row["idEmpresa"],
                            IdFuncionario = (int)row["id"],
                            Info = (curRow, dtPeriodosFechamento.Rows.Count),
                            InicioPeriodo = _parms.InicioPeriodo,
                            FimPeriodo = _parms.FimPeriodo,
                            Cpf = (string)row["CPF"],
                            Matricula = (string)row["matricula"],
                            Nome = (string)row["nome"],
                            DataBaseName = GetDataBaseName(),
                            UserEPays = dicFuncsEmpParam[(int)row["id"]].userEpays,
                            PasswordEPays = dicFuncsEmpParam[(int)row["id"]].passwordEpays,
                        });

                        parametrosReport = null;
                    }
                }

                _objProgressBar.incrementaPBCMensagem(count++, $"funcionário: {row["nome"]} documento processado!");
            }
            _objProgressBar.setaMensagem("Documentos Gerados com sucesso!");

            _objProgressBar.setaMinMaxPB(0, 100);
            _objProgressBar.setaValorPB(0);
            _objProgressBar.setaMensagem("Upload dos documentos p/ Epays");
            _objProgressBar.setaValorPB(10);
            _bllFechamentoEpays.UploadStorage(lstResult);

            _objProgressBar.setaValorPB(80);
            _bllFechamentoEpays.FinishUpload(IdTracking);
            _objProgressBar.setaValorPB(100);

            _objProgressBar.setaMensagem("Processo finalizado");

            return string.Empty;
        }

        protected override string GetRelatorioExcel()
        {
            throw new NotImplementedException();
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }

        protected override string SaveFile(string NomeArquivo, string fileNameExtension, byte[] renderedBytes)
        {
            if (string.IsNullOrEmpty(NomeArquivo))
                throw new ApplicationException("Nome do arquivo não foi informado");

            string fileName = Path.Combine(PathRelatorios, NomeArquivo + "." + fileNameExtension);
            string directory = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
                fs.Write(renderedBytes, 0, renderedBytes.Length);

            return fileName;
        }

        private DataTable GetDados(params int[] ids)
        {
            BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(_connString, _usuario);

            DataTable Dt = bllCartaoPonto.GetCartaoPontoRel(_parms.InicioPeriodo,
                _parms.FimPeriodo, "", "",
                "(" + String.Join(",", ids) + ")", _parms.TipoSelecao, _parms.TipoTurno, _parms.TipoSelecao, getProgressVazio(), false, "", _parms.quebraAuto);

            return Dt;
        }

        private string GetReportName()
        {
            string nomerel;
            if (_parms.Orientacao == 0)
            {
                nomerel = "rptCartaoPontoIndividual.rdlc";
            }
            else
            {
                nomerel = "rptCartaoPontoIndividualPaisagem.rdlc";
            }

            return nomerel;
        }

        private string GetDataBaseName()
        {
            return new Regex(@"(Catalog=[A-Z]*_[A-Z]*)").Match(_connString).Value.Replace("Catalog=", "");
        }

        private List<ReportParameter> GetParams()
        {
            Modelo.Parametros objParametro = _bllParametro.LoadPrimeiro();
            var parametros = new List<ReportParameter>();

            ReportParameter p1 = new ReportParameter("datainicial", _parms.InicioPeriodo.ToShortDateString());
            parametros.Add(p1);
            ReportParameter p2 = new ReportParameter("datafinal", _parms.FimPeriodo.ToShortDateString());
            parametros.Add(p2);
            ReportParameter p3 = new ReportParameter("observacao", objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
            parametros.Add(p3);
            ReportParameter p4 = new ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
            parametros.Add(p4);

            ReportParameter p5 = new ReportParameter("ordenadepartamento", false.ToString());
            parametros.Add(p5);

            ReportParameter p6 = new ReportParameter("visible", false.ToString());
            parametros.Add(p6);

            return parametros;
        }

        private int[] GetIdsFuncionario(string IdSelecionados)
        {
            return IdSelecionados.Split(',').Where(w => !String.IsNullOrEmpty(w)).Select(s => int.Parse(s)).ToArray();
        }


    }
}
