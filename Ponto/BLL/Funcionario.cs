using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FileHelpers;
using DAL.SQL;
using BLLIntegracaoPNL = BLL.IntegracaoPainel;
using Modelo.Proxy;
using Modelo;
using static Modelo.Enumeradores;
using System.Globalization;

namespace BLL
{
    public class Funcionario : IBLL<Modelo.Funcionario>
    {
        DAL.IFuncionario dalFuncionario;
        DAL.IEmpresa dalEmpresa;
        DAL.IDepartamento dalDepartamento;
        DAL.IContrato dalContrato;
        DAL.IMudancaHorario dalMudancaHorario;
        private DAL.IHorario dalHorario;
        private Modelo.ProgressBar objProgressBar;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private int tentativasNovoCodigo = 0;

        private TimeSpan MinutosParaAviso = TimeSpan.FromMinutes(10);

        public Modelo.ProgressBar ObjProgressBar {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public Funcionario()
            : this(null, null)
        {

        }

        public Funcionario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {
        }

        public Funcionario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {

            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            DataBase db = new DataBase(ConnectionString);
            dalFuncionario = new DAL.SQL.Funcionario(db);
            dalEmpresa = new DAL.SQL.Empresa(db);
            dalDepartamento = new DAL.SQL.Departamento(db);
            dalContrato = new DAL.SQL.Contrato(db);
            dalHorario = new DAL.SQL.Horario(db);
            dalMudancaHorario = new DAL.SQL.MudancaHorario(db);

            if (usuarioLogado != null)
            {
                dalFuncionario.UsuarioLogado = usuarioLogado;
            }
            dalEmpresa.UsuarioLogado = usuarioLogado;
            dalDepartamento.UsuarioLogado = usuarioLogado;
            dalContrato.UsuarioLogado = usuarioLogado;
            dalHorario.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
            dalMudancaHorario.UsuarioLogado = usuarioLogado;
        }

        public Funcionario(string connString, bool webApi)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            if (webApi)
            {
                DataBase db = new DataBase(ConnectionString);
                dalFuncionario = new DAL.SQL.Funcionario(db);
                dalEmpresa = new DAL.SQL.Empresa(db);
                dalDepartamento = new DAL.SQL.Departamento(db);
                dalContrato = new DAL.SQL.Contrato(db);
                dalHorario = new DAL.SQL.Horario(db);
            }
        }


        public Modelo.Funcionario ValidaEmpDepFunc(string empresa, string departamento, string funcionario, ref string erro)
        {
            Modelo.Funcionario func = new Modelo.Funcionario();

            Modelo.Empresa e = new Modelo.Empresa();
            int codEmpR;
            string codEmp = empresa.Split('|')[0].Trim();
            if (int.TryParse(codEmp, out codEmpR))
            {
                if (codEmpR != 0)
                {
                    e = dalEmpresa.LoadObjectByCodigo(codEmpR);
                }

                if (codEmpR != 0 && (e == null || e.Id <= 0))
                {
                    erro = "Empresa " + empresa + " não encontrada!";
                    return func;
                }
            }
            Modelo.Departamento dep = new Modelo.Departamento();
            int codDepR;
            string codDep = departamento.Split('|')[0].Trim();
            if (int.TryParse(codDep, out codDepR))
            {
                if (codDepR != 0)
                {
                    dep = dalDepartamento.LoadObjectByCodigo(codDepR);
                }

                if (codDepR != 0 && (dep == null || dep.Id <= 0))
                {
                    erro = "Departamento " + departamento + " não encontrado!";
                    return func;
                }
            }

            if (codDepR > 0 && codEmpR > 0)
            {
                if (dep.IdEmpresa != e.Id)
                {
                    erro = "Departamento " + departamento + " não pertence a empresa " + empresa;
                    return func;
                }
            }

            IList<string> split = funcionario.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string codFunc = split[0].Trim();

            if (codFunc != "")
            {
                func = RetornaFuncDsCodigo(codFunc);
            }

            if (func == null || func.Id <= 0)
            {
                erro = "Funcionario " + funcionario + " não encontrado!";
                return func;
            }

            if (codDepR > 0)
            {
                if (func.Iddepartamento != dep.Id)
                {
                    erro = "Funcionario " + funcionario + " não pertence ao departamento " + departamento;
                    return func;
                }
            }

            if (codEmpR > 0)
            {
                if (func.Idempresa != e.Id)
                {
                    erro = "Funcionario " + funcionario + " não pertence a empresa " + empresa;
                    return func;
                }
            }

            return func;
        }

        public bool ValidaEmpDepCont(string empresa, string departamento, string contrato, ref int idEmp, ref int idDep, ref int idCont, ref string erro)
        {
            Modelo.Empresa e = new Modelo.Empresa();
            int codEmpR;
            idEmp = 0;
            idDep = 0;
            string codEmp = empresa.Split('|')[0].Trim();
            if (int.TryParse(codEmp, out codEmpR))
            {
                if (codEmpR != 0)
                {
                    e = dalEmpresa.LoadObjectByCodigo(codEmpR);
                }

                if (codEmpR != 0 && (e == null || e.Id <= 0))
                {
                    erro = "Empresa " + empresa + " não encontrada!";
                    return false;
                }
                idEmp = e.Id;
            }

            Modelo.Departamento dep = new Modelo.Departamento();
            int codDepR;
            string codDep = departamento.Split('|')[0].Trim();
            if (int.TryParse(codDep, out codDepR))
            {
                if (codDepR != 0)
                {
                    dep = dalDepartamento.LoadObjectByCodigo(codDepR);
                }

                if (codDepR != 0 && (dep == null || dep.Id <= 0))
                {
                    erro = "Departamento " + departamento + " não encontrado!";
                    return false;
                }
                idDep = dep.Id;
            }

            Modelo.Contrato cont = new Modelo.Contrato();
            int codContR;
            string codCont = contrato.Split('|')[0].Trim();
            if (int.TryParse(codCont, out codContR))
            {
                if (codContR != 0)
                {
                    cont = dalContrato.LoadPorCodigo(codContR);
                }

                if (codContR != 0 && (cont == null || cont.Id <= 0))
                {
                    erro = "Contrato " + contrato + " não encontrado!";
                    return false;
                }
                idCont = cont.Id;
            }


            if (codDepR > 0 && codEmpR > 0)
            {
                if (dep.IdEmpresa != e.Id)
                {
                    erro = "Departamento " + departamento + " não pertence a empresa " + empresa;
                    return false;
                }
            }
            return true;


        }

        public bool ValidaEmpDepContFunc(string empresa, string departamento, string contrato, string funcionario, ref int idEmp, ref int idDep, ref int idCont, ref int idFunc, ref string erro)
        {
            bool retorno = true;
            retorno = ValidaEmpDepCont(empresa, departamento, contrato, ref idEmp, ref idDep, ref idCont, ref erro);
            if (String.IsNullOrEmpty(funcionario))
            {
                idFunc = 0;
            }
            else
            {
                Int64 codFuncR;
                IList<string> split = funcionario.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string codFunc = split[0].Trim();
                Modelo.Funcionario func = new Modelo.Funcionario();
                if (Int64.TryParse(codFunc, out codFuncR))
                {
                    if (codFuncR != 0)
                    {
                        func = RetornaFuncDsCodigo(codFuncR.ToString());
                    }

                    if (func == null || func.Id <= 0)
                    {
                        erro = "Funcionario " + funcionario + " não encontrado!";
                        retorno = false;
                    }

                    if (idDep > 0)
                    {
                        if (func.Iddepartamento != idDep)
                        {
                            erro = "Funcionario " + funcionario + " não pertence ao departamento " + departamento;
                            retorno = false;
                        }
                    }

                    if (idEmp > 0)
                    {
                        if (func.Idempresa != idEmp)
                        {
                            erro = "Funcionario " + funcionario + " não pertence a empresa " + empresa;
                            retorno = false;
                        }
                    }

                    if (retorno)
                    {
                        idFunc = func.Id;
                    }
                }
            }
            return retorno;
        }

        public bool ValidaEmpDep(string empresa, string departamento, ref int idEmp, ref int idDep, ref string erro)
        {
            Modelo.Empresa e = new Modelo.Empresa();
            int codEmpR;
            idEmp = 0;
            idDep = 0;
            string codEmp = empresa.Split('|')[0].Trim();
            if (int.TryParse(codEmp, out codEmpR))
            {
                if (codEmpR != 0)
                {
                    e = dalEmpresa.LoadObjectByCodigo(codEmpR);
                }

                if (codEmpR != 0 && (e == null || e.Id <= 0))
                {
                    erro = "Empresa " + empresa + " não encontrada!";
                    return false;
                }
                idEmp = e.Id;
            }

            Modelo.Departamento dep = new Modelo.Departamento();
            int codDepR;
            string codDep = departamento.Split('|')[0].Trim();
            if (int.TryParse(codDep, out codDepR))
            {
                if (codDepR != 0)
                {
                    dep = dalDepartamento.LoadObjectByCodigo(codDepR);
                }

                if (codDepR != 0 && (dep == null || dep.Id <= 0))
                {
                    erro = "Departamento " + departamento + " não encontrado!";
                    return false;
                }
                idDep = dep.Id;
            }

            if (codDepR > 0 && codEmpR > 0)
            {
                if (dep.IdEmpresa != e.Id)
                {
                    erro = "Departamento " + departamento + " não pertence a empresa " + empresa;
                    return false;
                }
            }
            return true;
        }

        public int MaxCodigo()
        {
            return dalFuncionario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFuncionario.GetAll();
        }

        public List<Modelo.Proxy.pxyFuncionarioGrid> GetAllGrid()
        {
            return dalFuncionario.GetAllGrid();
        }

        public List<Modelo.Proxy.pxyFuncionarioGrid> GetAllGrid(int flag)
        {
            return dalFuncionario.GetAllGrid(flag);
        }
        public DataTable GetAll(bool pegaTodos)
        {
            return dalFuncionario.GetAll(pegaTodos);
        }

        public List<Modelo.Funcionario> GetAllListLike(bool pegaInativos, bool pegaExcluidos, string nome)
        {
            return dalFuncionario.GetAllListLike(pegaInativos, pegaExcluidos, nome);
        }

        public List<Modelo.Funcionario> GetAllListByIds(string funcionarios)
        {
            return dalFuncionario.GetAllListByIds(funcionarios);
        }

        public List<PxyFuncionarioExcluidoGrid> GetExcluidosList()
        {
            return dalFuncionario.GetExcluidosList();
        }

        public Modelo.Funcionario LoadObjectByCodigo(int codigo)
        {
            return dalFuncionario.LoadObjectByCodigo(codigo);
        }

        public List<int> GetIds()
        {
            return dalFuncionario.GetIds();
        }

        /// <summary>
        /// Retorna uma tabela hash onde o código é a chave e o id é o valor
        /// </summary>
        /// <returns>Tabela Hash(Código, Id)</returns>
        public Hashtable GetHashCodigoId()
        {
            return dalFuncionario.GetHashCodigoId();
        }

        public Hashtable GetHashCodigoFunc()
        {
            return dalFuncionario.GetHashCodigoFunc();
        }

        public Hashtable GetHashIdFunc()
        {
            return dalFuncionario.GetHashIdFunc();
        }

        public bool ValidaPis(String plPIS)
        {
            if (plPIS.Length == 12)
                plPIS = plPIS.Substring(1, 11);
            int liTamanho = 0;
            string lsAux = null;
            string lsMultiplicador = "3298765432";
            int liTotalizador = 0;
            int liResto = 0;
            int liMultiplicando = 0;
            int liMultiplicador = 0;
            bool lbRetorno = true;
            int liDigito = 99;

            lsAux = plPIS;
            liTamanho = lsAux.Length;

            if (liTamanho != 11)
            {
                lbRetorno = false;
            }

            if (lbRetorno)
            {
                for (int i = 0; i < 10; i++)
                {

                    liMultiplicando = Convert.ToInt32(lsAux.Substring(i, 1));
                    liMultiplicador = Convert.ToInt32(lsMultiplicador.Substring(i, 1));
                    liTotalizador += liMultiplicando * liMultiplicador;
                }

                liResto = 11 - liTotalizador % 11;
                liResto = liResto == 10 || liResto == 11 ? 0 : liResto;

                liDigito = Convert.ToInt32(lsAux.Substring(10, 1));
                lbRetorno = liResto == liDigito;
            }

            return lbRetorno;
        }

        public DataTable GetFuncionariosAtivos()
        {
            return dalFuncionario.GetFuncionariosAtivos();
        }

        public DataTable GetExcluidos()
        {
            return dalFuncionario.GetExcluidos();
        }

        public DataTable GetParaProvisorio()
        {
            return dalFuncionario.GetParaProvisorio();
        }

        public DataTable GetOrdenadoPorCodigoRel(string pInicio, string pFim, string pEmpresas)
        {
            return dalFuncionario.GetOrdenadoPorCodigoRel(pInicio, pFim, pEmpresas);
        }

        public DataTable GetOrdenadoPorCodigoRel(List<int> idsFuncs)
        {
            return dalFuncionario.GetOrdenadoPorCodigoRel(idsFuncs);
        }

        public DataTable GetOrdenadoPorNomeRel(string pInicial, string pFinal, string pEmpresas)
        {
            return dalFuncionario.GetOrdenadoPorNomeRel(pInicial, pFinal, pEmpresas);
        }

        public DataTable GetOrdenadoPorNomeRel(List<int> idsFuncs)
        {
            return dalFuncionario.GetOrdenadoPorNomeRel(idsFuncs);
        }

        public DataTable GetPorDepartamentoRel(string pDepartamentos)
        {
            return dalFuncionario.GetPorDepartamentoRel(pDepartamentos);
        }

        public DataTable GetPorDepartamentoRel(List<int> idsFuncs)
        {
            return dalFuncionario.GetPorDepartamentoRel(idsFuncs);
        }

        public DataTable GetPorHorarioRel(string pHorarios, string pEmpresas)
        {
            return dalFuncionario.GetPorHorarioRel(pHorarios, pEmpresas);
        }

        public DataTable GetPorHorarioRel(List<int> idsFuncs)
        {
            return dalFuncionario.GetPorHorarioRel(idsFuncs);
        }

        public DataTable GetPorDataAdmissaoRel(DateTime? pInicial, DateTime? pFinal, string pEmpresas)
        {
            return dalFuncionario.GetPorDataAdmissaoRel(pInicial, pFinal, pEmpresas);
        }

        public DataTable GetPorDataAdmissaoRel(DateTime? pInicial, DateTime? pFinal, List<int> idsFuncs)
        {
            return dalFuncionario.GetPorDataAdmissaoRel(pInicial, pFinal, idsFuncs);
        }

        public DataTable GetPorDataDemissaoRel(DateTime? pInicial, DateTime? pFinal, string pEmpresas)
        {
            return dalFuncionario.GetPorDataDemissaoRel(pInicial, pFinal, pEmpresas);
        }

        public DataTable GetPorDataDemissaoRel(DateTime? pInicial, DateTime? pFinal, List<int> idsFuncs)
        {
            return dalFuncionario.GetPorDataDemissaoRel(pInicial, pFinal, idsFuncs);
        }

        public DataTable GetAtivosInativosRel(bool pAtivo, string pEmpresas)
        {
            return dalFuncionario.GetAtivosInativosRel(pAtivo, pEmpresas);
        }
        public DataTable GetAtivosInativosRel(bool pAtivo, List<int> idsFuncs)
        {
            return dalFuncionario.GetAtivosInativosRel(pAtivo, idsFuncs);
        }

        public DataTable GetRelatorio(string pEmpresas)
        {
            return dalFuncionario.GetRelatorio(pEmpresas);
        }

        public DataTable GetRelatorio(List<int> idsFuncs)
        {
            return dalFuncionario.GetRelatorio(idsFuncs);
        }

        /// <summary>
        /// Função que retorna todos os funcionários de um determinado departamento
        /// </summary>
        /// <param name="pIDEmpresa"> ID da Empresa</param>
        /// <param name="pIDDepartamento"> ID do Departamento</param>
        /// <param name="pPegarInativos">
        /// false = Pega somente os funcionarios ativos.
        /// true = Pega funcionarios ativos e inativos.</param>
        /// <returns></returns>
        public DataTable GetPorDepartamento(int pIDEmpresa, int pIDDepartamento, bool pPegarInativos)
        {
            return dalFuncionario.GetPorDepartamento(pIDEmpresa, pIDDepartamento, pPegarInativos);
        }

        public DataTable GetPorDepartamento(string pDepartamentos)
        {
            return dalFuncionario.GetPorDepartamento(pDepartamentos);
        }

        public void AtualizaMarcacao(int pIdFuncionario, DateTime pDataI, DateTime pDataF)
        {
            DateTime datai = System.DateTime.Today.AddMonths(-1);
            DateTime dataf = System.DateTime.Today.AddMonths(2);
            if (pDataI != new DateTime())
            {
                if (pDataI < datai)
                    datai = pDataI;
                if (pDataF > dataf)
                    dataf = pDataF;
            }

            BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(2, pIdFuncionario, datai, dataf, objProgressBar, false, ConnectionString, UsuarioLogado, false);
            bllCalculaMarcacao.CalculaMarcacoes();
        }

        public DataTable GetPorEmpresa(int pIDEmpresa, bool pPegaInativos)
        {
            return dalFuncionario.GetPorEmpresa(pIDEmpresa, pPegaInativos);
        }

        public DataTable GetListaPresenca(DateTime dataInicial, int tipo, string empresas, string departamentos, string funcionarios)
        {
            return dalFuncionario.GetListaPresenca(dataInicial, tipo, empresas, departamentos, funcionarios);
        }

        public DataTable GetParaDSR(int? pTipo, int pIdentificacao)
        {
            return dalFuncionario.GetParaDSR(pTipo, pIdentificacao);
        }

        public DataTable GetRelatorioAbsenteismo(int tipo, string empresas, string departamentos, string funcionarios)
        {
            return dalFuncionario.GetRelatorioAbsenteismo(tipo, empresas, departamentos, funcionarios);
        }

        /// <summary>
        /// Pega o retorno da totalização do banco de dados do numero de funcionarios (do banco todo)
        /// </summary>
        /// <param name="pIdEmpresa">Id da empresa</param>
        /// <returns>Numero de funcionarios da empresa</returns>
        public int getNumFuncionarios()
        {
            return dalFuncionario.GetNumFuncionarios();
        }

        public Modelo.Funcionario LoadObject(int id)
        {
            return dalFuncionario.LoadObject(id);
        }

        public Modelo.Funcionario LoadPorCPF(string CPF)
        {
            return dalFuncionario.LoadPorCPF(CPF);
        }

        public Modelo.Funcionario LoadAtivoPorCPF(string CPF)
        {
            return dalFuncionario.LoadAtivoPorCPF(CPF);
        }

        public List<Modelo.Funcionario> LoadAtivoPorListCPF(List<string> CPFs)
        {
            return dalFuncionario.LoadAtivoPorListCPF(CPFs);
        }

        public bool MudaCodigoFuncionario(int pFuncionarioID, string pCodigoNovo, DateTime pData)
        {
            return dalFuncionario.MudaCodigoFuncionario(pFuncionarioID, pCodigoNovo, pData);
        }

        public Modelo.Funcionario RetornaFuncDsCodigo(string pCodigo)
        {
            return dalFuncionario.RetornaFuncDsCodigo(pCodigo);
        }

        public bool DsCodigoUtilizado(string dsCodigo, out string mensagem)
        {
            Modelo.Funcionario objFuncionario = dalFuncionario.RetornaFuncDsCodigo(dsCodigo);
            if (objFuncionario != null)
            {
                mensagem = "Código já está sendo utilizado pelo funcionário " + objFuncionario.Nome;
                mensagem += objFuncionario.Funcionarioativo == 1 ? " (Ativo)." : " (Inativo).";
                return true;
            }
            mensagem = "";
            return false;
        }

        public bool PisUtilizado(Modelo.Funcionario objfunc, string pis, out string mensagem)
        {
            Modelo.Funcionario objFuncionario = dalFuncionario.RetornaFuncPis(objfunc.Id, pis);
            if (objFuncionario != null)
            {
                if ((objfunc.Datademissao != null) && objfunc.Datademissao != new DateTime())
                {
                    mensagem = "";
                    return false;
                }
                else
                {
                    mensagem = "Pis já está sendo utilizado pelo funcionário " + objFuncionario.Nome;
                    mensagem += objFuncionario.Funcionarioativo == 1 ? " (Ativo)." : " (Inativo).";
                    return true;

                }
            }
            mensagem = "";
            return false;
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Funcionario objeto, Modelo.Acao acao)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (acao == Modelo.Acao.Excluir && objeto.DataInativacao == null)
            {
                ret.Add("DataInativacao", " Exclusão não permitida. Funcionário sem 'data de inativação' preenchida.");
            }
            if (objeto.Codigo <= 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Dscodigo))
            {
                ret.Add("Dscodigo", "Campo obrigatório.");
            }
            else
            {
                bool eNumero = true;
                foreach (char c in objeto.Dscodigo)
                {
                    if (!Char.IsNumber(c))
                    {
                        eNumero = false;
                    }
                }
                if (!eNumero)
                {
                    ret.Add("Dscodigo", "Permitido somente números.");
                }
                else
                {
                    if (Convert.ToDouble(objeto.Dscodigo) <= 0)
                    {
                        ret.Add("Dscodigo", "O código deve ser maior que zero(0).");
                    }
                }
            }

            if (!String.IsNullOrEmpty(objeto.CPF))
            {
                if (objeto.CPF != "___.___.___-__")
                {
                    if (!cwkFuncoes.ValidaCpf(objeto.CPF))
                    {
                        ret.Add("CPF", " CPF inválido. Verifique!");
                    }
                }
            }

            if (String.IsNullOrEmpty(objeto.Nome))
            {
                ret.Add("Nome", "Campo obrigatório.");
            }

            if (objeto.Idempresa <= 0)
            {
                ret.Add("Empresa", "Campo obrigatório.");
            }

            if (objeto.Iddepartamento <= 0)
            {
                ret.Add("Departamento", "Campo obrigatório.");
            }

            if (objeto.Idfuncao <= 0)
            {
                ret.Add("Funcao", "Campo obrigatório.");
            }

            if (objeto.IdAlocacao <= 0)
            {
                ret.Add("Alocacao", "Campo obrigatório.");
            }

            if (objeto.Idhorario <= 0 && objeto.Tipohorario < 3)
            {
                ret.Add("Horario", "Campo obrigatório.");
            }

            if (objeto.Dataadmissao == null)
            {
                ret.Add("Dataadmissao", "Campo obrigatório.");
            }

            if (objeto.Pis != null)
            {
                if (!ValidaPis(objeto.Pis))
                {
                    ret.Add("Pis", "O PIS informado é inválido.");
                }
            }          


            BLL.Horario bllHorario = new BLL.Horario(ConnectionString, UsuarioLogado);

            List<Modelo.Funcionario> lFunc2Reg = GetAllListPorPis(new List<string>() { objeto.Pis });
            Modelo.Funcionario Func2Reg = lFunc2Reg.Where(s => s.Id != objeto.Id && s.Funcionarioativo == 1 && s.Excluido == 0).FirstOrDefault();
            if (Func2Reg != null && Func2Reg.Id > 0)
            {
                if (objeto.Dscodigo == Func2Reg.Dscodigo)
                {
                    ret.Add("Dscodigo", "O Código inserido já está cadastrado para outro funcionário.");
                }

                if (objeto.Matricula == Func2Reg.Matricula)
                {
                    ret.Add("Matricula", "A Mátricula inserida já está cadastrada para outro funcionário.");
                }

                if (objeto.bFuncionarioativo || (objeto.DataInativacao == null || objeto.DataInativacao > DateTime.Now)) // Apenas valida se o funcionário em questão não estiver sendo inativado ou inativo
                {
                    Modelo.Horario horario = bllHorario.LoadObject(objeto.Idhorario);
                    Modelo.Horario horario2Reg = bllHorario.LoadObject(Func2Reg.Idhorario);

                    List<int> IdsHorariosComConflito = new List<Int32>();
                    DAL.SQL.MudancaHorario.ValidarHorariosEmConflito(horario, new List<Modelo.Horario>() { horario2Reg }, out IdsHorariosComConflito);
                    if (IdsHorariosComConflito.Count > 0)
                    {
                        ret.Add("Horario", "O Horário informado esta em conflito com o horário de outro registro desse empregado.");
                    } 
                }
            }

            if (objeto.Tipohorario == 3 && objeto.CicloSequenciaIndice == null || objeto.CicloSequenciaIndice == 0)
            {
                ret.Add("CicloSequenciaIndice", "Para horário dinâmico é necessário selecionar uma sequência.");
            }

            if ((objeto.UtilizaAppPontofopag || objeto.UtilizaWebAppPontofopag || objeto.utilizaregistrador) && String.IsNullOrEmpty(objeto.Mob_Senha))
            {
                ret.Add("Mob_Senha", "Campo obrigatório.");
            }

            if (ret.Count == 0)
            {
                if (Int32.TryParse(objeto.Dscodigo, out int dsCodigoSemZeroEsquerada))
                {
                    objeto.Dscodigo = dsCodigoSemZeroEsquerada.ToString();
                }
                else
                {
                    ret.Add("Dscodigo", "Código Inválido.");
                }
            }

            return ret;
        }

        public Dictionary<string, string> ValidaObjeto1(Modelo.Funcionario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (objeto.Codigo <= 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Dscodigo))
            {
                ret.Add("txtCodigoDS", "Campo obrigatório.");
            }
            else
            {
                bool eNumero = true;
                foreach (char c in objeto.Dscodigo)
                {
                    if (!Char.IsNumber(c))
                    {
                        eNumero = false;
                    }
                }
                if (!eNumero)
                {
                    ret.Add("txtCodigoDS", "Permitido somente números.");
                }
                else
                {
                    if (Convert.ToDouble(objeto.Dscodigo) <= 0)
                    {
                        ret.Add("txtCodigoDS", "O código deve ser maior que zero(0).");
                    }
                }
            }

            if (String.IsNullOrEmpty(objeto.Nome))
            {
                ret.Add("txtNome", "Campo obrigatório.");
            }

            if (objeto.Idempresa <= 0)
            {
                ret.Add("cbIdEmpresa", "Campo obrigatório.");
            }

            if (objeto.Iddepartamento <= 0)
            {
                ret.Add("cbIdDepartamento", "Campo obrigatório.");
            }

            if (objeto.Idfuncao <= 0)
            {
                ret.Add("cbIdFuncao", "Campo obrigatório.");
            }

            if (objeto.IdAlocacao <= 0)
            {
                ret.Add("cbIdAlocacao", "Campo obrigatório.");
            }

            if (objeto.Idhorario <= 0)
            {
                ret.Add("cbIdHorario", "Campo obrigatório.");
            }

            if (objeto.Dataadmissao == null)
            {
                ret.Add("txtDataadmissao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Funcionario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto, pAcao);

            try
            {
                if (pAcao == Modelo.Acao.Alterar)
                {
                    if (objeto.Funcionarioativo_Ant == 0 && objeto.Funcionarioativo == 1)
                    {
                        Modelo.Empresa objEmpresa = dalEmpresa.GetEmpresaPrincipal();
                        if (objEmpresa.TipoLicenca != 1)
                        {
                            if (objEmpresa.Quantidade == this.getNumFuncionarios())
                            {
                                erros.Add("chbFuncionarioativo", "O número de funcionários ativos atingiu o limite.");
                            }
                        }
                    }
                }
                if (erros.Count == 0)
                {
                    List<string> log = new List<string>();
                    DateTime datai, dataf;
                    bool importou = false;
                    BLLIntegracaoPNL.EmpregadoImportar empregadoImportarPNL = new BLLIntegracaoPNL.EmpregadoImportar(ConnectionString, UsuarioLogado);
                    BLL.Pessoa bllPessoaSupervisor = new BLL.Pessoa(ConnectionString, UsuarioLogado);
                    objeto.ObjPessoaSupervisor = bllPessoaSupervisor.LoadObject(objeto.IdPessoaSupervisor.GetValueOrDefault());

                    BLL.ParametroPainelRH BllParametroPnlRH = new BLL.ParametroPainelRH(ConnectionString, UsuarioLogado);
                    Modelo.ParametroPainelRH parametroPainelRH = new Modelo.ParametroPainelRH();
                    parametroPainelRH = BllParametroPnlRH.GetAllList().FirstOrDefault();
                    switch (pAcao)
                    {

                        case Modelo.Acao.Incluir:

                            try
                            {
                                VerificaHorarioDinamico(objeto);
                                dalFuncionario.Incluir(objeto);
                            }
                            catch (Exception e)
                            {
                                TrataCodigoEmUso(objeto, erros, e);
                                if (erros.Count > 0)
                                {
                                    return erros;
                                }
                            }
                            if (!objeto.NaoRecalcular)
                            {
                                importou = this.ImportacaoBilhete(objeto, out datai, out dataf, log);
                                if (importou)
                                {
                                    this.AtualizaMarcacao(objeto.Id, datai, dataf);
                                }
                            }

                            if (parametroPainelRH != null && parametroPainelRH.IntegraPainel == true)
                            {
                                objeto = empregadoImportarPNL.IntegraPainel(objeto, pAcao, parametroPainelRH);
                            }


                            break;
                        case Modelo.Acao.Alterar:
                            objeto.Tipohorario = objeto.Tipohorario == 3 ? Convert.ToInt16(2) : objeto.Tipohorario;
                            dalFuncionario.Alterar(objeto);
                            if ((objeto.Funcionarioativo != objeto.Funcionarioativo_Ant) && (objeto.Funcionarioativo == 1))
                            {
                                importou = this.ImportacaoBilhete(objeto, out datai, out dataf, log);
                            }
                            else
                            {
                                datai = new DateTime();
                                dataf = new DateTime();
                            }

                            if (importou || (objeto.Funcionarioativo != objeto.Funcionarioativo_Ant) || (objeto.Iddepartamento_Ant != objeto.Iddepartamento)
                                || (objeto.Idempresa_Ant != objeto.Idempresa) || (objeto.Idfuncao_Ant != objeto.Idfuncao)
                                || (objeto.Dataadmissao_Ant != objeto.Dataadmissao) || (objeto.Datademissao_Ant != objeto.Datademissao)
                                || (objeto.Naoentrarbanco_Ant != objeto.Naoentrarbanco) || (objeto.Naoentrarcompensacao_Ant != objeto.Naoentrarcompensacao))
                            {
                                if (!objeto.NaoRecalcular)
                                {
                                    this.AtualizaMarcacao(objeto.Id, datai, dataf);
                                }
                            }

                            if (parametroPainelRH != null && parametroPainelRH.IntegraPainel == true)
                            {
                                objeto = empregadoImportarPNL.IntegraPainel(objeto, pAcao, parametroPainelRH);

                            }
                            break;
                        case Modelo.Acao.Excluir:
                            objeto.Excluido = 1;
                            objeto.Funcionarioativo = 0;
                            dalFuncionario.Alterar(objeto);

                            if (parametroPainelRH != null && parametroPainelRH.IntegraPainel == true)
                            {
                                empregadoImportarPNL.IntegraPainel(objeto, pAcao, parametroPainelRH);
                            }
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("UQ_Funcionario_RFID"))
                {
                    erros.Add("RFID", "O RFID " + objeto.RFID + " já esta sendo utilizado.");
                }
                else
                {
                    throw e;
                }
            }
            return erros;
        }

        private void VerificaHorarioDinamico(Modelo.Funcionario objeto)
        {
            if (objeto.Tipohorario == 3)
            {
                BLL.MudancaHorario bllMudancaHorario = new BLL.MudancaHorario(ConnectionString, UsuarioLogado);
                int tipoHorario = objeto.Tipohorario;
                int idHorario = objeto.IdHorarioDinamico.GetValueOrDefault();
                int? idHorarioDinamico = objeto.IdHorarioDinamico;
                bllMudancaHorario.ValidaHorarioDinamico(ref tipoHorario, ref idHorario, objeto.Dataadmissao, objeto.CicloSequenciaIndice, ref idHorarioDinamico);
                objeto.Tipohorario = Convert.ToInt16(tipoHorario);
                objeto.Idhorario = idHorario;
                objeto.IdHorarioDinamico = idHorarioDinamico;
            }
        }

        private void TrataCodigoEmUso(Modelo.Funcionario objeto, Dictionary<string, string> erros, Exception e)
        {
            if ((e.Message.Contains("O código informado já está sendo utilizado em outro registro") || e.Message.Contains("UX_Funcionario") && objeto.ForcarNovoCodigo) || e.Message.Contains("UX_Codigo"))
            {
                string mensagem = "";
                if (DsCodigoUtilizado(objeto.Dscodigo, out mensagem) && !erros.ContainsKey("txtCodigoDS"))
                {
                    erros.Add("txtCodigoDS", mensagem);
                }
                while (true)
                {
                    int ultimoCodigo = MaxCodigo();
                    objeto.Codigo = ultimoCodigo;
                    objeto.Dscodigo = ultimoCodigo.ToString();
                    try
                    {
                        dalFuncionario.Incluir(objeto);
                        erros = new Dictionary<string, string>();                        
                        break;
                    }
                    catch (Exception ex)
                    {
                        tentativasNovoCodigo++;
                        if ((tentativasNovoCodigo > 3) || !(e.Message.Contains("O código informado já está sendo utilizado em outro registro") || e.Message.Contains("UX_Funcionario") || e.Message.Contains("UX_Codigo")))
                        {
                            if (erros.Count() > 0)
                            {
                                break;
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }            
            else
            {
                throw e;
            }
        }

        public Dictionary<string, string> Salvar1(Modelo.Acao pAcao, Modelo.Funcionario objeto, int? tipo)
        {
            Dictionary<string, string> erros = new Dictionary<string, string>();
            if (tipo == 1)
                erros = ValidaObjeto(objeto, pAcao);
            if (tipo == 2)
                erros = ValidaObjeto1(objeto);

            if (pAcao == Modelo.Acao.Alterar)
            {
                if (objeto.Funcionarioativo_Ant == 0 && objeto.Funcionarioativo == 1)
                {
                    Modelo.Empresa objEmpresa = dalEmpresa.GetEmpresaPrincipal();
                    if (objEmpresa.TipoLicenca != 1)
                    {
                        if (objEmpresa.Quantidade == this.getNumFuncionarios())
                        {
                            erros.Add("chbFuncionarioativo", "O número de funcionários ativos atingiu o limite.");
                        }
                    }
                }
            }

            if (erros.Count == 0)
            {
                string mensagem = "";
                List<string> log = new List<string>();
                DateTime datai, dataf;
                bool importou = false;
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        if (DsCodigoUtilizado(objeto.Dscodigo, out mensagem))
                        {
                            erros.Add("txtCodigoDS", mensagem);
                        }
                        else
                        {
                            dalFuncionario.Incluir(objeto);
                            if (objeto.ImportarMarcacoes)
                            {
                                importou = this.ImportacaoBilhete(objeto, out datai, out dataf, log);
                                if (importou)
                                {
                                    this.AtualizaMarcacao(objeto.Id, datai, dataf);
                                }
                            }

                        }
                        break;
                    case Modelo.Acao.Alterar:
                        dalFuncionario.Alterar(objeto);
                        if (objeto.ImportarMarcacoes)
                        {
                            if ((objeto.Funcionarioativo != objeto.Funcionarioativo_Ant) && (objeto.Funcionarioativo == 1))
                            {
                                importou = this.ImportacaoBilhete(objeto, out datai, out dataf, log);
                            }
                            else
                            {
                                datai = new DateTime();
                                dataf = new DateTime();
                            }

                            if (importou || (objeto.Funcionarioativo != objeto.Funcionarioativo_Ant) || (objeto.Iddepartamento_Ant != objeto.Iddepartamento)
                                || (objeto.Idempresa_Ant != objeto.Idempresa) || (objeto.Idfuncao_Ant != objeto.Idfuncao)
                                || (objeto.Dataadmissao_Ant != objeto.Dataadmissao) || (objeto.Datademissao_Ant != objeto.Datademissao)
                                || (objeto.Naoentrarbanco_Ant != objeto.Naoentrarbanco) || (objeto.Naoentrarcompensacao_Ant != objeto.Naoentrarcompensacao))
                            {
                                this.AtualizaMarcacao(objeto.Id, datai, dataf);
                            }
                        }

                        break;
                    case Modelo.Acao.Excluir:
                        objeto.Excluido = 1;
                        objeto.Funcionarioativo = 0;
                        dalFuncionario.Alterar(objeto);
                        break;
                }

            }
            return erros;
        }

        public void Incluir(List<Modelo.Funcionario> funcionarios, bool salvarHistorico)
        {
            dalFuncionario.Incluir(funcionarios, salvarHistorico);
        }

        public void ExcluirDefinitivamente(Modelo.Funcionario objeto)
        {
            dalFuncionario.Excluir(objeto);
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
            return dalFuncionario.getId(pValor, pCampo, pValor2);
        }

        public bool ImportacaoBilhete(Modelo.Funcionario pFuncionario, out DateTime pDataI, out DateTime pDataF, List<string> log)
        {
            DateTime? dataInicial;
            DateTime? dataFinal;
            BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(ConnectionString, UsuarioLogado);
            bool importou = bllImportaBilhetes.ImportarBilhetes(pFuncionario.Dscodigo, false, null, null, out dataInicial, out dataFinal, objProgressBar, log);
            if (dataInicial != null && dataFinal != null)
            {
                pDataI = dataInicial.Value;
                pDataF = dataFinal.Value;
            }
            else
            {
                pDataI = new DateTime();
                pDataF = new DateTime();
            }
            return importou;
        }

        /// <summary>
        /// Método utilizado para carregar os funcionarios na tabela de marcações
        /// </summary>
        /// <param name="tipo">1-Empresa, 2-Departamento, 3-Todos</param>
        /// <param name="identificacao">id da empresa ou do departamento</param>
        /// <returns>Lista dos funcionários</returns>
        /// WNO - 28/12/2009
        public List<Modelo.Funcionario> GetTabelaMarcacao(int tipo, int identificacao, string consultaNomeFuncionario)
        {
            return dalFuncionario.GetTabelaMarcacao(tipo, identificacao, consultaNomeFuncionario);
        }

        public List<Modelo.Funcionario> GetAllList(bool pegaInativos, bool pegaExcluidos)
        {
            return dalFuncionario.GetAllList(pegaInativos, pegaExcluidos);
        }

        public List<Modelo.Funcionario> getLista(int pempresa)
        {
            return dalFuncionario.getLista(pempresa);
        }

        /// <summary>
        /// Pega do banco de dados os dados dos funcionarios de determinada função
        /// OBS: Não precisa de empresa nem departamento porque o ID da função é unico no banco
        /// </summary>
        /// <param name="pIDDepartamento">ID da Funcao</param>
        /// <returns>Lista de funcionarios daquela função</returns>
        //PAM - 11/12/2009
        public List<Modelo.Funcionario> GetPorFuncaoList(int pIdFuncao)
        {
            return dalFuncionario.GetPorFuncaoList(pIdFuncao);
        }

        /// <summary>
        /// Pega do banco de dados os dados dos funcionarios de determinada função
        /// OBS: Não precisa de empresa nem departamento porque o ID da função é unico no banco
        /// </summary>
        /// <param name="pIDDepartamento">ID da Funcao</param>
        /// <returns>Lista de funcionarios daquela função</returns>
        //PAM - 11/12/2009
        public List<Modelo.Funcionario> GetPorDepartamentoList(int pIdDepartamento)
        {
            return dalFuncionario.GetPorDepartamentoList(pIdDepartamento);
        }

        /// <summary>
        /// Pega todos os funcionarios que tem aquele horario
        /// </summary>
        /// <param name="pIdHorario">Id do horário</param>
        /// <returns>Lista (List) de funcionarios daquele horario</returns>
        //PAM 05/04/2010
        public List<Modelo.Funcionario> GetPorHorario(int pIdHorario)
        {
            return dalFuncionario.GetPorHorario(pIdHorario);
        }

        /// <summary>
        /// Pega todos os funcionarios de um determinado tipo
        /// </summary>
        /// <param name="pTipo">Tipo: 0: Empresa, 1: Departamento, 3: Função, 4:Horario, 5: Todos funcionarios do banco</param>
        /// <param name="pIdTipo">Id do tipo</param>
        /// <returns>Lista de funcionarios daquele tipo</returns>
        //PAM 05/04/2010
        //O padrão é que 2 seja por funcionario, mas nesse caso não faz sentido uma lista de funcionarios por funcionario        
        public List<Modelo.Funcionario> GetFuncionarios(int pTipo, int pIdTipo)
        {
            switch (pTipo)
            {
                case 0: return getLista(pIdTipo);//Empresa
                case 1: return GetPorDepartamentoList(pIdTipo);//Departamento
                case 2: //Funcionario -  tem que criar uma lista, ler o objeto e colocar na lista
                    List<Modelo.Funcionario> lstFuncionario = new List<Modelo.Funcionario>();
                    Modelo.Funcionario objFuncionario = LoadObject(pIdTipo);
                    lstFuncionario.Add(objFuncionario);
                    return lstFuncionario;
                case 3: return GetPorFuncaoList(pIdTipo);//Funcao
                case 4: return GetPorHorario(pIdTipo);//Horario
                case 5: return GetAllList(false, false);//Pega todos os funcionarios, menos os excluidos e inativos
                case 6: return GetAllListPorContrato(pIdTipo);
                default: return null;
            }
        }

        public List<int> IdsFuncPeriodoContratado(TipoFiltroFuncionario tipo, List<int> idsReg, DateTime dtIni, DateTime dtFin)
        {
            return dalFuncionario.IdsFuncPeriodoContratado(tipo, idsReg, dtIni, dtFin);
        }

        public string GetDsCodigo(string pPis)
        {
            return dalFuncionario.GetDsCodigo(pPis);
        }

        public DataTable GetPisCodigo(bool webApi)
        {
            return dalFuncionario.GetPisCodigo(webApi);
        }

        public string GetSenha(Modelo.Funcionario objFuncionario)
        {
            return ClSeguranca.Descriptografar(objFuncionario.Senha);
        }

        public void SetSenha(Modelo.Funcionario objFuncionario, string senha)
        {
            objFuncionario.Senha = ClSeguranca.Criptografar(senha);
        }

        public string GetMobSenha(Modelo.Funcionario objFuncionario)
        {
            try
            {
                return ClSeguranca.Descriptografar(objFuncionario.Mob_Senha);
            }
            catch (Exception)
            {
                return objFuncionario.Mob_Senha;
            }

        }

        public void SetMobSenha(Modelo.Funcionario objFuncionario, string senha)
        {
            objFuncionario.Mob_Senha = ClSeguranca.Criptografar(senha);
        }

        public DataTable GetPorEmpresa(string empresas)
        {
            return dalFuncionario.GetPorEmpresa(empresas);
        }

        public IList<Modelo.Funcionario> GetPorEmpresaList(string pEmpresas)
        {
            return dalFuncionario.GetPorEmpresaList(pEmpresas);
        }

        public int GetIdDsCodigo(string pDsCodigo)
        {
            return dalFuncionario.GetIdDsCodigo(pDsCodigo);
        }
        public int GetIdDsCodigoProximidade(string pDsCodigo)
        {
            return dalFuncionario.GetIdDsCodigoProximidade(pDsCodigo);
        }

        public IList<Modelo.Proxy.pxyFuncionarioRelatorio> GetRelFuncionariosRelatorios(string filtro)
        {
            return dalFuncionario.GetRelFuncionariosRelatorios(filtro);
        }

        public IList<Modelo.Funcionario> GetFuncionariosPorIds(string pIDs)
        {
            return dalFuncionario.GetFuncionariosPorIds(pIDs);
        }

        public bool ExportacaoRelogioTopData()
        {
            bool ret = false;

            List<Modelo.Funcionario> listaFuncionario = dalFuncionario.GetAllList(false, false);
            Modelo.Horario objHorario = new Modelo.Horario();

            ArrayList arr = new ArrayList();
            ArrayList arrHorario = new ArrayList();

            foreach (var func in listaFuncionario)
            {
                Modelo.Proxy.pxyExportacaoFuncionario record = new Modelo.Proxy.pxyExportacaoFuncionario();

                record.Codigo = String.Format("{0:0000000000000000}", Convert.ToInt64(func.Dscodigo));
                record.NomeFuncionario = func.Nome.Length > 40 ? func.Nome.Substring(0, 40) : func.Nome;

                if (func.TipoTickets == -1)  //Horario
                {
                    if (objHorario.Id != func.Idhorario)
                    {
                        objHorario = dalHorario.LoadObject(func.Idhorario);

                        if (objHorario.TipoHorario == 1)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (objHorario.HorariosDetalhe[i].Saida_1 == "--:--")
                                    continue;

                                Modelo.Proxy.pxyExportacaoHorario recordHorario = new Modelo.Proxy.pxyExportacaoHorario();

                                recordHorario.Codigo = String.Format("{0:000}", objHorario.Codigo);
                                recordHorario.DiaSemana = (i + 1).ToString();

                                string entrada = BLL.CalculoHoras.OperacaoHoras('-', objHorario.HorariosDetalhe[i].Saida_1, func.ToleranciaEntrada);
                                string saida = BLL.CalculoHoras.OperacaoHoras('+', objHorario.HorariosDetalhe[i].Entrada_2, func.ToleranciaSaida);

                                recordHorario.Horario = entrada + saida;

                                arrHorario.Add(recordHorario);
                            }
                        }
                    }

                    if (objHorario.TipoHorario == 1)
                    {
                        record.CodHorario = String.Format("{0:000}", func.Codigo);
                        record.Bloqueado = "0";
                        record.Liberado = "0";
                    }
                    else
                    {
                        record.CodHorario = "000";
                        record.Bloqueado = "0";
                        record.Liberado = "1";
                    }
                }
                else
                {
                    record.CodHorario = "000";
                    record.Bloqueado = "0";
                    record.Liberado = "1";
                }

                arr.Add(record);
            }

            FileHelperEngine engine = new FileHelperEngine(typeof(Modelo.Proxy.pxyExportacaoFuncionario));
            FileHelperEngine engineHorario = new FileHelperEngine(typeof(Modelo.Proxy.pxyExportacaoHorario));

            engine.WriteFile(@"ListaFuncionarioTopData.txt", arr.ToArray(typeof(Modelo.Proxy.pxyExportacaoFuncionario)));
            engineHorario.WriteFile(@"ListaHorarioTopData.txt", arrHorario.ToArray(typeof(Modelo.Proxy.pxyExportacaoHorario)));

            return ret;
        }
        
        #region Contrato Funcionario
        public void SetContratoFuncionarioIntegracao(int idIntegracao, int idIntegracaoContrato, Acao acao)
        {
            BLL.Contrato bllContrato = new BLL.Contrato(ConnectionString);
            BLL.ContratoFuncionario bllContratoFuncionario = new BLL.ContratoFuncionario(ConnectionString);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(ConnectionString);
            Dictionary<string, string> erros = new Dictionary<string, string>();

            try
            {
                int idIntegrFunc =idIntegracao;
                int? funcid = bllFuncionario.GetIdporIdIntegracao(idIntegrFunc);
                int? idIntegrContr = idIntegracaoContrato;
                int? contid = bllContrato.GetIdPorIdIntegracao(idIntegrContr.GetValueOrDefault());              
                if (funcid > 0 && contid > 0)
                {
                    Modelo.Contrato Contrato = bllContrato.LoadObject(contid.GetValueOrDefault());
                    Modelo.ContratoFuncionario ContratoFunc = new Modelo.ContratoFuncionario();
                    BLL.ContratoFuncionario bllContratoFun = new BLL.ContratoFuncionario(ConnectionString);
                    
                    ContratoFunc.IdContrato = contid.GetValueOrDefault();
                    ContratoFunc.IdFuncionario = funcid.GetValueOrDefault();
                    
                    int? idContratoAnt = bllContratoFun.getContratoId(ContratoFunc.IdFuncionario); 

                    Modelo.ContratoFuncionario ContFunc = new Modelo.ContratoFuncionario();
                    if ((acao == Acao.Incluir || acao == Acao.Alterar) && (idContratoAnt != contid))
                    {
                        int CodigoContratoAnt = bllContratoFun.getContratoCodigo(idContratoAnt.GetValueOrDefault(), ContratoFunc.IdFuncionario);
                        int IdContratoFuncAnt = CodigoContratoAnt != 0 ? bllContratoFun.getId(CodigoContratoAnt, null, null) : 0;
                        if (idContratoAnt != ContratoFunc.IdContrato && idContratoAnt.GetValueOrDefault() != 0)
                        {
                            Modelo.ContratoFuncionario ContFuncAnt = new Modelo.ContratoFuncionario();
                            ContFuncAnt = bllContratoFuncionario.LoadObject(IdContratoFuncAnt);
                            acao = Acao.Alterar;
                            ContFuncAnt.excluido = 1;
                            erros = bllContratoFuncionario.Salvar(acao, ContFuncAnt);
                        }
                        ContFunc.IdContrato = contid.GetValueOrDefault();
                        ContFunc.IdFuncionario = funcid.GetValueOrDefault();
                        ContFunc.Codigo = bllContratoFuncionario.MaxCodigo();
                        acao = Acao.Incluir;
                        erros = bllContratoFuncionario.Salvar(acao, ContFunc);
                    }
                    else if (contid != 0 && acao == Acao.Excluir)
                    {
                        ContFunc = bllContratoFuncionario.LoadObject(idContratoAnt.GetValueOrDefault());
                        acao = Acao.Alterar;
                        ContFunc.Acao = Acao.Alterar;
                        ContFunc.excluido = 1;
                        erros = bllContratoFuncionario.Salvar(acao, ContFunc);
                    }

                }

                return;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex); 
            }
            return;
        }

        public void SetContratoFuncionario(int idfuncionario, string contratofunc)
        {
            BLL.Contrato bllContrato = new BLL.Contrato(ConnectionString, UsuarioLogado);
            BLL.ContratoFuncionario bllContratoFun = new BLL.ContratoFuncionario(ConnectionString, UsuarioLogado);
            int? idContratoAnt = bllContratoFun.getContratoId(idfuncionario);
            int CodContrato = contratofunc != null ? Convert.ToInt32(contratofunc.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]) : 0;
            int IdContrato = CodContrato != 0 ? bllContrato.getId(CodContrato, null, null) : 0;
            if (idContratoAnt.GetValueOrDefault() != IdContrato)
            {
                int CodigoContratoAnt = bllContratoFun.getContratoCodigo(idContratoAnt.GetValueOrDefault(), idfuncionario);
                int IdAnt = CodigoContratoAnt != 0 ? bllContratoFun.getId(CodigoContratoAnt, null, null) : 0;
                Modelo.ContratoFuncionario ContFuncAnt = new Modelo.ContratoFuncionario();
                ContFuncAnt = bllContratoFun.LoadObject(IdAnt);
                ContFuncAnt.excluido = 1;
                if (contratofunc != null && IdContrato != 0)
                {
                    if (idContratoAnt != 0)
                    {
                        bllContratoFun.Salvar(Acao.Alterar, ContFuncAnt);
                    }
                    Modelo.ContratoFuncionario objContra = new Modelo.ContratoFuncionario();
                    objContra.IdContrato = IdContrato;
                    objContra.IdFuncionario = idfuncionario;
                    objContra.Codigo = bllContratoFun.MaxCodigo();
                    bllContratoFun.Salvar(Acao.Incluir, objContra);
                }
                else
                {
                    bllContratoFun.Salvar(Acao.Alterar, ContFuncAnt);
                }
            }

            return;
        }

        #endregion

        public List<Modelo.Funcionario> GetAllListPorContrato(int idContrato)
        {
            return dalFuncionario.GetAllListPorContrato(idContrato);
        }

        public List<Modelo.Funcionario> GetAllListContratos()
        {
            return dalFuncionario.GetAllListContratos();
        }

        public int? GetIdporIdIntegracao(int? IdIntegracao)
        {
            return dalFuncionario.GetIdporIdIntegracao(IdIntegracao);
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoFuncionario(List<int> ids)
        {
            return dalFuncionario.FechamentoPontoFuncionario(ids);
        }

        public void AtualizaHorariosFuncionariosMudanca(List<int> idsFuncionarios)
        {
            dalFuncionario.AtualizaHorariosFuncionariosMudanca(idsFuncionarios);
        }

        public List<Modelo.Funcionario> GetAllListComDataUltimoFechamento(bool pegaTodos)
        {
            return dalFuncionario.GetAllListComDataUltimoFechamento(pegaTodos);
        }

        public List<Modelo.Funcionario> GetAllListComDataUltimoFechamento(bool pegaTodos, IList<int> idsFuncs)
        {
            return dalFuncionario.GetAllListComDataUltimoFechamento(pegaTodos, idsFuncs);
        }

        public List<Modelo.Funcionario> GetAllListComUltimosFechamentos(bool pegaTodos)
        {
            return dalFuncionario.GetAllListComUltimosFechamentos(pegaTodos);
        }

        public List<Modelo.Funcionario> GetAllListComUltimosFechamentos(bool pegaTodos, IList<int> idsFuncs)
        {
            return dalFuncionario.GetAllListComUltimosFechamentos(pegaTodos, idsFuncs);
        }

        public Modelo.Funcionario GetFuncionarioPorCpfeMatricula(Int64 cpf, string matricula)
        {
            return dalFuncionario.GetFuncionarioPorCpfeMatricula(cpf, matricula);
        }

        public IList<Modelo.Proxy.PxyFuncionarioCabecalhoRel> GetFuncionariosCabecalhoRel(IList<int> IdFuncs)
        {
            return dalFuncionario.GetFuncionariosCabecalhoRel(IdFuncs);
        }

        /// <summary>
        /// Retorna uma lista com os ids dos funcionários de acordo com o parâmetro com valor maior que zero, ou seja caso id departamento > 0 traz por departamento, contrato > 0 por contrato, empresa > 0 por empresa, seguindo essa ordem
        /// </summary>
        /// <param name="idDep">Passar o id Departamento caso deseje os funcionários por Departamento</param>
        /// <param name="idCont">Passar o id Contrato caso deseje os funcionários por Contrato</param>
        /// <param name="idEmp">Passar o id Empresa caso deseje os funcionários por empresa</param>
        /// <returns>Retorna a lista de ids de funcionários dependendo dos parâmetros passados</returns>
        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(int idDep, int idCont, int idEmp)
        {
            return dalFuncionario.GetIdsFuncsPorIdsEmpOuDepOuContra(idDep, idCont, idEmp);
        }

        public List<int> GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(int idFuncao, int idDep, int idCont, int idEmp)
        {
            return dalFuncionario.GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(idFuncao, idDep, idCont, idEmp, false, false, false);
        }

        public List<int> GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(int idFuncao, int idDep, int idCont, int idEmp, bool verificaPermissao, bool removeInativo, bool removeExcluido)
        {
            return dalFuncionario.GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(idFuncao, idDep, idCont, idEmp, verificaPermissao, removeInativo, removeExcluido);
        }

        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(List<int> ListIdDep, List<int> ListIdCont, List<int> ListIdEmp, bool verificaPermissao, bool removeInativo, bool removeExcluido)
        {
            return dalFuncionario.GetIdsFuncsPorIdsEmpOuDepOuContra(ListIdDep, ListIdCont, ListIdEmp, verificaPermissao, removeInativo, removeExcluido);
        }

        public List<Modelo.Funcionario> GetAllListPorPis(List<string> lPis)
        {
            return dalFuncionario.GetAllListPorPis(lPis);
        }

        public List<int> GetAllListPorCPF(List<string> lCPF)
        {
            return dalFuncionario.GetAllListPorCPF(lCPF);
        }

        public List<Modelo.Funcionario> GetPorHorarioVigencia(int idHorario)
        {
            return dalFuncionario.GetPorHorarioVigencia(idHorario);
        }

        public List<int> GetIdsFuncsAtivos(string condicao)
        {
            return dalFuncionario.GetIdsFuncsAtivos(condicao);
        }

        public Modelo.Funcionario LoadObjectByPis(string PIS)
        {
            return dalFuncionario.LoadObjectByPis(PIS);
        }

        public List<Modelo.Funcionario> RetornaFuncDsCodigos(List<String> pCodigo)
        {
            return dalFuncionario.RetornaFuncDsCodigos(pCodigo);
        }

        /// <summary>
        /// Retorna os funcionários com o pis duplicado
        /// </summary>
        /// <param name="lPis">Quando passado pis como parâmetro retornará apenas os registros que possuem o mesmo dentro da lista, caso o parametros estaja vazio retorna todos os funcs com mais de um pis
        /// Select não desconsidera inativos e excluídos
        /// </param>
        /// <returns> Retorna os funcionários com o pis duplicado sem desconsiderar excluido ou inativo</returns>
        public List<Modelo.Funcionario> GetAllPisDuplicados(List<string> lPis)
        {
            return dalFuncionario.GetAllPisDuplicados(lPis);
        }

        public List<Modelo.Proxy.PxyFuncionarioDiaUtil> GetDiaUtilFuncionario(List<int> idsFuncs, DateTime dataIni, DateTime dataFin)
        {
            return dalFuncionario.GetDiaUtilFuncionario(idsFuncs, dataIni, dataFin);
        }

        #region Regras de bloqueio de estações

        #region ValidarBloqueioFuncionario
        public Modelo.BloqueioFuncionario ValidarBloqueioFuncionario(Modelo.Funcionario objFuncionario, DateTime data)
        {
            //Modelo.Funcionario objFuncionario = this.LoadAtivoPorCPF(cpf);
            BLL.BilhetesImp bllBilhete = new BilhetesImp(this.ConnectionString);

            BLL.Marcacao bllMarcacao = new BLL.Marcacao(this.ConnectionString);
            Modelo.Marcacao objMarcacao = bllMarcacao.GetPorData(objFuncionario, data);


            int[] HoraEntradasPrevitas = new int[] { -1, -1, -1, -1 };
            int[] HoraSaidasPrevistas = new int[] { -1, -1, -1, -1 };

            BLL.Horario bllHorario = new BLL.Horario(this.ConnectionString);
            Modelo.Horario objHorario = bllHorario.LoadObject(objFuncionario.Idhorario);

            DataTable horarioMarcacao = CarregarHorarioMarcacao(objMarcacao);

            //    BLL.HorarioDetalhe bllHorarioDetalhe = new BLL.HorarioDetalhe(this.ConnectionString);
            //    Modelo.HorarioDetalhe hd = bllHorarioDetalhe.LoadParaCartaoPonto(objHorario.Id, objMarcacao.Dia);

            //Modelo.JornadaAlternativa objJornadaAlternativa = new Modelo.JornadaAlternativa();
            //if (objMarcacao.LegendasConcatenadas.Contains("J"))
            //{
            //    BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(this.ConnectionString);
            //    objJornadaAlternativa = bllJornadaAlternativa.LoadParaUmaMarcacao(objMarcacao.Data, 2, objFuncionario.Id);

            //    HoraEntradasPrevitas = new int[] { -1, -1, -1, -1 };
            //    HoraSaidasPrevistas = new int[] { -1, -1, -1, -1 };
            //}

            //BLL.HorarioDetalhe bllHorarioDet = new BLL.HorarioDetalhe(this.ConnectionString);
            //Modelo.HorarioDetalhe objHorarioDet = bllHorarioDet.LoadParaCartaoPonto(objFuncionario.Idhorario);

            List<Modelo.BilhetesImp> bilhetesAnterior = bllBilhete.GetBilhetesFuncPeriodo(objFuncionario.Dscodigo, data.AddDays(-1), data.AddDays(-1)).Where(bilhete => bilhete.Ocorrencia != 'D').ToList();
            List<Modelo.BilhetesImp> bilhetesAtual = bllBilhete.GetBilhetesFuncPeriodo(objFuncionario.Dscodigo, data, data).Where(bilhete => bilhete.Ocorrencia != 'D').ToList();



            string motivo = "";
            DateTime? previsaoLiberacao = new DateTime();
            int? RegraBloqueio = 0;
            Modelo.BloqueioFuncionario bloqueio = new Modelo.BloqueioFuncionario { Funcionario = objFuncionario, Bloqueado = false };
            if (ValidarBloqueioPorInterjornada(objFuncionario, bilhetesAnterior, bilhetesAtual, objHorario, objMarcacao, horarioMarcacao, ref motivo, ref previsaoLiberacao, ref RegraBloqueio))
            {
                bloqueio.Bloqueado = true;
            }
            else if (ValidarBloqueioPorIntrajornada(objFuncionario, bilhetesAnterior, bilhetesAtual, objHorario, objMarcacao, horarioMarcacao, ref motivo, ref previsaoLiberacao, ref RegraBloqueio))
            {
                bloqueio.Bloqueado = true;
            }
            else if (ValidarBloqueioPorLimiteDiario(objFuncionario, bilhetesAnterior, bilhetesAtual, objHorario, objMarcacao, ref motivo, ref previsaoLiberacao, ref RegraBloqueio))
            {
                bloqueio.Bloqueado = true;
            }
            else if (ValidarBloqueioPorIntervalo(objFuncionario, bilhetesAnterior, bilhetesAtual, objHorario, objMarcacao, ref motivo, ref previsaoLiberacao, ref RegraBloqueio))
            {
                bloqueio.Bloqueado = true;
            }

            bloqueio.Motivo = motivo;
            bloqueio.PrevisaoLiberacao = previsaoLiberacao == new DateTime() ? null : previsaoLiberacao;

            //Se esta bloqueado ou se é apenas para emitir aviso, adiciono a regra.
            if (bloqueio.Bloqueado || (!String.IsNullOrEmpty(bloqueio.Motivo) && bloqueio.PrevisaoLiberacao != null))
            {
                bloqueio.RegraBloqueio = RegraBloqueio == 0 ? null : RegraBloqueio;
            }

            return bloqueio;
        }
        #endregion

        #region ValidarBloqueioPorInterjornada
        private bool ValidarBloqueioPorInterjornada(Modelo.Funcionario objFuncionario, List<Modelo.BilhetesImp> bilhetesAnterior, List<Modelo.BilhetesImp> bilhetesAtual, Modelo.Horario objHorario, Modelo.Marcacao objMarcacao, DataTable horarioMarcacao, ref string motivo, ref DateTime? previsaoLiberacao, ref int? RegraBloqueio)
        {
            bool retorno = false;
            // Se possui batidas no dia anterior verifica a interjornada com o dia anterior
            if (bilhetesAnterior.Count != 0)
            {
                // Se a jornada do dia anterior estive incompleto já bloqueio pois não é possível fazer a regra de interjornada sem o término da jornada
                if (bilhetesAnterior.Count % 2 != 0)
                {
                    motivo = "Jornada incompleta no dia anterior.";
                    RegraBloqueio = 1;
                    return true;
                }
                //Recupera a última saída do dia anterior (Término do Expediente)
                Modelo.BilhetesImp bilheteAnterior = bilhetesAnterior.Where(bilhete => bilhete.Ent_sai == "S").OrderByDescending(bilhete => bilhete.Posicao).First();
                DateTime dataAnterior = new DateTime();
                dataAnterior = bilheteAnterior.Data.Add(TimeSpan.Parse(bilheteAnterior.Mar_hora));

                //Verifica se a comparação vai ser pela hora corrente ou pela entrada do dia atual
                Modelo.BilhetesImp bilheteComparacao = bilhetesAtual.Where(bilhete => bilhete.Ent_sai == "E").OrderBy(bilhete => bilhete.Posicao).FirstOrDefault();
                DateTime dataComparacao = DateTime.Now;
                if (bilheteComparacao != null)
                {
                    DateTime entrada = bilheteComparacao.Data.Add(TimeSpan.Parse(bilheteComparacao.Mar_hora));
                    if (entrada > dataComparacao)
                    {
                        dataComparacao = entrada;
                    }
                }

                //Valida a Regra de Interjornada
                retorno = RegraInterjornada(objHorario, ref motivo, ref previsaoLiberacao, ref RegraBloqueio, dataComparacao, dataAnterior, false);
            }

            // Se não foi bloqueado pelo dia Anterior Verifico o dia Corrente
            if (!retorno && bilhetesAtual.Count() >= 2)
            {
                //Sapara as batidas previstas para o funcionário
                DataRow row = horarioMarcacao.Rows[0];
                int[] entradasPrevistas = new int[] { -1, -1, -1, -1 };
                int[] saidasPrevistas = new int[] { -1, -1, -1, -1 };
                for (int i = 0; i < 4; i++)
                {
                    entradasPrevistas[i] = Modelo.cwkFuncoes.ConvertBatidaMinuto(horarioMarcacao.Rows[0]["entrada_" + (i + 1)].ToString());
                    saidasPrevistas[i] = Modelo.cwkFuncoes.ConvertBatidaMinuto(horarioMarcacao.Rows[0]["saida_" + (i + 1)].ToString());
                }

                int qtdBatidasPrevistas = entradasPrevistas.Where(w => w >= 0).Count() + saidasPrevistas.Where(w => w >= 0).Count();
                int totalHorasTrabalhadasPrevista = CalculaMarcacao.CalculaTotalHorasTrabalhadasMin(entradasPrevistas, saidasPrevistas, objMarcacao.Data, false);
                DateTime TerminoDoExpediente = new DateTime();
                // Se a quantidade e batidas for maior ou igual a quantidade contratual e a quantidade trabalhada excede a quantidade prevista entende que a jornada ja foi cumprida por tanto o expediente terminado, comecando assim a validar a interjornada
                // Para o totalHorasTrabalhadas deve sempre ser contabilizado em cima de entrada e saida registrada, pois se o funcionário já tinha cumprido a jornada, mas mesmo assim bateu o ponto para voltar a trabalhar a máquina não permitirá (deve estar bloqueada), pois ele não cumpriu o tempo de descanço de interjornada
                if (
                    Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.TotalTrabalhadasDiuNot) >= (totalHorasTrabalhadasPrevista - (totalHorasTrabalhadasPrevista * 0.1))) // a Retirada de 60 minutos é a tolerancia de uma hora, se o funcionário trabalha 8 horas por dia, mas já trabalho 7 ou mais, o sistema vai entender que ele cumpriu a Jornada pre estabelecida.
                {
                    // Pega a ultima batida do dia ("Fim do Expediente") para verificar a interjornada
                    Modelo.BilhetesImp ultimaSaida = bilhetesAtual.Where(bilhete => bilhete.Ent_sai == "S").OrderByDescending(bilhete => bilhete.Posicao).First();
                    TerminoDoExpediente = ultimaSaida.Mar_data.GetValueOrDefault().Date.Add(TimeSpan.Parse(ultimaSaida.Hora));
                }

                // Valida se a ultima batida do dia ("Fim do Expediente") menos a hora atual já deu o tempo de interjornada
                retorno = RegraInterjornada(objHorario, ref motivo, ref previsaoLiberacao, ref RegraBloqueio, DateTime.Now, TerminoDoExpediente, true);
            }
            return retorno;
        }

        private static bool RegraInterjornada(Modelo.Horario objHorario, ref string motivo, ref DateTime? previsaoLiberacao, ref int? RegraBloqueio, DateTime dataComparacao, DateTime dataAnterior, bool RegraJornadaTrabalho)
        {
            string intervalo = string.IsNullOrEmpty(objHorario.LimiteInterjornada) ? "11:00" : objHorario.LimiteInterjornada;
            TimeSpan intervaloMinimo = TimeSpan.Parse(intervalo);
            if (dataComparacao > dataAnterior)
            {
                TimeSpan intervaloExecutado = dataComparacao - dataAnterior;
                if (intervaloExecutado < intervaloMinimo)
                {
                    if (RegraJornadaTrabalho)
                    {
                        motivo = "Bloqueio por exceder o limite da Jornada de Trabalho.";
                        previsaoLiberacao = dataAnterior.Add(intervaloMinimo);
                        motivo += String.Format(" Liberação prevista para {0:dd/MM} às {0:HH:mm}.", previsaoLiberacao);
                        RegraBloqueio = 1;
                        return true;
                    }
                    else
                    {
                        motivo = string.Format("Bloqueio por limite de interjornada de {0} horas", intervaloMinimo.Hours);
                        if (intervaloMinimo.Minutes > 0)
                            motivo += string.Format(" e {0} minutos", intervaloMinimo.Minutes);
                        motivo += ".";
                        previsaoLiberacao = dataAnterior.Add(intervaloMinimo);
                        motivo += string.Format(" Liberação prevista para {0:dd/MM} às {0:HH:mm}.", previsaoLiberacao);
                        RegraBloqueio = 1;
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region ValidarBloqueioPorIntrajornada
        private bool ValidarBloqueioPorIntrajornada(Modelo.Funcionario objFuncionario, List<Modelo.BilhetesImp> bilhetesAnterior, List<Modelo.BilhetesImp> bilhetesAtual, Modelo.Horario objHorario, Modelo.Marcacao objMarcacao, DataTable horarioMarcacao, ref string motivo, ref DateTime? previsaoLiberacao, ref int? RegraBloqueio)
        {
            if (bilhetesAtual.Count == 0)
            {
                return false;
            }

            Modelo.BilhetesImp saida = null;
            if (objMarcacao != null)
            {
                TimeSpan saida1;
                TimeSpan entrada2 = new TimeSpan();
                bool temEntrada2 = TimeSpan.TryParse(Convert.ToString(horarioMarcacao.Rows[0]["entrada_2"]), out entrada2);
                if (TimeSpan.TryParse(Convert.ToString(horarioMarcacao.Rows[0]["saida_1"]), out saida1))
                {
                    DateTime inicio = objMarcacao.Data.Add(saida1).AddHours(-1);
                    DateTime fim = new DateTime();
                    if (temEntrada2)
                        fim = objMarcacao.Data.Add(entrada2).AddHours(1);
                    else
                        fim = objMarcacao.Data.Add(saida1).AddHours(1);
                    var bilSaida = bilhetesAtual
                        .Where(bilhete => bilhete.Ent_sai == "S")
                        .Select(bilhete => new { ID = bilhete.Id, DataHora = bilhete.Mar_data.Value.Add(TimeSpan.Parse(bilhete.Mar_hora)) })
                        .Where(bilhete => bilhete.DataHora >= inicio && bilhete.DataHora <= fim).FirstOrDefault();
                    if (bilSaida != null)
                        saida = bilhetesAtual.FirstOrDefault(bilhete => bilhete.Id == bilSaida.ID);
                }
            }
            else
            {
                saida = bilhetesAtual.Where(bilhete => bilhete.Ent_sai == "S").OrderBy(bilhete => bilhete.Posicao).FirstOrDefault();
            }

            if (saida == null)
            {
                //Ainda não teve uma saída.
                return false;
            }

            DateTime datahoraComparacao = DateTime.Now;
            //Modelo.BilhetesImp entrada = bilhetesAtual.Where(bilhete => bilhete.Ent_sai == "E" && bilhete.Posicao > saida.Posicao).OrderBy(bilhete => bilhete.Posicao).FirstOrDefault();
            //if (entrada != null)
            //{
            //    datahoraComparacao = entrada.Data.Add(TimeSpan.Parse(entrada.Hora));
            //}

            string intervalo = string.IsNullOrEmpty(objHorario.LimiteMinimoHorasAlmoco) ? "01:00" : objHorario.LimiteMinimoHorasAlmoco;
            TimeSpan intervaloMinimo = TimeSpan.Parse(intervalo);

            DateTime datahoraSaida = saida.Data.Add(TimeSpan.Parse(saida.Hora));
            if (datahoraComparacao - datahoraSaida < intervaloMinimo)
            {
                motivo = string.Format("Bloqueio por limite mínimo de intrajornada de {0} horas", intervaloMinimo.Hours);
                if (intervaloMinimo.Minutes > 0)
                    motivo += string.Format(" e {0} minutos", intervaloMinimo.Minutes);
                motivo += ".";
                previsaoLiberacao = datahoraSaida.Add(intervaloMinimo);
                motivo += string.Format(" Liberação prevista para {0:dd/MM} às {0:HH:mm}.", previsaoLiberacao);
                RegraBloqueio = 2;
                return true;
            }

            return false;
        }
        #endregion

        #region ValidarBloqueioPorLimiteDiario
        private bool ValidarBloqueioPorLimiteDiario(Modelo.Funcionario objFuncionario, List<Modelo.BilhetesImp> bilhetesAnterior, List<Modelo.BilhetesImp> bilhetesAtual, Modelo.Horario objHorario, Modelo.Marcacao objMarcacao, ref string motivo, ref DateTime? previsaoLiberacao, ref int? RegraBloqueio)
        {
            string strLimite = string.IsNullOrEmpty(objHorario.LimiteHorasTrabalhadasDia) ? "10:00" : objHorario.LimiteHorasTrabalhadasDia;

            if (bilhetesAtual.Count == 0)
            {
                return false;
            }

            List<Modelo.BilhetesImp> entradas = bilhetesAtual.Where(bilhete => bilhete.Ent_sai == "E").OrderBy(bilhete => bilhete.Posicao).ToList();

            TimeSpan limite = TimeSpan.Parse(strLimite);
            TimeSpan acumulado = new TimeSpan(0);
            DateTime datahoraEntrada = new DateTime();
            DateTime datahoraSaida = new DateTime();
            TimeSpan tempoTrabalhado = new TimeSpan(); // Tempo onde possui a entrada e saída já registrado.
            foreach (Modelo.BilhetesImp entrada in entradas)
            {
                Modelo.BilhetesImp saida = bilhetesAtual.FirstOrDefault(bilhete => bilhete.Ent_sai == "S" && bilhete.Posicao == entrada.Posicao);
                datahoraEntrada = entrada.Data.Add(TimeSpan.Parse(entrada.Hora));
                datahoraSaida = saida == null ? DateTime.Now : saida.Data.Add(TimeSpan.Parse(saida.Hora));
                //TempoTrabalhado considera apenas horas o tempo com entrada e saída já registrado
                if (saida != null)
                {
                    tempoTrabalhado += datahoraSaida - datahoraEntrada;
                }
                // acumulado tem a previsão
                acumulado += datahoraSaida - datahoraEntrada;
            }

            if (acumulado > limite)
            {
                motivo = string.Format("Bloqueio por limite de horas trabalhadas por dia de {0} horas", limite.Hours);
                if (limite.Minutes > 0)
                    motivo += string.Format(" e {0} minutos", limite.Minutes);
                motivo += ".";
                //. Liberação prevista para 03/03 às 00:00 hs.";
                RegraBloqueio = 3;
                return true;
            }
            else if (acumulado > limite.Subtract(MinutosParaAviso) && acumulado != tempoTrabalhado) // Apenas exibirá aviso quando o funcionário estiver trabalhando, portando quando o acumulado for igual ao tempo trabalhado significa que o funcionário tem todas as entradas e saída, ou seja, não tem uma entrada faltando uma saída.
            {
                if (limite > tempoTrabalhado)
                {
                    TimeSpan diff = limite.Subtract(tempoTrabalhado);
                    previsaoLiberacao = datahoraEntrada.Add(diff);
                }

                RegraBloqueio = 3;
                motivo = string.Format("Seu acesso será bloqueado às {0:HH:mm}, por estar trabalhando mais que {1} horas no dia.", previsaoLiberacao, limite.Hours);
                return false;
            }

            return false;
        }
        #endregion

        #region ValidarBloqueioPorIntervalo
        private bool ValidarBloqueioPorIntervalo(Modelo.Funcionario objFuncionario, List<Modelo.BilhetesImp> bilhetesAnterior, List<Modelo.BilhetesImp> bilhetesAtual, Modelo.Horario objHorario, Modelo.Marcacao objMarcacao, ref string motivo, ref DateTime? previsaoLiberacao, ref int? RegraBloqueio)
        {
            if (bilhetesAtual == null || bilhetesAtual.Count == 0)
            {
                return false;
            }

            //Compara sempre a primeira entrada, pois depois dessa já significa que teve intervalo, por tanto não tem mais que bloquear. A regra só se aplica para o primeiro período, pois se já fez intervalo não importa se no proximo período vai trabalhar mais que 06:00 horas
            Modelo.BilhetesImp entrada = bilhetesAtual.Where(bilhete => bilhete.Ent_sai == "E" && bilhete.Posicao == 1).FirstOrDefault();
            if (entrada != null)
            {
                Modelo.BilhetesImp saida = bilhetesAtual.Where(bilhete => bilhete.Ent_sai == "S" && bilhete.Posicao == entrada.Posicao).FirstOrDefault();

                if (saida != null)
                {
                    //Já existe uma saída vinculada à última entrada, não está em intervalo.
                    return false;
                }

                TimeSpan limite = TimeSpan.Parse("06:00");
                DateTime datahoraEntrada = entrada.Data.Add(TimeSpan.Parse(entrada.Hora));

                if (DateTime.Now - datahoraEntrada > limite)
                {
                    motivo = string.Format("Bloqueio por limite de horas trabalhadas sem intervalo de {0} horas", limite.Hours);
                    if (limite.Minutes > 0)
                        motivo += string.Format(" e {0} minutos", limite.Minutes);
                    previsaoLiberacao = null;
                    RegraBloqueio = 4;
                    return true;
                }
                else if (DateTime.Now - datahoraEntrada > limite.Subtract(MinutosParaAviso))
                {
                    DateTime previsao = datahoraEntrada.Add(limite);
                    motivo = string.Format("Seu acesso será bloqueado às {0:HH:mm}, por estar trabalhando mais que {1} horas seguidas sem intervalo.", previsao, limite.Hours);
                    previsaoLiberacao = previsao;
                    RegraBloqueio = 4;
                    return false;
                }
            }

            return false;
        }
        #endregion

        #region CarregarHorarioMarcacao
        private DataTable CarregarHorarioMarcacao(Modelo.Marcacao objMarcacao)
        {
            int idMarcacao = objMarcacao.Id;
            int diaSemana = Convert.ToInt32(objMarcacao.Data.DayOfWeek);
            return dalFuncionario.CarregarHorarioMarcacao(idMarcacao, diaSemana);
        }
        #endregion

        #endregion

        /// <summary>
        /// Retorna os funcionários ativos para publicação pela API.
        /// </summary>
        /// <returns>DataTable contendo ID, CPF, matrícula e nome dos funcionários ativos na base, sem restrição de empresa ou permissão.</returns>
        public DataTable CarregarTodosParaAPI()
        {
            return dalFuncionario.CarregarTodosParaAPI();
        }

        public DataTable CarregarTodosParaAPI(Int16 ativo, Int16 excluido)
        {
            return dalFuncionario.CarregarTodosParaAPI(ativo, excluido);
        }

        public List<Modelo.Funcionario> GetAllFuncsListPorCPF(List<string> lCPF)
        {
            return dalFuncionario.GetAllFuncsListPorCPF(lCPF);
        }

        /// <summary>
        /// Retorna os "qtdRegistros" proximos funcionários de acordo com o fitro
        /// </summary>
        /// <param name="tipo">1: empresa; 2: Departamento; 3: funcionario; 5: Contrato
        /// <param name="identificacao">Id do registro de acordo com o tipo</param>
        /// <param name="qtdRegistros"> qtd de registros que deseja que o select retorne</param>
        /// <param name="nomeFuncionario">Nome do funcionário caso deseje que os dados seja retornado a partir de um funcionário</param>
        /// <param name="tipoOrdenacao">0: ascendente; 1: decrescente, a ordenação é baseada no nome do funcionário</param>
        /// <param name="proximoOuAnterior">0: proximo; 1: anterior</param>
        /// <returns>Retorna a quantidade de registros solicitados de acordo com os parâmetros</returns>
        public List<Modelo.Funcionario> GetProximoOuAnterior(int tipo, int identificacao, int qtdRegistros, string nomeFuncionario, int tipoOrdenacao, int proximoOuAnterior)
        {
            return dalFuncionario.GetProximoOuAnterior(tipo, identificacao, qtdRegistros, nomeFuncionario, tipoOrdenacao, proximoOuAnterior);
        }

        /// <summary>
        /// Retorna uma lista de ids de acordo com uma lista de dscodigos
        /// </summary>
        /// <param name="lDsCodigos">Lista de DsCodigos</param>
        /// <returns>Retorna lista de IDs</returns>
        public List<int> GetIDsByDsCodigos(List<string> lDsCodigos)
        {
            return dalFuncionario.GetIDsByDsCodigos(lDsCodigos);
        }

        /// <summary>
        /// Retorna os ids dos funcionário relacionados a o tipo passado como parâmetro
        /// </summary>
        /// <param name="pTipo">0:Empresa, 1:Departamento, 2:Funcionário,3:Função,4:Horário</param>
        /// <param name="pIdentificacao">Id do registro passado no tipo</param>
        /// <param name="pegaExcluidos">Indica se deseja retornar os funcionário excluídos</param>
        /// <param name="pegaInativos">Indica se deseja retornar os funcionários inativos</param>
        /// <returns>Retorna a lista dos ids dos funcionários</returns>
        public List<int> GetIDsByTipo(int? pTipo, List<int> pIdentificacoes, bool pegaExcluidos, bool pegaInativos)
        {
            return dalFuncionario.GetIDsByTipo(pTipo, pIdentificacoes, pegaExcluidos, pegaInativos);
        }

        public DataTable GetPeriodoFechamentoPonto(List<int> idsFuncs)
        {
            return dalFuncionario.GetPeriodoFechamentoPonto(idsFuncs);
        }

        public List<PxyUltimoFechamentoPonto> GetUltimoFechamentoPontoFuncionarios(List<int> idsFuncs)
        {
            return dalFuncionario.GetUltimoFechamentoPontoFuncionarios(idsFuncs);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Funcionario objeto)
        {
            throw new NotImplementedException();
        }
        public void setFuncionariosEmpresa(int idEmpresa, bool FuncionarioAtivo)
        {
            dalFuncionario.setFuncionariosEmpresa(idEmpresa, FuncionarioAtivo);
        }

        public List<pxyFuncionarioRelatorio> GetRegistrosEmpregoFuncionario(int idFuncionario)
        {
            return dalFuncionario.GetRelFuncionariosRelatorios(string.Format(CultureInfo.CurrentCulture, " AND f.cpf = (select cpf from funcionario where id = {0}) ", idFuncionario)).ToList();
        }

        public List<string> GetDsCodigosByIDs(List<int> lIds)
        {
            return dalFuncionario.GetDsCodigosByIDs(lIds);
        }

        public List<PxyFuncionarioFechamentosPontoEBH> GetFuncionariosComUltimoFechamentosPontoEBH(bool pegaTodos, IList<int> idsFuncs, DateTime dataInicio, DateTime dataFim)
        {
            return dalFuncionario.GetFuncionariosComUltimoFechamentosPontoEBH(pegaTodos, idsFuncs, dataInicio, dataFim);
        }
    }
}
