using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class LancamentoCartaoPonto : IBLL<Modelo.LancamentoCartaoPonto>
    {
        DAL.ILancamentoCartaoPonto dalLancamentoCartaoPonto;
        private string ConnectionString;
        private readonly Modelo.Cw_Usuario UsuarioLogado;

        public LancamentoCartaoPonto() : this(null)
        {
            
        }

        public LancamentoCartaoPonto(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LancamentoCartaoPonto(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalLancamentoCartaoPonto = new DAL.SQL.LancamentoCartaoPonto(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalLancamentoCartaoPonto = new DAL.SQL.LancamentoCartaoPonto(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalLancamentoCartaoPonto.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLancamentoCartaoPonto.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLancamentoCartaoPonto.GetAll();
        }

        public Modelo.LancamentoCartaoPonto LoadObject(int id)
        {
            return dalLancamentoCartaoPonto.LoadObject(id);
        }

        public List<Modelo.LancamentoCartaoPonto> GetAllList()
        {
            return dalLancamentoCartaoPonto.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LancamentoCartaoPonto objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Regs.Where(w => w.Editavel).Count() == 0)
            {
                ret.Add("Regs", "Nenhum registro novo lançado para o funcionário.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LancamentoCartaoPonto objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLancamentoCartaoPonto.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLancamentoCartaoPonto.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLancamentoCartaoPonto.Excluir(objeto);
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
            return dalLancamentoCartaoPonto.getId(pValor, pCampo, pValor2);
        }

        public Dictionary<string, string> SalvarCartaoPonto(Modelo.LancamentoCartaoPonto obj, out List<Modelo.BilhetesImp> bilhetes, out Modelo.Funcionario funcionario)
        {
            Dictionary<string, string> erros = ValidaObjeto(obj);
            bilhetes = new List<Modelo.BilhetesImp>();
            funcionario = new Modelo.Funcionario();
            if (erros.Count == 0)
            {   
                bool virouDia = false;
                LancamentoCartaoPontoRegistros[] registrosSalvar = obj.Regs.Where(w => w.Editavel).ToArray();
                Modelo.LancamentoCartaoPontoRegistros reg = new Modelo.LancamentoCartaoPontoRegistros();
                BilhetesImp bllBilhetesImp = new BilhetesImp(ConnectionString, UsuarioLogado);
                Funcionario bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
                Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
                funcionario = bllFuncionario.LoadObject(obj.IdFuncionario);
                int maxCodigoBilhetes = bllBilhetesImp.MaxCodigo();
                for (int i = 0; i < registrosSalvar.Count(); i++)
                {
                    string ent = "";
                    string sai = "";
                    string regAnt = "";
                    reg = registrosSalvar[i];
                    virouDia = false;
                    for (int j = 1; j < 9; j++)
                    {
                        var ev = reg.GetType().GetProperty("E" + j).GetValue(reg, null);
                        ent = ev == null ? "" : ev.ToString();
                        ValidaViradaDia(ref virouDia, ref regAnt, ent);
                        AddBilhete(bilhetes, !virouDia ? reg.Data : reg.Data.AddDays(+1), reg.Data, ent, "E", funcionario, obj.IdJustificativa, obj.Motivo, maxCodigoBilhetes++, j);
                        var sv = reg.GetType().GetProperty("S" + j).GetValue(reg, null);
                        sai = sv == null ? "" : sv.ToString();
                        ValidaViradaDia(ref virouDia, ref regAnt, sai);
                        AddBilhete(bilhetes, !virouDia ? reg.Data : reg.Data.AddDays(+1), reg.Data, sai, "S", funcionario, obj.IdJustificativa, obj.Motivo, maxCodigoBilhetes++, j);
                    }
                }

                if (bilhetes.Count > 0)
                {
                    DateTime pdataInicial = bilhetes.Min(m => m.Mar_data).GetValueOrDefault();
                    DateTime pDataFinal = bilhetes.Max(m => m.Mar_data).GetValueOrDefault();
                    TimeSpan ts = pDataFinal.AddDays(1) - pdataInicial;

                    int qtd = bllMarcacao.QuantidadeMarcacoes(funcionario.Id, pdataInicial, pDataFinal);

                    if (ts.TotalDays > qtd)
                    {
                        Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                        bllMarcacao.AtualizaData(pdataInicial, pDataFinal, funcionario);
                    }
                    registrosSalvar.ToList().ForEach(f => f.IdFuncionario = obj.IdFuncionario);
                    dalLancamentoCartaoPonto.IncluirRegistros(bilhetes, registrosSalvar.ToList());
                }
            }
            return erros;
        }

        private static void ValidaViradaDia(ref bool virouDia, ref string regAnt, string hora)
        {
            if (!string.IsNullOrEmpty(regAnt) && !string.IsNullOrEmpty(regAnt) && regAnt.ConvertHorasMinuto() > hora.ConvertHorasMinuto())
            {
                virouDia = true;
            }
            regAnt = hora;
        }

        private void AddBilhete(List<Modelo.BilhetesImp> bilhetes, DateTime dt, DateTime dt_mar, string hora, string entSai, Modelo.Funcionario funcionario, int idJustificativa, string motivo, int codigo, int posicao)
        {
            if (!string.IsNullOrEmpty(hora))
            {
                bilhetes.Add(new Modelo.BilhetesImp()
                {
                    Acao = Acao.Incluir,
                    Codigo = codigo,
                    Data = dt,
                    Mar_data = dt_mar,
                    Hora = hora,
                    Mar_hora = hora,
                    Relogio = "MA",
                    Mar_relogio = "MA",
                    Idjustificativa = idJustificativa,
                    IdFuncionario = funcionario.Id,
                    PIS = funcionario.Pis,
                    DsCodigo = funcionario.Dscodigo,
                    Func = funcionario.Dscodigo,
                    Ent_sai = entSai,
                    Importado = 1,
                    Ordem = entSai == "E" ? "010" : "011",
                    Posicao = posicao,
                    Incdata = DateTime.Now.Date,
                    Inchora = DateTime.Now,
                    Altdata = null,
                    Althora = null,
                    Incusuario = UsuarioLogado.Login,
                    Ocorrencia = 'I',
                    Motivo = motivo
                });
            }
        }
    }
}
