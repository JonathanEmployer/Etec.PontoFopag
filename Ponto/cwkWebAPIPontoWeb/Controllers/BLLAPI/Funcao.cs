using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Controllers.BLLAPI
{
    public class Funcao
    {
        public static void SalvarFuncaoPorDescricao(String DescricaoFuncao, BLL.Funcao bllFuncao, out Modelo.Funcao DadosAntFunc, out Dictionary<string, string> erros)
        {
            int? idfuncao = bllFuncao.getFuncaoNome(DescricaoFuncao);
            DadosAntFunc = bllFuncao.LoadObject(idfuncao.GetValueOrDefault());
            Acao acao = new Acao();
            if (DadosAntFunc.Id == 0)
            {
                acao = Acao.Incluir;
                DadosAntFunc.Codigo = bllFuncao.MaxCodigo();
            }
            else
            {
                acao = Acao.Alterar;
            }

            DadosAntFunc.Descricao = DescricaoFuncao;

            erros = new Dictionary<string, string>();
            erros = bllFuncao.Salvar(acao, DadosAntFunc);
        }
    }
}