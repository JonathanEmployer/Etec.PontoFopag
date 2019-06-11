using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioFuncionarioBLL : RelatorioBaseBLL, IRelatorioBLL
    {
		public RelatorioFuncionarioBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
		}

		protected override string GetRelatorioExcel()
        {
            DataTable Dt;
            string NomeEmpresa, nomeDoArquivo, nomerel;
            RelatorioFuncionarioModel parms;
            byte[] arquivo = null;

            parms = ((RelatorioFuncionarioModel)_relatorioFiltro);

            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out NomeEmpresa, out Dt);

            arquivo = GetArquivo(Dt, parms);

            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;
        }

        private byte[] GetArquivo(DataTable Dt, RelatorioFuncionarioModel parms)
        {
            byte[] retorno;
            switch (parms.Relatorio)
            {
                case "1":
                    retorno = BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Nome(Dt);
					break;
                case "2":
                    retorno = BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Codigo(Dt);
					break;
                case "3":
                    retorno = BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Departamento(Dt);
                    break;
                case "4":
                    retorno = BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Empresa(Dt);
                    break;
                case "5":
                    retorno = BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Horario(Dt);
                    break;
                case "6":
                    retorno = BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Ativo_Inativo(Dt, (parms.AtivoInativo == 0) ? "Ativos" : "Inativos");
                    break;
                case "7":
                    retorno = BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Admissao(Dt);
                    break;
                case "8":
                    retorno = BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Demissao(Dt);
                    break;
                default:
                    retorno = new byte[0];
                    break;
            }

            return retorno;
        }

        protected override string GetRelatorioPDF()
        {
            RelatorioFuncionarioModel parms;
            string NomeEmpresa,nomeDoArquivo, nomerel;
            DataTable Dt;

			parms = ((RelatorioFuncionarioModel)_relatorioFiltro);

            GetDadosRel(parms, out nomeDoArquivo, out nomerel, out NomeEmpresa, out Dt);
	
			List<ReportParameter> parametros = SetaParametrosRelatorio(parms, NomeEmpresa);			
			ParametrosReportView parametrosReport = new ParametrosReportView()
			{
				DataSourceName = "dsFuncionarios_Funcionarios",
				DataTable = Dt,
				NomeArquivo = nomeDoArquivo,
				ReportRdlcName = nomerel,
				ReportParameter = parametros,
				TipoArquivo = Modelo.Enumeradores.TipoArquivo.PDF
                         };

            string caminho = GerarArquivoReportView(parametrosReport);
            return caminho;
        }

		private void GetDadosRel(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable Dt)
        {
            switch (parms.Relatorio.ToString())
            {
                case "1":
                    FuncionarioPorNome(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
                case "2":
                    FuncionarioPorCodigo(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
                case "3":
                    FuncionarioPorDepartamento(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
                case "4":
                    FuncionarioPorEmpresa(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
                case "5":
                    FuncionarioPorHorario(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
                case "6":
                    FuncionarioPorAtivoInativo(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
                case "7":
                    FuncionarioPorDataAdmissao(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
                case "8":
                    FuncionarioPorDataDemissao(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
                default:
                    FuncionarioPorNome(parms, out nomeDoArquivo, out nomerel, out nomeEmpresa, out Dt);
                    break;
            }


        }

        private void FuncionarioPorDataDemissao(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable dt)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            var empPrincipal = bllEmpresa.GetEmpresaPrincipal();
            nomeEmpresa = empPrincipal.Nome;
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();
            List<ReportParameter> parametros = new List<ReportParameter>();

            dt = bllFuncionario.GetPorDataDemissaoRel(parms.InicioPeriodo, parms.FimPeriodo, idsFuncs);
            
            nomerel = "rptFuncionariosDemissao.rdlc";
            
            nomeDoArquivo = "Relatório_Funcionários_Demitidos";
        }

        private void FuncionarioPorDataAdmissao(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable dt)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            var empPrincipal = bllEmpresa.GetEmpresaPrincipal();
            nomeEmpresa = empPrincipal.Nome;
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            dt = bllFuncionario.GetPorDataAdmissaoRel(parms.InicioPeriodo, parms.FimPeriodo, idsFuncs);
            
            nomerel = "rptFuncionariosAdmissao.rdlc";
            
            nomeDoArquivo = "Relatório_Funcionários_Admitidos";
        }

        private void FuncionarioPorAtivoInativo(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable dt)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            var empPrincipal = bllEmpresa.GetEmpresaPrincipal();
            nomeEmpresa = empPrincipal.Nome;
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            dt = bllFuncionario.GetAtivosInativosRel(!Convert.ToBoolean(parms.AtivoInativo), idsFuncs);
            
            nomerel = "rptFuncionariosAtivosInativos.rdlc";
           
            nomeDoArquivo = "Relatório_Funcionários_Ativos-Inativos";
        }

        private void FuncionarioPorHorario(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable dt)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            var empPrincipal = bllEmpresa.GetEmpresaPrincipal();
            nomeEmpresa = empPrincipal.Nome;
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            dt = bllFuncionario.GetPorHorarioRel(idsFuncs);
           
            nomerel = "rptFuncionariosPorHorario.rdlc";
          
            nomeDoArquivo = "Relatório_Funcionários_por_Horário";
        }

        private void FuncionarioPorEmpresa(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable Dt)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            var empPrincipal = bllEmpresa.GetEmpresaPrincipal();
            nomeEmpresa = empPrincipal.Nome;
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            Dt = bllFuncionario.GetRelatorio(idsFuncs);
            
            nomerel = "rptFuncionariosPorEmpresa.rdlc";
            
            nomeDoArquivo = "Relatório_Funcionários_por_Empresa";
        }

        private void FuncionarioPorDepartamento(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable Dt)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            var empPrincipal = bllEmpresa.GetEmpresaPrincipal();
            nomeEmpresa = empPrincipal.Nome;
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            Dt = bllFuncionario.GetPorDepartamentoRel(idsFuncs);
            
            nomerel = "rptFuncionariosPorDepartamento.rdlc";
            
            nomeDoArquivo = "Relatório_Funcionários_por_Departamento";
        }

        private void FuncionarioPorCodigo(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable Dt)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            var empPrincipal = bllEmpresa.GetEmpresaPrincipal();
            nomeEmpresa = empPrincipal.Nome;
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            Dt = bllFuncionario.GetOrdenadoPorCodigoRel(idsFuncs);
           
            nomerel = "rptFuncionarios.rdlc";
            
            nomeDoArquivo = "Relatório_Funcionários_por_Código";
        }

        private void FuncionarioPorNome(RelatorioFuncionarioModel parms, out string nomeDoArquivo, out string nomerel, out string nomeEmpresa, out DataTable Dt)
        {

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usuario.ConnectionString, _usuario);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados...");

            var empPrincipal = bllEmpresa.GetEmpresaPrincipal();
            nomeEmpresa = empPrincipal.Nome;
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            Dt = bllFuncionario.GetOrdenadoPorNomeRel(idsFuncs);
            
            nomerel = "rptFuncionarios.rdlc";

            nomeDoArquivo = "Relatório_Funcionário_Por_Nome";
           
        }

        private DataTable GetDataTableAbono()
        {
            var dt = new DataTable();
            dt.Columns.Add("empresa");
            dt.Columns.Add("cnpj_cpf");
            dt.Columns.Add("departamento");
            dt.Columns.Add("funcionario");
            dt.Columns.Add("dscodigo");
            dt.Columns.Add("ocorrencia");
            dt.Columns.Add("dtMarcacao");
            dt.Columns.Add("dia");
            dt.Columns.Add("abonoparcial");
            dt.Columns.Add("abonototal");
            return dt;
        }

        private static List<ReportParameter> SetaParametrosRelatorio(Modelo.Relatorios.RelatorioFuncionarioModel parms, string NomeEmpresa)
        {
            List<ReportParameter> parametros = new List<ReportParameter>();

            parametros.Add( new ReportParameter("empresa", NomeEmpresa));

            switch (parms.Relatorio)
            {
                case "1":
                    parametros.Add( new ReportParameter("ordenacao", "Nome"));
                    break;
                case "2":
                    parametros.Add( new ReportParameter("ordenacao", "Código"));
                    break;
                case "6":
                    if (parms.AtivoInativo == 0)
                        parametros.Add(new ReportParameter("ordenacao", "Ativos"));  
                    else
                        parametros.Add(new ReportParameter("ordenacao", "Inativos"));

                    break;
                default:
                    break;
            }

           return parametros;
        }

        private object[] DataRowToObject(DataRow pAfast)
        {
            var empresa = pAfast["empresa"].ToString();
            var cnpj_cpf = pAfast["cnpj_cpf"].ToString();
            var departamento = pAfast["departamento"].ToString();
            var funcionario = pAfast["funcionario"].ToString();
            var dscodigo = pAfast["dscodigo"].ToString();
            var ocorrencia = pAfast["ocorrencia"].ToString();
            var abonoTotal = (pAfast["abonototal"].ToString() != "--:--" ? pAfast["abonototal"].ToString() : "");
            var abonoParcial = (pAfast["abonoparcial"].ToString() != "--:--" ? pAfast["abonoparcial"].ToString() : "");
            var abonoTotalMin = (pAfast["abonototalmin"].ToString() != "0" ? pAfast["abonototalmin"].ToString() : "");
            var abonoParcialMin = (pAfast["abonoparcialmin"].ToString() != "0" ? pAfast["abonoparcialmin"].ToString() : "");
            var data = Convert.ToDateTime(pAfast["dtMarcacao"]);
            var dia = (pAfast["dia"].ToString());

            return new object[] { empresa, cnpj_cpf, departamento, funcionario, dscodigo, ocorrencia, data.ToShortDateString(), dia, abonoParcial, abonoTotal };
        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }
    }
}