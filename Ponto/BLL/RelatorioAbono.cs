using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Modelo.Proxy;
using System.Data.SqlClient;
using DAL.SQL;

namespace BLL
{
    public class RelatorioAbono
    {

        private DateTime _DataInicial;
        private DateTime _DataFinal;
        private int _Tipo;
        private string _IdTipo;
        private int _ModoOrdenacao;
        private int _AgrupaDepartamento;
        private string _IdSelecionadosOcorrencias;

        private BLL.Afastamento bllAfastamento;

        string ocorrencia, empresa, departamento, funcionario
        , dscodigo, cnpj_cpf;

        DateTime data;

        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        string abonoTotal, abonoParcial, abonoParcialMin, abonoTotalMin, dia;

        private Modelo.Cw_Usuario usuarioLogado;

        /// <summary>
        /// Contrutor do relatorio de ocorrencia
        /// </summary>
        /// <param name="pDataInicial">Data Inicial</param>
        /// <param name="pDataFinal">Data Final</param>
        /// <param name="pTipo">Tipo do relatorio: 0 = Empresa, 1 = Departamento, 2 = Funcionário</param>
        /// <param name="pIdTipo">Id do Tipo</param>
        /// <param name="descOcorrencia"> descrição das ocorrencias selecionadas - pode nao existir</param>
        /// <param name="pModoOrdenacao">Modo de ordenação</param>
        /// <param name="pListaOcorrencias">Lista de ocorrencias que marca como true o tipo de ocorrencia que o usuário marcou para gerar o relatorio.

        public RelatorioAbono(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pIdTipo, int pModoOrdenacao, int pAgrupaDepartamento, string idSelecionadosOcorrencias)
        {
            _DataInicial = pDataInicial;
            _DataFinal = pDataFinal;
            _Tipo = pTipo;
            _IdTipo = pIdTipo;
            _ModoOrdenacao = pModoOrdenacao;
            _AgrupaDepartamento = pAgrupaDepartamento;
            _IdSelecionadosOcorrencias = idSelecionadosOcorrencias;

            bllAfastamento = new Afastamento();
        }

        public RelatorioAbono(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pIdTipo, int pModoOrdenacao, int pAgrupaDepartamento, string idSelecionadosOcorrencias, string ConnectionString, Modelo.Cw_Usuario usuarioLogado):
            this(pDataInicial, pDataFinal, pTipo, pIdTipo, pModoOrdenacao, pAgrupaDepartamento, idSelecionadosOcorrencias)
        {
            _DataInicial = pDataInicial;
            _DataFinal = pDataFinal;
            _Tipo = pTipo;
            _IdTipo = pIdTipo;
            _ModoOrdenacao = pModoOrdenacao;
            _AgrupaDepartamento = pAgrupaDepartamento;
            _IdSelecionadosOcorrencias = idSelecionadosOcorrencias;
            this.ConnectionString = ConnectionString;
            this.usuarioLogado = usuarioLogado;
            bllAfastamento = new Afastamento(ConnectionString);
        }

        public DataTable GeraRelatorio()
        {
            DataTable ret = new DataTable();

            DataColumn[] colunas = new DataColumn[]
			{
                new DataColumn("empresa"),
                new DataColumn("cnpj_cpf"),
                new DataColumn("departamento"),
				new DataColumn("funcionario"),
                new DataColumn("dscodigo"),
				new DataColumn("ocorrencia"),
				new DataColumn("dtMarcacao"),
                new DataColumn("dia"),
				new DataColumn("abonoparcial"),
				new DataColumn("abonototal"),
			};

            ret.Columns.AddRange(colunas);

            //Carrega as informações
            DataTable listaAfastamento = bllAfastamento.GetParaRelatorioAbono(_Tipo, _IdTipo, _DataInicial, _DataFinal, _ModoOrdenacao, _AgrupaDepartamento, _IdSelecionadosOcorrencias);

            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            foreach (DataRow afast in listaAfastamento.Rows)
            {
                if (_IdSelecionadosOcorrencias != null) //Ocorrencia
                {
                    InsereOcorrenciaAfastamento(ret, afast);
                }
            }
            return ret;
        }

        private void SetaVariaveisRelatorio(DataRow pAfast)
         {
            empresa = pAfast["empresa"].ToString();
            cnpj_cpf = pAfast["cnpj_cpf"].ToString();
            departamento = pAfast["departamento"].ToString();
            funcionario = pAfast["funcionario"].ToString();
            dscodigo = pAfast["dscodigo"].ToString();
            ocorrencia = pAfast["ocorrencia"].ToString();
            abonoTotal = (pAfast["abonototal"].ToString() != "--:--" ? pAfast["abonototal"].ToString() : "");
            abonoParcial = (pAfast["abonoparcial"].ToString() != "--:--" ? pAfast["abonoparcial"].ToString() : "");
            abonoTotalMin = (pAfast["abonototalmin"].ToString() != "0" ? pAfast["abonototalmin"].ToString() : "");
            abonoParcialMin = (pAfast["abonoparcialmin"].ToString() != "0" ? pAfast["abonoparcialmin"].ToString() : "");
            data = Convert.ToDateTime(pAfast["dtMarcacao"]);
            dia = (pAfast["dia"].ToString());
        }

        private void InsereOcorrenciaAfastamento(DataTable pRet, DataRow pAfast)
        {
            SetaVariaveisRelatorio(pAfast);
            InsereOcorrenciaLista(pRet);
        }

        private void InsereOcorrenciaLista(DataTable pRet)
        {
            object[] values = new object[]
						{
							empresa,
                            cnpj_cpf,
							departamento,
							funcionario,
							dscodigo,
                            ocorrencia,
							data.ToShortDateString(),
                            dia,
							abonoParcial,
                            abonoTotal
						};

            pRet.Rows.Add(values);
        }

    }
}
