﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.Prefeitura
{
    public class ServicoMunicipalUnificado
    {
        public int totalRecords { get; set; }
        public IEnumerable<ServicoUnificado> data { get; set; }
    }
}
