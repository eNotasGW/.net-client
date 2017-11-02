using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Models.Prefeitura
{
    public class CaracteristicaPrefeitura
    {
        /// <summary>
        /// 0 - Nenhuma;
        /// 1 - Certificado;
        /// 2 - Usuario e Senha;
        /// 3 - Token
        /// </summary>
        public enum TipoAutenticacao
        {
            Nenhuma,
            Certificado,
            UsuarioESenha,
            Token
        }

        /// <summary>
        /// 0 - Nao Utiliza;
        /// 1 - Opcional;
        /// 2 - Obrigatorio
        /// </summary>
        public enum TipoAssinaturaDigital
        {
            NaoUtiliza,
            Opcional,
            Obrigatorio
        }

        public int tipoAutenticacao { get; set; }
        public int assinaturaDigital { get; set; }
        public Helptipoautenticacao helpTipoAutenticacao { get; set; }
        public int campoLoginProvedor { get; set; }
        public bool suportaCancelamento { get; set; }
        public bool usaAEDF { get; set; }
        public bool usaRegimeEspecialTributacao { get; set; }
        public bool usaCodigoServicoMunicipal { get; set; }
        public bool usaDescricaoServico { get; set; }
        public bool usaCNAE { get; set; }
        public bool usaItemListaServico { get; set; }
        public string helpInscricaoMunicipal { get; set; }
        public string helpRegimeEspecialTributacao { get; set; }
        public string helpCodigoServicoMunicipal { get; set; }
        public string helpDescricaoServico { get; set; }
        public string helpCNAE { get; set; }
        public string helpItemListaServico { get; set; }
        public bool suportaEmissaoNFeSemCliente { get; set; }
        public bool suportaEmissaoNFeClienteSemCpf { get; set; }
        public bool suportaEmissaoNFeClienteSemEndereco { get; set; }
        public bool suportaCancelamentoNFeSemCliente { get; set; }
        public bool suportaCancelamentoNFeClienteSemCpf { get; set; }
        public IEnumerable<Regimesespecialtributacao> regimesEspecialTributacao { get; set; }
    }

    public class Helptipoautenticacao
    {
        public string certificadoDigital { get; set; }
        public object usuario { get; set; }
        public object senha { get; set; }
        public object token { get; set; }
        public object fraseSecreta { get; set; }
    }

    public class Regimesespecialtributacao
    {
        public string codigo { get; set; }
        public string nome { get; set; }
    }
}
