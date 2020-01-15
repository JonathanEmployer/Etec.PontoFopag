using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class MudancaHorario : IBLL<Modelo.MudancaHorario>
    {
        DAL.IMudancaHorario dalMudancaHorario;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public MudancaHorario() : this(null)
        {
            
        }

        public MudancaHorario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public MudancaHorario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalMudancaHorario = new DAL.SQL.MudancaHorario(new DataBase(ConnectionString));
            dalMudancaHorario.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalMudancaHorario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalMudancaHorario.GetAll();
        }

        public List<Modelo.MudancaHorario> GetPeriodo(DateTime pDataI, DateTime pDataF, List<int> idsFuncionario)
        {
            return dalMudancaHorario.GetPeriodo(pDataI, pDataF, idsFuncionario);
        }

        public DataTable GetPorFuncionario(int pIdFuncionario)
        {
            return dalMudancaHorario.GetPorFuncionario(pIdFuncionario);
        }

        public Modelo.MudancaHorario LoadObject(int id)
        {
            return dalMudancaHorario.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.MudancaHorario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.MudancaHorario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalMudancaHorario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalMudancaHorario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalMudancaHorario.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Dictionary<string, string> MudarHorario(int pTipoMudanca,int pIDFuncao, int pIDEmpresa, int pIDDepartamento, int pIdFuncionario, int pTipoTurno, int pIDHorario, DateTime? pData, int? cicloSequenciaIndice)
        {
            Funcionario bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
            Dictionary<string, string> erros = new Dictionary<string, string>();

            if (pTipoMudanca == -1)
            {
                erros.Add("rgTipo", "Selecione o tipo de mudança.");
            }

            if (pTipoMudanca == 2 && pIDEmpresa == 0)
            {
                erros.Add("cbIdEmpresa", "Campo obrigatório.");
            }

            if (pTipoMudanca == 1 && pIDDepartamento == 0)
            {
                erros.Add("cbIdDepartamento", "Campo obrigatório.");
            }

            if (pTipoMudanca == 0 && pIdFuncionario == 0)
            {
                erros.Add("cbIdFuncionario", "Campo obrigatório.");
            }
            if (pTipoMudanca == 3 && pIDFuncao == 0)
            {
                erros.Add("cbIdFuncao", "Campo obrigatório.");
            }
            else
            {
                Modelo.Funcionario objFuncionario = bllFuncionario.LoadObject(pIdFuncionario);
                if (pIDHorario != 0)
                {
                    if (objFuncionario.Idhorario == pIDHorario)
                    {
                        erros.Add("cbIdTurnoNormal", "O funcionário já está utilizando o horário selecionado.");
                    }
                }
                else
                {
                    erros.Add("cbIdTurnoNormal", "Campo obrigatório.");
                }
            }

            if (pData == null)
            {
                erros.Add("txtData", "Campo obrigatório.");
            }

            if (pTipoTurno == -1)
            {
                erros.Add("rgTipoHorario", "Campo obrigatório.");
            }

            if (pIDHorario == -1)
            {
                erros.Add("cbIdTurnoNormal", "Campo obrigatório.");
            }

            string erroFechamento = ValidaFechamentoPonto(pTipoMudanca, pIDFuncao, pIDEmpresa, pIDDepartamento, pIdFuncionario, pData);
            if (!String.IsNullOrEmpty(erroFechamento))
            {
                erros.Add("txtData", erroFechamento);
            }

            if (erros.Count == 0)
            {
                int? pIDHorarioDinamico = null;
                ValidaHorarioDinamico(ref pTipoTurno, ref pIDHorario, pData, cicloSequenciaIndice, ref pIDHorarioDinamico);
                dalMudancaHorario.MudarHorario(pTipoMudanca, pIDFuncao, pIDEmpresa, pIDDepartamento, pIdFuncionario, pTipoTurno, pIDHorario, pData.Value, pIDHorarioDinamico, cicloSequenciaIndice);

            }

            return erros;
        }

        public void MudarHorarioWeb(int pTipoMudanca, int pIdFuncao, int pIDEmpresa, int pIDDepartamento, int pIdFuncionario, int pTipoTurno, int pIDHorario, DateTime? pData, int? cicloSequenciaIndice)
        {
            string erroFechamento = ValidaFechamentoPonto(pTipoMudanca, pIdFuncao, pIDEmpresa, pIDDepartamento, pIdFuncionario, pData);
            if (!String.IsNullOrEmpty(erroFechamento))
            {
                throw new Exception(" Fechamento de Ponto <br/> " + erroFechamento);
            }

            int? pIDHorarioDinamico = null;
            ValidaHorarioDinamico(ref pTipoTurno, ref pIDHorario, pData, cicloSequenciaIndice, ref pIDHorarioDinamico);

            dalMudancaHorario.MudarHorario(pTipoMudanca, pIdFuncao, pIDEmpresa, pIDDepartamento, pIdFuncionario, pTipoTurno, pIDHorario, pData.Value, pIDHorarioDinamico, cicloSequenciaIndice);
        }

        public void ValidaHorarioDinamico(ref int pTipoTurno, ref int pIDHorario, DateTime? pData, int? cicloSequenciaIndice, ref int? pIDHorarioDinamico)
        {
            if (pTipoTurno == 3)
            {
                pIDHorarioDinamico = pIDHorario;
                BLL.HorarioDinamico bllHorarioDinamico = new HorarioDinamico(ConnectionString, UsuarioLogado);
                pIDHorario = bllHorarioDinamico.GerarHorario(pIDHorario, pData.GetValueOrDefault(), cicloSequenciaIndice.GetValueOrDefault());
                pTipoTurno = 2;
            }
        }

        public string ValidaFechamentoPonto(int pTipoMudanca, int pIdFuncao, int pIDEmpresa, int pIDDepartamento, int pIdFuncionario, DateTime? pData)
        {
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            int tipoFechamento = 0;
            int idTipo = 0;
            switch (pTipoMudanca)
            {
                case 0: // Funcionario
                    tipoFechamento = 2;
                    idTipo = pIdFuncionario;
                    break;
                case 1: // Departamento
                    tipoFechamento = 1;
                    idTipo = pIDDepartamento;
                    break;
                case 2: // Empresa
                    tipoFechamento = 0;
                    idTipo = pIDEmpresa;
                    break;
                case 3: // Função
                    tipoFechamento = 3;
                    idTipo = pIdFuncao;
                    break;
                default: // Todos
                    tipoFechamento = 4;
                    break;
            }
            return bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(tipoFechamento, new List<int>() { idTipo }, pData.GetValueOrDefault());
        }

        public bool ExcluiMudanca(int pIdMudancaHorario, Modelo.ProgressBar objProgressBar)
        {
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            if (pIdMudancaHorario > 0)
            {
                Modelo.MudancaHorario objMudancaHorario = dalMudancaHorario.LoadObject(pIdMudancaHorario);
                int idfuncionario = objMudancaHorario.Idfuncionario;
                DateTime data = objMudancaHorario.Data.Value;

                DateTime? ultimaMudanca = dalMudancaHorario.GetUltimaMudanca(idfuncionario);
                bool ret = false;
                if (data >= ultimaMudanca.Value)
                {

                    ret = dalMudancaHorario.ExcluirMudancao(objMudancaHorario);
                    DateTime? dataFinal = bllMarcacao.GetUltimaDataFuncionario(idfuncionario);
                    List<Modelo.Marcacao> marcacoes = null;
                    if (dataFinal != null)
                    {
                        marcacoes = bllMarcacao.GetPorFuncionario(idfuncionario, data, dataFinal.Value, true);
                        BLL.CalculaMarcacao bllCalcula = new CalculaMarcacao(2, idfuncionario, data, dataFinal.Value, objProgressBar, false, ConnectionString, UsuarioLogado, false);
                        bllCalcula.CalculaMarcacoes();
                    }
                }
                return ret;

            }
            return false;
        }

        public bool ExcluiMudancaWeb(Modelo.MudancaHorario objMudancaHorario)
        {
            if (objMudancaHorario != null && objMudancaHorario.Id > 0)
            {
                int idfuncionario = objMudancaHorario.Idfuncionario;
                DateTime data = objMudancaHorario.Data.Value;

                DateTime? ultimaMudanca = dalMudancaHorario.GetUltimaMudanca(idfuncionario);
                bool ret = false;
                if (data >= ultimaMudanca.Value)
                {

                    ret = dalMudancaHorario.ExcluirMudancao(objMudancaHorario);
                }
                return ret;

            }
            return false;
        }

        /// <summary>
        /// Verifica se existe uma mudança de horário para um funcionario em uma data
        /// </summary>
        /// <param name="pIdFuncionario">id do funcionario</param>
        /// <param name="pData">data do funcionario</param>
        /// <returns>verdadeiro caso exista, falso caso contrário</returns>
        public bool VerificaExiste(int pIdFuncionario, DateTime pData)
        {
            return dalMudancaHorario.VerificaExiste(pIdFuncionario, pData);
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
            return dalMudancaHorario.getId(pValor, pCampo, pValor2);
        }   
  
        public List<Modelo.MudancaHorario> GetAllFuncionarioList(int pIdFuncionario)
        {
            return dalMudancaHorario.GetAllFuncionarioList(pIdFuncionario);
        }
        public List<Modelo.MudancaHorario> GetAllList()
        {
            return dalMudancaHorario.GetAllList();
        }

        public Hashtable GetHashTableByList(DateTime DataInicial, DateTime DataFinal, List<Modelo.Funcionario> ListaFuncionario)
        {

            var dict = ListaFuncionario.GroupBy(x => x.Idhorario).Select(x => x.First()).ToDictionary(x => x.Idhorario, x=> default(int?));
            List<int> idsFuncs = ListaFuncionario.Select(x => x.Id).ToList<int>();

            var hTlistaIdsHorario = new Hashtable(dict);

            var listaMudancaHorario =  this.GetPeriodo(DataInicial, DataFinal, idsFuncs);

            foreach (var m in listaMudancaHorario)
            {
                if (!hTlistaIdsHorario.ContainsKey(m.Idhorario))
                    hTlistaIdsHorario.Add(m.Idhorario, null);
                if (!hTlistaIdsHorario.ContainsKey(m.Idhorario_ant))
                    hTlistaIdsHorario.Add(m.Idhorario_ant, null);
            }

            return hTlistaIdsHorario;
        }
    }
}
