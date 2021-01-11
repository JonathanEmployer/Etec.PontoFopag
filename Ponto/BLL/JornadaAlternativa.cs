using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class JornadaAlternativa : IBLL<Modelo.JornadaAlternativa>
    {
        DAL.IJornadaAlternativa dalJornadaAlternativa;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public JornadaAlternativa() : this(null)
        {
            
        }

        public JornadaAlternativa(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public JornadaAlternativa(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;                  
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            
            dalJornadaAlternativa = new DAL.SQL.JornadaAlternativa(new DataBase(ConnectionString));
            dalJornadaAlternativa.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalJornadaAlternativa.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalJornadaAlternativa.GetAll();
        }

        public List<Modelo.JornadaAlternativa> GetAllList(bool loadDiasJA)
        {
            return dalJornadaAlternativa.GetAllList(loadDiasJA);
        }

        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF, int? pTipo, List<int> pIdentificacoes)
        {
            return dalJornadaAlternativa.GetHashIdObjeto(pDataI, pDataF, pTipo, pIdentificacoes);
        }

        public Modelo.JornadaAlternativa LoadObject(int id)
        {
            return dalJornadaAlternativa.LoadObject(id);
        }

        public Modelo.JornadaAlternativa LoadParaUmaMarcacao(DateTime pData, int tipo, int identificacao)
        {
            return dalJornadaAlternativa.LoadParaUmaMarcacao(pData, tipo, identificacao);
        }

        /// <summary>
        /// Caso o registro exista retorna true, caso contrário false
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="tipo"></param>
        /// <param name="identificacao"></param>
        /// <returns></returns>
        public bool VerificaExiste(int pId, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao)
        {
            return (dalJornadaAlternativa.VerificaExiste(pId, dataInicial, dataFinal, tipo, identificacao) > 0);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.JornadaAlternativa objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (objeto.DataInicial != null && objeto.DataFinal != null)
            {
                if (objeto.DataInicial > objeto.DataFinal)
                {
                    ret.Add("txtDataFinal", "A data final deve ser maior ou igual à data inicial.");
                }
            }
            else
            {
                if (objeto.DataInicial == null)
                {
                    ret.Add("txtDataInicial", "Campo obrigatório.");
                }

                if (objeto.DataFinal == null)
                {
                    ret.Add("txtDataFinal", "Campo obrigatório.");
                }
            }

            if (objeto.Tipo == -1)
            {
                ret.Add("rgTipo", "Campo obrigatório.");
            }

            //if (objeto.Identificacao == 0)
            //{
            //    ret.Add("cbIdIdentificacao", "Campo obrigatório");
            //}
            if (String.IsNullOrEmpty(objeto.IdsJornadaAlternativaFuncionarios))
            {
                ret.Add("cbIdIdentificacao", "Campo obrigatório");
            }

            if (objeto.LimiteMin == "--:--")
            {
                ret.Add("txtLimiteMin", "Campo obrigatório");
            }
            if (objeto.LimiteMax == "--:--")
            {
                ret.Add("txtLimiteMax", "Campo obrigatório");
            }

            if ((objeto.DataInicial != null) && (objeto.DataFinal != null))
            {
                if (VerificaExiste(objeto.Id, objeto.DataInicial.Value, objeto.DataFinal.Value, objeto.Tipo, objeto.Identificacao))
                {
                    ret.Add("cbIdIdentificacao", "Já existe um registro gravado dentro deste período.");
                }
            }

            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(objeto.Tipo, new List<int>() {objeto.Identificacao}, objeto.DataInicial.GetValueOrDefault());
            if (!String.IsNullOrEmpty(mensagemFechamento))
            {
                ret.Add("Fechamento Ponto", mensagemFechamento);    
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.JornadaAlternativa pObjJornadaAlternativa)
        {
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            DiasJornadaAlternativa bllDiasJA = new BLL.DiasJornadaAlternativa(ConnectionString, UsuarioLogado);
            Dictionary<string, string> erros = ValidaObjeto(pObjJornadaAlternativa);
            if (erros.Count == 0)
            {
                var idsFuncionarios = pObjJornadaAlternativa.IdsJornadaAlternativaFuncionarios.Split(',').ToList();
                foreach (var idFuncionario in idsFuncionarios)
                {
                    pObjJornadaAlternativa.Identificacao = Convert.ToInt32(idFuncionario);
                    switch (pAcao)
                    {
                        case Modelo.Acao.Incluir:
                            dalJornadaAlternativa.Incluir(pObjJornadaAlternativa);
                            bllMarcacao.InsereMarcacoesNaoExistentes(pObjJornadaAlternativa.Tipo, pObjJornadaAlternativa.Identificacao, pObjJornadaAlternativa.DataInicial.Value, pObjJornadaAlternativa.DataFinal.Value, objProgressBar, false);
                            break;
                        case Modelo.Acao.Alterar:
                            dalJornadaAlternativa.Alterar(pObjJornadaAlternativa);
                            break;
                        case Modelo.Acao.Excluir:
                            if (pObjJornadaAlternativa.DiasJA != null)
                            {
                                if (pObjJornadaAlternativa.DiasJA.Count > 0)
                                {
                                    foreach (var item in pObjJornadaAlternativa.DiasJA)
                                    {
                                        bllDiasJA.Salvar(Modelo.Acao.Excluir, item);
                                    }
                                }
                            }
                            dalJornadaAlternativa.Excluir(pObjJornadaAlternativa);
                            break;
                    }
                }
            }
            return erros;
        }

        public List<Modelo.JornadaAlternativa> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal)
        {
            return dalJornadaAlternativa.GetPeriodo(pDataInicial, pDataFinal);
        }

        public Modelo.JornadaAlternativa PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao)
        {
            return dalJornadaAlternativa.PossuiRegistro(pData, pEmpresa, pDepartamento, pFuncionario, pFuncao);
        }

        /// <summary>
        /// Verifica se existe uma jornada alternativa em uma lista
        /// </summary>
        /// <param name="pJornadasAlternativas">lista com as jornadas alternativas</param>
        /// <param name="pData">data para verificar</param>        
        /// <returns>caso exista retorna a jornada alternativa, caso contrário retorna null</returns>
        /// WNO - 05/01/2010
        public Modelo.JornadaAlternativa PossuiRegistro(List<Modelo.JornadaAlternativa> pJornadasAlternativas, DateTime pData, int pIdFuncionario, int pIdFuncao, int pIdDepartamento, int pIdEmpresa)
        {
            if (pJornadasAlternativas.Count > 0)
            {
                var aux = pJornadasAlternativas.Where(j => (pData >= j.DataInicial && pData <= j.DataFinal) || j.DiasJA.Exists(d => d.DataCompensada == pData));

                if (aux.Count() > 0)
                {
                    //Verifica se possui registro por Funcionario
                    var jornada = aux.Where(j => j.Tipo == 2 && j.Identificacao == pIdFuncionario);
                    if (jornada.Count() > 0)
                    {
                        return jornada.Last();
                    }

                    //Verifica se possui registro por Funcao
                    jornada = aux.Where(j => j.Tipo == 3 && j.Identificacao == pIdFuncao);
                    if (jornada.Count() > 0)
                    {
                        return jornada.Last();
                    }

                    //Verifica se possui registro por Departamento
                    jornada = aux.Where(j => j.Tipo == 1 && j.Identificacao == pIdDepartamento);
                    if (jornada.Count() > 0)
                    {
                        return jornada.Last();
                    }

                    //Verifica se possui registro por Empresa
                    jornada = aux.Where(j => j.Tipo == 0 && j.Identificacao == pIdEmpresa);
                    if (jornada.Count() > 0)
                    {
                        return jornada.Last();
                    }
                }
            }
            
            return null;
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
            return dalJornadaAlternativa.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.JornadaAlternativa> GetPeriodoFuncionarios(DateTime pDataInicial, DateTime pDataFinal, List<int> idsFuncs)
        {
            return dalJornadaAlternativa.GetPeriodoFuncionarios(pDataInicial, pDataFinal, idsFuncs);
        }
    }
}
