using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using Veridis.Biometric;

namespace BLL
{
    public class Biometria : IBLL<Modelo.Biometria>
    {
        DAL.IBiometria dalBiometria;
        private readonly string ConnectionString;
        private readonly Modelo.Cw_Usuario UsuarioLogado;

        public Biometria()
            : this(null)
        {

        }

        public Biometria(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Biometria(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalBiometria = new DAL.SQL.Biometria(new DataBase(ConnectionString));

            UsuarioLogado = usuarioLogado;
            dalBiometria.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalBiometria.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalBiometria.GetAll();
        }

        public List<Modelo.Biometria> GetAllList()
        {
            return dalBiometria.GetAllList();
        }

        public Modelo.Biometria LoadObject(int id)
        {
            return dalBiometria.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Biometria objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Biometria objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalBiometria.Incluir(objeto);
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalBiometria.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalBiometria.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public void Salvar(Modelo.Biometria Biometria)
        {
            dalBiometria.Adicionar(Biometria, false);
        }

        public Modelo.Biometria LoadObjectByCodigo(int codBiometria)
        {
            return dalBiometria.LoadObjectByCodigo(codBiometria);
        }

        public List<Modelo.Biometria> LoadPorFuncionario(int idfuncionario)
        {
            return dalBiometria.LoadPorFuncionario(idfuncionario);
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalBiometria.getId(pValor, pCampo, pValor2);
        }

        public static Int32 GetIDFuncionarioByBiometrias(List<Modelo.Biometria> lstBiometria, BiometricSample objBiometricSample)
        {
            Int32 idFuncionario = 0;

            try
            {
                BiometricTemplate bioTemplateFuncionario = new BiometricTemplate(objBiometricSample);
                return GetIDFuncionarioByBiometrias(lstBiometria, bioTemplateFuncionario);
            }
            catch (Exception)
            {
                idFuncionario = -1;
            }

            return idFuncionario;
        }

        public static Int32 GetIDFuncionarioByBiometrias(List<Modelo.Biometria> lstBiometria, BiometricTemplate bioTemplateFuncionario)
        {
            Int32 idFuncionario = 0;
            try
            {
                BiometricIdentification bioId = bioTemplateFuncionario.StartIdentification();

                for (int contador = 0; contador < lstBiometria.Count; contador++)
                {
                    BiometricTemplate bioTemplateCandidata = new BiometricTemplate(lstBiometria[contador].valorBiometria);

                    ComparisonResult ret = bioId.CompareTo(bioTemplateCandidata);

                    Boolean bEncontrada = ret.Match;
                    if (bEncontrada)
                    {
                        idFuncionario = lstBiometria[contador].idfuncionario;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                idFuncionario = -1;
            }

            return idFuncionario;
        }

        public List<Modelo.Biometria> GetBiometriaTipoBiometria(int IdFuncionario)
        {
            return dalBiometria.GetBiometriaTipoBiometria(IdFuncionario);
        }

    }
}
