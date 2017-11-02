using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.Configuracao
{
    public class ConfiguracaoApi
    {
        public string ApiKey { get; set; }
        public string Versao { get; set; }
        public string BaseEndPoint { get; set; }
        public string DefaultContentType { get; set; }
    }
}
