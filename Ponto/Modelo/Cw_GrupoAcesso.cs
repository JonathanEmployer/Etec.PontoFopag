using System;
using System.Text;

namespace Modelo
{
    public class Cw_GrupoAcesso : Modelo.ModeloBase
    {
        public Cw_GrupoAcesso()
        {
            Acao = Acao.Consultar;
        }

        /// <summary>
        /// ID do Grupo
        /// </summary> 
        public int IDCw_Grupo { get; set; }
        /// <summary>
        /// ID do Acesso
        /// </summary>
        public int IDCw_Acesso { get; set; }

        /// <summary>
        /// Indica se usuário pode alterar o registro (0 = Não, 1 = Sim)
        /// </summary>
        private int _Consultar;

        public int Consultar
        {
            get { return _Consultar; }
            set { _Consultar = value; }
        }

        /// <summary>
        /// Indica se usuário pode excluir o registro (0 = Não, 1 = Sim)
        /// </summary>
        private int _Excluir;

        public int Excluir
        {
            get { return _Excluir; }
            set { _Excluir = value; }
        }

        /// <summary>
        /// Indica se usuário pode cadastrar o registro (0 = Não, 1 = Sim)
        /// </summary>
        private int _Cadastrar;

        public int Cadastrar
        {
            get { return _Cadastrar; }
            set { _Cadastrar = value; }
        }

        /// <summary>
        /// Indica se usuário pode Alterar o registro (0 = Não, 1 = Sim)
        /// </summary>
        private int _Alterar;

        public int Alterar
        {
            get { return _Alterar; }
            set { _Alterar = value; }
        }

        public virtual string Controller { get; set; }

        /// <summary>
        /// Apresenta o nome da funcionalidade. Este campo não é persistido em banco
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Apresenta o menu onde a funcionalidade está localizada. Este campo não é persistido em banco
        /// </summary>
        public string Menu { get; set; }

        public bool HabilitarConsulta { get; set; }
        public bool HabilitarCadastro { get; set; }
        public bool HabilitarAlteracao { get; set; }
        public bool HabilitarExclusao { get; set; }
        public bool bCadastrar
        {
            get 
            {
                return Convert.ToBoolean(Cadastrar);
            }
            set
            {
                Cadastrar = Convert.ToInt32(value);
            }
        }

        public bool bConsultar
        {
            get
            {
                return Convert.ToBoolean(Consultar);
            }
            set
            {
                Consultar = Convert.ToInt32(value);
            }
        }

        public bool bAlterar
        {
            get
            {
                return Convert.ToBoolean(Alterar);
            }
            set
            {
                Alterar = Convert.ToInt32(value);
            }
        }

        public bool bExcluir
        {
            get
            {
                return Convert.ToBoolean(Excluir);
            }
            set
            {
                Excluir = Convert.ToInt32(value);
            }
        }

    }
}
