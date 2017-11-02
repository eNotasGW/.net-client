using eNotasGW.Client.Lib.Models.NFe;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.NFe
{
    public class NFe
    {
        public enum AmbienteEmissao
        {
            Homologacao,
            Producao
        }

        public Cliente cliente { get; set; }
        public bool enviarPorEmail { get; set; }
        public Guid id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AmbienteEmissao ambienteEmissao { get; set; }

        public string tipo
        {
            get
            {
                return "NFS-e";
            }
        }

        public string idExterno { get; set; }
        public bool consumidorFinal { get; set; }
        public string indicadorPresencaConsumidor { get; set; }
        public Servico servico { get; set; }
        public decimal valorTotal { get; set; }
        public string idExternoSubstituir { get; set; }
        public string nfeIdSubstitituir { get; set; }
        public string linkDownloadPDF { get; set; }
        public string linkDownloadXML { get; set; }
    }
}
