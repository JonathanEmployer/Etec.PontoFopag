using DAL.SQL;
using Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class HorarioDinamicoCiclo : IBLL<Modelo.HorarioDinamicoCiclo>
    {
        DAL.IHorarioDinamicoCiclo dalHorarioDinamicoCiclo;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public HorarioDinamicoCiclo() : this(null) { }
        public HorarioDinamicoCiclo(string connString) : this(connString, cwkControleUsuario.Facade.getUsuarioLogado) { }
        public HorarioDinamicoCiclo(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalHorarioDinamicoCiclo = new DAL.SQL.HorarioDinamicoCiclo(db);
                    break;
            }
            dalHorarioDinamicoCiclo.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public DataTable GetAll()
        {
            return dalHorarioDinamicoCiclo.GetAll();
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalHorarioDinamicoCiclo.getId(pValor, pCampo, pValor2);
        }

        public Modelo.HorarioDinamicoCiclo LoadObject(int id)
        {
            return dalHorarioDinamicoCiclo.LoadObject(id);
        }

        public Dictionary<string, string> Salvar(Acao pAcao, Modelo.HorarioDinamicoCiclo objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorarioDinamicoCiclo.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorarioDinamicoCiclo.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorarioDinamicoCiclo.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }
        public List<Dictionary<string, string>> Salvar(Modelo.HorarioDinamico HorarioDinamico)
        {
            List<Dictionary<string, string>> retornos = new List<Dictionary<string, string>>();
            Acao pAcao;
            foreach (var ciclo in HorarioDinamico.LHorarioCiclo)
            {
                if (ciclo.Id > 0)
                    pAcao = Acao.Alterar;
                else
                {
                    pAcao = Acao.Incluir;
                    ciclo.Codigo = this.dalHorarioDinamicoCiclo.MaxCodigo();
                }

                retornos.Add( this.Salvar(pAcao, ciclo));
            }

            return retornos;
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.HorarioDinamicoCiclo objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            return ret;
        }

        public IList<Modelo.HorarioDinamicoCiclo> AtualizarListaCiclos(Modelo.HorarioDinamico horarioDinamico, BLL.Jornada bllJornada)
        {
            
            foreach (var ciclo in horarioDinamico.LHorarioCiclo)
            {
                var objJornada = bllJornada.LoadObjectCodigo(ciclo.CodigoJornada);

                ciclo.IdhorarioDinamico = horarioDinamico.Id;
                ciclo.Idjornada = objJornada.Id;
            }
            return horarioDinamico.LHorarioCiclo;
        }

        public List<Modelo.HorarioDinamicoCiclo> GetHorarioDinamicoCiclo(List<int> idshorario)
        {
            return dalHorarioDinamicoCiclo.GetHorarioDinamicoCiclo(idshorario);
        }

        public List<Modelo.HorarioDinamicoCiclo> GetHorarioDinamicoCiclo(int idhorario)
        {
            return dalHorarioDinamicoCiclo.GetHorarioDinamicoCiclo(idhorario);
        }
    }
}
