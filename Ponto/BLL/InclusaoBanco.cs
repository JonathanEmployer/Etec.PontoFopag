using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class InclusaoBanco : IBLL<Modelo.InclusaoBanco>
    {
        DAL.IInclusaoBanco dalInclusaoBanco;

        private Modelo.ProgressBar objProgressBar;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public InclusaoBanco() : this(null)
        {
            
        }

        public InclusaoBanco(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public InclusaoBanco(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            
            dalInclusaoBanco = new DAL.SQL.InclusaoBanco(new DataBase(ConnectionString));


            dalInclusaoBanco.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalInclusaoBanco.MaxCodigo();
        }

        public int getCreditoPeriodoAcumuladoMes(int idFuncionario, DateTime data)
        {
            DateTime dataInicio = new DateTime(data.StartOfMonth().Year, data.StartOfMonth().Month, data.StartOfMonth().Day);
            DateTime dataFim = new DateTime(data.Year, data.Month, data.Day);
            dataFim = dataFim.AddDays(-1);

            return dalInclusaoBanco.getCreditoPeriodoAcumuladoMes(idFuncionario, dataInicio, dataFim);
        }

        public int getCreditoPeriodoAcumuladoMesPDia(int idFuncionario, DateTime data, int diaInt)
        {
            DateTime dataInicio = new DateTime(data.StartOfMonth().Year, data.StartOfMonth().Month, data.StartOfMonth().Day);
            DateTime dataFim = new DateTime(data.Year, data.Month, data.Day);
            dataFim = dataFim.AddDays(-1);

            return dalInclusaoBanco.getCreditoPeriodoAcumuladoMesPDia(idFuncionario, dataInicio, dataFim, diaInt);
        }

        public int getCreditoPeriodoAtual(int idFuncionario, DateTime dataInicio, DateTime dataFim)
        {
            return dalInclusaoBanco.getCreditoPeriodoAtual(idFuncionario, dataInicio, dataFim);
        }

        public DataTable GetAll()
        {
            return dalInclusaoBanco.GetAll();
        }

        public List<Modelo.InclusaoBanco> GetAllList()
        {
            return dalInclusaoBanco.GetAllList();
        }

        public Modelo.InclusaoBanco LoadObject(int id)
        {
            return dalInclusaoBanco.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.InclusaoBanco objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (objeto.Tipo == -1)
            {
                ret.Add("rgTipo", "Campo obrigatório.");
            }

            if (objeto.Identificacao == 0)
            {
                ret.Add("cbIdentificacao", "Campo obrigatório.");
            }

            if (objeto.Data == null)
            {
                ret.Add("txtData", "Campo obrigatório.");
            }

            if (objeto.Tipocreditodebito == -1)
            {
                ret.Add("rgTipocreditodebito", "Campo obrigatório.");
            }
            else if (objeto.Tipocreditodebito == 0 && String.IsNullOrEmpty(objeto.Credito))
            {
                ret.Add("txtCredito", "Campo obrigatório.");
            }
            else if (objeto.Tipocreditodebito == 1 && String.IsNullOrEmpty(objeto.Debito))
            {
                ret.Add("txtDebito", "Campo obrigatório.");
            }

            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(objeto.Tipo, new List<int>() { objeto.Identificacao }, objeto.Data.GetValueOrDefault());
            if (!String.IsNullOrEmpty(mensagemFechamento))
            {
                ret.Add("Fechamento Ponto", mensagemFechamento);
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.InclusaoBanco objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalInclusaoBanco.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalInclusaoBanco.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalInclusaoBanco.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public void getSaldo(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao, out int credito, out int debito)
        {
            dalInclusaoBanco.getSaldo(pData, pEmpresa, pDepartamento, pFuncionario, pFuncao, out credito, out debito);
        }

        /// <summary>
        /// Busca o saldo das inclusões em banco de horas em uma lista
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pEmpresa"></param>
        /// <param name="pDepartamento"></param>
        /// <param name="pFuncionario"></param>
        /// <param name="pFuncao"></param>
        /// <param name="credito"></param>
        /// <param name="debito"></param>
        /// <param name="pInclusaoBancoLista"></param>
        /// WNO - 26/01/2010
        public void getSaldo(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao, out int credito, out int debito, List<Modelo.InclusaoBanco> pInclusaoBancoLista)
        {
            int cre = 0, deb = 0;

            var lista = pInclusaoBancoLista.Where(i => i.Data == pData && i.Fechado == 0);

            if (lista.Count() > 0)
            {
                string[] horastr;
                int hora, minuto;
                foreach (Modelo.InclusaoBanco ib in lista.Where(i => (i.Tipo == 0 && i.Identificacao == pEmpresa)
                                                                  || (i.Tipo == 1 && i.Identificacao == pDepartamento)
                                                                  || (i.Tipo == 2 && i.Identificacao == pFuncionario)
                                                                  || (i.Tipo == 3 && i.Identificacao == pFuncao)))
                {
                    if (ib.Credito[0] != '-')
                    {
                        horastr = ib.Credito.Split(':');
                        hora = Convert.ToInt32(horastr[0]);
                        minuto = Convert.ToInt32(horastr[1]);
                        cre = cre + ((hora * 60) + minuto);
                    }

                    if (ib.Debito[0] != '-')
                    {
                        horastr = ib.Debito.Split(':');
                        hora = Convert.ToInt32(horastr[0]);
                        minuto = Convert.ToInt32(horastr[1]);
                        deb = deb + ((hora * 60) + minuto);
                    }
                }
            }

            credito = cre;
            debito = deb;
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
            return dalInclusaoBanco.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Retorna a lista de Inclusões de banco dos funcionários solicitados
        /// </summary>
        /// <param name="idsFuncs">Ids dos funcionários que seja carregar as inclusões</param>
        /// <returns></returns>
        public List<Modelo.InclusaoBanco> GetAllListByFuncionarios(List<int> idsFuncs)
        {
            return dalInclusaoBanco.GetAllListByFuncionarios(idsFuncs);
        }
    }
}
