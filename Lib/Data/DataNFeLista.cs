using eNotasGW.Client.Lib.Models.NFe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Data
{
    public class DataNFeLista
    {
        public int totalRecords { get; set; }
        public List<NFe> data { get; set; }
    }
}
