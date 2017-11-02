using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.Prefeitura
{
    public class ServicoUnificado
    {
        public int codigoIbge { get; set; }
        public int codigoIbgeUF { get; set; }
        public string uf { get; set; }
        public string nome { get; set; }
    }
}
