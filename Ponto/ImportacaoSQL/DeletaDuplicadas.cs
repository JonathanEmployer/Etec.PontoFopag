using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace ImportacaoSQL
{
    public class DeletaDuplicadas
    {
        private static DataTable GetMarcacoesDuplicadas()
        {
            //FbParameter[] parms = new FbParameter[]
            //{
            //    new FbParameter("@datai", FbDbType.TimeStamp),
            //    new FbParameter("@dataf", FbDbType.TimeStamp)
            //};

            //parms[0].Value = pDataI;
            //parms[1].Value = pDataF;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select \"v\".*, \"mar\".* from");
            sql.AppendLine("(");
            sql.AppendLine("select    \"func\".\"id\" as \"funcionarioid\"");
            sql.AppendLine(", \"mar\".\"data\" as \"data\"");
            sql.AppendLine(", COUNT(\"mar\".\"id\") as \"qtd\"");
            sql.AppendLine("from \"marcacao\" as \"mar\"");
            sql.AppendLine("left join \"funcionario\" as \"func\" on \"func\".\"id\" = \"mar\".\"idfuncionario\"");
            //sql.AppendLine("where \"mar\".\"data\" >= @datai");
            //sql.AppendLine("and \"mar\".\"data\" <= @dataf");
            sql.AppendLine("group by \"func\".\"id\" , \"mar\".\"data\"");
            sql.AppendLine(") as \"v\"");
            sql.AppendLine("inner join \"marcacao\" as \"mar\" on \"mar\".\"idfuncionario\" = \"v\".\"funcionarioid\" and \"mar\".\"data\" = \"v\".\"data\"");
            sql.AppendLine("where \"v\".\"qtd\" > 1");

            DataTable dt = new DataTable();
            dt.Load(DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql.ToString(), null));
            return dt;
        }

        private static void DeletarMarcacoes(List<int> pIdsMarcacoes)
        {
            FbParameter[] parms = new FbParameter[0];

            StringBuilder sql = new StringBuilder();

            sql.AppendLine("DELETE FROM \"marcacao\" WHERE \"marcacao\".\"id\" IN (");
            bool primeiro = true;
            foreach (int id in pIdsMarcacoes)
            {
                if (!primeiro)
                {
                    sql.Append(", ");
                }
                else
                {
                    primeiro = false;
                }
                sql.Append(id.ToString());
            }
            sql.Append(")");

            DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, sql.ToString(), true, parms);
        }

        private static void PreencheVetorMarcacoes(string[] pVetor, DataTable pDt, int pIndice)
        {
            int count = 0;
            for (int j = 1; j <= 8; j++)
            {
                pVetor[count++] = pDt.Rows[pIndice]["entrada_" + j.ToString()].ToString();
                pVetor[count++] = pDt.Rows[pIndice]["saida_" + j.ToString()].ToString();
            }
        }

        private static void AdicionaListaExcluir(int pIdMarcacao, List<int> pMarcacoesExcluir)
        {
            pMarcacoesExcluir.Add(pIdMarcacao);
        }

        public static void Deletar()
        {
            List<int> marcacoesExcluir = new List<int>();
            DataTable dt = GetMarcacoesDuplicadas();

            int idFunc = 0, idFuncAnt = 0, idMarcacao, idMarcValida = 0;
            string[] marcacoesValidas = new string[16];
            string[] marcacoesAtuais = new string[16];
            DateTime dataAnt = new DateTime(), data;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                idFunc = Convert.ToInt32(dt.Rows[i]["funcionarioid"]);
                idMarcacao = Convert.ToInt32(dt.Rows[i]["id"]);
                data = Convert.ToDateTime(dt.Rows[i]["data"]);
                if (idFuncAnt == 0 || idFuncAnt == idFunc)
                {
                    idFuncAnt = idFunc;
                    if (idMarcValida == 0)
                    {
                        dataAnt = data;
                        idMarcValida = idMarcacao;
                        PreencheVetorMarcacoes(marcacoesValidas, dt, i);
                    }
                    else
                    {
                        if (dataAnt == data)
                        {
                            PreencheVetorMarcacoes(marcacoesAtuais, dt, i);
                            if (marcacoesAtuais.Where(m => m != "--:--").Count() > marcacoesValidas.Where(m => m != "--:--").Count())
                            {
                                AdicionaListaExcluir(idMarcValida, marcacoesExcluir);
                                idMarcValida = idMarcacao;
                                marcacoesValidas = marcacoesAtuais;
                            }
                            else
                            {
                                AdicionaListaExcluir(idMarcacao, marcacoesExcluir);
                            }
                        }
                        else
                        {
                            dataAnt = data;
                            idMarcValida = idMarcacao;
                            PreencheVetorMarcacoes(marcacoesValidas, dt, i);
                        }
                    }
                }
                else
                {
                    dataAnt = data;
                    idFuncAnt = idFunc;
                    idMarcValida = idMarcacao;
                    PreencheVetorMarcacoes(marcacoesValidas, dt, i);
                }
            }

            if (marcacoesExcluir.Count > 0)
            {
                DeletarMarcacoes(marcacoesExcluir);
            }
        }
    }
}
