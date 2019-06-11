using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegistradorBiometrico.Model
{
    public class Usuario
    {

        [JsonProperty("Username")]
        public String Login { get; set; }

        [JsonProperty("Password")]
        public String Senha { get; set; }

        [JsonIgnore]
        public String ConnectionString { get; set; }
    }
}
