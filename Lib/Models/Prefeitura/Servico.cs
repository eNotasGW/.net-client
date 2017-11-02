using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.Prefeitura
{
    public class Servico
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
        public int codigoIBGECidade { get; set; }
        public decimal aliquotaSugerida { get; set; }
        public bool construcaoCivil { get; set; }
        public decimal percentualAproximadoFederalIBPT { get; set; }
        public decimal percentualAproximadoEstadualIBPT { get; set; }
        public decimal percentualAproximadoMunicipalIBPT { get; set; }
        public string chaveTabelaIBPT { get; set; }
    }
}
