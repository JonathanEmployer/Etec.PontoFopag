using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Modelo
{
    public class DiasCompensacao : Modelo.ModeloBase
    {
        /// <summary>
        /// ID da Compensação
        /// </summary>
        public int Idcompensacao {get; set; }
        /// <summary>
        /// Data que foi Compensada
        /// </summary>
        [Display(Name = "Data Compensada")]
        [MinDate("31/12/1999")]
        public DateTime? Datacompensada {get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Compensacao Compensacao { get; set; }

        public bool Delete { get; set; }
    }
}
