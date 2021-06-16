using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MarcacaoHorasExtrasPercentual : IBLL<Modelo.MarcacaoHorasExtrasPercentual>
    {
        DAL.IMarcacaoHorasExtrasPercentual dalMarcHoraExtraPercent;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        #region Construtores
        public MarcacaoHorasExtrasPercentual()
            : this(null)
        {

        }

        public MarcacaoHorasExtrasPercentual(string connString)
            : this(connString, null)
        {

        }

        public MarcacaoHorasExtrasPercentual(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalMarcHoraExtraPercent = new DAL.SQL.MarcacaoHorasExtrasPercentual(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalMarcHoraExtraPercent.UsuarioLogado = usuarioLogado;
        }
        #endregion

        #region Metodos Padrão
        public DataTable GetAll()
        {
            return dalMarcHoraExtraPercent.GetAll();
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalMarcHoraExtraPercent.getId(pValor, pCampo, pValor2);
        }

        public Modelo.MarcacaoHorasExtrasPercentual LoadObject(int id)
        {
            return dalMarcHoraExtraPercent.LoadObject(id);
        }

        public Dictionary<string, string> Salvar(Acao pAcao, Modelo.MarcacaoHorasExtrasPercentual objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalMarcHoraExtraPercent.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalMarcHoraExtraPercent.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalMarcHoraExtraPercent.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.MarcacaoHorasExtrasPercentual objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }
        #endregion

        public List<RateioHorasExtras> CalculaPercentualHorasExtra(int idFunc, DateTime dataInicial, DateTime dataFinal)
        {
            return dalMarcHoraExtraPercent.CarregarPorPeriodoFunc(idFunc, dataInicial, dataFinal);
        }



    }
}
