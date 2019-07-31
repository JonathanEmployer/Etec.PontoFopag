using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Modelo;
using DAL.SQL;

namespace BLL
{
    public class HorarioPHExtra : IBLL<Modelo.HorarioPHExtra>
    {
        DAL.IHorarioPHExtra dalHorarioPHExtra;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public HorarioPHExtra() : this(null)
        {

        }

        public HorarioPHExtra(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public HorarioPHExtra(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalHorarioPHExtra = new DAL.SQL.HorarioPHExtra(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalHorarioPHExtra = DAL.FB.HorarioPHExtra.GetInstancia;
                    break;
            }
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalHorarioPHExtra.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalHorarioPHExtra.GetAll();
        }

        public Modelo.HorarioPHExtra LoadObject(int id)
        {
            return dalHorarioPHExtra.LoadObject(id);
        }

        public List<Modelo.HorarioPHExtra> LoadPorHorario(int idHorario)
        {
            return dalHorarioPHExtra.LoadPorHorario(idHorario);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.HorarioPHExtra objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.HorarioPHExtra objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorarioPHExtra.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorarioPHExtra.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorarioPHExtra.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalHorarioPHExtra.getId(pValor, pCampo, pValor2);
        }

        #region Relatórios

        public DataTable GetPercentualHoraExtra(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, System.Windows.Forms.ProgressBar pb)
        {            
            DataTable dt = dalHorarioPHExtra.GetPercentualHoraExtra(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo);

            pb.Minimum = 0;
            pb.Maximum = dt.Rows.Count;
            pb.Value = 0;

            DataTable ret = new DataTable();

            #region Criação das colunas do DataTable
            ret.Columns.Add("idempresa");
            ret.Columns.Add("empresa");
            ret.Columns.Add("endereco");
            ret.Columns.Add("cidade");
            ret.Columns.Add("estado");
            ret.Columns.Add("cnpj");
            ret.Columns.Add("cep");
            ret.Columns.Add("iddepartamento");
            ret.Columns.Add("departamento");
            ret.Columns.Add("idfuncionario");
            ret.Columns.Add("funcionario");
            ret.Columns.Add("dscodigo");
            ret.Columns.Add("percentuais");
            ret.Columns.Add("trabDiurna");
            ret.Columns.Add("trabNoturna");
            ret.Columns.Add("extraDiurna");
            ret.Columns.Add("extraNoturna");
            ret.Columns.Add("faltaDiurna");
            ret.Columns.Add("faltaNoturna");
            ret.Columns.Add("dsr");
            ret.Columns.Add("totalPerc1");
            ret.Columns.Add("totalPerc2");
            ret.Columns.Add("totalPerc3");
            ret.Columns.Add("totalPerc4");
            ret.Columns.Add("totalPerc5");
            ret.Columns.Add("totalPerc6");
            ret.Columns.Add("totalPerc7");
            ret.Columns.Add("totalPerc8");
            ret.Columns.Add("totalPerc9");
            ret.Columns.Add("totalPerc10");
            ret.Columns.Add("totalPerc11");
            ret.Columns.Add("totalPerc12");
            ret.Columns.Add("totalPerc13");
            ret.Columns.Add("totalPerc14");
            ret.Columns.Add("totalPerc15");
            ret.Columns.Add("totalPerc16");
            ret.Columns.Add("totalTrabD");
            ret.Columns.Add("totalTrabN");
            ret.Columns.Add("totalExtraD");
            ret.Columns.Add("totalExtraN");
            ret.Columns.Add("totalFaltaD");
            ret.Columns.Add("totalFaltaN");
            ret.Columns.Add("totalDsr");
            #endregion

            int totalTrabD = 0, totalTrabN = 0, totalExtraD = 0, totalExtraN = 0, totalFaltaD = 0, totalFaltaN = 0, totalDsr = 0;
            int idEmpresaAnt = 0;
            Modelo.TotalHoras objTotalHoras = null;
            Dictionary<int, Turno> totalPercentuais = new Dictionary<int, Turno>();
            string[] percentuaisImprimir = null;
            foreach (DataRow row in dt.Rows)
            {
                objTotalHoras = new Modelo.TotalHoras(dataInicial, dataFinal);
                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(Convert.ToInt32(row["idempresa"]), Convert.ToInt32(row["iddepartamento"]), Convert.ToInt32(row["idfuncionario"]), Convert.ToInt32(row["idfuncao"]), dataInicial, dataFinal, null, null, null, null, ConnectionString, UsuarioLogado);
                totalizadorHoras.CalcularAtraso = false;
                totalizadorHoras.TotalizeHoras(objTotalHoras);
                int idempresa = Convert.ToInt32(row["idempresa"]);

                if (idempresa != idEmpresaAnt)
                {
                    totalTrabD = 0; totalTrabN = 0; totalExtraD = 0; totalExtraN = 0; totalFaltaD = 0; totalFaltaN = 0; totalDsr = 0;
                    idEmpresaAnt = idempresa;
                    totalPercentuais = new Dictionary<int, Turno>();
                    percentuaisImprimir = new string[16];
                }

                StringBuilder percentuais = new StringBuilder();
                int i = 0;
                foreach (var rhe in objTotalHoras.RateioHorasExtras)
                {
                    if (i++ > 0) percentuais.Append("| ");
                    percentuais.Append(String.Format("{0:000}", rhe.Key) + "%: ");
                    percentuais.Append(Modelo.cwkFuncoes.ConvertMinutosHora(4, rhe.Value.Diurno) + "D ");
                    percentuais.Append(Modelo.cwkFuncoes.ConvertMinutosHora(4, rhe.Value.Noturno) + "N");

                    if (!totalPercentuais.ContainsKey(rhe.Key))
                    {
                        totalPercentuais.Add(rhe.Key, new Turno() { Diurno = 0, Noturno = 0 });
                    }
                    Turno t = totalPercentuais[rhe.Key];
                    t.Diurno += rhe.Value.Diurno;
                    t.Noturno += rhe.Value.Noturno;
                    totalPercentuais[rhe.Key] = t;
                }

                totalTrabD += objTotalHoras.horasTrabDiurnaMin;
                totalTrabN += objTotalHoras.horasTrabNoturnaMin;
                totalExtraD += objTotalHoras.horasExtraDiurnaMin;
                totalExtraN += objTotalHoras.horasExtraNoturnaMin;
                totalFaltaD += objTotalHoras.horasFaltaDiurnaMin;
                totalFaltaN += objTotalHoras.horasFaltaNoturnaMin;

                SetPercentuaisImprimir(totalPercentuais, percentuaisImprimir);
                object[] values = new object[]
                {
                    row["idempresa"],
                    row["empresa"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["cnpj"],
                    row["cep"],
                    row["iddepartamento"],
                    row["departamento"],
                    row["idfuncionario"],
                    row["funcionario"],
                    row["dscodigo"],
                    percentuais.ToString(),
                    objTotalHoras.horasTrabDiurna,
                    objTotalHoras.horasTrabNoturna,
                    objTotalHoras.horasExtraDiurna,
                    objTotalHoras.horasExtraNoturna,
                    objTotalHoras.horasFaltaDiurna,
                    objTotalHoras.horasFaltaNoturna,
                    objTotalHoras.horasDDSR,
                    percentuaisImprimir[0],
                    percentuaisImprimir[1],
                    percentuaisImprimir[2],
                    percentuaisImprimir[3],
                    percentuaisImprimir[4],
                    percentuaisImprimir[5],
                    percentuaisImprimir[6],
                    percentuaisImprimir[7],
                    percentuaisImprimir[8],
                    percentuaisImprimir[9],
                    percentuaisImprimir[10],
                    percentuaisImprimir[11],
                    percentuaisImprimir[12],
                    percentuaisImprimir[13],
                    percentuaisImprimir[14],
                    percentuaisImprimir[15],
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalTrabD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalTrabN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalExtraD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalExtraN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalFaltaD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalFaltaN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalDsr)
                };

                ret.Rows.Add(values);

                pb.Value++;
                System.Windows.Forms.Application.DoEvents();
            }

            return ret;
        }

        public DataTable GetPercentualHoraExtraWeb(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, Modelo.ProgressBar pb)
        {
            DataTable dt = dalHorarioPHExtra.GetPercentualHoraExtra(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo);

            pb.setaMinMaxPB(0, dt.Rows.Count);
            pb.setaValorPB(0);

            DataTable ret = new DataTable();

            #region Criação das colunas do DataTable
            ret.Columns.Add("idempresa");
            ret.Columns.Add("empresa");
            ret.Columns.Add("endereco");
            ret.Columns.Add("cidade");
            ret.Columns.Add("estado");
            ret.Columns.Add("cnpj");
            ret.Columns.Add("cep");
            ret.Columns.Add("iddepartamento");
            ret.Columns.Add("departamento");
            ret.Columns.Add("idfuncionario");
            ret.Columns.Add("funcionario");
            ret.Columns.Add("dscodigo");
            ret.Columns.Add("percentuais");
            ret.Columns.Add("trabDiurna");
            ret.Columns.Add("trabNoturna");
            ret.Columns.Add("extraDiurna");
            ret.Columns.Add("extraNoturna");
            ret.Columns.Add("faltaDiurna");
            ret.Columns.Add("faltaNoturna");
            ret.Columns.Add("dsr");
            ret.Columns.Add("totalPerc1");
            ret.Columns.Add("totalPerc2");
            ret.Columns.Add("totalPerc3");
            ret.Columns.Add("totalPerc4");
            ret.Columns.Add("totalPerc5");
            ret.Columns.Add("totalPerc6");
            ret.Columns.Add("totalPerc7");
            ret.Columns.Add("totalPerc8");
            ret.Columns.Add("totalPerc9");
            ret.Columns.Add("totalPerc10");
            ret.Columns.Add("totalPerc11");
            ret.Columns.Add("totalPerc12");
            ret.Columns.Add("totalPerc13");
            ret.Columns.Add("totalPerc14");
            ret.Columns.Add("totalPerc15");
            ret.Columns.Add("totalPerc16");
            ret.Columns.Add("totalTrabD");
            ret.Columns.Add("totalTrabN");
            ret.Columns.Add("totalExtraD");
            ret.Columns.Add("totalExtraN");
            ret.Columns.Add("totalFaltaD");
            ret.Columns.Add("totalFaltaN");
            ret.Columns.Add("totalDsr");
            #endregion

            int totalTrabD = 0, totalTrabN = 0, totalExtraD = 0, totalExtraN = 0, totalFaltaD = 0, totalFaltaN = 0, totalDsr = 0;
            int idEmpresaAnt = 0;
            Modelo.TotalHoras objTotalHoras = null;
            Dictionary<int, Turno> totalPercentuais = new Dictionary<int, Turno>();
            string[] percentuaisImprimir = null;
            foreach (DataRow row in dt.Rows)
            {
                objTotalHoras = new Modelo.TotalHoras(dataInicial, dataFinal);
                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(Convert.ToInt32(row["idempresa"]), Convert.ToInt32(row["iddepartamento"]), Convert.ToInt32(row["idfuncionario"]), Convert.ToInt32(row["idfuncao"]), dataInicial, dataFinal, null, null, null, null, ConnectionString, UsuarioLogado);
                totalizadorHoras.CalcularAtraso = false;
                totalizadorHoras.TotalizeHoras(objTotalHoras);
                int idempresa = Convert.ToInt32(row["idempresa"]);

                if (idempresa != idEmpresaAnt)
                {
                    totalTrabD = 0; totalTrabN = 0; totalExtraD = 0; totalExtraN = 0; totalFaltaD = 0; totalFaltaN = 0; totalDsr = 0;
                    idEmpresaAnt = idempresa;
                    totalPercentuais = new Dictionary<int, Turno>();
                    percentuaisImprimir = new string[16];
                }

                StringBuilder percentuais = new StringBuilder();
                int i = 0;
                foreach (var rhe in objTotalHoras.RateioHorasExtras)
                {
                    if (i++ > 0) percentuais.Append("| ");
                    percentuais.Append(String.Format("{0:000}", rhe.Key) + "%: ");
                    percentuais.Append(Modelo.cwkFuncoes.ConvertMinutosHora(4, rhe.Value.Diurno) + "D ");
                    percentuais.Append(Modelo.cwkFuncoes.ConvertMinutosHora(4, rhe.Value.Noturno) + "N");

                    if (!totalPercentuais.ContainsKey(rhe.Key))
                    {
                        totalPercentuais.Add(rhe.Key, new Turno() { Diurno = 0, Noturno = 0 });
                    }
                    Turno t = totalPercentuais[rhe.Key];
                    t.Diurno += rhe.Value.Diurno;
                    t.Noturno += rhe.Value.Noturno;
                    totalPercentuais[rhe.Key] = t;
                }

                totalTrabD += objTotalHoras.horasTrabDiurnaMin;
                totalTrabN += objTotalHoras.horasTrabNoturnaMin;
                totalExtraD += objTotalHoras.horasExtraDiurnaMin;
                totalExtraN += objTotalHoras.horasExtraNoturnaMin;
                totalFaltaD += objTotalHoras.horasFaltaDiurnaMin;
                totalFaltaN += objTotalHoras.horasFaltaNoturnaMin;

                SetPercentuaisImprimir(totalPercentuais, percentuaisImprimir);
                object[] values = new object[]
                {
                    row["idempresa"],
                    row["empresa"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["cnpj"],
                    row["cep"],
                    row["iddepartamento"],
                    row["departamento"],
                    row["idfuncionario"],
                    row["funcionario"],
                    row["dscodigo"],
                    percentuais.ToString(),
                    objTotalHoras.horasTrabDiurna,
                    objTotalHoras.horasTrabNoturna,
                    objTotalHoras.horasExtraDiurna,
                    objTotalHoras.horasExtraNoturna,
                    objTotalHoras.horasFaltaDiurna,
                    objTotalHoras.horasFaltaNoturna,
                    objTotalHoras.horasDDSR,
                    percentuaisImprimir[0],
                    percentuaisImprimir[1],
                    percentuaisImprimir[2],
                    percentuaisImprimir[3],
                    percentuaisImprimir[4],
                    percentuaisImprimir[5],
                    percentuaisImprimir[6],
                    percentuaisImprimir[7],
                    percentuaisImprimir[8],
                    percentuaisImprimir[9],
                    percentuaisImprimir[10],
                    percentuaisImprimir[11],
                    percentuaisImprimir[12],
                    percentuaisImprimir[13],
                    percentuaisImprimir[14],
                    percentuaisImprimir[15],
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalTrabD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalTrabN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalExtraD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalExtraN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalFaltaD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalFaltaN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalDsr)
                };

                ret.Rows.Add(values);

                pb.incrementaPB(1);
            }

            return ret;
        }

        private static void SetPercentuaisImprimir(Dictionary<int, Turno> totalPercentuais, string[] percentuaisImprimir)
        {
            int i = 0;
            foreach (var item in totalPercentuais)
            {
                percentuaisImprimir[i++] = "Hr. Extra " + String.Format("{0:000}", item.Key) + "%: "
                                         + Modelo.cwkFuncoes.ConvertMinutosHora(4, item.Value.Diurno) + "D "
                                         + Modelo.cwkFuncoes.ConvertMinutosHora(4, item.Value.Noturno) + "N";
                if (i > 15) break;
            }
        }

        public DataTable GetHoraExtra(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, bool bDepartamento, System.Windows.Forms.ProgressBar pb)
        {            
            DataTable dt;
            if (bDepartamento)
            {
                dt = dalHorarioPHExtra.GetPercentualHoraExtraDepartamento(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo);
            }
            else
            {
                dt = dalHorarioPHExtra.GetPercentualHoraExtra(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo);
            }

            pb.Minimum = 0;
            pb.Maximum = dt.Rows.Count;
            pb.Value = 0;

            DataTable ret = new DataTable();

            #region Criação das colunas do DataTable
            /*01*/
            ret.Columns.Add("idempresa");
            /*02*/
            ret.Columns.Add("empresa");
            /*03*/
            ret.Columns.Add("endereco");
            /*04*/
            ret.Columns.Add("cidade");
            /*05*/
            ret.Columns.Add("estado");
            /*06*/
            ret.Columns.Add("cnpj");
            /*07*/
            ret.Columns.Add("cep");
            /*08*/
            ret.Columns.Add("iddepartamento");
            /*09*/
            ret.Columns.Add("departamento");
            /*10*/
            ret.Columns.Add("idfuncionario");
            /*11*/
            ret.Columns.Add("funcionario");
            /*12*/
            ret.Columns.Add("dscodigo");
            /*13*/
            ret.Columns.Add("trabDiurna");
            /*14*/
            ret.Columns.Add("trabNoturna");
            /*15*/
            ret.Columns.Add("extraDiurna");
            /*16*/
            ret.Columns.Add("extraNoturna");
            /*17*/
            ret.Columns.Add("faltaDiurna");
            /*18*/
            ret.Columns.Add("faltaNoturna");
            /*19*/
            ret.Columns.Add("totalTrabD");
            /*20*/
            ret.Columns.Add("totalTrabN");
            /*21*/
            ret.Columns.Add("totalExtraD");
            /*22*/
            ret.Columns.Add("totalExtraN");
            /*23*/
            ret.Columns.Add("totalFaltaD");
            /*24*/
            ret.Columns.Add("totalFaltaN");
            /*25*/
            ret.Columns.Add("totalHoraTrabExtraMin");
            /*26*/
            ret.Columns.Add("totalHoraTrabExtraStr");

            #endregion

            int totalTrabD = 0, totalTrabN = 0, totalExtraD = 0, totalExtraN = 0, totalFaltaD = 0, totalFaltaN = 0, totalTrab_Extra = 0;
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            int idDepartamentoAnt = 0;
            Modelo.TotalHoras objTotalHoras = null;
            foreach (DataRow row in dt.Rows)
            {
                objTotalHoras = new Modelo.TotalHoras(dataInicial, dataFinal);
                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(Convert.ToInt32(row["idempresa"]), Convert.ToInt32(row["iddepartamento"]), Convert.ToInt32(row["idfuncionario"]), Convert.ToInt32(row["idfuncao"]), dataInicial, dataFinal, null, bllMarcacao.GetParaTotalizaHoras(Convert.ToInt32(row["idfuncionario"]), dataInicial, dataFinal, true), null, null, ConnectionString, UsuarioLogado);
                totalizadorHoras.CalcularAtraso = false;
                totalizadorHoras.TotalizeHoras(objTotalHoras);
                int iddepartamento = Convert.ToInt32(row["iddepartamento"]);

                if (iddepartamento != idDepartamentoAnt)
                {
                    totalTrabD = 0; totalTrabN = 0; totalExtraD = 0; totalExtraN = 0; totalFaltaD = 0; totalFaltaN = 0; totalTrab_Extra = 0;

                    idDepartamentoAnt = iddepartamento;
                }

                totalTrabD += objTotalHoras.horasTrabDiurnaMin;
                totalTrabN += objTotalHoras.horasTrabNoturnaMin;
                totalExtraD += objTotalHoras.horasExtraDiurnaMin;
                totalExtraN += objTotalHoras.horasExtraNoturnaMin;
                totalFaltaD += objTotalHoras.horasFaltaDiurnaMin;
                totalFaltaN += objTotalHoras.horasFaltaNoturnaMin;

                object[] values = new object[]
                {
                    row["idempresa"],
                    row["empresa"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["cnpj"],
                    row["cep"],
                    row["iddepartamento"],
                    row["departamento"],
                    row["idfuncionario"],
                    row["funcionario"],
                    row["dscodigo"],                     
                    objTotalHoras.horasTrabDiurna,
                    objTotalHoras.horasTrabNoturna,
                    objTotalHoras.horasExtraDiurna,
                    objTotalHoras.horasExtraNoturna,
                    objTotalHoras.horasFaltaDiurna,
                    objTotalHoras.horasFaltaNoturna,                  
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalTrabD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalTrabN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalExtraD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalExtraN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalFaltaD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalFaltaN),
                    objTotalHoras.TotalHorasTrabalhadas_ExtrasMin,
                    objTotalHoras.TotalHorasTrabalhadas_Extras
                };

                ret.Rows.Add(values);

                pb.Value++;
                System.Windows.Forms.Application.DoEvents();
            }

            return ret;
        }

        public DataTable GetHoraExtraWeb(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, bool bDepartamento, Modelo.ProgressBar pb)
        {
            DataTable dt;
            if (bDepartamento)
            {
                dt = dalHorarioPHExtra.GetPercentualHoraExtraDepartamento(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo);
            }
            else
            {
                dt = dalHorarioPHExtra.GetPercentualHoraExtra(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo);
            }

            pb.setaMinMaxPB(0, dt.Rows.Count+1);
            pb.setaValorPB(0);

            DataTable ret = new DataTable();

            #region Criação das colunas do DataTable
            /*01*/
            ret.Columns.Add("idempresa");
            /*02*/
            ret.Columns.Add("empresa");
            /*03*/
            ret.Columns.Add("endereco");
            /*04*/
            ret.Columns.Add("cidade");
            /*05*/
            ret.Columns.Add("estado");
            /*06*/
            ret.Columns.Add("cnpj");
            /*07*/
            ret.Columns.Add("cep");
            /*08*/
            ret.Columns.Add("iddepartamento");
            /*09*/
            ret.Columns.Add("departamento");
            /*10*/
            ret.Columns.Add("idfuncionario");
            /*11*/
            ret.Columns.Add("funcionario");
            /*12*/
            ret.Columns.Add("dscodigo");
            /*13*/
            ret.Columns.Add("trabDiurna");
            /*14*/
            ret.Columns.Add("trabNoturna");
            /*15*/
            ret.Columns.Add("extraDiurna");
            /*16*/
            ret.Columns.Add("extraNoturna");
            /*17*/
            ret.Columns.Add("faltaDiurna");
            /*18*/
            ret.Columns.Add("faltaNoturna");
            /*19*/
            ret.Columns.Add("totalTrabD");
            /*20*/
            ret.Columns.Add("totalTrabN");
            /*21*/
            ret.Columns.Add("totalExtraD");
            /*22*/
            ret.Columns.Add("totalExtraN");
            /*23*/
            ret.Columns.Add("totalFaltaD");
            /*24*/
            ret.Columns.Add("totalFaltaN");
            /*25*/
            ret.Columns.Add("totalHoraTrabExtraMin");
            /*26*/
            ret.Columns.Add("totalHoraTrabExtraStr");
            #endregion

            int totalTrabD = 0, totalTrabN = 0, totalExtraD = 0, totalExtraN = 0, totalFaltaD = 0, totalFaltaN = 0;

            int idDepartamentoAnt = 0;
            Modelo.TotalHoras objTotalHoras = null;
            foreach (DataRow row in dt.Rows)
            {
                objTotalHoras = new Modelo.TotalHoras(dataInicial, dataFinal);
                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(Convert.ToInt32(row["idempresa"]), Convert.ToInt32(row["iddepartamento"]), Convert.ToInt32(row["idfuncionario"]), Convert.ToInt32(row["idfuncao"]), dataInicial, dataFinal, null, null, null, null, ConnectionString, UsuarioLogado);
                totalizadorHoras.CalcularAtraso = false;
                totalizadorHoras.TotalizeHoras(objTotalHoras);
                int iddepartamento = Convert.ToInt32(row["iddepartamento"]);

                if (iddepartamento != idDepartamentoAnt)
                {
                    totalTrabD = 0; totalTrabN = 0; totalExtraD = 0; totalExtraN = 0; totalFaltaD = 0; totalFaltaN = 0;

                    idDepartamentoAnt = iddepartamento;
                }

                totalTrabD += objTotalHoras.horasTrabDiurnaMin;
                totalTrabN += objTotalHoras.horasTrabNoturnaMin;
                totalExtraD += objTotalHoras.horasExtraDiurnaMin;
                totalExtraN += objTotalHoras.horasExtraNoturnaMin;
                totalFaltaD += objTotalHoras.horasFaltaDiurnaMin;
                totalFaltaN += objTotalHoras.horasFaltaNoturnaMin;

                object[] values = new object[]
                {
                    row["idempresa"],
                    row["empresa"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["cnpj"],
                    row["cep"],
                    row["iddepartamento"],
                    row["departamento"],
                    row["idfuncionario"],
                    row["funcionario"],
                    row["dscodigo"],                     
                    objTotalHoras.horasTrabDiurna,
                    objTotalHoras.horasTrabNoturna,
                    objTotalHoras.horasExtraDiurna,
                    objTotalHoras.horasExtraNoturna,
                    objTotalHoras.horasFaltaDiurna,
                    objTotalHoras.horasFaltaNoturna,                  
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalTrabD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalTrabN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalExtraD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalExtraN),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalFaltaD),
                    Modelo.cwkFuncoes.ConvertMinutosHora(6,totalFaltaN),
                    objTotalHoras.TotalHorasTrabalhadas_ExtrasMin,
                    objTotalHoras.TotalHorasTrabalhadas_Extras
                };

                ret.Rows.Add(values);

                pb.incrementaPB(1);
            }

            return ret;
        }

        #endregion
    }
}


