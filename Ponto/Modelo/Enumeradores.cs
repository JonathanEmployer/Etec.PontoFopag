using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class Enumeradores
    {
        public enum SituacaoLog
        {
            Sucesso = 0,
            Erro = 1,
            [Description("Informação")]
            Informacao = 2
        };

        public enum SituacaoRegistroPonto
        {
            [Description("Incluído")]
            Incluido = 'I',
            Reprocessar = 'R',
            Processando = 'P',
            [Description("Concluído")]
            Concluido = 'C',
            Erro = 'E',
            Todos = 'T'
        };

        public enum TipoArquivo
        {
            PDF,
            Imagem,
            Excel,
            Word
        }

        public enum TipoFiltroFuncionario
        {
            Empresa = 0,
            Departamento = 1,
            Funcionario = 2,
            Funcao = 3,
            Horario = 4
        }

        public enum PontoComFuncoes
        {
            Atualizar = 1,
            EnviarConfiguracoesDataHora = 2,
            EnviarEmpregadoEmpregador = 3
        }
    }
}
