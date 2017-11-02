using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.NFe
{
    public class Cliente
    {
        public Endereco endereco { get; set; }
        public string tipoPessoa { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string cpfCnpj { get; set; }
        public string inscricaoMunicipal { get; set; }
        public string inscricaoEstadual { get; set; }
        public string indicadorContribuinteICMS { get; set; }
        public string telefone { get; set; }
    }
}
