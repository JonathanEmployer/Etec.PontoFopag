using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwkAtualizadorComunicadorPontoWeb.ViewModels
{
    public class VersaoPastaViewModel
    {
        /// <summary>
        /// Nome do arquivo.
        /// </summary>
        public String Nome { get; set; }
        private String versaoString;
        public String VersaoString
        {
            get { return versaoString; }
            set
            {
                //Verifico se foi informada a versão, e se existe '_' na versão () o padrão deve ser "nome do produto_1_0_0_1"
                if (value != null && value.IndexOf('_') >= 0)
                {
                    //Pego apenas o pedaço da stringo com a versão ex: "1_0_0_11"
                    string versao = value.Substring(value.IndexOf('_')+1);
                    //Verifico se a versão esta no padrão, ou seja separada por 3 '_'
                    if (versao.Count(c => c == '_') == 3)
                    {
                        //Desmembro a versão e coloco nas suas respectivas propriedades
                        String[] vs = versao.Split('_');
                        principal = Convert.ToInt32(vs[0]);
                        secundaria = Convert.ToInt32(vs[1]);
                        compilacao = Convert.ToInt32(vs[2]);
                        revisao = Convert.ToInt32(vs[3]);
                        versaoStringResumida = versao.Replace('_','.');
                    }
                }
                versaoString = value; }
        }

        private String versaoStringResumida;

        public String VersaoStringResumida
        {
            get { return versaoStringResumida; }
        }

        private int principal;

        public int Principal
        {
            get { return principal; }
        }

        private int secundaria;

        public int Secundaria
        {
            get { return secundaria; }
        }

        private int compilacao;

        public int Compilacao
        {
            get { return compilacao; }
        }

        private int revisao;

        public int Revisao
        {
            get { return revisao; }
        }
        
    }
}
