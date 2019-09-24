using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Eventos : IBLL<Modelo.Eventos>
    {
        DAL.IEventos dalEventos;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Eventos() : this(null)
        {
            
        }

        public Eventos(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Eventos(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalEventos = new DAL.SQL.Eventos(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalEventos = DAL.FB.Eventos.GetInstancia;
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalEventos.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEventos.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEventos.GetAll();
        }

        public List<Modelo.Eventos> GetAllList()
        {
            return dalEventos.GetAllList();
        }

        public Modelo.Eventos LoadObject(int id)
        {
            return dalEventos.LoadObject(id);
        }

        public Modelo.Eventos LoadObjectByCodigo(int codigo)
        {
            return dalEventos.LoadObjectByCodigo(codigo);
        }
        public Dictionary<string, string> ValidaObjeto(Modelo.Eventos objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            if (objeto.Tipohoras < 0)
            {
                ret.Add("rgTipohoras", "Campo obrigatório.");
            }

            if (!objeto.ClassificarHorasExtras)
            {
                if (objeto.PercentualExtra1 > 0 && objeto.He50 == 0 && objeto.He50N == 0)
                    ret.Add("PercentualExtra1", "Percentual Extra 1: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra2 > 0 && objeto.He60 == 0 && objeto.He60N == 0)
                    ret.Add("PercentualExtra2", "Percentual Extra 2: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra3 > 0 && objeto.He70 == 0 && objeto.He70N == 0)
                    ret.Add("PercentualExtra3", "Percentual Extra 3: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra4 > 0 && objeto.He80 == 0 && objeto.He80N == 0)
                    ret.Add("PercentualExtra4", "Percentual Extra 4: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra5 > 0 && objeto.He90 == 0 && objeto.He90N == 0)
                    ret.Add("PercentualExtra5", "Percentual Extra 5: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra6 > 0 && objeto.He100 == 0 && objeto.He100N == 0)
                    ret.Add("PercentualExtra6", "Percentual Extra 6: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra7 > 0 && objeto.Hesab == 0 && objeto.HesabN == 0)
                    ret.Add("PercentualExtra7", "Percentual Extra 7: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra8 > 0 && objeto.Hedom == 0 && objeto.HedomN == 0)
                    ret.Add("PercentualExtra8", "Percentual Extra 8: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra9 > 0 && objeto.Hefer == 0 && objeto.HeferN == 0)
                    ret.Add("PercentualExtra9", "Percentual Extra 9: Selecione Diurna e/ou Noturna.");

                if (objeto.PercentualExtra10 > 0 && objeto.Folga == 0 && objeto.FolgaN == 0)
                    ret.Add("PercentualExtra10", "Percentual Extra 10: Selecione Diurna e/ou Noturna."); 
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Eventos objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEventos.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalEventos.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEventos.Excluir(objeto);
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
            return dalEventos.getId(pValor, pCampo, pValor2);
        }
     
    }
}
