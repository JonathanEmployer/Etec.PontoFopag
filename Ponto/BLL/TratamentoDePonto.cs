using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modelo;
using Modelo.Proxy.Relatorios;

namespace BLL
{
    public class TratamentoDePonto : IBLL<Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto>
    {
        DAL.ITratamentoDePonto dalTratamentoDePonto;
        private string ConnectionString;

        public TratamentoDePonto() : this(null)
        {

        }

        public TratamentoDePonto(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public TratamentoDePonto(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalTratamentoDePonto = new DAL.SQL.TratamentoDePonto(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalTratamentoDePonto = new DAL.SQL.TratamentoDePonto(new DataBase(ConnectionString));
                    break;
            }
            dalTratamentoDePonto.UsuarioLogado = usuarioLogado;
        }
        
        /// <summary>
        /// Retorna os dados para Geração de Relatório de tratamento de ponto
        /// </summary>
        /// <param name="cpfs">Lista com cpfs dos funcionários</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <returns>Retorna lista para geração de relatório</returns>
        public List<Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto> RelatorioTratamentoDePonto(List<int> idsFuncs, DateTime datainicial, DateTime datafinal)
        {
            return dalTratamentoDePonto.RelatorioTratamentoDePonto(idsFuncs, datainicial, datafinal);
        }

        public DataTable GetAll()
        {
            throw new NotImplementedException();
        }

        public PxyRelatorioTratamentoDePonto LoadObject(int id)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> ValidaObjeto(PxyRelatorioTratamentoDePonto objeto)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> Salvar(Acao pAcao, PxyRelatorioTratamentoDePonto objeto)
        {
            throw new NotImplementedException();
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            throw new NotImplementedException();
        }
    }
}
