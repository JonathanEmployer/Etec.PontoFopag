using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BLL
{
    public class Alertas : IBLL<Modelo.Alertas>
    {
        DAL.IAlertas dalAlertas;
        private string ConnectionString;

        public Alertas() : this(null)
        {
            
        }

        public Alertas(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Alertas(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalAlertas = new DAL.SQL.Alertas(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalAlertas = new DAL.SQL.Alertas(new DataBase(ConnectionString));
                    break;
            }
            dalAlertas.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalAlertas.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalAlertas.GetAll();
        }

        public Modelo.Alertas LoadObject(int id)
        {
            return dalAlertas.LoadObject(id);
        }

        public List<Modelo.Alertas> GetAllList()
        {
            return dalAlertas.GetAllList();
        }

        #region validacoes
        public Dictionary<string, string> ValidaObjeto(Modelo.Alertas objeto, Modelo.Acao acao)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (acao != Modelo.Acao.Excluir)
            {
                if (objeto.Codigo == 0)
                {
                    ret.Add("Codigo", "Campo obrigatório.");
                }

                objeto.IdFuncsSelecionados = String.IsNullOrEmpty(objeto.IdFuncsSelecionados) ? "" : objeto.IdFuncsSelecionados;
                objeto.IdFuncsSelecionados_Ant = String.IsNullOrEmpty(objeto.IdFuncsSelecionados_Ant) ? "" : objeto.IdFuncsSelecionados_Ant;

                ValidaUltimaExecucao(objeto, ret);
                ValidaPessoaSupervisor(objeto);
                ValidarDestinatario(objeto, ref ret);
                if (objeto.ProcedureAlerta != "p_enviaAlertasAcompanhamentoRep")
                {
                    ValidarMonitorados(objeto, ref ret); 
                }

                if (objeto.ProcedureAlerta == "p_enviaAlertasAcompanhamentoRep")
                {
                    if (String.IsNullOrEmpty(objeto.IdRepsSelecionados))
                    {
                        ret.Add("IdRepsSelecionados", "Para enviar o alerta é necessário selecionar o rep a ser acompanhado.");
                    }
                }
            }
            return ret;
        }

        private static void ValidaUltimaExecucao(Modelo.Alertas objeto, Dictionary<string, string> ret)
        {
            DateTime dataMinLimite = DateTime.Now.AddDays(-31);
            DateTime dataMaxLimite = DateTime.Now;
            if (objeto.UltimaExecucao <= dataMinLimite)
            {
                ret.Add("UltimaExecucao", "A última execução do alerta não pode ser alterada para uma data menor " + dataMinLimite.ToString("t"));
            }
            else if (objeto.UltimaExecucao > dataMaxLimite)
            {
                ret.Add("UltimaExecucao", "A última execução do alerta não pode ser alterada para uma data/hora maior que a hora atual (" + dataMinLimite.ToString("t") + ")");
            }
        }

        private void ValidarMonitorados(Modelo.Alertas obj, ref Dictionary<string, string> erro)
        {
            if ((obj.ObjPessoaSupervisor == null || obj.ObjPessoaSupervisor.Id == 0) && String.IsNullOrEmpty(obj.IdFuncsSelecionados))
            {
                erro.Add("IdFuncsSelecionados", "Para enviar o alerta é necessário informar um supervisor ou selecionar algum funcionário a ser monitorado.");
            }
            else if (obj.ObjPessoaSupervisor != null && obj.ObjPessoaSupervisor.Id > 0)
            {
                BLL.Funcionario bllFunc = new BLL.Funcionario(ConnectionString, dalAlertas.UsuarioLogado);
                List<int> idFuncs = bllFunc.GetIdsFuncsAtivos(" and IdPessoaSupervisor = " + obj.ObjPessoaSupervisor.Id);
                if (idFuncs.Count == 0)
                {
                    erro.Add("Pessoa", "Supervisor informado não possui funcionários vinculados, vincule funcionários ao supervisor ou selecione algum funcionário na tabela de funcionários!");
                }
            }
        }

        private void ValidarDestinatario(Modelo.Alertas obj, ref Dictionary<string, string> erro)
        {
            if (String.IsNullOrEmpty(obj.EmailUsuario) && String.IsNullOrEmpty(obj.Pessoa))
            {
                erro.Add("EmailUsuario", "Para enviar o alerta é necessário informar o e-mail do destinatário ou informar um supervisor para receber o alerta");
            }
            else if (obj.ObjPessoaSupervisor != null && obj.ObjPessoaSupervisor.Id > 0 && String.IsNullOrEmpty(obj.ObjPessoaSupervisor.Email) && String.IsNullOrEmpty(obj.EmailUsuario))
            {
                erro.Add("EmailUsuario", "Supervisor informado não possui e-mail cadastrado, verifique o cadastro!");
            }

            if (!String.IsNullOrEmpty(obj.EmailUsuario))
            {
                string erroDesc = cwkFuncoes.ValidarEmails(obj.EmailUsuario);
                if (!String.IsNullOrEmpty(erroDesc))
                {
                    erro.Add("EmailUsuario", erroDesc);
                }
            }

            if (obj.ObjPessoaSupervisor != null && obj.ObjPessoaSupervisor.Id > 0)
            {
                string erroDesc = cwkFuncoes.ValidarEmails(obj.ObjPessoaSupervisor.Email);
                if (!String.IsNullOrEmpty(erroDesc))
                {
                    erro.Add("Pessoa", erroDesc);
                }
            }
        }

        private void ValidaPessoaSupervisor(Modelo.Alertas alerta)
        {
            BLL.Pessoa bllPessoa = new BLL.Pessoa(ConnectionString, dalAlertas.UsuarioLogado);
            if (!String.IsNullOrEmpty(alerta.Pessoa))
            {
                try
                {
                    int codigo = Convert.ToInt32(alerta.Pessoa.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    alerta.ObjPessoaSupervisor = bllPessoa.GetPessoaPorCodigo(codigo).FirstOrDefault();
                    alerta.IdPessoa = alerta.ObjPessoaSupervisor.Id;
                }
                catch (Exception e)
                {
                    alerta.IdPessoa = null;
                }
            }
            else
            {
                alerta.IdPessoa = null;
            }
        }
        #endregion

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Alertas objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto, pAcao);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalAlertas.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalAlertas.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalAlertas.Excluir(objeto);
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
            return dalAlertas.getId(pValor, pCampo, pValor2);
        }


        public Dictionary<string, string> ValidaObjeto(Modelo.Alertas objeto)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Alertas> GetAllListAcompanhamentoRep()
        {
            return dalAlertas.GetAllListAcompanhamentoRep();
        }
    }
}
