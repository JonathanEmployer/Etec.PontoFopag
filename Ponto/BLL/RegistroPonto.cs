using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class RegistroPonto : IBLL<Modelo.RegistroPonto>
    {
        DAL.IRegistroPonto dalRegistroPonto;
        private string ConnectionString;

        public RegistroPonto() : this(null)
        {
            
        }

        public RegistroPonto(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }
        public RegistroPonto(string connString, string usuario)
            : this(connString, new Modelo.Cw_Usuario { Login = usuario})
        {
        }

        public RegistroPonto(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalRegistroPonto = new DAL.SQL.RegistroPonto(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalRegistroPonto = new DAL.SQL.RegistroPonto(new DataBase(ConnectionString));
                    break;
            }
            dalRegistroPonto.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalRegistroPonto.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalRegistroPonto.GetAll();
        }

        public Modelo.RegistroPonto LoadObject(int id)
        {
            return dalRegistroPonto.LoadObject(id);
        }

        public List<Modelo.RegistroPonto> GetAllList()
        {
            return dalRegistroPonto.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.RegistroPonto objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.RegistroPonto objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalRegistroPonto.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalRegistroPonto.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalRegistroPonto.Excluir(objeto);
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
            return dalRegistroPonto.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Carrega uma lista de Registros ponto de acordo com uma lista de ids e situação do registros
        /// </summary>
        /// <param name="ids">Lista com os ids a serem carregados</param>
        /// <param name="situacoes">Situações dos registros desejados</param>
        /// <returns>Lista de Registros de Ponto</returns>
        public List<Modelo.RegistroPonto> GetAllListByIds(List<int> ids, List<Modelo.Enumeradores.SituacaoRegistroPonto> situacoes)
        {
            return dalRegistroPonto.GetAllListByIds(ids, situacoes);
        }
                /// <summary>
        /// Carrega uma lista de Registros ponto de acordo com uma lista de situações do registros
        /// </summary>
        /// <param name="situacoes">Situações dos registros desejados</param>
        /// <returns>Lista de Registros de Ponto</returns>
        public List<Modelo.RegistroPonto> GetAllListBySituacoes(List<Modelo.Enumeradores.SituacaoRegistroPonto> situacoes)
        {
            return dalRegistroPonto.GetAllListBySituacoes(situacoes);
        }


        public void SetarSituacaoRegistros(List<int> idsRegistros, Modelo.Enumeradores.SituacaoRegistroPonto situacao)
        {
            dalRegistroPonto.SetarSituacaoRegistros(idsRegistros, situacao);
        }

        public void SetarSituacaoRegistrosByLote(List<string> lotes, Modelo.Enumeradores.SituacaoRegistroPonto situacao)
        {
            dalRegistroPonto.SetarSituacaoRegistrosByLote(lotes, situacao);
        }

        public void SetarJobId(List<int> idsRegistros, string jobId)
        {
            dalRegistroPonto.SetarJobId(idsRegistros, jobId);
        }

        public void SetarSituacaoJobIDRegistros(List<int> idsRegistros, Modelo.Enumeradores.SituacaoRegistroPonto situacao, string jobId)
        {
            dalRegistroPonto.SetarSituacaoJobIDRegistros(idsRegistros, situacao, jobId);
        }

        /// <summary>
        /// Carrega uma lista de Registros ponto de acordo com uma lista de ids de funcionários e período
        /// </summary>
        /// <param name="idsFuncs">Lista com os ids dos funcionários a serem carregados</param>
        /// <param name="dataI">Data inicial a ser considerada</param>
        /// <param name="dataF">Data final a ser considerada</param>
        /// <returns>Lista de Registros de Ponto</returns>
        public List<Modelo.RegistroPonto> GetAllListByFuncsData(List<int> idsFuncs, DateTime dataI, DateTime dataF)
        {
            return dalRegistroPonto.GetAllListByFuncsData(idsFuncs, dataI, dataF);
        }

        /// <summary>
        /// Carrega uma lista de Registros ponto de acordo com uma lista de idsIntegracao
        /// </summary>
        /// <param name="idsIntegracao">Lista com os ids a serem carregados</param>
        /// <returns>Lista de Registros de Ponto</returns>
        public List<Modelo.RegistroPonto> GetAllListByIdsIntegracao(List<string> idsIntegracao)
        {
            return dalRegistroPonto.GetAllListByIdsIntegracao(idsIntegracao);
        }

        public void AtualizarRegistros(List<Modelo.RegistroPonto> registros)
        {
            if (registros.Count > 0)
            {
                dalRegistroPonto.AtualizarRegistros(registros); 
            }
        }

        public void InserirRegistros(List<Modelo.RegistroPonto> registros)
        {
            if (registros.Count > 0)
            {
                dalRegistroPonto.InserirRegistros(registros);   
            }
        }

        public bool ProcessarRegistrosIntegrados(Modelo.Proxy.PxyRegistrosPontoIntegrar registrosFuncionarios, string origem, Modelo.UsuarioPontoWeb usuario)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuario.ConnectionString, usuario);
            List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
            funcionarios = bllFuncionario.GetAllFuncsListPorCPF(registrosFuncionarios.Funcionarios.Select(s => s.CPF).ToList());
            if (funcionarios.Count() <= 0)
            {
                registrosFuncionarios.Erro = "Nenhum funcionário foi informado no lote";
                return false;

            }

            foreach (Modelo.Proxy.PxyFuncionarioRP funcionarioReg in registrosFuncionarios.Funcionarios)
            {
                if (!String.IsNullOrEmpty(funcionarioReg.CPF) && !String.IsNullOrEmpty(funcionarioReg.Matricula))
                {
                    Modelo.Funcionario func = funcionarios.Where(w => Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(w.CPF)) == Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(funcionarioReg.CPF)) && w.Matricula == funcionarioReg.Matricula).FirstOrDefault();
                    if (func != null && func.Id > 0)
                    {
                        funcionarioReg.FuncionarioPonto = func;
                    }
                    else
                    {
                        funcionarioReg.Erro = "Funcionário não encontrado";
                    }
                }
            }

            if (registrosFuncionarios.ProcessarApenasSeEncontrarTodosFuncionario && registrosFuncionarios.ProcessarApenasSeEncontrarTodosFuncionario && registrosFuncionarios.Funcionarios.Count() != registrosFuncionarios.Funcionarios.Where(s => String.IsNullOrEmpty(s.Erro)).Count())
            {
                registrosFuncionarios.Erro = "Alguns funcionários não foram encontrados";
                return false;
            }
            else
            {
                BLL.RegistroPonto bllRegistroPonto = new BLL.RegistroPonto(usuario.ConnectionString, usuario);
                List<int> idsFuncs = registrosFuncionarios.Funcionarios.Where(w => String.IsNullOrEmpty(w.Erro)).Select(s => s.FuncionarioPonto).Select(s => s.Id).ToList();
                DateTime dataIni = registrosFuncionarios.Funcionarios.Where(w => String.IsNullOrEmpty(w.Erro)).SelectMany(s => s.RegistrosPonto).Min(s => s.Batida);
                DateTime dataFin = registrosFuncionarios.Funcionarios.Where(w => String.IsNullOrEmpty(w.Erro)).SelectMany(s => s.RegistrosPonto).Max(s => s.Batida);
                List<Modelo.RegistroPonto> registrosExistentes = bllRegistroPonto.GetAllListByFuncsData(idsFuncs, dataIni, dataFin);
                List<string> idsIntegracoes = registrosFuncionarios.Funcionarios.Where(w => String.IsNullOrEmpty(w.Erro)).SelectMany(s => s.RegistrosPonto).Select(s => s.IdIntegracao).ToList();
                List<Modelo.RegistroPonto> registrosExistentesPorIdIntegrador = bllRegistroPonto.GetAllListByIdsIntegracao(idsIntegracoes);
                string loteInc = Guid.NewGuid().ToString();
                string loteAlt = Guid.NewGuid().ToString();
                foreach (Modelo.Proxy.PxyFuncionarioRP funcionarioReg in registrosFuncionarios.Funcionarios.Where(s => String.IsNullOrEmpty(s.Erro)))
                {
                    foreach (Modelo.Proxy.PxyRegistroPonto reg in funcionarioReg.RegistrosPonto)
                    {
                        reg.Batida = reg.Batida.AddSeconds(-reg.Batida.Second);
                        Modelo.RegistroPonto regMesmoIdIntegrador = registrosExistentesPorIdIntegrador.Where(w => w.IdIntegracao == reg.IdIntegracao).FirstOrDefault();
                        if (regMesmoIdIntegrador != null)
                        {
                            regMesmoIdIntegrador.Batida = regMesmoIdIntegrador.Batida.AddSeconds(-regMesmoIdIntegrador.Batida.Second);
                        }
                        Modelo.RegistroPonto regMesmoFuncionario = registrosExistentes.Where(w => w.IdFuncionario == funcionarioReg.FuncionarioPonto.Id && w.Batida.AddSeconds(-w.Batida.Second) == reg.Batida).FirstOrDefault();
                        if (regMesmoFuncionario != null)
                        {
                            regMesmoFuncionario.Batida = regMesmoFuncionario.Batida.AddSeconds(-regMesmoFuncionario.Batida.Second);
                        }
                        if (reg.acao == 0 && (regMesmoIdIntegrador != null && regMesmoIdIntegrador.Id > 0) && (reg.acao == (Int16)regMesmoIdIntegrador.Acao))
                        {
                            reg.Erro = "Registro com o IdIntegracao informado já existe";
                        }
                        else if (reg.acao == 0 && (regMesmoIdIntegrador == null || regMesmoIdIntegrador.Id == 0) && (regMesmoFuncionario != null && regMesmoFuncionario.Id > 0))
                        {
                            reg.Erro = "Registro duplicado, já existe um registro para esse funcionário nesse mesmo horário";
                        }
                        else
                        {
                            if (reg.acao == 2 || (regMesmoIdIntegrador != null && reg.acao != (Int16)regMesmoIdIntegrador.Acao))
                            {
                                // Se não encontrar o registro de inclusão entre os novos registros, ou entre os registros já incluídos anteriormente, não permite realizar a desconsideração
                                if (funcionarioReg.RegistrosPonto.Where(w => w.acao == 0 && w.Batida == reg.Batida).Count() == 0 && (regMesmoIdIntegrador == null || regMesmoIdIntegrador.Id == 0))
                                {
                                    reg.Erro = "Registro de inclusão não encontrado, não é possível desconsiderar um registro que ainda não foi incluído";
                                }
                                else
                                {
                                    if (regMesmoIdIntegrador != null && regMesmoIdIntegrador.Id > 0)
                                    {
                                        // Remove os segundos na comparação, pois o ponto não guarda os segundos
                                        if (reg.Batida == regMesmoIdIntegrador.Batida)
                                        {
                                            if ((Modelo.Acao)reg.acao != regMesmoIdIntegrador.Acao)
                                            {
                                                regMesmoIdIntegrador.Acao = (Modelo.Acao)reg.acao; // Mudo o registro para Desconsiderar
                                                regMesmoIdIntegrador.Situacao = "R"; // Mudo a situação para reprocessar
                                                reg.Registro = regMesmoIdIntegrador;
                                            }
                                        }
                                        else
                                        {
                                            reg.Erro = "Não é possível alterar a Data/Hora do registro já incluído, desconsidere o registro e envie um novo";
                                        }
                                    }
                                    else
                                    {
                                        GerarRegistro(origem, funcionarioReg, reg, "I");
                                    }
                                }
                            }
                            else
                            {
                                GerarRegistro(origem, funcionarioReg, reg, "I");
                            }
                        }
                    }
                }

                List<Modelo.RegistroPonto> registrosSalvar = registrosFuncionarios.Funcionarios.Where(s => String.IsNullOrEmpty(s.Erro)).SelectMany(s => s.RegistrosPonto).Where(w => String.IsNullOrEmpty(w.Erro) && w.Registro != null).Select(s => s.Registro).ToList();
                if ((registrosFuncionarios.ProcessarApenasSeTodosOsRegistrosOK && registrosFuncionarios.Funcionarios.SelectMany(s => s.RegistrosPonto).Where(w => !String.IsNullOrEmpty(w.Erro)).Count() == 0) ||
                    (!registrosFuncionarios.ProcessarApenasSeTodosOsRegistrosOK && registrosSalvar.Count > 0))
                {
                    if (registrosSalvar.Count > 0)
                    {
                        List<Modelo.RegistroPonto> registrosIncluir = registrosSalvar.Where(w => w.Id == 0).ToList();
                        registrosIncluir.ForEach(f => f.Lote = loteInc);
                        try
                        {
                            InserirRegistros(registrosIncluir);
                        }
                        catch (Exception e)
                        {
                            registrosFuncionarios.Funcionarios.SelectMany(s => s.RegistrosPonto).Where(w => registrosIncluir.Select(s => s.IdIntegracao).Contains(w.IdIntegracao)).ToList().ForEach(f => f.Erro = "Erro ao incluir o registro, erro: " + e.Message);
                            throw;
                        }

                        List<Modelo.RegistroPonto> registrosAlterar = registrosSalvar.Where(w => w.Id > 0).ToList();
                        registrosAlterar.ForEach(f => f.Lote = loteAlt);
                        try
                        {
                            AtualizarRegistros(registrosAlterar);
                        }
                        catch (Exception e)
                        {
                            registrosFuncionarios.Funcionarios.SelectMany(s => s.RegistrosPonto).Where(w => registrosAlterar.Select(s => s.IdIntegracao).Contains(w.IdIntegracao)).ToList().ForEach(f => f.Erro = "Erro ao alterar o registro, erro: " + e.Message);
                            throw;
                        }
                    }
                }
            }
            return true;
        }

        private void GerarRegistro(string origem, Modelo.Proxy.PxyFuncionarioRP funcionarioReg, Modelo.Proxy.PxyRegistroPonto reg, string situacao)
        {
            Modelo.RegistroPonto regPonto = new Modelo.RegistroPonto();
            regPonto.Batida = reg.Batida;
            regPonto.IdIntegracao = reg.IdIntegracao;
            regPonto.OrigemRegistro = origem;
            regPonto.Situacao = situacao;
            regPonto.IdFuncionario = funcionarioReg.FuncionarioPonto.Id;
            regPonto.Funcionario = funcionarioReg.FuncionarioPonto;
            reg.Registro = regPonto;
        }

        public Hashtable GetHashPorPISPeriodo(DateTime pDataI, DateTime pDataF, List<string> lPis)
        {
            return dalRegistroPonto.GetHashPorPISPeriodo(pDataI, pDataF, lPis);
        }

        public Modelo.RegistroPonto GetUltimoRegistroByOrigem(string origemRegistro)
        {
            return dalRegistroPonto.GetUltimoRegistroByOrigem(origemRegistro);
        }
    }
}
