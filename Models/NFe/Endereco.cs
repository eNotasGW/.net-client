using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.NFe
{
    public class Endereco
    {
        public string pais { get; set; }
        public string uf { get; set; }
        public string cidade { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
    }
}
