using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using DAL.SQL;

namespace BLL
{
    public class Backup : IBLL<Modelo.Backup>
    {
        DAL.IBackup dalBackup;
        private string ConnectionString;

        public Backup() : this(null)
        {

        }

        #region Singleton

        public Backup(string connString)
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
                    dalBackup = new DAL.SQL.Backup(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalBackup = DAL.FB.Backup.GetInstancia;
                    break;
            }
        }

        #endregion

        public int MaxCodigo()
        {
            return dalBackup.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalBackup.GetAll();
        }

        public List<Modelo.Backup> GetAllList()
        {
            return dalBackup.GetAllList();
        }

        public Modelo.Backup LoadObject(int id)
        {
            return dalBackup.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Backup objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (String.IsNullOrEmpty(objeto.Diretorio))
            {
                ret.Add("txtDiretorio", "Campo obrigatório.");
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(objeto.Diretorio);
                if (!dir.Exists && !objeto.Diretorio.Contains("\\"))
                {
                    ret.Add("txtDiretorio", "Diretório inválido.");
                }
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Backup objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalBackup.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalBackup.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalBackup.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalBackup.getId(pValor, pCampo, pValor2);
        }
    }

}
