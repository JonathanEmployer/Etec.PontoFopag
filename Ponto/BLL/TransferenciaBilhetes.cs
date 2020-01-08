using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class TransferenciaBilhetes : IBLL<Modelo.TransferenciaBilhetes>
    {
        DAL.ITransferenciaBilhetes dalTransferenciaBilhetes;
        DAL.ITransferenciaBilhetesDetalhes dalTransferenciaBilhetesDetalhes;
        private string ConnectionString;

        public TransferenciaBilhetes() : this(null)
        {
            
        }

        public TransferenciaBilhetes(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public TransferenciaBilhetes(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalTransferenciaBilhetes = new DAL.SQL.TransferenciaBilhetes(new DataBase(ConnectionString));
            dalTransferenciaBilhetesDetalhes = new DAL.SQL.TransferenciaBilhetesDetalhes(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalTransferenciaBilhetes = new DAL.SQL.TransferenciaBilhetes(new DataBase(ConnectionString));
                    break;
            }
            dalTransferenciaBilhetes.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalTransferenciaBilhetes.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalTransferenciaBilhetes.GetAll();
        }

        public Modelo.TransferenciaBilhetes LoadObject(int id)
        {
            return dalTransferenciaBilhetes.LoadObject(id);
        }

        public List<Modelo.TransferenciaBilhetes> GetAllList()
        {
            return dalTransferenciaBilhetes.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.TransferenciaBilhetes objeto)
        {
            List<Tuple<string, string>> listErro = new List<Tuple<string, string>>();
            if (objeto.Codigo == 0)
            {
                listErro.Add(new Tuple<string, string>(nameof(objeto.Codigo), "Campo obrigatório."));
            }

            if (objeto.IdFuncionarioOrigem == 0)
            {
                listErro.Add(new Tuple<string, string>(nameof(objeto.IdFuncionarioOrigem), "Funcionário de origem não informado."));
            }

            if (objeto.IdFuncionarioDestino == 0)
            {
                listErro.Add(new Tuple<string, string>(nameof(objeto.IdFuncionarioDestino), "Funcionário de destino não informado."));
            }

            if (objeto.DataInicio == null || objeto.DataFim == null || objeto.DataInicio > objeto.DataFim)
            {
                listErro.Add(new Tuple<string, string>(nameof(objeto.DataInicio), "A data de início e fim devem sem informadas e a data início deve ser menor que a fim."));
            }
            else
            {
                // O Fechamento é validado o dia solicitado menos 1, pois o registro movimentado pode estar locado ou ser alocado no dia anterior.
                BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new FechamentoPontoFuncionario(ConnectionString, dalTransferenciaBilhetes.UsuarioLogado);
                string msgFechamentoOrigem = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(2, new List<int>() { objeto.IdFuncionarioOrigem }, objeto.DataInicio.GetValueOrDefault().AddDays(-1));
                if (!String.IsNullOrEmpty(msgFechamentoOrigem))
                    listErro.Add(new Tuple<string, string>(nameof(objeto.IdFuncionarioOrigem), msgFechamentoOrigem));

                string msgFechamentoDestino = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(2, new List<int>() { objeto.IdFuncionarioOrigem }, objeto.DataInicio.GetValueOrDefault().AddDays(-1));
                if (!String.IsNullOrEmpty(msgFechamentoDestino))
                    listErro.Add(new Tuple<string, string>(nameof(objeto.IdFuncionarioDestino), msgFechamentoDestino));
            }

            if (listErro.Count == 0)
            {
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(ConnectionString, dalTransferenciaBilhetes.UsuarioLogado);
                List<Modelo.BilhetesImp> bilhetesTransferir = bllBilhetesImp.GetImportadosPeriodo(new List<int>() { objeto.IdFuncionarioOrigem }, objeto.DataInicio.GetValueOrDefault(), objeto.DataFim.GetValueOrDefault(), true);
                if (bilhetesTransferir != null)
                {
                    bilhetesTransferir = bilhetesTransferir.Where(w => w.Relogio != "PA").ToList();
                }
                if (bilhetesTransferir == null || bilhetesTransferir.Count == 0)
                {
                    listErro.Add(new Tuple<string, string>(nameof(objeto.IdFuncionarioOrigem), "O período informado não possui registros a serem transferidos."));
                }
                else
                {
                    objeto.TransferenciaBilhetesDetalhes = objeto.TransferenciaBilhetesDetalhes ?? new List<Modelo.TransferenciaBilhetesDetalhes>();
                    bilhetesTransferir.ForEach(s => objeto.TransferenciaBilhetesDetalhes.Add(new Modelo.TransferenciaBilhetesDetalhes() { IdBilhetesImp = s.Id, BilhetesImp = s }));
                } 
            }

            if (listErro.Count == 0)
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(ConnectionString, dalTransferenciaBilhetes.UsuarioLogado);
                Modelo.Funcionario origem = bllFuncionario.LoadObject(objeto.IdFuncionarioOrigem);
                Modelo.Funcionario destino = bllFuncionario.LoadObject(objeto.IdFuncionarioDestino);

                if (origem.CPF != destino.CPF)
                {
                    listErro.Add(new Tuple<string, string>(nameof(objeto.IdFuncionarioOrigem), "O funcionário de origem não corresponde ao funcionário de destino"));
                }
            }
            Dictionary<string, string> ret = new Dictionary<string, string>();
            listErro.GroupBy(g => g.Item1).ToList().ForEach(f => ret.Add(f.Key, String.Join(" ", f.Select(s => s.Item2))));
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.TransferenciaBilhetes objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalTransferenciaBilhetes.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalTransferenciaBilhetes.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalTransferenciaBilhetes.Excluir(objeto);
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
            return dalTransferenciaBilhetes.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.Proxy.PxyGridTransferenciaBilhetes> GetAllListGrid()
        {
            return dalTransferenciaBilhetes.GetAllListGrid();
        }

        public void CarregaTransferenciaBilhetesDetalhes(ref Modelo.TransferenciaBilhetes objeto)
        {
            objeto.TransferenciaBilhetesDetalhes = dalTransferenciaBilhetesDetalhes.GetAllListByTransferenciaBilhetes(objeto.Id);
        }

        public void TransferirBilhetes(int idTransferenciaBilhetes)
        {
            dalTransferenciaBilhetes.TransferirBilhetes(idTransferenciaBilhetes);
        }

        public int CountDetalhes(int idTransferenciaBilhetesDetalhes)
        {
            return dalTransferenciaBilhetes.CountCampo("TransferenciaBilhetesDetalhes", "IdTransferenciaBilhetes", idTransferenciaBilhetesDetalhes, 0);
        }
    }
}
