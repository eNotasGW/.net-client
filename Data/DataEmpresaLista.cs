using eNotasGW.Client.Lib.Models;
using eNotasGW.Client.Lib.Models.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Data
{
    public class DataEmpresaLista
    {
        public int totalRecords { get; set; }
        public IEnumerable<Empresa> data { get; set; }
    }
}
