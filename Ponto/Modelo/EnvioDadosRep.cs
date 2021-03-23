using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class EnvioDadosRep : Modelo.ModeloBase
    {
        public List<Empresa> Empresas { get; set; }
        public List<Funcionario> Funcionarios { get; set; }
        public List<pxyEmpresa> pxyEmpresas { get; set; }
        public List<pxyFuncionarioRelatorio> pxyFuncionarios { get; set; }

        public int idRelogioSelecionado { get; set; }
        public Modelo.REP relogioSelecionado { get; set; }
        private string _nomeRelogioSelecionado;
        public string nomeRelogioSelecionado 
        { 
            get { return _nomeRelogioSelecionado; }
            set { _nomeRelogioSelecionado = value; }
        }
        public IList<EnvioDadosRepDet> listEnvioDadosRepDet { get; set; }

        /// <summary>
        /// 0 = Inclusão
        /// 1 = Exclusão
        /// </summary>
        public Int16 bOperacao { get; set; }
        public string OperacaoDesc
        {
            get
            {
                if (bOperacao == 1)
                {
                    return "Excluir";
                }
                else
                {
                    return "Incluir/Alterar";
                }
            }
                                   }
        [Display(Name = "Funcionários")]
        public bool bEnviarFunc { get; set; }
        [Display(Name = "Empresa")]
        public bool bEnviarEmpresa { get; set; }
        [Display(Name = "Utilizar Grupo Econômico")]
        public bool bUtilizaGrupoEconomico { get; set; }
        public bool UtilizaControleContrato { get; set; }

        public int idEmpresaRelogio { get; set; }


        [Display(Name = "Nome")]
        private string _Nome;
        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }

        private string _Empresa;
        [Display(Name = "Empresa")]
        public string Empresa
        {
            get
            {
                if (String.IsNullOrEmpty(_Empresa))
                {
                    return Nome;
                }
                return _Empresa;
            }
            set { _Empresa = value; }
        }
        public string idsEmpresasSelecionadas { get; set; }
        public string idsFuncionariosSelecionados { get; set; }
        public string TipoComunicacao { get; set; }
        
    }
}
