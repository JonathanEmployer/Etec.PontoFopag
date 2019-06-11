using System;
using System.Text;

namespace Modelo
{
    public class EmpresaCw_Usuario : Modelo.ModeloBase
    {
        public EmpresaCw_Usuario()
        {
            Acao = Acao.Consultar;
        }

        /// <summary>
        /// ID da Empresa
        /// </summary> 
        public int IdEmpresa { get; set; }
        /// <summary>
        /// ID do Usuário
        /// </summary>
        public int IdCw_Usuario { get; set; }

    }
}
