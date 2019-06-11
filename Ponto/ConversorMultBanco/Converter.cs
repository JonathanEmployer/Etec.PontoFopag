using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ConversorMultBanco
{
    public class Converter
    {
        string VERSAO = "";
        /************INSERIR A NOVA VERSÃO AQUI!!!*************/
        public string[] versoes = { "3.17.017", "3.18.018", "3.19.019", "3.20.020", "3.21.021", "3.22.023", "3.23.024" };

        public bool Conversor(string conn, ref string retorno, string cbVersao)
        {
            Modelo.cwkGlobal.objUsuarioLogado = new Modelo.Cw_Usuario();
            Modelo.cwkGlobal.objUsuarioLogado.Login = "cwork";
            Modelo.cwkGlobal.CONN_STRING = conn;
            DataBase db = new DataBase(Modelo.cwkGlobal.CONN_STRING);

            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    if (cbVersao == "3.15.015")
                    {
                        Versao315015.Converter(db);
                    }

                    if (cbVersao == "3.16.016")
                    {
                        Versao316016.Converter(db);
                    }

                    if (cbVersao == "3.17.017")
                    {
                        Versao317017.Converter(db);
                    }

                    if (cbVersao == "3.18.018")
                    {
                        Versao318018.Converter(db);
                    }

                    if (cbVersao == "3.19.019")
                    {
                        Versao319019.Converter(db);
                    }
                    if (cbVersao == "3.20.020")
                    {
                        Versao320020.Converter(db);
                    }

                    if (cbVersao == "3.21.021")
                    {
                        Versao321021.Converter(db);
                    }
                    if (cbVersao == "3.22.023")
                    {
                        Versao322023.Converter(db);
                    }
                    if (cbVersao == "3.23.024")
                    {
                        Versao323024.Converter(db);
                    }

                    /************INSERIR A NOVA VERSÃO AQUI!!!*************/

                    string versao = Convert.ToString(cbVersao);
                    string[] versoes = versao.Split('.');

                    VERSAO = String.Format("{0}.{1:00}.{2:000}", versoes[0], Convert.ToInt32(versoes[1]), Convert.ToInt32(versoes[2]));
                    AtualizarVersao(db, VERSAO);
                    trans.Complete();
                }

                retorno = "Base de dados atualizada para a versão " + VERSAO + ".";
                return false;
            }
            catch (Exception ex)
            {
                retorno = "Erro ao realizar conversão:\n" + ex.ToString();
                return true;
            }
        }

        public static void AtualizarVersao(DataBase db, string Versao)
        {
            BLL.Empresa bllEmpresa = new BLL.Empresa();
            SqlParameter[] parms2 = new SqlParameter[]
                            {
                                new SqlParameter("@cwk", SqlDbType.Binary)
                            };
            Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();
            parms2[0].Value = bllEmpresa.GeraVersao(Versao + objEmpresa.Numeroserie);
            string cmd15 = "UPDATE cwkvsnsys SET cwk = @cwk";
            db.ExecuteNonQuery(CommandType.Text, cmd15, parms2);
            if (String.IsNullOrEmpty(objEmpresa.UltimoAcesso))
            {
                bllEmpresa.SetUltimoAcesso();
            }
        }
    }
}
