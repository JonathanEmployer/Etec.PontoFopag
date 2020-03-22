using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy.CentralCliente
{
    public class PxyComunicadorServico
    {
        public PxyComunicadorServico()
        {
        }

        public PxyComunicadorServico(int id, int? codigo, string descricao, string observacao, string mAC, string serverName)
        {
            Id = id;
            Codigo = codigo;
            Descricao = descricao;
            Observacao = observacao;
            MAC = mAC;
            ServerName = ServerName;
        }

        public int Id { get; set; }
        [Required]
        public Nullable<int> Codigo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public string Observacao { get; set; }
        [Required]
        public string MAC { get; set; }
        public string ServerName { get; set; }
    }
}
