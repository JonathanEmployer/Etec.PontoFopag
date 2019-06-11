using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class HorarioDinamicoCicloSequencia : IBLL<Modelo.HorarioDinamicoCicloSequencia>
    {
        DAL.IHorarioDinamicoCicloSequencia dalHorarioDinamicoCicloSequencia;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public HorarioDinamicoCicloSequencia() : this(null) { }
        public HorarioDinamicoCicloSequencia(string connString) : this(connString, cwkControleUsuario.Facade.getUsuarioLogado) { }
        public HorarioDinamicoCicloSequencia(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    DataBase db = new DataBase(ConnectionString);
                    dalHorarioDinamicoCicloSequencia = new DAL.SQL.HorarioDinamicoCicloSequencia(db);
                    break;
            }
            dalHorarioDinamicoCicloSequencia.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalHorarioDinamicoCicloSequencia.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalHorarioDinamicoCicloSequencia.GetAll();
        }

        public List<Modelo.HorarioDinamicoCicloSequencia> GetAllListByHorarioDinamicoCiclo(int idHorarioDinamicoCiclo)
        {
            return dalHorarioDinamicoCicloSequencia.GetAllListByHorarioDinamicoCiclo(idHorarioDinamicoCiclo);
        }

        public List<Modelo.HorarioDinamicoCicloSequencia> GetAllListByHorarioDinamicoCiclo(List<int> idsHorarioDinamicoCiclo)
        {
            return dalHorarioDinamicoCicloSequencia.GetAllListByHorarioDinamicoCiclo(idsHorarioDinamicoCiclo);
        }

        public Modelo.HorarioDinamicoCicloSequencia LoadObject(int id)
        {
            return dalHorarioDinamicoCicloSequencia.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.HorarioDinamicoCicloSequencia objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.HorarioDinamicoCicloSequencia objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorarioDinamicoCicloSequencia.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorarioDinamicoCicloSequencia.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorarioDinamicoCicloSequencia.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public List<Dictionary<string, string>> Salvar(Modelo.HorarioDinamico HorarioDinamico)
        {
            List<Dictionary<string, string>> erros = new List<Dictionary<string, string>>();
            Modelo.Acao pAcao;
            foreach (var ciclo in HorarioDinamico.LHorarioCiclo)
            {
                foreach (var sequencia in ciclo.LHorarioCicloSequencia)
                {
                    if (sequencia.Id > 0)
                        pAcao = Modelo.Acao.Alterar;
                    else
                    {
                        pAcao = Modelo.Acao.Incluir;
                        sequencia.Codigo = this.dalHorarioDinamicoCicloSequencia.MaxCodigo();
                    }

                    erros.Add(this.Salvar(pAcao, sequencia));
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
            return dalHorarioDinamicoCicloSequencia.getId(pValor, pCampo, pValor2);
        }


        public IList<Modelo.HorarioDinamicoCiclo> AtualizarListaSequencias(Modelo.HorarioDinamico HorarioDinamico)
        {
            foreach (var ciclo in HorarioDinamico.LHorarioCiclo)
            {
                foreach (var sequencia in ciclo.LHorarioCicloSequencia)
                {
                    sequencia.IdHorarioDinamicoCiclo = ciclo.Id;
                }
            }            
            return HorarioDinamico.LHorarioCiclo;
        }
    }
}
