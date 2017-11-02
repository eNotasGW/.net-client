using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Exceptions
{
    public class GWLibErro
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Codigo, this.Mensagem);
        }
    }
}
