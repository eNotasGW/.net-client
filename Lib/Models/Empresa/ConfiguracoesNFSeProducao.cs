using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.Empresa
{
    public class ConfiguracoesNFSeProducao
    {
        public int sequencialNFe { get; set; }
        public string serieNFe { get; set; }
        public int? sequencialLoteNFe { get; set; }
        public string usuarioAcessoProvedor { get; set; }
        public string senhaAcessoProvedor { get; set; }
        public string tokenAcessoProvedor { get; set; }
    }
}
