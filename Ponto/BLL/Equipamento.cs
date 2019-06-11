using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL.SQL;

namespace BLL
{
    public class Equipamento : IBLL<Modelo.Equipamento>
    {
        DAL.IEquipamento dalEquipamento;
        private string ConnectionString;
        public Equipamento() : this(null)
        {

        }

        public Equipamento(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalEquipamento = new DAL.SQL.Equipamento(new DataBase(ConnectionString));
            dalEquipamento.UsuarioLogado = cwkControleUsuario.Facade.getUsuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEquipamento.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEquipamento.GetAll();
        }

        public DataTable GetEquipamentoAtivo()
        {
            return dalEquipamento.GetEquipamentoAtivo();
        }

        public Modelo.Equipamento LoadObject(int id)
        {
            return dalEquipamento.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Equipamento objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Valor tem que ser diferente de zero (0).");
            }

            if (objeto.Descricao == null)
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            if (objeto.MensagemPadrao == null)
            {
                ret.Add("txtMensagemPadrao", "Campo obrigatório.");
            }

            if (objeto.NumInner == 0)
            {
                ret.Add("txtNumInner", "Valor tem que ser diferente de zero (0).");
            }

            if (objeto.TipoCartao == -1)
            {
                ret.Add("rgTipoCartao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Equipamento objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);


            if (dalEquipamento.BuscaInner(objeto.NumInner, objeto.Id))
            {
                erros.Add("txtNumInner", "Esse campo é único no banco de dados e já existe um registro com esse valor. Favor escolher outro INNER.");
                return erros;
            }


            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEquipamento.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalEquipamento.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEquipamento.Excluir(objeto);
                        break;
                    default:
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalEquipamento.getId(pValor, pCampo, pValor2);
        }
    }
}
