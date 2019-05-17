using eNotasGW.Client.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace eNotasGW.Client.Lib.Models.NFe
{
    public class Servico
    {
        public int? codigoInternoServicoMunicipal { get; set; }
        public string descricao { get; set; }
        public decimal aliquotaIss { get; set; }
        public bool issRetidoFonte { get; set; }
        public string cnae { get; set; }
        public string codigoServicoMunicipio { get; set; }
        public string descricaoServicoMunicipio { get; set; }
        public string itemListaServicoLC116 { get; set; }
        public string ufPrestacaoServico { get; set; }
        public string municipioPrestacaoServico { get; set; }
        public decimal valorCofins { get; set; }
        public decimal valorCsll { get; set; }
        public decimal valorInss { get; set; }
        public decimal valorIr { get; set; }
        public decimal valorPis { get; set; }
    }
}
