using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    /// <summary>
    /// Método responsável por gerenciar os arquivos a serem atualizados
    /// </summary>
    class ArquivosAtualizacaoViewModel
    {
        public string NomeArquivo { get; set; }
        public DateTime DataModificacao { get; set; }

        private int tamanhoBytes;
        public int TamanhoBytes
        {
            get { return tamanhoBytes; }
            set
            {
                tamanhoKB = Math.Round((value / 1024f));
                tamanhoBytes = value;
            }
        }

        private double tamanhoKB;

        public double TamanhoKB
        {
            get { return tamanhoKB; }
        }
        public acao Acao { get; set; }
    }

    enum acao
    {
        Atualizar,
        Incluir,
        Excluir,
        Baixado
    }
}
