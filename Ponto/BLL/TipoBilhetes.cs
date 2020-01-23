using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLL
{
    public class TipoBilhetes : IBLL<Modelo.TipoBilhetes>
    {
        DAL.ITipoBilhetes dalTipoBilhetes;
        private string ConnectionString;
        public TipoBilhetes() : this(null)
        {
           
        }

        public TipoBilhetes(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public TipoBilhetes(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalTipoBilhetes = new DAL.SQL.TipoBilhetes(new DataBase(ConnectionString));
            dalTipoBilhetes.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalTipoBilhetes.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalTipoBilhetes.GetAll();
        }

        public Modelo.TipoBilhetes LoadObject(int id)
        {
            return dalTipoBilhetes.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.TipoBilhetes objeto)
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

            if (objeto.FormatoBilhete == -1)
            {
                ret.Add("cbFormatoBilhete", "Campo obrigatório.");
            }

            if (objeto.FormatoBilhete == 9 && String.IsNullOrEmpty(objeto.StrLayout))
            {
                ret.Add("txtLayout", "Selecione o(s) campo(s) do layout.");
            }

            if (objeto.FormatoBilhete != 4)
            {
                if (string.IsNullOrEmpty(objeto.Diretorio))
                {
                    ret.Add("txtDiretorio", "Campo obrigatório.");
                }
                else if (objeto.Diretorio.Length > 255)
                {
                    ret.Add("txtDiretorio", "O diretório não pode exceder 255 caracteres.");
                }
            }

            return ret;
        }

        private static void AtribuiCT(Modelo.TipoBilhetes objeto, char c, int posicao, int tamanho)
        {
            //posicao++;
            switch (c)
            {
                case 'O':
                    objeto.Ordem_c = posicao;
                    objeto.Ordem_t = tamanho;
                    break;
                case 'D':
                    objeto.Dia_c = posicao;
                    objeto.Dia_t = tamanho;
                    break;
                case 'M':
                    objeto.Mes_c = posicao;
                    objeto.Mes_t = tamanho;
                    break;
                case 'a':
                    objeto.Ano_c = posicao;
                    objeto.Ano_t = tamanho;
                    break;
                case 'A':
                    objeto.Ano_c = posicao;
                    objeto.Ano_t = tamanho;
                    break;
                case 'h':
                    objeto.Hora_c = posicao;
                    objeto.Hora_t = tamanho;
                    break;
                case 'm':
                    objeto.Minuto_c = posicao;
                    objeto.Minuto_t = tamanho;
                    break;
                case 'F':
                    objeto.Funcionario_c = posicao;
                    objeto.Funcionario_t = tamanho;
                    break;
                case 'R':
                    objeto.Relogio_c = posicao;
                    objeto.Relogio_t = tamanho;
                    break;
            }
        }

        private void AtribuiFormato(Modelo.TipoBilhetes objeto)
        {
            objeto.Ano_c = 0;
            objeto.Ano_t = 0;
            objeto.Dia_c = 0;
            objeto.Dia_t = 0;
            objeto.Funcionario_c = 0;
            objeto.Funcionario_t = 0;
            objeto.Hora_c = 0;
            objeto.Hora_t = 0;
            objeto.Mes_c = 0;
            objeto.Mes_t = 0;
            objeto.Minuto_c = 0;
            objeto.Minuto_t = 0;
            objeto.Ordem_c = 0;
            objeto.Ordem_t = 0;
            objeto.Relogio_c = 0;
            objeto.Relogio_t = 0;

            if (objeto.FormatoBilhete == 2)
            {
                char c = '.';
                int t, p;
                t = 0;
                p = 0;

                for (int i = 0; i < objeto.StrLayout.Length; i++)
                {
                    if (objeto.StrLayout[i] != c)
                    {
                        if (c == '.')
                        {
                            c = objeto.StrLayout[i];
                            p = i;
                        }
                        else
                        {
                            AtribuiCT(objeto, c, p, t);

                            c = objeto.StrLayout[i];
                            p = i;
                            t = 0;
                        }
                    }
                    t++;
                }

                AtribuiCT(objeto, c, p, t);
            }
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.TipoBilhetes objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                AtribuiFormato(objeto);
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalTipoBilhetes.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalTipoBilhetes.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalTipoBilhetes.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public void MontaLayout(ref string pStrLayout, char pCaractere, int pTamanho)
        {
            StringBuilder str = new StringBuilder(pStrLayout);

            for (int i = 0; i < pTamanho; i++)
            {
                str.Append(pCaractere);
            }

            pStrLayout = str.ToString();
        }

        public List<Modelo.TipoBilhetes> getListaImportacao()
        {
            return dalTipoBilhetes.getListaImportacao();
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
            return dalTipoBilhetes.getId(pValor, pCampo, pValor2);
        }

        public int ContaNumRegistros()
        {
            return dalTipoBilhetes.ContaNumRegistros();
        }
    }
}
