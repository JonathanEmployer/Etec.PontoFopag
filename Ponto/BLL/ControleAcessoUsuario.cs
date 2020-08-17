using DAL.SQL;
using Modelo;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class ControleAcessoUsuario : IcwkBLL<Modelo.UsuarioControleAcesso>
    {
        DAL.IContrato dalContrato;
        DAL.IEmpresa dalEmpresa;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private Modelo.ProgressBar objProgressBar;


        public ControleAcessoUsuario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            dalContrato = new DAL.SQL.Contrato(new DataBase(ConnectionString));
            dalEmpresa = new DAL.SQL.Empresa(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
        }

        public List<UsuarioControleAcesso> GetListaControleAcesso(int pIdUsuario)
        {
            List<UsuarioControleAcesso> Acessos = new List<UsuarioControleAcesso>();

            var EmpresasList = dalEmpresa.GetEmpresasUsuarioId(pIdUsuario);
            var ContratosList = dalContrato.ContratosPorUsuario(pIdUsuario);

            if (EmpresasList.Count != 0)
            {
                foreach (var empresa in EmpresasList)
                {
                    UsuarioControleAcesso Acesso = new UsuarioControleAcesso()
                    {
                        Id = empresa.Id,
                        Codigo = empresa.Codigo,
                        Descricao = empresa.Nome,
                        Tipo = "Empresa"
                    };
                    Acessos.Add(Acesso);
                }
            }

            if (ContratosList.Count != 0)
            {
                foreach (var contrato in ContratosList)
                {
                    UsuarioControleAcesso Acesso = new UsuarioControleAcesso()
                    {
                        Id = contrato.Id,
                        Codigo = contrato.Codigo,
                        Descricao = contrato.CodigoContrato + " | " + contrato.DescricaoContrato,
                        Tipo = "Contrato"
                    };
                    Acessos.Add(Acesso);
                }
            }

            return Acessos;
        }

        public DataTable GetAll()
        {
            throw new NotImplementedException();
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            throw new NotImplementedException();
        }

        public UsuarioControleAcesso LoadObject(int id)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> Salvar(cwkAcao pAcao, UsuarioControleAcesso objeto)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> ValidaObjeto(UsuarioControleAcesso objeto)
        {
            throw new NotImplementedException();
        }
    }
}
