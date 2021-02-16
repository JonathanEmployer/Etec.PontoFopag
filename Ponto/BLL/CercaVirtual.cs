using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using Veridis.Biometric;

namespace BLL
{
    public class CercaVirtual : IBLL<Modelo.CercaVirtual>
    {
        DAL.ICercaVirtual dalCercaVirtual;
        private readonly string ConnectionString;
        private readonly Modelo.Cw_Usuario UsuarioLogado;

        public CercaVirtual()
            : this(null)
        {

        }

        public CercaVirtual(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public CercaVirtual(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalCercaVirtual = new DAL.SQL.CercaVirtual(new DataBase(ConnectionString));

            UsuarioLogado = usuarioLogado;
            dalCercaVirtual.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalCercaVirtual.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalCercaVirtual.GetAll();
        }

        public List<Modelo.CercaVirtual> GetAllList(int Codigo)
        {
            return dalCercaVirtual.GetAllList(Codigo);
        }

        public Modelo.CercaVirtual LoadObject(int id)
        {
            return dalCercaVirtual.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.CercaVirtual objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            //if (objeto.Codigo == 0)
            //{
            //    ret.Add("txtCodigo", "Campo obrigatório.");
            //}

            return ret;
        }

        public void Excluir(int CodigoCercaVirtual, int CodigoFuncionario)
        {
            dalCercaVirtual.Excluir(CodigoCercaVirtual, CodigoFuncionario);
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.CercaVirtual objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        objeto.Codigo = dalCercaVirtual.MaxCodigo();
                        dalCercaVirtual.Incluir(objeto);
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalCercaVirtual.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalCercaVirtual.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public void Salvar(Modelo.CercaVirtual CercaVirtual)
        {
            dalCercaVirtual.Adicionar(CercaVirtual, false);
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalCercaVirtual.getId(pValor, pCampo, pValor2);
        }
    }
}
