using eNotasGW.Client.Lib.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.Empresa
{
    public class Empresa
    {
        public Endereco endereco { get; set; }
        public Guid id { get; set; }
        public string status { get; set; }
        public int prazo { get; set; }
        public bool dadosObrigatoriosPreenchidos { get; set; }
        public string cnpj { get; set; }
        public string inscricaoMunicipal { get; set; }
        public string inscricaoEstadual { get; set; }
        public string razaoSocial { get; set; }
        public string nomeFantasia { get; set; }
        public bool optanteSimplesNacional { get; set; }
        public string email { get; set; }
        public string telefoneComercial { get; set; }
        public bool? incentivadorCultural { get; set; }
        public string regimeEspecialTributacao { get; set; }
        public string aedf { get; set; }
        public string codigoServicoMunicipal { get; set; }
        public string itemListaServicoLC116 { get; set; }
        public string cnae { get; set; }
        public decimal? aliquotaIss { get; set; }
        public string descricaoServico { get; set; }
        public bool enviarEmailCliente { get; set; }
        public ConfiguracoesNFSeHomologacao ConfiguracoesNFSeHomologacao { get; set; }
        public ConfiguracoesNFSeProducao ConfiguracoesNFSeProducao { get; set; }
    }
}
