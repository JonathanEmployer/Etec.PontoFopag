using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ServicoIntegracaoRep.Modelo
{
    public class Funcionario
    {
        public string CodBarras { get; set; }
        public string MiFareDado { get; set; }
        [JsonIgnore]
        public int Tempo { get; set; }
        public string BioDados { get; set; }
        public string ID { get; set; }
        public string PIS { get; set; }
        public string Nome { get; set; }
        public string Passwd { get; set; }
        [JsonIgnore]
        public int IDEnvioDadosRep { get; set; }
        [JsonIgnore]
        public int IDEnvioDadosRepDet { get; set; }
        [JsonIgnore]
        public bool BOperacao { get; set; }
    }

    public class CfgFuncionarios
    {
        public string cmd = "cfg_funcionarios";
        public List<Funcionario> Funcionarios { get; set; }
    }

    public class PedeDadosPIS: ReqResp
    {
        public Funcionario Funcionario { get; set; }
    }

    public class RespCfgFuncionarios : ReqResp
    {
        public int Total { get; set; }
        public int Alterados { get; set; }
        public int Incluidos { get; set; }
    }


    public class PededadosPIS
    {
        public string cmd = "pede_dados_PIS";
        public string PIS { get; set; }
    }

    public class LPIS
    {
        public string PIS { get; set; }
    }

    public class ExcluirFuncionarios
    {
        public string cmd = "exclui_funcionarios";
        public List<LPIS> Funcionarios { 
            get {
                    List<LPIS> lPIS = new List<LPIS>();
                    foreach (string pis in FuncionariosExcluir.Select(s => s.PIS))
                    {
                        lPIS.Add(new LPIS { PIS = pis });
                    }
                    return lPIS;
                }
        }
        [JsonIgnore]
        public List<Funcionario> FuncionariosExcluir { get; set; }
    }
}
